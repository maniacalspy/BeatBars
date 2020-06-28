using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Diagnostics;
using IronPython.Runtime.Operations;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BeatBarsGame
{
    public interface IInputDataStream
    {
        List<Keys> StreamInputKeys { get; }

        void BeginDataStream();
        void EndDataStream();
    }

    public class TwitchInputHandler : GameComponent, IInputDataStream
    {
        ProcessStartInfo start;
        Process process;
        StringBuilder output;

        Dictionary<String, Keys> KeywordKeyBinds = new Dictionary<string, Keys>
        {
            {"north", Keys.Up },
            {"south", Keys.Down },
            {"east", Keys.Right },
            {"west", Keys.Left }
        };

        List<Keys> ChatRequestedKeys;
        public List<Keys> StreamInputKeys { get
            {
                var outKeys = new List<Keys>(ChatRequestedKeys);
                ChatRequestedKeys.Clear();
                output.Clear();
                return outKeys;
            } 
        }

        public TwitchInputHandler(Game game) : base(game)
        {
            output = new StringBuilder();
            ChatRequestedKeys = new List<Keys>();
            start = new ProcessStartInfo();
            start.FileName = @"C:\Program Files (x86)\Microsoft Visual Studio\Shared\Python37_64\python.exe";
            start.Arguments = string.Format("{0} {1}", @"C:\Users\Nolan\Desktop\BeatBars\BeatBarsGame\Util\ChatReader\ChatBot.py", "BeatBars");
            start.UseShellExecute = false;
            start.CreateNoWindow = true;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            start.LoadUserProfile = true;


            start.EnvironmentVariables["TMI_TOKEN"] = ChatBotData.TMI_TOKEN;
            start.EnvironmentVariables["CLIENT_ID"] = ChatBotData.CLIENT_ID;
            start.EnvironmentVariables["BOT_NICK"] = ChatBotData.BOT_NICK;
            start.EnvironmentVariables["BOT_PREFIX"] = ChatBotData.BOT_PREFIX;
            start.EnvironmentVariables["CHANNEL"] = ChatBotData.CHANNEL;

            process = new Process();

            game.Services.AddService(typeof(IInputDataStream), this);

        }

        public void BeginDataStream()
        {
            StartChatIntegration();
        }

        public void EndDataStream()
        {
            StopChatIntegration();
        }

        void StartChatIntegration()
        {
            process = Process.Start(start);
            
            process.OutputDataReceived += ProcessOutputHandler;
            process.BeginOutputReadLine();
            
            process.ErrorDataReceived += ProcessErrorHandler;
            process.BeginErrorReadLine();
        }

        async void ProcessOutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                output.Append("\n" + outLine.Data);
            }
        }
        
        async void ProcessErrorHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                Console.WriteLine("\n" + outLine.Data);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (output.Length > 0)
            {
                var chatData = output.ToString().Split('\n');
                for (int i = 0; i < chatData.Length; i++)
                {
                    string splitOnString = ":";
                    char[] splitChars = splitOnString.ToCharArray();
                    var message = chatData[i];
                    if (message.Length > 0)
                    {
                        message = message.Replace("\'", "");
                        var msgData = (message.Remove(message.Length - 1,1)).Remove(0,1).Split(splitChars, 2);
                        if(msgData.Length > 1)
                        {
                            var username = msgData[0];
                            var chatMessage = msgData[1].Trim();
                            if (KeywordKeyBinds.ContainsKey(chatMessage))
                            {
                                if (!ChatRequestedKeys.Contains(KeywordKeyBinds[chatMessage])) ChatRequestedKeys.Add(KeywordKeyBinds[chatMessage]);
                            }

                        }
                    }
                }
            }
            base.Update(gameTime);
        }

        void StopChatIntegration()
        {
            process.Close();
        }
    }
}

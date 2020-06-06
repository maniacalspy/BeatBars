using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatBarsGame
{
    class GameConsoleBar : Bar
    {
        GameConsole console;
        public GameConsoleBar()
        {
            this.console = null;
        }

        public GameConsoleBar(GameConsole console)
        {
            this.console = console;
        }

        public override void Log(string s)
        {
            if (console != null)
            {
                console.GameConsoleWrite(s);
            }
            else
            {
                base.Log(s);
            }
        }
    }
}

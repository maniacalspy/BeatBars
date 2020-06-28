using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatBarsGame
{

    sealed class BBPlayerController
    {
        enum InputMode {Controller, Chat}

        InputMode _mode;
        InputMode Mode
        {
            get { return _mode; }

            set {
                if(_mode != value)
                {
                    if (value == InputMode.Chat)
                    {
                        twitchInput.BeginDataStream();
                        _mode = value;

                    }
                    else
                    {
                        twitchInput.EndDataStream();
                        _mode = value;
                    }
                }
            }
        }


        InputHandler input;
        TwitchInputHandler twitchInput;

        public List<RowCompassLocation> ChangingRows { get; private set; }

        public bool RowsNeedChanged;

        Dictionary<Keys, RowCompassLocation> keyRowBindings;

        public BBPlayerController(Game game)
        {
            _mode = InputMode.Controller;
            twitchInput = (TwitchInputHandler)game.Services.GetService<IInputDataStream>();
            input = (InputHandler)game.Services.GetService<IInputHandler>();

            RowsNeedChanged = false;
            if (input == null)
            {
                throw new Exception("PlayerController relies on InputHandler as a service, and so you must ensure you have added it as a service first");
            }

            ChangingRows = new List<RowCompassLocation>();

            keyRowBindings = new Dictionary<Keys, RowCompassLocation>
            {
                { Keys.Up, RowCompassLocation.North },
                { Keys.Down, RowCompassLocation.South },
                { Keys.Left, RowCompassLocation.West },
                { Keys.Right, RowCompassLocation.East },
            };
        }

        public void Update()
        {

            CheckSwitchInputMode();

            List<RowCompassLocation> rowsPressed = new List<RowCompassLocation>();

            switch (_mode)
            {
                case (InputMode.Controller):
                    rowsPressed = KeyboardUpdate();
                    break;

                case (InputMode.Chat):
                    rowsPressed = ChatUpdate();
                    break;
            }
            ChangingRows = rowsPressed;
            if (ChangingRows.Count > 0) {
                RowsNeedChanged = true;
            }
        }

        void CheckSwitchInputMode()
        {
            if (input.WasKeyPressed(Keys.OemTilde))
            {
                if (Mode == InputMode.Controller) Mode = InputMode.Chat;
                else Mode = InputMode.Controller;
            }
        }

        List<RowCompassLocation> KeyboardUpdate()
        {
            List<RowCompassLocation> output = new List<RowCompassLocation>();
            List<RowCompassLocation> keyRowsPressed = HandleKeyboardRows();

            foreach (var row in keyRowsPressed)
            {
                if (!output.Contains(row)) output.Add(row);
            }

            return output;
        }

        List<RowCompassLocation> ChatUpdate()
        {
            List<RowCompassLocation> output = new List<RowCompassLocation>();
            //TODO: FINISH WRITING TWITCH INPUT HANDLER
            List<Keys> ChatKeysRequested = twitchInput.StreamInputKeys;

            foreach (var input in ChatKeysRequested)
            {
                if (keyRowBindings.ContainsKey(input))
                {
                    if (!output.Contains(keyRowBindings[input])) output.Add(keyRowBindings[input]);
                }
            }

            return output;
        }

        public List<RowCompassLocation> HandleKeyboardRows()
        {

            List<RowCompassLocation> rowsPressed = new List<RowCompassLocation>();

            foreach (var pair in keyRowBindings)
            {
                if (input.WasKeyPressed(pair.Key))
                {
                    rowsPressed.Add(pair.Value);
                }
            }
            return rowsPressed;
        }
    }
}

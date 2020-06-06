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
        InputHandler input;

        public List<RowCompassLocation> ChangingRows { get; private set; }

        public bool RowsNeedChanged;

        Dictionary<Keys, RowCompassLocation> keyRowBindings;

        public BBPlayerController(Game game)
        {
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
            List<RowCompassLocation> rowsPressed = new List<RowCompassLocation>();
            List<RowCompassLocation> keyRowsPressed = HandleKeyboardRows();

            foreach(var row in keyRowsPressed)
            {
                if (!rowsPressed.Contains(row)) rowsPressed.Add(row);
            }


            ChangingRows = rowsPressed;
            if (ChangingRows.Count > 0) {
                RowsNeedChanged = true;
            }
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

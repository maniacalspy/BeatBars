using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using static BeatBarsGame.BarGlobals;

namespace BeatBarsGame
{
    public static class BarGlobals
    {
        public enum BarSideState { Green, Red};
        public const int BarSideCount = 2;

        public enum BarCollisionResult { NoCollision, RightSide, WrongSide};

        public static BarSideState GetNextBarState(BarSideState curstate)
        {
            switch (curstate)
            {
                case BarSideState.Green:
                   return BarSideState.Red;
                case BarSideState.Red:
                   return BarSideState.Green;
                
                default:
                    return BarSideState.Green;
            }
        }

        public static BarSideState GetStateFromInt(int state)
        {
            switch (state)
            {
                case 0:
                    return BarSideState.Green;
                case 1:
                    return BarSideState.Red;
                default:
                    return BarSideState.Green;
            }
        }

        public static Color GetColorFromState(BarSideState curState)
        {
            switch (curState)
            {
                case BarSideState.Green:
                    return Color.Green;
                case BarSideState.Red:
                    return Color.Red;
                default:
                    return Color.Green;
            }
        }
    }
    class Bar
    {
        
        protected BarSideState _state;

        public BarSideState State { 
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    _state = value;
                }
            }
        }


        public Bar()
        {
            this.State = BarSideState.Green;
        }

        public virtual void Log(string s)
        {
            //nothing
            Console.WriteLine(s);
        }
    }
}

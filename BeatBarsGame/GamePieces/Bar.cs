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
                    this.Log(string.Format("{0} was: {1} now {2}", this.ToString(), _state, value));

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatBarsGame
{
    public enum BarSideState { Green, Red, Blue};

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

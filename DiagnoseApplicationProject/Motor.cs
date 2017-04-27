using Packager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControlServer
{
    public class Motor
    {
        private int _Id = 0;
        private int angle = 0;
        private int velocity = 0;
        private GlobalDataSet.RobotActions action = GlobalDataSet.RobotActions.doNothing;
        private GlobalDataSet.ActionStates state = GlobalDataSet.ActionStates.init;
        private int duration = 0;
        private int rowCounter = 0;
        private int maxRows = 0;
        private bool actionIsSet = false;

        public int Id
        {
            get
            {
                return _Id;
            }

            set
            {
                _Id = value;
            }
        }

        public int Angle
        {
            get
            {
                return angle;
            }

            set
            {
                angle = value;
            }
        }

        public int Velocity
        {
            get
            {
                return velocity;
            }

            set
            {
                velocity = value;
            }
        }

        public GlobalDataSet.RobotActions Action
        {
            get
            {
                return action;
            }

            set
            {
                action = value;
            }
        }

        public GlobalDataSet.ActionStates State
        {
            get
            {
                return state;
            }

            set
            {
                state = value;
            }
        }

        public int Duration
        {
            get
            {
                return duration;
            }

            set
            {
                duration = value;
            }
        }

        public int RowCounter
        {
            get
            {
                return rowCounter;
            }

            set
            {
                rowCounter = value;
            }
        }

        public int MaxRows
        {
            get
            {
                return maxRows;
            }

            set
            {
                maxRows = value;
            }
        }

        public bool ActionIsSet
        {
            get
            {
                return actionIsSet;
            }

            set
            {
                actionIsSet = value;
            }
        }
    }
}

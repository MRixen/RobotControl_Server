using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Packager;

namespace Packager
{
    ///\brief Globally used program data.
    
    public class GlobalDataSet
    {
        private long timerValue;
        private bool debugMode;
        private Stopwatch timer_programExecution = new Stopwatch();
        private bool showProgramDuration;
        private bool abortServerOperation = false;
        private int MAX_MOTORS = 4;
        private int MAX_DATAPACKAGE_ELEMENT = 8;

        private byte[,] currentRecValues = new byte[4,8] { { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } };

        private RobotActions[] robotAction = { RobotActions.doNothing, RobotActions.doNothing, RobotActions.doNothing, RobotActions.doNothing };
        private RobotOptions robotOption = RobotOptions.nothingSelected;
        private RobotCompletetions robotCompletion = RobotCompletetions.incomplete;
        private ActionStates robotPending = ActionStates.init;
        private bool indicatorLed = false;
        private byte motorId = 0;
        private byte velocity = 0;

        // TEST
        private int sollAngleTest = 0;

        public enum RobotActions
        {
            doNothing,
            newPosition,
            saveToEeprom,
            disablePidController,
            enablePidController
        };

        public enum Incoming_Package_Content
        {
            action,
            motorId,
            actionState
        };

        public enum Outgoing_Package_Content
        {
            action,
            motorId,
            velocity,
            angle_1,
            angle_2,
            motorDir
        };

        public enum RobotOptions
        {
            nothingSelected,
            disablePidController
        };

        public enum RobotCompletetions
        {
            incomplete,
            savedToEeprom,
            positionReached
        };

        public enum ActionStates
        {
            init,
            complete,
            pending
        };

        public int MAX_DATAPACKAGE_ELEMENTS
        {
            get
            {
                return MAX_DATAPACKAGE_ELEMENT;
            }

            set
            {
                MAX_DATAPACKAGE_ELEMENT = value;
            }
        }

        public bool DebugMode
        {
            get
            {
                return debugMode;
            }

            set
            {
                debugMode = value;
            }
        }

        public Stopwatch Timer_programExecution
        {
            get
            {
                return timer_programExecution;
            }

            set
            {
                timer_programExecution = value;
            }
        }

        public long TimerValue
        {
            get
            {
                return timerValue;
            }

            set
            {
                timerValue = value;
            }
        }

        internal bool ShowProgramDuration
        {
            get
            {
                return showProgramDuration;
            }

            set
            {
                showProgramDuration = value;
            }
        }

        public RobotActions[] Action
        {
            get
            {
                return robotAction;
            }

            set
            {
                robotAction = value;
            }
        }

        public RobotOptions Mode
        {
            get
            {
                return robotOption;
            }

            set
            {
                robotOption = value;
            }
        }

        public bool AbortServerOperation
        {
            get
            {
                return abortServerOperation;
            }

            set
            {
                abortServerOperation = value;
            }
        }

        public byte[,] DataPackage_In
        {
            get
            {
                return currentRecValues;
            }

            set
            {
                currentRecValues = value;
            }
        }

        public RobotCompletetions Robot_Completion
        {
            get
            {
                return robotCompletion;
            }

            set
            {
                robotCompletion = value;
            }
        }

        public int SollAngleTest
        {
            get
            {
                return sollAngleTest;
            }

            set
            {
                sollAngleTest = value;
            }
        }

        public ActionStates PendingAction
        {
            get
            {
                return robotPending;
            }

            set
            {
                robotPending = value;
            }
        }

        public bool IndicatorLed
        {
            get
            {
                return indicatorLed;
            }

            set
            {
                indicatorLed = value;
            }
        }

        public int MAX_MOTOR_AMOUNT
        {
            get
            {
                return MAX_MOTORS;
            }

            set
            {
                MAX_MOTORS = value;
            }
        }

        public byte MotorId
        {
            get
            {
                return motorId;
            }

            set
            {
                motorId = value;
            }
        }

        public byte Velocity
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
    }
}

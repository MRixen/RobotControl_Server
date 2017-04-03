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

        private byte[] currentRecValues = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        private byte[] DEFAULT_DATA_PACKAGE = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        private RobotActions robotAction = RobotActions.doNothing;
        private RobotOptions robotOption = RobotOptions.nothingSelected;
        private RobotCompletetions robotCompletion = RobotCompletetions.incomplete;
        private ActionStates robotPending = ActionStates.init;
        private bool indicatorLed = false;
        private int MAX_MOTORS = 4;
        private int MAX_TABLE_ELEMENT = 4;

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

        public byte[] DEFAULT_DATAPACKAGE
        {
            get
            {
                return DEFAULT_DATA_PACKAGE;
            }

            set
            {
                DEFAULT_DATA_PACKAGE = value;
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

        public RobotActions Action
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

        public byte[] DataPackage_In
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

        public int MAX_TABLE_ELEMENTS
        {
            get
            {
                return MAX_TABLE_ELEMENT;
            }

            set
            {
                MAX_TABLE_ELEMENT = value;
            }
        }
    }
}

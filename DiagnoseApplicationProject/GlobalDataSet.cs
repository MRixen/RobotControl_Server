using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Packager;
using System.Windows.Forms;

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
        private const int MAX_MOTORS = 4;
        private const int MAX_DATAPACKAGE_ELEMENT = 8;
        private const int MAX_TABLE_ENTRY = 5;

        private byte[][] currentRecValues = new byte[MAX_MOTORS][];
        private int[] controlDataIncrement = new int[10];
        private RobotActions[] robotAction = { RobotActions.doNothing, RobotActions.doNothing, RobotActions.doNothing, RobotActions.doNothing };
        private RobotOptions robotOption = RobotOptions.nothingSelected;
        private RobotCompletetions robotCompletion = RobotCompletetions.incomplete;
        private ActionStates robotPending = ActionStates.init;
        private bool[] indicatorLed = { false, false, false, false };
        private byte motorId = 0;
        private int motorSollVelocity = 0;
        private int motorSollAngle = 0;
        private bool autoModeIsActive = false;

        public GlobalDataSet()
        {
            // Init data array            
            for (int i = 0; i < MAX_MOTORS; i++)
            {
                currentRecValues[i] = new byte[8];
                for (int j = 0; j < MAX_DATAPACKAGE_ELEMENT; j++) currentRecValues[i][j] = 0;
            }
        }

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

        public byte[][] DataPackage_In
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

        public bool[] IndicatorLed
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

        public int MotorSollVelocity
        {
            get
            {
                return motorSollVelocity;
            }

            set
            {
                motorSollVelocity = value;
            }
        }

        public int MotorSollAngle
        {
            get
            {
                return motorSollAngle;
            }

            set
            {
                motorSollAngle = value;
            }
        }

        public bool AutoModeIsActive
        {
            get
            {
                return autoModeIsActive;
            }

            set
            {
                autoModeIsActive = value;
            }
        }

        public int MAX_TABLE_ENTRIES
        {
            get
            {
                return MAX_TABLE_ENTRY;
            }
        }

        public int[] ControlDataIncrement
        {
            get
            {
                return controlDataIncrement;
            }

            set
            {
                controlDataIncrement = value;
            }
        }
    }
}

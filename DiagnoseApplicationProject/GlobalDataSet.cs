using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Packager;
using System.Windows.Forms;
using FormRobotControlServer;
using RobotControlServer;

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
        private bool abortActionSelector = false;
        private const int MAX_MOTORS = 6;
        private const int MAX_DATAPACKAGE_ELEMENT = 4;
        private const int MAX_TABLE_ENTRY = 5;

        private byte[][] currentRecValues = new byte[MAX_MOTORS][];
        private byte[] currentRecValuesTest = new byte[8];
        /// controlDataMaxRow includes the maximum number of rows of motor table (for all motors)
        private int[] controlDataMaxRow = new int[MAX_MOTORS];

        /// controlDataRowCounter includes the current counter for a row of motor table (for all motors)
        private int[] controlDataRowCounter = new int[MAX_MOTORS]; 
        private RobotActions[] robotAction = { RobotActions.doNothing, RobotActions.doNothing, RobotActions.doNothing, RobotActions.doNothing };
        private RobotOptions robotOption = RobotOptions.nothingSelected;
        private RobotCompletetions robotCompletion = RobotCompletetions.incomplete;
        private ActionStates robotPending = ActionStates.init;
        private bool[] indicatorLed = new bool[MAX_MOTORS];
        private int motorId = 1;
        private int motorSollVelocity = 0;
        private int motorSollAngle = 0;
        private bool autoModeIsActive = false;
        private Motor[] motor = new Motor[MAX_MOTORS];
        private bool stepForward;

        public GlobalDataSet()
        {
            // Init data arrays           
            for (int i = 0; i < MAX_MOTORS; i++)
            {
                currentRecValues[i] = new byte[8];
                for (int j = 0; j < MAX_DATAPACKAGE_ELEMENT; j++) currentRecValues[i][j] = 0;
                indicatorLed[i] = false;
            }

            // Init control data counter array 
            for (int i = 0; i < MAX_MOTORS; i++) controlDataRowCounter[i] = 0;

            for (int i = 0; i < MAX_MOTORS; i++) motor[i] = new Motor();
        }

        public enum RobotActions
        {
            doNothing,
            newPosition,
            saveRefPosToEeprom,
            disablePidController,
            enablePidController,
            saveActPosToEeprom,
            actionIsSet
        };

        public enum Incoming_Package_Content
        {
            action,
            motorId,
            actionState
        };

        //public enum Outgoing_Package_Content
        //{
        //    action,
        //    motorId,
        //    velocity,
        //    angle_1,
        //    angle_2,
        //    motorDir
        //};

            // UPDATE
        public enum Outgoing_Package_Content
        {
            action,
            angle,
            velocity,
            motorDir,
            motorId
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

                public byte[] DataPackage_In_Test
        {
            get
            {
                return currentRecValuesTest;
            }

            set
            {
                currentRecValuesTest = value;
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

        public int MotorId
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

        public int[] ControlDataMaxRows
        {
            get
            {
                return controlDataMaxRow;
            }

            set
            {
                controlDataMaxRow = value;
            }
        }

        public bool AbortActionSelector
        {
            get
            {
                return abortActionSelector;
            }

            set
            {
                abortActionSelector = value;
            }
        }

        public int[] ControlDataRowCounter
        {
            get
            {
                return controlDataRowCounter;
            }

            set
            {
                controlDataRowCounter = value;
            }
        }

        public Motor[] Motor
        {
            get
            {
                return motor;
            }

            set
            {
                motor = value;
            }
        }

        public bool StepForward
        {
            get
            {
                return stepForward;
            }

            set
            {
                stepForward = value;
            }
        }
    }
}

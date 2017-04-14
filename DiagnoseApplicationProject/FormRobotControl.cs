using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MySql.Data.MySqlClient;
using Packager;
using Networking;
using RobotControlServer;
using System.Threading;

namespace FormRobotControlServer
{
    ///\brief HMI to control and parametrize the robot.

    public partial class FormRobotControl : Form
    {
        private int sampleStep;
        private float SAMPLE_TIME = Properties.Settings.Default.SAMPLE_TIME;
        private int DEFAULT_SAMPLE_TIME_FACTOR = Properties.Settings.Default.DEFAULT_SAMPLE_TIME_FACTOR;
        private int MAX_ALIVE_SIGNAL_PAUSE = Properties.Settings.Default.MAX_ALIVE_SIGNAL_PAUSE;
        private int sampleTimeFactor;

        private bool notExecuted;
        private HelperFunctions helperFunctions;
        private bool aliveBit;
        private int aliveTimer;
        private Stopwatch aliveStopWatch = new Stopwatch();

        private GlobalDataSet globalDataSet;
        private DataPackager dataPackager;
        private ServerUnit tcpServer = null;
        private Thread indicatorLedThread;

        Stopwatch timer_timeStamp = new Stopwatch();

        public FormRobotControl()
        {
            InitializeComponent();

            // Definition
            globalDataSet = new GlobalDataSet();
            helperFunctions = new HelperFunctions(globalDataSet);
            dataPackager = new DataPackager(globalDataSet);
            tcpServer = new ServerUnit(globalDataSet);

            // Initialize
            sampleStep = DEFAULT_SAMPLE_TIME_FACTOR;
            aliveBit = false;
            notExecuted = true;

            // User defines
            globalDataSet.DebugMode = false;
            globalDataSet.ShowProgramDuration = false;

            // Set event that is fired by datapackager named newPackageEvent with method dataPackageReceived (listener: server)
            // Set event that is fired by server named newServerEvent with method dataPackageServerReceived (listener: dataPackager)
            dataPackager.newPackageEvent += new DataPackager.DataPackagedEventHandler(tcpServer.dataPackageReceived);

            // Start server
            tcpServer.startServer();

            // Start thread to package data (the data were packaged before the client is connected
            dataPackager.startPackaging();

            // Check sensor alive in background
            indicatorLedThread = new Thread(new ThreadStart(indicatorLed));
            indicatorLedThread.Start();

            ActionSelector actionSelector = new ActionSelector(globalDataSet);
        }

        private void FormDatabase_Load(object sender, EventArgs e)
        {

        }

        private void FormDatabase_Closed(object sender, FormClosedEventArgs e)
        {

        }

        private void FormDatabase_Closing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.ApplicationExitCall)
            {
                // Stop alive state checker
                if (bWorker_IndicatorLed.IsBusy) bWorker_IndicatorLed.CancelAsync();

                // Stop timer for measuring the program execution
                if (globalDataSet.ShowProgramDuration) globalDataSet.Timer_programExecution.Stop();

                // Stop running the server
                tcpServer.stopServer();

                // Close WinForms app
                if (Application.MessageLoop) Application.Exit();
                // Close Console app
                else Environment.Exit(1);
            }
            else { }
        }

        private void indicatorLed()
        {
            Label[] label_aliveIcons = { label_aliveIcon_1, label_aliveIcon_2 };

            while (true)
            {
                for (int ledCounter = 0; ledCounter < 2; ledCounter++)
                {
                    int counter = ledCounter;
                    if (globalDataSet.IndicatorLed[ledCounter])
                    {
                        if (IsHandleCreated)
                        {
                            // Show green icon in gui
                            label_aliveIcons[counter].BeginInvoke((MethodInvoker)delegate () { label_aliveIcons[counter].BackColor = Color.LightGreen; });
                        }
                    }
                    if (!globalDataSet.IndicatorLed[ledCounter])
                    {
                        if (IsHandleCreated)
                        {
                            // Show red icon in gui
                            label_aliveIcons[counter].BeginInvoke((MethodInvoker)delegate () { label_aliveIcons[counter].BackColor = Color.Red; });
                        }
                    }
                    Thread.Sleep(20);
                }
                if (IsHandleCreated)
                {
                    // Change label for step forward 
                    if(globalDataSet.StepForward) label_stepForward.BeginInvoke((MethodInvoker)delegate () { label_stepForward.BackColor = Color.Red; });
                    else label_stepForward.BeginInvoke((MethodInvoker)delegate () { label_stepForward.BackColor = Color.LightGreen; });
                }
            }
        }

        private void buttonSaveRefPos_Click(object sender, EventArgs e)
        {
            if ((textBox_motorId.Text.Length != 0) & (!globalDataSet.AutoModeIsActive))
            {
                globalDataSet.Motor[Int32.Parse(textBox_motorId.Text)].Id = Int32.Parse(textBox_motorId.Text);
                globalDataSet.Action[globalDataSet.MotorId] = GlobalDataSet.RobotActions.saveToEeprom;
            }
            else if (checkBox_all_motors.Checked & (!globalDataSet.AutoModeIsActive))
            {
                for (int i = 0; i < globalDataSet.MAX_MOTOR_AMOUNT; i++)
                {
                    globalDataSet.Motor[i].Id = i + 1;
                    globalDataSet.Motor[i].Action = GlobalDataSet.RobotActions.saveToEeprom;
                }
            }
        }

        private void button_MoveTo_Clicked(object sender, EventArgs e)
        {
            if ((textBox_soll_angle.Text.Length != 0) & (textBox_motorId.Text.Length != 0) & (!globalDataSet.AutoModeIsActive) & !checkBox_all_motors.Checked)
            {
                globalDataSet.Motor[Int32.Parse(textBox_motorId.Text) - 1].Id = Int32.Parse(textBox_motorId.Text);
                globalDataSet.Motor[Int32.Parse(textBox_motorId.Text) - 1].Angle = Int32.Parse(textBox_soll_angle.Text);
                globalDataSet.Motor[Int32.Parse(textBox_motorId.Text) - 1].Action = GlobalDataSet.RobotActions.newPosition;
                //Debug.WriteLine("globalDataSet.Motor[0].Id: " + globalDataSet.Motor[0].Id);
                //Debug.WriteLine("globalDataSet.Motor[0].Angle: " + globalDataSet.Motor[0].Angle);
                //Debug.WriteLine("globalDataSet.Motor[0].Action: " + globalDataSet.Motor[0].Action);
            }
            else if (checkBox_all_motors.Checked & (textBox_soll_angle.Text.Length != 0) & (!globalDataSet.AutoModeIsActive))
            {
                for (int i = 0; i < globalDataSet.MAX_MOTOR_AMOUNT; i++)
                {
                    globalDataSet.Motor[i].Id = i + 1;
                    globalDataSet.Motor[i].Angle = Int32.Parse(textBox_soll_angle.Text);
                    globalDataSet.Motor[i].Action = GlobalDataSet.RobotActions.newPosition;
                }
            }
        }

        private void checkChanged_disablePidController(object sender, EventArgs e)
        {
            if (textBox_motorId.Text.Length != 0)
            {
                globalDataSet.Motor[Int32.Parse(textBox_motorId.Text) - 1].Id = Int32.Parse(textBox_motorId.Text);

                if (checkBox_disablePidController.Checked) globalDataSet.Motor[Int32.Parse(textBox_motorId.Text) - 1].Action = GlobalDataSet.RobotActions.disablePidController;
                else globalDataSet.Motor[Int32.Parse(textBox_motorId.Text) - 1].Action = GlobalDataSet.RobotActions.enablePidController;
            }
            else if (checkBox_all_motors.Checked & (!globalDataSet.AutoModeIsActive))
            {
                for (int i = 0; i < globalDataSet.MAX_MOTOR_AMOUNT; i++)
                {
                    globalDataSet.Motor[i].Id = i + 1;
                    if (checkBox_disablePidController.Checked) globalDataSet.Motor[i].Action = GlobalDataSet.RobotActions.disablePidController;
                    else globalDataSet.Motor[i].Action = GlobalDataSet.RobotActions.enablePidController;
                }
            }
        }

        private void button_Stopp_Click(object sender, EventArgs e)
        {
            if ((textBox_motorId.Text.Length != 0) & (!globalDataSet.AutoModeIsActive))
            {

                globalDataSet.Motor[Int32.Parse(textBox_motorId.Text)].Id = Int32.Parse(textBox_motorId.Text);
                globalDataSet.Motor[Int32.Parse(textBox_motorId.Text)].Action = GlobalDataSet.RobotActions.doNothing;
            }
            else if (checkBox_all_motors.Checked & (!globalDataSet.AutoModeIsActive))
            {
                for (int i = 0; i < globalDataSet.MAX_MOTOR_AMOUNT; i++)
                {
                    globalDataSet.Motor[i].Id = i + 1;
                    globalDataSet.Motor[i].Action = GlobalDataSet.RobotActions.doNothing;
                }
            }
        }

        private void checkChanged_autoMode(object sender, EventArgs e)
        {
            if (checkBox_AutoMode.Checked) globalDataSet.AutoModeIsActive = true;
            else globalDataSet.AutoModeIsActive = false;
        }

        private void button_stepForward_Click(object sender, EventArgs e)
        {
            globalDataSet.StepForward = true;
        }
    }
}


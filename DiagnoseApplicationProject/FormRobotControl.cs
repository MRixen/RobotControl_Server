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
            bWorker_IndicatorLed.DoWork += new DoWorkEventHandler(bWorker_IndicatorLed_DoWork);

            // User defines
            globalDataSet.DebugMode = false;
            globalDataSet.ShowProgramDuration = false;

            // Set event that is fired by datapackager named newPackageEvent with method dataPackageReceived (listener: server)
            // Set event that is fired by server named newServerEvent with method dataPackageServerReceived (listener: dataPackager)
            //dataPackager.newPackageEvent += new DataPackager.DataPackagedEventHandler(tcpServer.dataPackageReceived);

            // Start server
            //tcpServer.startServer();

            // Start thread to package data (the data were packaged before the client is connected
            //dataPackager.startPackaging();

            // Check sensor alive in background
            //bWorker_IndicatorLed.RunWorkerAsync();

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

        private void bWorker_IndicatorLed_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            bool[] iconLock_green = { false, false, false, false };
            bool[] iconLock_red = { false, false, false, false };
            Label[] label_aliveIcons = { label_aliveIcon_1, label_aliveIcon_2, label_aliveIcon_3, label_aliveIcon_4 };


            while (true)
            {
                if (globalDataSet.IndicatorLed[globalDataSet.MotorId] & !iconLock_green[globalDataSet.MotorId])
                {
                    // Show green icon in gui
                    iconLock_green[globalDataSet.MotorId] = true;
                    iconLock_red[globalDataSet.MotorId] = false;
                    label_aliveIcons[globalDataSet.MotorId].BeginInvoke((MethodInvoker)delegate () { label_aliveIcons[globalDataSet.MotorId].BackColor = Color.LightGreen; });

                }
                if (!globalDataSet.IndicatorLed[globalDataSet.MotorId] & !iconLock_red[globalDataSet.MotorId])
                {
                    // Show red icon in gui
                    iconLock_red[globalDataSet.MotorId] = true;
                    iconLock_green[globalDataSet.MotorId] = false;
                    label_aliveIcons[globalDataSet.MotorId].BeginInvoke((MethodInvoker)delegate () { label_aliveIcons[globalDataSet.MotorId].BackColor = Color.Red; });
                }
            }
        }

        private void buttonSaveRefPos_Click(object sender, EventArgs e)
        {
            if (textBox_motorId.Text.Length != 0)
            {
                globalDataSet.MotorId = (byte)Int32.Parse(textBox_motorId.Text);
                globalDataSet.Action[globalDataSet.MotorId] = GlobalDataSet.RobotActions.saveToEeprom;
            }
        }

        private void button_MoveTo_Clicked(object sender, EventArgs e)
        {
            if ((textBox_soll_angle.Text.Length != 0) & (textBox_motorId.Text.Length != 0))
            {
                globalDataSet.MotorId = (byte)Int32.Parse(textBox_motorId.Text);
                globalDataSet.SollAngleTest = Int32.Parse(textBox_soll_angle.Text);
                globalDataSet.Action[globalDataSet.MotorId] = GlobalDataSet.RobotActions.newPosition;
            }
        }

        private void checkChanged_disablePidController(object sender, EventArgs e)
        {
            if (textBox_motorId.Text.Length != 0)
            {
                globalDataSet.MotorId = (byte)Int32.Parse(textBox_motorId.Text);
                if (checkBox_disablePidController.Checked) globalDataSet.Action[globalDataSet.MotorId] = GlobalDataSet.RobotActions.disablePidController;
                else globalDataSet.Action[globalDataSet.MotorId] = GlobalDataSet.RobotActions.enablePidController;
            }
        }

        private void button_Stopp_Click(object sender, EventArgs e)
        {
            if (textBox_motorId.Text.Length != 0)
            {
                globalDataSet.MotorId = (byte)Int32.Parse(textBox_motorId.Text);
                globalDataSet.Action[globalDataSet.MotorId] = GlobalDataSet.RobotActions.doNothing;
            }
        }
    }
}


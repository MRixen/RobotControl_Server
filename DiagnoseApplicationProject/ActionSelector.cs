using MySql.Data.MySqlClient;
using Packager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace RobotControlServer
{
    ///\brief Upper control algorithm. Select an action in dependent of outer influences.

    /// This class handle the relation between outer influences such as balance problematic pulses, vision navigation, etc. and the control variable.
    /// It has a autonomous functionality to control the robot in an environment.
    /// At the beginning (after booting up) the control data were loaded from remote server (sql database) and were stored locally.

    class ActionSelector
    {
        private string[] dbName = { "stepforward" };
        private string localDbDescription = FormRobotControlServer.Properties.Settings.Default.ConnectionString_DataBase;
        private string conString = String.Empty;
        private int MAX_MOTOR_ID = FormRobotControlServer.Properties.Settings.Default.MAX_TABLE_AMOUNT; // Amount of tables inside specific database (s0, s1, s2, etc.)
        DataSet dataSetLocal = new DataSet();
        private LocalDatabaseManager localDatabaseManager;

        private Thread loadDatabaseThread;
        private GlobalDataSet globalDataSet;
        private long duration = 0;
        private const int SAMPLE_TIME = 10; // In milliseconds
        private Timer controlTimer;
        private bool resetDurationCounter = true;
        private int resetValue = 0;

        /// Constructor of the ActionSelector class
        public ActionSelector(GlobalDataSet globalDataSet)
        {
            this.globalDataSet = globalDataSet;

            Task loadDatabaseTask = Task.Factory.StartNew(() => updateLocalDatabase());
            loadDatabaseTask.Wait();

            // Start robot control as a timer
            controlTimer = new Timer(this.controlRobot, null, 0, SAMPLE_TIME);

            //loadDatabaseThread = new Thread(controlRobot);
            //loadDatabaseThread.Start();
        }


        ///\brief Start controlling the robot.

        /// In every cycle:
        /// Check incoming dta (vision, stand up stability, etc.).
        /// Set control data for one motor so that the packager can handle it.
        private void controlRobot(object state)
        {
            // Data array for motor angle and motor velocity
            // Byte 1: motor soll angle
            // Byte 2: motor velocity
            int[] controlData = new int[3];

            //while (!globalDataSet.AbortActionSelector)
            //{
            // Single step forward
            if (globalDataSet.AutoModeIsActive)
            {
                // Here we do the following:
                // 1. Set motor id
                // 2. Get a row from motor table (for specific movement context, i.e. step forward, step backward, etc.)
                // 3. Check if current duration is equal to time value in the row of the motor table
                //   3.1 This time value specifies the moment of a turning point
                // 4. Check if current motor is in state <init>
                //   4.1 If motor is in state <init> set the control data and increment the row counter        
                //   4.2 If motor is not in state <init> set no control data --> TODO: Maybe we need to change this behaviour
                // 5. Increment the duration value 
                for (int motorCounter = 0; motorCounter < globalDataSet.MAX_MOTOR_AMOUNT; motorCounter++)
                {
                    //Debug.WriteLine(motorCounter);

                    // Get control data for specific motor (angle, velocity, time)
                    controlData = getControlData(motorCounter, globalDataSet.ControlDataRowCounter[motorCounter]);

                    // Check if actual sampletime equal to time value in actual row of specific motor table
                    // We need to multiplicate the time value because the smallest value inside database is 1
                    // For example: 1 (value in database table) * 10 = 10ms (smallest time value)
                    if (duration == (controlData[2])* 10)
                    {
                        // Set control data for specific motor when:
                        //  - Current action for specific motor is <doNothing>
                        //  - Incoming action state for specific motor is <init>
                        // NOTE: This will control the motor when the state is as init only!
                        if (((int)globalDataSet.Action[motorCounter] == (int)GlobalDataSet.RobotActions.doNothing) & (globalDataSet.DataPackage_In[motorCounter][(int)GlobalDataSet.Incoming_Package_Content.actionState] == (int)GlobalDataSet.ActionStates.init))
                        {
                            // Set motor id
                            globalDataSet.MotorId = motorCounter;

                            // Get endposition from local db and set it to global data
                            globalDataSet.MotorSollAngle = controlData[0];

                            // Set velocity
                            globalDataSet.MotorSollVelocity = controlData[1];

                            // Set task number for new position
                            globalDataSet.Action[motorCounter] = GlobalDataSet.RobotActions.newPosition;

                            // Increment the counter for row in a motor table to get the next control value (angle, velocity)
                            // If last row is reached reset the counter to zero
                            if (globalDataSet.ControlDataRowCounter[motorCounter] != globalDataSet.ControlDataMaxRows[motorCounter]) globalDataSet.ControlDataRowCounter[motorCounter]++;

                    Debug.WriteLine(motorCounter + ";" + controlData[0] + ";" + controlData[1] + ";" + globalDataSet.Action[motorCounter] + ";" + globalDataSet.ControlDataRowCounter[motorCounter] + ";" + duration);
                            //Thread.Sleep(20);
                        }
                    }
                }

                resetValue = 0;
                resetDurationCounter = true;

                for (int i = 0; i < globalDataSet.ControlDataRowCounter.Length-resetValue; i++)
                {
                    if (globalDataSet.ControlDataRowCounter[i] < globalDataSet.ControlDataMaxRows[i])
                    {
                        resetDurationCounter = false;
                        //resetValue = globalDataSet.ControlDataRowCounter.Length;
                    }
                    else  globalDataSet.ControlDataRowCounter[i] = 0;
                }
                if (resetDurationCounter) duration = 0;
                else duration = duration + SAMPLE_TIME;
                //TODO: Array of duration values for every motor
                // Motor class with parameters (getter, setter)
            }
            //}
        }

        ///\brief Getting control data from local database.

        /// Gets an specific value (motor angle, velocity, etc.) from local database.
        public int[] getControlData(int motorId, int increment)
        {
            DataRow dataRowTemp;
            int[] retVal = new int[3];

            // Create dataset fromlocal db
            dataSetLocal = localDatabaseManager.createDatasetsForDb(localDbDescription);

            // Get row from local db
            dataRowTemp = dataSetLocal.Tables[motorId].Rows[increment];

            // Get soll angle and velocity from row
            for (int i = 0; i < retVal.Length; i++) retVal[i] = (int)dataRowTemp.ItemArray.GetValue(i + 1);

            return retVal;
        }

        ///\brief Load control data from remote sql database.

        /// Create connection to remote sql database.
        /// Copy content of remote database to local database.
        private void updateLocalDatabase()
        {
            // Todo: Check against local db content if there are some changes

            localDatabaseManager = new LocalDatabaseManager();
            DataSet dataSetRemote = new DataSet();
            DataRow dataRowRemote, dataRowLocal;

            // Delete old db content 
            localDatabaseManager.deleteDatabaseContent(localDbDescription);

            // Reset identifier to start at 1
            localDatabaseManager.resetId(localDbDescription);

            // Create datasets
            dataSetLocal = localDatabaseManager.createDatasetsForDb(localDbDescription);

            // Copy content of remote db to local db
            // Iterate through remote db for every control context, i.e. db name (moveforward, step forward, etc.)
            for (int dbNameCounter = 0; dbNameCounter < dbName.Length; dbNameCounter++)
            {
                // Open specific table in remote db
                conString = "Server=127.0.0.1;Database=" + dbName[dbNameCounter] + "; Uid=root;Pwd=rbc;";
                MySqlConnection connection = new MySqlConnection(conString);
                connection.Open();
                try
                {
                    MySqlCommand cmd = connection.CreateCommand();
                    // Iterate through remote db for every motorId, i.e. s0, s1, etc.
                    for (int motorId = 1; motorId <= globalDataSet.MAX_MOTOR_AMOUNT; motorId++)
                    {
                        // Get content of specific table in remote db
                        cmd.CommandText = "SELECT * FROM m" + motorId;
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        adapter.Fill(dataSetRemote, "m" + motorId);

                        // Load dataset to local db
                        try
                        {
                            // Get max rows from current table in remote database
                            int maxTableRow = dataSetRemote.Tables[motorId - 1].Rows.Count;

                            // Copy row by row to specific table in local db
                            for (int rowCounter = 0; rowCounter < maxTableRow; rowCounter++)
                            {
                                // Get a row from specific table of remote db
                                dataRowRemote = dataSetRemote.Tables[motorId - 1].Rows[rowCounter];

                                // Create new row in specific table of local db
                                dataRowLocal = dataSetLocal.Tables[motorId - 1].NewRow();

                                // Copy every item of the remote table row to local table row
                                for (int itemCounter = 0; itemCounter < dataSetRemote.Tables[motorId - 1].Columns.Count; itemCounter++) dataRowLocal[itemCounter] = (int)dataRowRemote.ItemArray.GetValue(itemCounter);

                                // Set new row to table of local db
                                dataSetLocal.Tables[motorId - 1].Rows.Add(dataRowLocal);
                            }

                            // Upodate local db
                            localDatabaseManager.UpdateDatabase(dataSetLocal, localDbDescription);

                            // Save amount of rows for each table
                            globalDataSet.ControlDataMaxRows[motorId - 1] = maxTableRow;
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show(err.Message);
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Exception in updateLocalDatabase");
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }
    }
}

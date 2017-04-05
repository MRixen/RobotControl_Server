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
        private string[] dbName = { "moveforward" };
        private string conString = String.Empty;
        private int MAX_MOTOR_ID = FormRobotControlServer.Properties.Settings.Default.MAX_TABLE_AMOUNT; // Amount of tables inside specific database (s0, s1, s2, etc.)
        DataSet dataSetLocal = new DataSet();
        private DatabaseConnection databaseConnection;

        private Thread loadDatabaseThread;
        private GlobalDataSet globalDataSet;

        /// Constructor of the ActionSelector class
        public ActionSelector(GlobalDataSet globalDataSet)
        {
            this.globalDataSet = globalDataSet;

            Task loadDatabaseTask = Task.Factory.StartNew(() => updateLocalDatabase());
            loadDatabaseTask.Wait();

            loadDatabaseThread = new Thread(controlRobot);
            loadDatabaseThread.Start();
        }


        ///\brief Start controlling the robot.

        /// In every cycle:
        /// Check incoming dta (vision, stand up stability, etc.).
        /// Set control data for one motor so that the packager can handle it.
        private void controlRobot()
        {
            // Data array for motor angle and motor velocity
            // Byte 1: motor soll angle
            // Byte 2: motor velocity
            int[] controlData = new int[2];

            while (!globalDataSet.AbortServerOperation)
            {
                // Single step forward
                //if (globalDataSet.StartControlling)
                if (globalDataSet.AutoModeIsActive)
                {
                    // Set motor id
                    globalDataSet.MotorId = 0;

                    // Get control data for specific motor
                    controlData = getControlData(globalDataSet.MotorId);

                    // Get endposition from local db and set it to global data
                    globalDataSet.MotorSollAngle = (byte)controlData[0];

                    // Set velocity
                    globalDataSet.Velocity = (byte)controlData[1];

                    // Set task number for new position
                    globalDataSet.Action[globalDataSet.MotorId] = GlobalDataSet.RobotActions.newPosition;
                }
            }

        }

        ///\brief Getting control data from local database.

        /// Gets an specific value (motor angle, velocity, etc.) from local database.
        public int[] getControlData(int motorId)
        {
            DataRow dataRowTemp;
            int[] retVal = new int[2];

            // Create dataset fromlocal db
            dataSetLocal = databaseConnection.createDatasetsForDb(FormRobotControlServer.Properties.Settings.Default.ConnectionString_DataBase);

            // Get row from local db
            dataRowTemp = dataSetLocal.Tables[motorId].Rows[0];

            // Get soll angle and velocity from row
            for (int i = 1; i < 3; i++) retVal[i] = (int)dataRowTemp.ItemArray.GetValue(i);

            return retVal;
        }

        ///\brief Load control data from remote sql database.

        /// Create connection to remote sql database.
        /// Copy content of remote database to local database.
        private void updateLocalDatabase()
        {
            // Todo: Check against local db content if there are some changes

            databaseConnection = new DatabaseConnection();
            DataSet dataSetRemote = new DataSet();
            DataRow dataRowRemote, dataRowLocal;

            // Delete old db content
            databaseConnection.deleteDatabaseContent(FormRobotControlServer.Properties.Settings.Default.ConnectionString_DataBase);

            // Create datasets
            dataSetLocal = databaseConnection.createDatasetsForDb(FormRobotControlServer.Properties.Settings.Default.ConnectionString_DataBase);

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
                    for (int motorId = 0; motorId < globalDataSet.MAX_MOTOR_AMOUNT; motorId++)
                    {
                        // Get content of specific table in remote db
                        cmd.CommandText = "SELECT * FROM s" + motorId;
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        adapter.Fill(dataSetRemote, "tbl_rl_j" + motorId);

                        // Load dataset to local db
                        try
                        {
                            // Get max rows from current table in remote database
                            int maxTableRow = dataSetRemote.Tables[motorId].Rows.Count;

                            // Copy row by row to specific table in local db
                            for (int rowCounter = 0; rowCounter < maxTableRow; rowCounter++)
                            {
                                // Get a row from specific table of remote db
                                dataRowRemote = dataSetRemote.Tables[motorId].Rows[rowCounter];

                                // Create new row in specific table of local db
                                dataRowLocal = dataSetLocal.Tables[motorId].NewRow();

                                // Copy every item of the remote table row to local table row
                                for (int itemCounter = 0; itemCounter < globalDataSet.MAX_MOTOR_AMOUNT; itemCounter++) dataRowLocal[itemCounter + 1] = (int)dataRowRemote.ItemArray.GetValue(itemCounter);

                                // Set new row to table of local db
                                dataSetLocal.Tables[motorId].Rows.Add(dataRowLocal);
                            }

                            // Upodate local db
                            databaseConnection.UpdateDatabase(dataSetLocal, motorId);
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

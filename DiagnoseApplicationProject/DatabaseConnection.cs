using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RobotControlServer
{
    ///\brief Handle the connection to local database.

    /// This class handle the connection to the local database.
    /// It is possible to delete or create dataset with the structure of remote database.
    /// Later it is possible to get content of the local copy.
    public class LocalDatabaseManager
    {
        private int DATABASE_SIZE = 4;
        private string strCon;
        SqlDataAdapter dataAdapter, dataAdapter1, dataAdapter2, dataAdapter3, dataAdapterX;
        private DataSet dataSet, dataSetX;
        private int[] maxTableRows;

        /// Constructor of the DatabaseConnection class
        public LocalDatabaseManager()
        {

        }

        ///\brief Update local database with new content.

        /// Update all tables inside the local database with a new dataset.
        public void UpdateDatabase(System.Data.DataSet dataSet)
        {
            try
            {
                System.Data.SqlClient.SqlCommandBuilder commandBuilder = new System.Data.SqlClient.SqlCommandBuilder(dataAdapter);
                commandBuilder.DataAdapter.Update(dataSet.Tables[0]);

                System.Data.SqlClient.SqlCommandBuilder commandBuilder1 = new System.Data.SqlClient.SqlCommandBuilder(dataAdapter1);
                commandBuilder1.DataAdapter.Update(dataSet.Tables[1]);

                System.Data.SqlClient.SqlCommandBuilder commandBuilder2 = new System.Data.SqlClient.SqlCommandBuilder(dataAdapter2);
                commandBuilder2.DataAdapter.Update(dataSet.Tables[2]);

                System.Data.SqlClient.SqlCommandBuilder commandBuilder3 = new System.Data.SqlClient.SqlCommandBuilder(dataAdapter3);
                commandBuilder3.DataAdapter.Update(dataSet.Tables[3]);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        ///\brief Create a dataset for local database.

        /// Create a dataset with structur of the remote database.
        /// In this dataset we copy the content of the remote database and set it to the local database.
        /// Later we can access some content of this local database.
        public DataSet createDatasetsForDb(string dBdescription)
        {
            // Create and open connection to specific database
            SqlConnection dataBase_connection = new SqlConnection(dBdescription);
            dataBase_connection.Open();

            // Copy content of database to dataset and close connection
            dataSet = new DataSet();
            dataAdapter = new SqlDataAdapter("SELECT * FROM tbl_rl_j0", dataBase_connection);
            dataAdapter.Fill(dataSet, "tbl_rl_j0");

            dataAdapter1 = new SqlDataAdapter("SELECT * FROM tbl_rl_j1", dataBase_connection);
            dataAdapter1.Fill(dataSet, "tbl_rl_j1");

            dataAdapter2 = new SqlDataAdapter("SELECT * FROM tbl_rl_j2", dataBase_connection);
            dataAdapter2.Fill(dataSet, "tbl_rl_j2");

            dataAdapter3 = new SqlDataAdapter("SELECT * FROM tbl_rl_j3", dataBase_connection);
            dataAdapter3.Fill(dataSet, "tbl_rl_j3");

            dataBase_connection.Close();

            return dataSet;
        }

        ///\brief Delete local database.

        /// Connect to local database
        /// Delete each row in dataset
        /// Update database with empty dataset
        public void deleteDatabaseContent(string dBdescription)
        {
            DataRow dataRowTemp;
            int maxTableRow;

            // Create dataset from local db
            DataSet dataSet = createDatasetsForDb(dBdescription);

            int maxTables = dataSet.Tables.Count;

            // Get row from local db
            for (int tableCounter = 0; tableCounter < maxTables; tableCounter++)
            {
                maxTableRow = dataSet.Tables[tableCounter].Rows.Count;

                for (int rowCounter = 0; rowCounter < maxTableRow; rowCounter++)
                {
                    dataSet.Tables[tableCounter].Rows[rowCounter].Delete();
                }
            }

            UpdateDatabase(dataSet);
        }

        ///\brief Gets the size of all tables in the local database.

        /// Gets the maximum amount of available rows in all tables of the local database / dataset.
        public int[] getTableSizeForDb(DataSet dataSet)
        {
            maxTableRows = new int[DATABASE_SIZE];
            for (int i = 0; i < DATABASE_SIZE; i++)
            {
                maxTableRows[i] = dataSet.Tables[i].Rows.Count;
            }
            return maxTableRows;
        }

        ///\brief Delete whole content of the local database.

        /// Deletes all data that is stored in the local database.
        public void deleteDatabaseContentQuery(string dBdescription)
        {
            SqlConnection dataBase_connection = new SqlConnection(dBdescription);
            int MAX_TABLE_AMOUNT = FormRobotControlServer.Properties.Settings.Default.MAX_TABLE_AMOUNT;
            if (dataBase_connection != null)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = dataBase_connection;

                for (int i = 0; i <= MAX_TABLE_AMOUNT; i++)
                {
                    cmd.CommandText = "DELETE tbl_rl_j" + i;
                    dataBase_connection.Open();
                    cmd.ExecuteNonQuery();
                    dataBase_connection.Close();
                }

            }
        }


        public void resetId(string dBdescription)
        {
            SqlConnection dataBase_connection = new SqlConnection(dBdescription);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = dataBase_connection;

            dataBase_connection.Open();
            for (int i = 0; i < 4; i++)
            {
                cmd.CommandText = "DBCC CHECKIDENT ('tbl_rl_j" + i + "', RESEED, 0) ";
                cmd.ExecuteNonQuery();
            }
            dataBase_connection.Close();
        }


        ///\brief Delete specific table of the local database.

        /// Deletes a specific table that is stored in the local database.
        public void deleteDatabaseContentQuery(string dBdescription, int tableId)
        {
            SqlConnection dataBase_connection = new System.Data.SqlClient.SqlConnection(dBdescription);
            int MAX_TABLE_AMOUNT = FormRobotControlServer.Properties.Settings.Default.MAX_TABLE_AMOUNT;
            if (dataBase_connection != null)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = dataBase_connection;

                cmd.CommandText = "DELETE tbl_rl_j" + tableId;
                dataBase_connection.Open();
                cmd.ExecuteNonQuery();
                dataBase_connection.Close();
            }
        }

    }

}

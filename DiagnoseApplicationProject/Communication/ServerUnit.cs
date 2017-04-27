using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Packager;

namespace Networking
{
    ///\brief Server implementation.

    class ServerUnit
    {
        public delegate void ServerEventHandler(byte[][] dataPackageServer);
        public event ServerEventHandler newServerEvent;

        private TcpListener serverSocket_send = new TcpListener(4001);
        private TcpListener serverSocket_receive = new TcpListener(4002);
        private TcpClient clientSocket_send, clientSocket_receive;
        private Thread serverThread_send, serverThread_receive;
        private byte[][] dataPackage;
        private int DELAY_TIME_THREAD_SENDER = 40;
        private int DELAY_TIME_THREAD_RECEIVER = 10;
        private bool newDataFromPackager = false;
        private int motorCounter = 0;

        private bool workOnTask = false;

        private GlobalDataSet globalDataSet;


        public ServerUnit(GlobalDataSet globalDataSet)
        {
            this.globalDataSet = globalDataSet;
            dataPackage = new byte[this.globalDataSet.MAX_MOTOR_AMOUNT][];
            //// Set event for new available packaged data
            //dataPackager.newPackageEvent += new DataPackager.DataPackagedEventHandler(dataPackageReceived);

            // Initialize data array with default values
            for (int i = 0; i < this.globalDataSet.MAX_MOTOR_AMOUNT; i++)
            {
                dataPackage[i] = new byte[8];
                for (int j = 0; j < this.globalDataSet.MAX_DATAPACKAGE_ELEMENTS; j++) dataPackage[i][j] = 0;
            }
        }

        public void dataPackageReceived(byte[][] dataPackage)
        {
            // Write new data to current context and change the flag to show the sending thread that new data is available
            this.dataPackage = dataPackage;
            //byte[] tempByte0 = (byte[])dataPackage.GetValue(0);

            //for (int i = 0; i < tempByte0.Length; i++) { 
            //    Debug.WriteLine("tempByte0: " + tempByte0[i]);
            //}
            //Debug.WriteLine("-------------");

            //byte[] tempByte1 = (byte[])dataPackage.GetValue(1);

            //for (int i = 0; i < tempByte1.Length; i++)
            //{
            //    Debug.WriteLine("tempByte1: " + tempByte1[i]);
            //}
            //Debug.WriteLine("-------------");
            //newDataFromPackager = true;
        }

        public void startServer()
        {
            // Start thread to handle the sending of data to clients
            clientSocket_send = default(TcpClient);
            this.serverThread_send = new Thread(new ThreadStart(serverLoop_send));
            serverThread_send.Start();


            // Start thread to handle the sending of data to clients
            clientSocket_receive = default(TcpClient);
            this.serverThread_receive = new Thread(new ThreadStart(serverLoop_receive));
            serverThread_receive.Start();
        }

        public void stopServer()
        {
            globalDataSet.AbortServerOperation = true;

            // Close send server threading
            clientSocket_send.Close();
            serverThread_send.Abort();

            // Close receive server threading
            clientSocket_receive.Close();
            serverThread_receive.Abort();
        }

        private void serverLoop_send()
        {
            serverSocket_send.Start();
            clientSocket_send = serverSocket_send.AcceptTcpClient();
            bool showMsg = true;
            bool firstStart = true;

            while (!globalDataSet.AbortServerOperation)
            {
                if (clientSocket_send.Connected)
                {
                    // ------------
                    // For testing
                    // ------------
                    if (showMsg)
                    {
                        Debug.WriteLine("Client is connected to send socket");
                        showMsg = false;
                    }
                    // ------------
                    // ------------

                    if (firstStart)
                    {
                        // Wait some time until client is initialized before we send a big amount of data
                        firstStart = false;
                        Thread.Sleep(2000);
                    }

                    //if (newDataFromPackager)
                    //{
                        // Copy the new data to local variable and reset the flag
                        //localDataPackage = dataPackage;
                        //newDataFromPackager = false;
                    //}
                    // Send dataPackage to client
                    sender(dataPackage[motorCounter]);
                    if (motorCounter < globalDataSet.Motor.Length-1) motorCounter++;
                    else motorCounter = 0;

                    // Wait some time that the client can handle the new data
                    Thread.Sleep(DELAY_TIME_THREAD_SENDER);
                }
            }

        }

        private void serverLoop_receive()
        {
            serverSocket_receive.Start();
            clientSocket_receive = serverSocket_receive.AcceptTcpClient();
            bool showMsg = true;

            while (!globalDataSet.AbortServerOperation)
            {
                if (clientSocket_receive.Connected)
                {
                    // ------------
                    // For testing
                    // ------------
                    if (showMsg)
                    {
                        Debug.WriteLine("Client is connected to receive socket");
                        showMsg = false;
                    }
           
                    // Set received data to global data array
                    //globalDataSet.DataPackage_In[globalDataSet.MotorId] = receiver();
                    globalDataSet.DataPackage_In_Test = receiver();

                    // Decrement motor id to prevent exception (index start at 1 but with no motor selected the index is 0)
                    // Get action state for current motor 
                    if (globalDataSet.DataPackage_In_Test[1] > 0)
                    {
                        globalDataSet.Motor[globalDataSet.DataPackage_In_Test[1] - 1].State = (GlobalDataSet.ActionStates)globalDataSet.DataPackage_In_Test[2];
                        //Debug.WriteLine("globalDataSet.Motor["+(globalDataSet.DataPackage_In_Test[1] - 1)+"].State: " + globalDataSet.Motor[globalDataSet.DataPackage_In_Test[1] - 1].State);
                    }

                    //for (int i = 0; i < globalDataSet.DataPackage_In.Length; i++) Debug.WriteLine("receive_bytes[" + i + "]: " + globalDataSet.DataPackage_In[i]);

                    // Wait some time that the client can handle the new data
                    Thread.Sleep(DELAY_TIME_THREAD_RECEIVER);
                }
            }

        }

        private void sender(byte[] message)
        {
            try
            {
                NetworkStream networkStream_server_send = clientSocket_send.GetStream();
                networkStream_server_send.Write(message, 0, message.Length);
                networkStream_server_send.Flush();
                //for (int i = 0; i < message.Length; i++) Debug.WriteLine("send message[" + i + "]: " + message[i]);

                //Debug.WriteLine("send message[" + 3 + "]: " + message[3]);
                //Debug.WriteLine("send message[" + 4 + "]: " + message[4]);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception: " + e + "in sender");
            }
        }

        private Byte[] receiver()
        {
            Byte[] receive_bytes = new Byte[8];
            try
            {
                NetworkStream networkStream_receive = clientSocket_receive.GetStream();
                networkStream_receive.Read(receive_bytes, 0, 8);
                //short tasknumber = receiveBytes[0];
                //short motorId = receiveBytes[1];
                //int motorAngleTemp = BitConverter.ToInt16(receiveBytes, 2);
                //for (int i = 0; i < receive_bytes.Length; i++) Debug.WriteLine("receive receiveBytes[" + i + "]: " + receive_bytes[i]);

            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception: " + e + "in sender");
            }
            return receive_bytes;
        }
    }
}

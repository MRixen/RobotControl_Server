using FormRobotControlServer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Packager
{
    ///\brief Packaging new data from upper control algorithm.

    /// This class handle the actions that were called from upper control algorithm and results (pendings, completings) that comes from the client.
    /// It packs the selected action with specific parameters to byte array.
    /// The byte array contains among others: Tasknumber, Velocity, Endposition
    /// When data is packed an event is fired that the server can send the new packaged data to clients
    /// 
    /// <B>EXAMPLE:</B>
    /// 
    /// Upper control algorithm sends "New Position" like 10 deg with veloctity 10% and motor ID 1
    /// DataPackager package this data to an byte array and fire event.
    /// ServerUnit receive this new data and send it to client.
    /// The client responds at first with the pending state and after completion with completion state.
    /// The DataPackager reset its state after receiving the completion state
    /// 
    /// <B>PACKAGE CONSTRUCT OUTGOING</B>
    /// 
    /// Every cycle the algorithm send 8 byte via tcp/ip to the client
    /// One data block per send cycle contains the data for one motor
    ///
    /// 0. byte: action
    /// 1. byte: motor id
    /// 2. byte: velocity
    /// 3. byte: angle
    /// 4. byte: angle
    /// 5. byte: Direction of the motor 
    /// 6. byte: 
    /// 7. byte: 
    ///
    /// <B>PACKAGE CONSTRUCT INCOMING</B>
    /// 
    /// 0. byte: action
    /// 1. byte: motor id
    /// 2. byte: action state
    /// 3. byte: 
    /// 4. byte: 
    /// 5. byte: 
    /// 6. byte: 
    /// 7. byte:                                   

    class DataPackager
    {
        public delegate void DataPackagedEventHandler(byte[][] dataPackage);
        public event DataPackagedEventHandler newPackageEvent; //!< This event is fired after new data is packaged

        private Thread serverThread_packaging; //!< This thread is necessary to run the packaging loop
        private byte[][] dataPackage_out; //!< Package that contains the packaged control data
        private GlobalDataSet globalDataSet; //!< Contains global data (different classes use the same object to share data)
        private short angleValueTemp = 0; //!< Temporary angle that is set to the package

        /// Constructor of the Packager class
        public DataPackager(GlobalDataSet globalDataSet)
        {
            this.globalDataSet = globalDataSet;
            dataPackage_out = new byte[this.globalDataSet.MAX_MOTOR_AMOUNT][];

            // Initialize the packaged data array with default values
            for (int i = 0; i < this.globalDataSet.MAX_MOTOR_AMOUNT; i++)
            {
                dataPackage_out[i] = new byte[8];
                for (int j = 0; j < this.globalDataSet.MAX_DATAPACKAGE_ELEMENTS; j++) dataPackage_out[i][j] = 0;
            }
        }

        ///\brief Start thread to handle the packaging of new data.

        /// Start a thread to package control data inside while loop.
        public void startPackaging()
        {
            this.serverThread_packaging = new Thread(new ThreadStart(packagingLoop));
            serverThread_packaging.Start();
        }


        ///\brief Checks states / actions and pack data.

        /// Check inside loop the current action that comes from upper control algorithm.
        /// Check the states that comes from the client.
        /// Pack all the control data to byte array package (global data) and fire event.
        private void packagingLoop()
        {
            bool newData = false;

            Pulses pulses = new Pulses();
            int[] pulseData = pulses.Pulse_sinus;
            Byte[] velocityValueTemp = new Byte[2];

            while (!globalDataSet.AbortServerOperation)
            {
                // Iterate through the motor array
                for (int motorCounter = 0; motorCounter < globalDataSet.MAX_MOTOR_AMOUNT; motorCounter++)
                {
                    //Debug.WriteLine(globalDataSet.Motor[motorCounter].Id + ";" + globalDataSet.Motor[motorCounter].Angle + ";" + globalDataSet.Motor[motorCounter].Velocity + ";" + globalDataSet.Motor[motorCounter].Action + ";" + globalDataSet.ControlDataRowCounter[motorCounter]);

                    // Send nothing when:
                    // - Actual action for specific motor is <doNothing>
                    // - Incoming action state is <init>
                    //if (((int)globalDataSet.Action[globalDataSet.MotorId] == (int)GlobalDataSet.RobotActions.doNothing) & (globalDataSet.DataPackage_In[globalDataSet.MotorId][(int)GlobalDataSet.Incoming_Package_Content.actionState] == (int)GlobalDataSet.ActionStates.init))
                    if (((int)globalDataSet.Motor[motorCounter].Action == (int)GlobalDataSet.RobotActions.doNothing) & !(globalDataSet.Motor[motorCounter].ActionIsSet) & (globalDataSet.Motor[motorCounter].Id != 0))
                    {
                        // Set indicator led to green
                        //globalDataSet.IndicatorLed[motorCounter] = true;
                        //dataPackage_out[motorCounter][0] = Convert.ToByte(globalDataSet.Motor[motorCounter].Action);
                        //for (int i = 1; i < globalDataSet.MAX_DATAPACKAGE_ELEMENTS; i++) dataPackage_out[motorCounter][i] = 0;
                        //dataPackage_out[motorCounter][(int)GlobalDataSet.Outgoing_Package_Content.motorId] = (byte)globalDataSet.Motor[motorCounter].Id;
                        //newData = true;
                    }

                    //if (globalDataSet.DataPackage_In[globalDataSet.MotorId][(int)GlobalDataSet.Incoming_Package_Content.actionState] != (int)GlobalDataSet.ActionStates.init)
                    if (globalDataSet.Motor[motorCounter].State != (int)GlobalDataSet.ActionStates.init)
                    {
                        globalDataSet.IndicatorLed[motorCounter] = false;
                    }

                    // Send request to save ref pos when:
                    // - Actual action for specific motor is <saveToEeprom>
                    // - Incoming action for specific motor state is <init>
                    //if (((int)globalDataSet.Action[globalDataSet.MotorId] == (int)GlobalDataSet.RobotActions.saveToEeprom) & (globalDataSet.DataPackage_In[globalDataSet.MotorId][(int)GlobalDataSet.Incoming_Package_Content.actionState] == (int)GlobalDataSet.ActionStates.init))
                    if (((int)globalDataSet.Motor[motorCounter].Action == (int)GlobalDataSet.RobotActions.saveRefPosToEeprom) & !(globalDataSet.Motor[motorCounter].ActionIsSet))
                    {
                        dataPackage_out[motorCounter][0] = Convert.ToByte(globalDataSet.Motor[motorCounter].Action);
                        dataPackage_out[motorCounter][(int)GlobalDataSet.Outgoing_Package_Content.motorId] = (byte)globalDataSet.Motor[motorCounter].Id;
                        newData = true;
                    }

                    // Send request to disable pid controller when:
                    // - Actual action is <disablePidController>
                    // - Incoming action state is <init>
                    //if (((int)globalDataSet.Action[globalDataSet.MotorId] == (int)GlobalDataSet.RobotActions.disablePidController) & (globalDataSet.DataPackage_In[globalDataSet.MotorId][(int)GlobalDataSet.Incoming_Package_Content.actionState] == (int)GlobalDataSet.ActionStates.init))
                    if (((int)globalDataSet.Motor[motorCounter].Action == (int)GlobalDataSet.RobotActions.disablePidController) & !(globalDataSet.Motor[motorCounter].ActionIsSet))
                    {
                        dataPackage_out[motorCounter][0] = Convert.ToByte(globalDataSet.Motor[motorCounter].Action);
                        // Set motor id
                        dataPackage_out[motorCounter][(int)GlobalDataSet.Outgoing_Package_Content.motorId] = (byte)globalDataSet.Motor[motorCounter].Id;
                        newData = true;
                    }

                    // Send request to enable pid controller when:
                    // - Actual action is <enablePidController>
                    // - Incoming action state is <init>
                    //if (((int)globalDataSet.Action[globalDataSet.MotorId] == (int)GlobalDataSet.RobotActions.enablePidController) & (globalDataSet.DataPackage_In[globalDataSet.MotorId][(int)GlobalDataSet.Incoming_Package_Content.actionState] == (int)GlobalDataSet.ActionStates.init))
                    if (((int)globalDataSet.Motor[motorCounter].Action == (int)GlobalDataSet.RobotActions.enablePidController) & !(globalDataSet.Motor[motorCounter].ActionIsSet))
                    {
                        dataPackage_out[motorCounter][0] = Convert.ToByte(globalDataSet.Motor[motorCounter].Action);
                        // Set motor id
                        dataPackage_out[motorCounter][(int)GlobalDataSet.Outgoing_Package_Content.motorId] = (byte)globalDataSet.Motor[motorCounter].Id;
                        newData = true;
                    }

                    // Send request to save act pos when:
                    // - Actual action for specific motor is <saveActPosToEeprom>
                    // - Incoming action for specific motor state is <init>
                    if (((int)globalDataSet.Motor[motorCounter].Action == (int)GlobalDataSet.RobotActions.saveActPosToEeprom) & !(globalDataSet.Motor[motorCounter].ActionIsSet))
                    {
                        dataPackage_out[motorCounter][0] = Convert.ToByte(globalDataSet.Motor[motorCounter].Action);
                        dataPackage_out[motorCounter][(int)GlobalDataSet.Outgoing_Package_Content.motorId] = (byte)globalDataSet.Motor[motorCounter].Id;
                        newData = true;
                    }

                    // Send control data when:
                    // - Actual action is <newPosition>
                    // - Incoming action state is <init>
                    //if (((int)globalDataSet.Action[globalDataSet.MotorId] == (int)GlobalDataSet.RobotActions.newPosition) & (globalDataSet.DataPackage_In[globalDataSet.MotorId][(int)GlobalDataSet.Incoming_Package_Content.actionState] == (int)GlobalDataSet.ActionStates.init))
                    //if (((int)globalDataSet.Motor[motorCounter].Action == (int)GlobalDataSet.RobotActions.newPosition) & (globalDataSet.Motor[motorCounter].State == (int)GlobalDataSet.ActionStates.init))
                    if (((int)globalDataSet.Motor[motorCounter].Action == (int)GlobalDataSet.RobotActions.newPosition) & !(globalDataSet.Motor[motorCounter].ActionIsSet))
                    {
                        globalDataSet.Motor[motorCounter].ActionIsSet = true;

                        // Set action
                        dataPackage_out[motorCounter][(int)GlobalDataSet.Outgoing_Package_Content.action] = Convert.ToByte(globalDataSet.Motor[motorCounter].Action);

                        // Set motor id
                        dataPackage_out[motorCounter][(int)GlobalDataSet.Outgoing_Package_Content.motorId] = (byte)(globalDataSet.Motor[motorCounter].Id);

                        // Set direction and angle
                        if (globalDataSet.Motor[motorCounter].Angle < 0)
                        {
                            angleValueTemp = (short)(globalDataSet.Motor[motorCounter].Angle * (-1));
                            dataPackage_out[motorCounter][(int)GlobalDataSet.Outgoing_Package_Content.motorDir] = (byte)0;
                        }
                        else
                        {
                            angleValueTemp = (short)(globalDataSet.Motor[motorCounter].Angle);
                            dataPackage_out[motorCounter][(int)GlobalDataSet.Outgoing_Package_Content.motorDir] = (byte)1;
                        }
                        byte[] angleValue_converted = BitConverter.GetBytes(angleValueTemp);
                        if (BitConverter.IsLittleEndian) Array.Reverse(angleValue_converted);
                        dataPackage_out[motorCounter][(int)GlobalDataSet.Outgoing_Package_Content.angle_1] = angleValue_converted[0];
                        dataPackage_out[motorCounter][(int)GlobalDataSet.Outgoing_Package_Content.angle_2] = angleValue_converted[1];

                        // Set velocity
                        velocityValueTemp = BitConverter.GetBytes(globalDataSet.Motor[motorCounter].Velocity);
                        dataPackage_out[motorCounter][(int)GlobalDataSet.Outgoing_Package_Content.velocity] = velocityValueTemp[0];
                        //Debug.WriteLine("globalDataSet.Motor["+motorCounter+"].ActionIsSet) 1:" + globalDataSet.Motor[motorCounter].ActionIsSet);

                        //Debug.WriteLine("action: " + dataPackage_out[motorCounter][(int)GlobalDataSet.Outgoing_Package_Content.action]);
                        //Debug.WriteLine("id: " + dataPackage_out[motorCounter][(int)GlobalDataSet.Outgoing_Package_Content.motorId]);
                        //Debug.WriteLine("angle1: " + dataPackage_out[motorCounter][(int)GlobalDataSet.Outgoing_Package_Content.angle_1]);
                        //Debug.WriteLine("angle2: " + dataPackage_out[motorCounter][(int)GlobalDataSet.Outgoing_Package_Content.angle_2]);
                        //Debug.WriteLine("vel: " + dataPackage_out[motorCounter][(int)GlobalDataSet.Outgoing_Package_Content.velocity]);

                        newData = true;
                    }

                    // Reset action to zero (for specific motor) when task is completed (state is complete, e.g. position is reached)
                    //if (globalDataSet.DataPackage_In[globalDataSet.MotorId][(int)GlobalDataSet.Incoming_Package_Content.actionState] == (int)GlobalDataSet.ActionStates.complete)
                    if ((int)globalDataSet.Motor[motorCounter].State == (int)GlobalDataSet.ActionStates.complete)
                    {
                        //if (motorCounter == 1) Debug.WriteLine("(int)globalDataSet.Motor["+motorCounter+"].State: " + (int)globalDataSet.Motor[motorCounter].State);


                        //globalDataSet.Motor[motorCounter].Action = (int)GlobalDataSet.RobotActions.doNothing;
                        //dataPackage_out[motorCounter][0] = Convert.ToByte(globalDataSet.Motor[motorCounter].Action);
                        //dataPackage_out[motorCounter][(int)GlobalDataSet.Outgoing_Package_Content.motorId] = (byte)(globalDataSet.Motor[motorCounter].Id);


                        //Debug.WriteLine("---------------o-------------");
                        //Debug.WriteLine("action: " + dataPackage_out[1][(int)GlobalDataSet.Outgoing_Package_Content.action]);
                        //Debug.WriteLine("id: " + dataPackage_out[1][(int)GlobalDataSet.Outgoing_Package_Content.motorId]);
                        //Debug.WriteLine("----------------------------");

                        //newData = true;
                    }

                    // TODO: motor id zählt hoch (1, 2) aber gesendet wird nur 1 oder 2
                    //Debug.WriteLine(dataPackage_out[motorCounter][1]);
                    //Debug.WriteLine("motorCounter " + motorCounter);

                    // Fire event that server can handle new data
                    // Only at the end when the hole datapackage with all motor ids is packed
                    if (newData & (motorCounter == globalDataSet.Motor.Length-1))
                    {
                        //Debug.WriteLine("motorCounter: " + motorCounter);
                        newData = false;
                        //byte[] tempByte = (byte[])dataPackage_out.GetValue(motorCounter);
                        //for (int i = 0; i < tempByte.Length; i++) Debug.WriteLine("tempByte: " + tempByte[i]);
                        //Debug.WriteLine("---------------u-------------");
                        //Debug.WriteLine("action: " + dataPackage_out[1][(int)GlobalDataSet.Outgoing_Package_Content.action]);
                        //Debug.WriteLine("id: " + dataPackage_out[1][(int)GlobalDataSet.Outgoing_Package_Content.motorId]);
                        //Debug.WriteLine("angle1: " + dataPackage_out[motorCounter][(int)GlobalDataSet.Outgoing_Package_Content.angle_1]);
                        //Debug.WriteLine("angle2: " + dataPackage_out[motorCounter][(int)GlobalDataSet.Outgoing_Package_Content.angle_2]);
                        //Debug.WriteLine("vel: " + dataPackage_out[motorCounter][(int)GlobalDataSet.Outgoing_Package_Content.velocity]);
                        //Debug.WriteLine("----------------------------");

                        this.newPackageEvent(dataPackage_out);
                    }
                    //Thread.Sleep(500);
                }
            }
        }
    }
}

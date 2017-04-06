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
    /// 1. byte: action
    /// 2. byte: motor id
    /// 3. byte: velocity
    /// 4. byte: angle
    /// 5. byte: angle
    /// 6. byte: Direction of the motor 
    /// 7. byte: 
    /// 8. byte: 
    ///
    /// <B>PACKAGE CONSTRUCT INCOMING</B>
    /// 
    /// 1. byte: action
    /// 2. byte: motor id
    /// 3. byte: action state
    /// 4. byte: 
    /// 5. byte: 
    /// 6. byte: 
    /// 7. byte: 
    /// 8. byte:                                   

    class DataPackager
    {
        public delegate void DataPackagedEventHandler(byte[] dataPackage);
        public event DataPackagedEventHandler newPackageEvent; //!< This event is fired after new data is packaged

        private Thread serverThread_packaging; //!< This thread is necessary to run the packaging loop
        private byte[][] dataPackage_out = new byte[4][]; //!< Package that contains the packaged control data
        private GlobalDataSet globalDataSet; //!< Contains global data (different classes use the same object to share data)
        private short angleValueTemp = 0; //!< Temporary angle that is set to the package

        /// Constructor of the Packager class
        public DataPackager(GlobalDataSet globalDataSet)
        {
            this.globalDataSet = globalDataSet;
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
                // Send nothing when:
                // - Actual action for specific motor is <doNothing>
                // - Incoming action state is <init>
                if (((int)globalDataSet.Action[globalDataSet.MotorId] == (int)GlobalDataSet.RobotActions.doNothing) & (globalDataSet.DataPackage_In[globalDataSet.MotorId][(int)GlobalDataSet.Incoming_Package_Content.actionState] == (int)GlobalDataSet.ActionStates.init))
                {
                    // Set indicator led to green
                    globalDataSet.IndicatorLed[globalDataSet.MotorId] = true;
                    dataPackage_out[globalDataSet.MotorId][0] = Convert.ToByte(globalDataSet.Action[globalDataSet.MotorId]);
                    for (int i = 1; i < globalDataSet.MAX_DATAPACKAGE_ELEMENTS; i++) dataPackage_out[globalDataSet.MotorId][i] = 0;
                    newData = true;
                }

                if (globalDataSet.DataPackage_In[globalDataSet.MotorId][(int)GlobalDataSet.Incoming_Package_Content.actionState] != (int)GlobalDataSet.ActionStates.init)
                {
                    globalDataSet.IndicatorLed[globalDataSet.MotorId] = false;
                }

                // Send request to save ref pos when:
                // - Actual action for specific motor is <saveToEeprom>
                // - Incoming action for specific motor state is <init>
                if (((int)globalDataSet.Action[globalDataSet.MotorId] == (int)GlobalDataSet.RobotActions.saveToEeprom) & (globalDataSet.DataPackage_In[globalDataSet.MotorId][(int)GlobalDataSet.Incoming_Package_Content.actionState] == (int)GlobalDataSet.ActionStates.init))
                {
                    dataPackage_out[globalDataSet.MotorId][0] = Convert.ToByte(globalDataSet.Action[globalDataSet.MotorId]);
                    newData = true;
                }

                // Send request to disable pid controller when:
                // - Actual action is <disablePidController>
                // - Incoming action state is <init>
                if (((int)globalDataSet.Action[globalDataSet.MotorId] == (int)GlobalDataSet.RobotActions.disablePidController) & (globalDataSet.DataPackage_In[globalDataSet.MotorId][(int)GlobalDataSet.Incoming_Package_Content.actionState] == (int)GlobalDataSet.ActionStates.init))
                {
                    dataPackage_out[globalDataSet.MotorId][0] = Convert.ToByte(globalDataSet.Action[globalDataSet.MotorId]);
                    // Set motor id
                    dataPackage_out[globalDataSet.MotorId][(int)GlobalDataSet.Outgoing_Package_Content.motorId] = (byte)globalDataSet.MotorId;
                    newData = true;
                }

                // Send request to enable pid controller when:
                // - Actual action is <enablePidController>
                // - Incoming action state is <init>
                if (((int)globalDataSet.Action[globalDataSet.MotorId] == (int)GlobalDataSet.RobotActions.enablePidController) & (globalDataSet.DataPackage_In[globalDataSet.MotorId][(int)GlobalDataSet.Incoming_Package_Content.actionState] == (int)GlobalDataSet.ActionStates.init))
                {
                    dataPackage_out[globalDataSet.MotorId][0] = Convert.ToByte(globalDataSet.Action[globalDataSet.MotorId]);
                    // Set motor id
                    dataPackage_out[globalDataSet.MotorId][(int)GlobalDataSet.Outgoing_Package_Content.motorId] = (byte)globalDataSet.MotorId;
                    newData = true;
                }

                // Send control data when:
                // - Actual action is <newPosition>
                // - Incoming action state is <init>
                if (((int)globalDataSet.Action[globalDataSet.MotorId] == (int)GlobalDataSet.RobotActions.newPosition) & (globalDataSet.DataPackage_In[globalDataSet.MotorId][(int)GlobalDataSet.Incoming_Package_Content.actionState] == (int)GlobalDataSet.ActionStates.init))
                {
                    // Set action
                    dataPackage_out[globalDataSet.MotorId][(int)GlobalDataSet.Outgoing_Package_Content.action] = Convert.ToByte(globalDataSet.Action[globalDataSet.MotorId]);

                    // Set motor id
                    dataPackage_out[globalDataSet.MotorId][(int)GlobalDataSet.Outgoing_Package_Content.motorId] = (byte)globalDataSet.MotorId;

                    // Set direction and angle
                    if (globalDataSet.MotorSollAngle < 0)
                    {
                        angleValueTemp = (short)(globalDataSet.MotorSollAngle * (-1));
                        dataPackage_out[globalDataSet.MotorId][(int)GlobalDataSet.Outgoing_Package_Content.motorDir] = (byte)0;
                    }
                    else
                    {
                        angleValueTemp = (short)(globalDataSet.MotorSollAngle);
                        dataPackage_out[globalDataSet.MotorId][(int)GlobalDataSet.Outgoing_Package_Content.motorDir] = (byte)1;
                    }
                    byte[] angleValue_converted = BitConverter.GetBytes(angleValueTemp);
                    if (BitConverter.IsLittleEndian) Array.Reverse(angleValue_converted);
                    dataPackage_out[globalDataSet.MotorId][(int)GlobalDataSet.Outgoing_Package_Content.angle_1] = angleValue_converted[0];
                    dataPackage_out[globalDataSet.MotorId][(int)GlobalDataSet.Outgoing_Package_Content.angle_2] = angleValue_converted[1];

                    // Set velocity
                    velocityValueTemp = BitConverter.GetBytes(globalDataSet.MotorSollVelocity);
                    dataPackage_out[globalDataSet.MotorId][(int)GlobalDataSet.Outgoing_Package_Content.velocity] = velocityValueTemp[0];

                    newData = true;
                }

                // Reset action to zero (for specific motor) when task is completed (state is complete, e.g. position is reached)
                if (globalDataSet.DataPackage_In[globalDataSet.MotorId][(int)GlobalDataSet.Incoming_Package_Content.actionState] == (int)GlobalDataSet.ActionStates.complete)
                {
                    globalDataSet.Action[globalDataSet.MotorId] = (int)GlobalDataSet.RobotActions.doNothing;
                    dataPackage_out[globalDataSet.MotorId][0] = Convert.ToByte(globalDataSet.Action[globalDataSet.MotorId]);
                    newData = true;
                }

                // Fire event that server can handle new data
                if (newData)
                {
                    newData = false;
                    this.newPackageEvent((byte[])dataPackage_out.GetValue(globalDataSet.MotorId));
                }

                //Debug.WriteLine("state " + globalDataSet.DataPackage_In[(int)GlobalDataSet.Incoming_Package_Content.actionState]);
                //Debug.WriteLine("action " + globalDataSet.Action);

            }
        }
    }
}

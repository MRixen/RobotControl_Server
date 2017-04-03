using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packager
{
    ///\brief File operations (read, write).

    class ReadWriteFile
    {

        private string FILE_SAVE_PATH = "C:\\Users\\Manuel.Rixen\\Desktop\\";

        // Save a string to txt file
        // NOTE: If header is empty like "" the data string will be apennd
        public void writeToTxtFile(string header, string fileName, string element)
        {
            if (header.Length != 0)
            {
                using (StreamWriter writer = new StreamWriter(FILE_SAVE_PATH + fileName, false))
                {
                    writer.WriteLine(header);
                }
            }

            else
            {
                using (StreamWriter writer = new StreamWriter(FILE_SAVE_PATH + fileName, true))
                {
                    writer.WriteLine(element);
                }
            }
        }

        public int readFromTxtFile(int fieldPos, string fileName)
        {
            string returnString = string.Empty;
            String[] messageData = { "-1", "-1" };
            using (StreamReader reader = new StreamReader(FILE_SAVE_PATH + fileName))
            {
                // Read the first line with header
                returnString = reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    returnString = reader.ReadLine();

                    // Create message array from semicolon seperated text file 
                    messageData = returnString.Split(';');

                }
            }
            // Return the item at the specified position
            return Int32.Parse(messageData[fieldPos]);
        }
    }
}

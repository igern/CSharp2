using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class SerialGenerator
    {
        public SerialGenerator()
        {
            string currentDirectory = Directory.GetCurrentDirectory().ToString();
            string pathString = currentDirectory + "/validSerials.txt";
            if (!File.Exists(pathString))
            {
                FileStream fs = File.Create(pathString);
                string[] serialList = new string[100];
                for (int i = 0; i < 100; i++)
                {
                    serialList[i] = GetSerialNumber();
                }
            }

        }

        public string GetSerialNumber()
        {
            Guid serialGuid = Guid.NewGuid();
            string uniqueSerial = serialGuid.ToString("N");

            string uniqueSerialLength = uniqueSerial.Substring(0, 28).ToUpper();

            char[] serialArray = uniqueSerialLength.ToCharArray();
            string finalSerialNumber = "";

            int j = 0;
            for (int i = 0; i < 28; i++)
            {
                for (j = i; j < 4 + i; j++)
                {
                    finalSerialNumber += serialArray[j];
                }
                if (j == 28)
                {
                    break;
                }
                else
                {
                    i = (j) - 1;
                    finalSerialNumber += "-";
                }
            }

            return finalSerialNumber;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MyLibrary
{
    public class SerialGenerator
    {
        public const string fileName = "ValidSerials.txt";
        public StorageFile sampleFile { get; set; }
        public string[] serialList { get; set; }
        public SerialGenerator()
        {
            serialList = new string[100];

            Task task = Task.Factory.StartNew(() => WriteToFileAsync());
            task.ContinueWith(anotherTask => ReadFileAsync());
            
        }

        public async Task WriteToFileAsync()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            try
            {
                sampleFile = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.FailIfExists);
            } catch (System.Exception e)
            {
                Debug.WriteLine("File already exists : {0}", e);
                return;
            } 
            
            var stream = await sampleFile.OpenAsync(FileAccessMode.ReadWrite);

            using (var outputStream = stream.GetOutputStreamAt(0))
            {
                using (var dataWriter = new Windows.Storage.Streams.DataWriter(outputStream))
                {
                    for(int i = 0; i < 100; i++)
                    {
                        dataWriter.WriteString(GetSerialNumber());
                        dataWriter.WriteString("|");
                    }
                    await dataWriter.StoreAsync();
                    await outputStream.FlushAsync();
                }
            }
            stream.Dispose();

        }

        public async Task ReadFileAsync()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await storageFolder.GetFileAsync(fileName);
            string text = await FileIO.ReadTextAsync(sampleFile);
            serialList = text.Split('|');
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MyLibrary
{
    public class Submissions
    {
        public Submission[] subList { get; set; }
        public int next { get; set; }
        public StorageFile subFile;
        public const string fileName = "Subs.txt";

        public Submissions()
        {
            subList = new Submission[100];
            next = 0;

            Task task = Task.Factory.StartNew(() => CreateSubFile());
            task.ContinueWith(anotherTask => ReadFileAsync());
        }
        

        public bool checkSub(Submission sub)
        {
            foreach(Submission currentSub in subList)
            {
                if (currentSub == null) continue;
                if (sub.serial.Equals(currentSub.serial)) return true;
            }
            return false;
        }

        public async Task CreateSubFile()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            try
            {
                subFile = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.FailIfExists);
            }
            catch (Exception e)
            {
                Debug.WriteLine("File already exists \n {0}", e);
                return;
            }
        }

        public async Task WriteToFileAsync(Submission sub)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            subFile = await storageFolder.GetFileAsync(fileName);
            string text = await FileIO.ReadTextAsync(subFile);

            var stream = await subFile.OpenAsync(FileAccessMode.ReadWrite);

            using (var outputStream = stream.GetOutputStreamAt(0))
            {
                using (var dataWriter = new Windows.Storage.Streams.DataWriter(outputStream))
                {
                    dataWriter.WriteString(text);
                    dataWriter.WriteString(sub.ToString() + Environment.NewLine);
                    await dataWriter.StoreAsync();
                    await outputStream.FlushAsync();
                }
            }
            stream.Dispose();
            subList[next] = sub;
            next++;

        }

        public async Task ReadFileAsync()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            subFile = await storageFolder.GetFileAsync(fileName);
            string text = await FileIO.ReadTextAsync(subFile);
            string[] stringSubList = text.Split( new[] { Environment.NewLine }, StringSplitOptions.None);

            next = 0;
            for (int i = 0; i < 100; i++)
            {
                string currentSub = stringSubList[i];
                if (currentSub == null) continue;
                else
                {
                    string[] sub = currentSub.Split('|');
                    subList[next] = new Submission(sub[0], sub[1], sub[2], sub[3], sub[4], sub[5]);
                    next++;
                }
            }
        }
    }


}

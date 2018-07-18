using System.IO;

namespace Northwind.WebRole.Utils
{
    public class FileAccessHelper
    {
        private readonly string _filePath;

        public FileAccessHelper(string filePath)
        {
            _filePath = filePath;
        }

        public string Read()
        {
            using (FileStream fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        public void Write(string content)
        {
            FileStream fs;
            StreamWriter sw;
            using (fs = new FileStream(_filePath, FileMode.Create, FileAccess.Write))
            {
                using (sw = new StreamWriter(fs))
                {
                    sw.Write(content);
                    sw.Flush();
                }
            }
        }
    }
}
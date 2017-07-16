using System.IO;

namespace AgeAggregator.Logic.Utils
{
    class FileHelper : IFileHelper
    {
        public string ReadFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        public void SaveFile(string filePath, string content)
        {
            File.WriteAllText(filePath, content);
        }
    }
}

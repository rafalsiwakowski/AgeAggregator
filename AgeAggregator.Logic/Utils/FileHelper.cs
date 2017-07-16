using System.IO;

namespace AgeAggregator.Logic.Utils
{
    public interface IFileHelper
    {
        void SaveFile(string filePath, string content);
        string ReadFile(string filePath);
    }

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

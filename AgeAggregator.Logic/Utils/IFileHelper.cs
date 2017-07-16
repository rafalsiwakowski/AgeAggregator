namespace AgeAggregator.Logic.Utils
{
    public interface IFileHelper
    {
        void SaveFile(string filePath, string content);
        string ReadFile(string filePath);
    }
}

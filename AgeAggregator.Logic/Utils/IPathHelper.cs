namespace AgeAggregator.Logic.Utils
{
    public interface IPathHelper
    {
        string GetFullPath(string path);
        bool FileExists(string path);
        bool CanWrite(string path);
        string GetExtension(string path);
    }
}

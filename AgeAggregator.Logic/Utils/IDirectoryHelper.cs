namespace AgeAggregator.Logic.Utils
{
    public interface IDirectoryHelper
    {
        bool ValidateDirectoryPath(string directoryPath);
        string[] GetDirectoryFiles(string directoryPath, params string[] desiredExtensions);
    }
}

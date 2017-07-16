using System.IO;
using System.Linq;

namespace AgeAggregator.Logic.Utils
{
    class DirectoryHelper : IDirectoryHelper
    {
        public bool ValidateDirectoryPath(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        public string[] GetDirectoryFiles(string directoryPath, params string[] desiredExtensions)
        {
            desiredExtensions = desiredExtensions ?? new string[] { };
            if (desiredExtensions.Any())
            {
                return desiredExtensions.Select(x => Directory.EnumerateFiles(directoryPath).Where(p => p.EndsWith(x, System.StringComparison.OrdinalIgnoreCase))).SelectMany(x => x).ToArray();
            }
            return Directory.EnumerateFiles(directoryPath).ToArray();
        }
    }
}

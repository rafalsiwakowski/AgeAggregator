using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace AgeAggregator.Logic.Utils
{
    class PathHelper : IPathHelper
    {
        public string GetFullPath(string path)
        {
            return Path.GetFullPath(path);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public bool CanWrite(string path)
        {
            //Code found on StackOverflow
            try
            {
                FileSystemSecurity security;
                if (File.Exists(path))
                {
                    security = File.GetAccessControl(path);
                }
                else
                {
                    security = Directory.GetAccessControl(Path.GetDirectoryName(path));
                }
                var rules = security.GetAccessRules(true, true, typeof(NTAccount));

                var currentuser = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                bool result = false;
                foreach (FileSystemAccessRule rule in rules)
                {
                    if (0 == (rule.FileSystemRights &
                        (FileSystemRights.WriteData | FileSystemRights.Write)))
                    {
                        continue;
                    }

                    if (rule.IdentityReference.Value.StartsWith("S-1-"))
                    {
                        var securityIdentifier = new SecurityIdentifier(rule.IdentityReference.Value);
                        if (!currentuser.IsInRole(securityIdentifier))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (!currentuser.IsInRole(rule.IdentityReference.Value))
                        {
                            continue;
                        }
                    }

                    if (rule.AccessControlType == AccessControlType.Deny)
                        return false;
                    if (rule.AccessControlType == AccessControlType.Allow)
                        result = true;
                }
                return result;
            }
            catch
            {
                return false;
            }
        }

        public string GetExtension(string path)
        {
            return Path.GetExtension(path);
        }
    }
}

using System.IO;

namespace pdxpartyparrot.Core.Util
{
    public static class DirectoryExtensions
    {
        public static void DeleteUnityDirectory(string path)
        {
            DeleteUnityDirectory(path, false);
        }

        public static void DeleteUnityDirectory(string path, bool recursive)
        {
            Directory.Delete(path, recursive);
            if(path.EndsWith("/") || path.EndsWith("\\")) {
                path = path.Remove(path.Length - 1);
            }

            if(File.Exists($"{path}.meta")) {
                File.Delete($"{path}.meta");
            }
        }
    }
}

using System.IO;

namespace EasyExceptions.Utils.Io
{
    public static class PathUtils
    {
        public static char DirectorySeparatorChar
        {
            get
            {
                return Path.Combine("a", "b")[1];
            }
        }
    }
}

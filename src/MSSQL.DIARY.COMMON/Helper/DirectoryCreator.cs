using System;
using System.IO;
using System.Linq;

namespace MSSQL.DIARY.COMN.Helper
{
    public static class DirectoryCreator
    {
        public static string IfDirectoryNotExistThenCreate(this string astrDirectorPath)
        {
            if (!Directory.Exists(astrDirectorPath))
                try
                {
                    //Directory.CreateDirectory(astrDirectorPath);
                }
                catch (Exception)
                {
                    // ignored
                }

            var filepath = astrDirectorPath + $"\\{astrDirectorPath.Split('\\').LastOrDefault()}.json";
            if (!File.Exists(filepath))
            {
                // File.Create(filepath);
            }

            return astrDirectorPath;
        }
    }
}
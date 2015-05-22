using System.IO;

namespace FeatureSearch
{
    class FileUtility
    {
        /// <summary>
        /// Deletes a directory recursively even when it contains read-only files.
        /// </summary>
        /// <param name="targetDir">Directory that will be deleted</param>
        public static void RecursiveDeleteDirectory(string targetDir)
        {
            File.SetAttributes(targetDir, FileAttributes.Normal);

            string[] files = Directory.GetFiles(targetDir);
            string[] dirs = Directory.GetDirectories(targetDir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
                RecursiveDeleteDirectory(dir);

            Directory.Delete(targetDir, false);
        }
    }
}

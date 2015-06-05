using FeatureSearch;
using System.IO;
using Xunit;

namespace FeatureSearchUnitTests
{
    public class FileUtilityTests
    {
        [Fact]
        public void RecursiveDeleteDirectoryTest()
        {
            string path = Path.Combine(System.Environment.CurrentDirectory, "FileUtilityTests");

            // Creates the test folder
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            // Creates a file inside the folder
            File.WriteAllText(Path.Combine(path, "test.txt"), "-");

            // Creates a sub folder
            string subfolderPath = Path.Combine(path, "subfolder");
            if (!Directory.Exists(subfolderPath))
                Directory.CreateDirectory(subfolderPath);

            // Creates a read-only file inside the sub folder
            string readOnlyFilePath = Path.Combine(subfolderPath, "read-only.txt");
            if (File.Exists(readOnlyFilePath))
                File.SetAttributes(readOnlyFilePath, FileAttributes.Normal);

            File.WriteAllText(readOnlyFilePath, "-");
            File.SetAttributes(readOnlyFilePath, FileAttributes.ReadOnly);

            // Deletes the folder
            FileUtility.RecursiveDeleteDirectory(path);
            Assert.False(Directory.Exists(path));
        }
    }
}

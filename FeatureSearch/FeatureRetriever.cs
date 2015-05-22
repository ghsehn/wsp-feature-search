using Microsoft.Deployment.Compression.Cab;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FeatureSearch
{
    class FeatureRetriever
    {
        public string DirectoryPath { get; set; }
        public string DecompressedDirectoryPath { get { return DirectoryPath + Path.DirectorySeparatorChar + "decompressed"; } }

        public FeatureRetriever(string wspFolderPath)
        {
            this.DirectoryPath = wspFolderPath;
        }

        /// <summary>
        /// Generates a dictionary of feature files with their GUIDs for each WSP in the folder specified.
        /// </summary>
        public Dictionary<string, Dictionary<string, Guid>> GetFeaturesPerWSP()
        {
            CreateDecompressedDirectory();

            var featuresPerWSP = new Dictionary<string, Dictionary<string, Guid>>();
            string[] wspFiles = GetWSPFiles();

            foreach (string wspFile in wspFiles)
            {
                string fileName = Path.GetFileName(wspFile);
                string folderPath = DecompressedDirectoryPath + Path.DirectorySeparatorChar + fileName;

                new CabInfo(wspFile).Unpack(folderPath);

                string[] features = GetFeatureFiles(folderPath);
                Dictionary<string, Guid> featureIds = GetFeatureGUIDs(features);

                featuresPerWSP[fileName] = featureIds;
            }

            return featuresPerWSP;
        }

        /// <summary>
        /// Gets the GUIDs for an list of feature.xml files.
        /// </summary>
        /// <param name="files">Array containing the feature.xml file paths</param>
        /// <returns>Dictionary containing the feature.xml path and its GUID</returns>
        private Dictionary<string, Guid> GetFeatureGUIDs(string[] files)
        {
            var guids = new Dictionary<string, Guid>();

            foreach (string filePath in files)
            {
                string content = File.ReadAllText(filePath);
                Match match = Regex.Match(content, "Id=\"([^\"]+)\"");

                if (match.Success)
                {
                    string id = match.Groups[1].Value;
                    string fileName = filePath.Replace(DecompressedDirectoryPath + Path.DirectorySeparatorChar, "");
                    guids[fileName] = new Guid(id);
                }
            }

            return guids;
        }

        /// <summary>
        /// Gets a list of all feature.xml files inside a WSP folder.
        /// </summary>
        /// <param name="wspFolderPath">Uncompressed WSP folder path</param>
        /// <returns>List of feature.xml paths</returns>
        private string[] GetFeatureFiles(string wspFolderPath)
        {
            var featureFiles = new List<string>();
            var queue = new Queue<string>();
            
            queue.Enqueue(wspFolderPath);

            do
            {
                // Finds feature.xml files
                string folderPath = queue.Dequeue();
                string[] files = Directory.GetFiles(folderPath)
                    .Where(f => f.EndsWith("feature.xml", StringComparison.InvariantCultureIgnoreCase))
                    .ToArray();

                featureFiles.AddRange(files);

                // Adds the directories in the current folder to the queue
                string[] directories = Directory.GetDirectories(folderPath);
                foreach (string directory in directories)
                    queue.Enqueue(directory);
            }
            while (queue.Any());

            return featureFiles.ToArray();
        }

        /// <summary>
        /// Finds all the WSP files inside the directory specified.
        /// </summary>
        /// <returns></returns>
        private string[] GetWSPFiles()
        {
            return Directory.GetFiles(DirectoryPath)
                .Where(f => f.EndsWith(".wsp", StringComparison.InvariantCultureIgnoreCase))
                .ToArray();
        }

        /// <summary>
        /// Creates the directory that will hold the decompressed wsp files.
        /// </summary>
        private void CreateDecompressedDirectory()
        {
            if (Directory.Exists(DecompressedDirectoryPath))
                FileUtility.RecursiveDeleteDirectory(DecompressedDirectoryPath);

            Directory.CreateDirectory(DecompressedDirectoryPath);
        }
    }
}

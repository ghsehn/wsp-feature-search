using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using FeatureSearch;
using System.Collections.Generic;

namespace FeatureSearchUnitTests
{
    [TestClass]
    public class FeatureRetrieverTests
    {
        private const string WspFolderName = "WSPs";

        [TestMethod]
        [DeploymentItem(@"UnitTestData\WSPs\Project1.wsp", WspFolderName)]
        [DeploymentItem(@"UnitTestData\WSPs\Project2.wsp", WspFolderName)]
        public void GetFeaturesPerWSPTest()
        {
            string wspFolderPath = Path.Combine(System.Environment.CurrentDirectory, WspFolderName);

            var featureRetriever = new FeatureRetriever(wspFolderPath);
            var featuresPerWSP = featureRetriever.GetFeaturesPerWSP();

            var firstWSP = new Dictionary<string, Guid>();
            firstWSP[Path.Combine("Project1.wsp", "Project1_Feature1", "Feature.xml")] = new Guid("5335a9fa-a933-46ee-992a-66571c6ebb57");
            firstWSP[Path.Combine("Project1.wsp", "Project1_Feature2", "Feature.xml")] = new Guid("4bacf7be-d8f3-49d0-a935-167c371f9ddc");

            var secondWSP = new Dictionary<string, Guid>();
            secondWSP[Path.Combine("Project2.wsp", "Project2_Feature1", "Feature.xml")] = new Guid("2765e99d-7dc6-4691-a149-f2cc86e3868f");

            var dictionary = new Dictionary<string, Dictionary<string, Guid>>()
            {
                {"Project1.wsp", firstWSP},
                {"Project2.wsp", secondWSP},
            };

            CollectionAssert.AreEqual(dictionary.Keys, featuresPerWSP.Keys);
            CollectionAssert.AreEqual(firstWSP, featuresPerWSP["Project1.wsp"]);
            CollectionAssert.AreEqual(secondWSP, featuresPerWSP["Project2.wsp"]);
        }
    }
}

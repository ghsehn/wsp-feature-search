using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeatureSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            string directory = RequestDirectory();
            Guid? guid = RequestGuid();

            var featureSearch = new FeatureRetriever(directory);
            var featuresPerWSP = featureSearch.GetFeaturesPerWSP();

            if (guid != null)
                SearchForGuid((Guid)guid, featuresPerWSP);
            else
                ShowFeatures(featuresPerWSP);

            Console.ReadLine();
        }

        static string RequestDirectory()
        {
            Console.WriteLine("Type the path of the directory that contains the WSP files:");
            string directory = Console.ReadLine();
            Console.WriteLine();
            return directory;
        }

        static Guid? RequestGuid()
        {
            string guidString;

            Console.WriteLine("Feature GUID to find (leave empty to list all features):");
            guidString = Console.ReadLine();

            Console.WriteLine();

            return (!String.IsNullOrWhiteSpace(guidString)) ? new Guid(guidString) : (Guid?)null;
        }

        static void SearchForGuid(Guid guid, Dictionary<string, Dictionary<string, Guid>> featuresPerWSP)
        {
            bool found = false;

            foreach (var wsp in featuresPerWSP)
            {
                var feature = wsp.Value.Where(f => f.Value == guid).Take(1).ToList();

                if (feature.Any())
                {
                    found = true;

                    Console.WriteLine("Found:");
                    Console.WriteLine(wsp.Key);
                    Console.WriteLine(String.Format("{0} ({1})", feature[0].Value, feature[0].Key));
                    Console.WriteLine();
                }
            }

            if (!found)
                Console.WriteLine("Feature not found");
        }

        static void ShowFeatures(Dictionary<string, Dictionary<string, Guid>> featuresPerWSP)
        {
            foreach (var wsp in featuresPerWSP)
            {
                Console.WriteLine(wsp.Key + ":");

                if (wsp.Value.Any())
                {
                    foreach (KeyValuePair<string, Guid> feature in wsp.Value)
                        Console.WriteLine(String.Format("{0} ({1})", feature.Value, feature.Key));
                }
                else
                {
                    Console.WriteLine("No features");
                }

                Console.WriteLine();
                Console.WriteLine();
            }

            Console.ReadLine();
        }
    }
}

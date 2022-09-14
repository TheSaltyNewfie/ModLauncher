using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Json.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace ModLoaderCsharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConfigurationManager.InitializeDefaults();

            string downloadDir = ConfigurationManager.DownloadDirectory;

            Dictionary<string, string> modUriDict = DoJsonStuff();

            WebClient wc = new WebClient();

            foreach (KeyValuePair<string, string> element in modUriDict)
            {
                Console.WriteLine($"Attempting to download: {element.Key}...");

                try
                { 
                    wc.DownloadFile(element.Value, $"{downloadDir}\\{element.Key}");
                }
                catch (WebException ex)
                { 
                    if(ex.Response != null)
                    {
                        var response = ex.Response;
                        var dataStream = response.GetResponseStream();
                        var reader = new StreamReader(dataStream);
                        var details = reader.ReadToEnd();
                    }
                }
                Console.WriteLine($"Succeeded");
            }
        }

        public static Dictionary<string, string> DoJsonStuff()
        {
            Dictionary<string, string> modUriDict = new Dictionary<string, string>();

            string modConfDir = ConfigurationManager.ModConfigurationsDirectory;
            string modConfigName = ConfigurationManager.ModConfigName;

            string modConfigJson = $"{modConfDir}\\{modConfigName}.jar";

            dynamic jsonResponse = JsonConvert.DeserializeObject(File.ReadAllText(modConfigJson));
            jsonResponse.Works = true;
            foreach (var mod in jsonResponse.mods)
            {
                Console.WriteLine($"{(string)mod.Name} : {(string)mod.First}");
                modUriDict.Add((string)mod.Name, (string)mod.First);
            }
            Assert.That(jsonResponse, Is.InstanceOf<dynamic>());
            Assert.That(jsonResponse, Is.TypeOf<JObject>());

            return modUriDict;
        }
    }


}


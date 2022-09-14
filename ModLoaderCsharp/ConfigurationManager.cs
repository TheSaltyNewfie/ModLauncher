using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace ModLoaderCsharp
{
    public class ConfigurationManager
    {
        public static string DownloadDirectory { get; set; }
        public static string ModConfigurationsDirectory { get; set; }
        public static string ModConfigName { get; set; }

        public static void InitializeDefaults()
        {
            var workingDir = Directory.GetCurrentDirectory();
            string configDir = $"{workingDir}\\config";
            string configJson = $"{configDir}\\config.json";

            if (!Directory.Exists(configDir))
            {
                Directory.CreateDirectory(configDir);
                Console.WriteLine("Default configuration folder was not present, it has been remade along with a template json config file.\nPlease Create a proper config file using the provided template.");
                return;
            }
            if (!File.Exists(configJson))
            {
                Console.WriteLine("No config.json found. Please create one using the provided template.");
                return;
            }

            dynamic jsonObj = JsonConvert.DeserializeObject(File.ReadAllText(configJson));
            jsonObj.Works = true;

            foreach (var option in jsonObj.settings)
            {
                if(option.Name == "DefaultDownloadFolder")
                    DownloadDirectory = option.First;
                if(option.Name == "DefaultModConfigurationsFolder")
                    ModConfigurationsDirectory = option.First;
                if(option.Name == "ModConfigFileName")
                    ModConfigName = option.First;
            }

            Assert.That(jsonObj, Is.InstanceOf<dynamic>());
            Assert.That(jsonObj, Is.TypeOf<JObject>());

            Console.WriteLine($"{DownloadDirectory}\n{ModConfigurationsDirectory}\n{ModConfigName}");
        }
    }
}

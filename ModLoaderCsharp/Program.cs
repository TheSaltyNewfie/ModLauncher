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
            UserConfig config = JsonManageer.Config();

            string downloadDir = config.DownloadDirectory;

            Dictionary<string, string> modUriDict = JsonManageer.JsonModDeserializer(config);

            DownloadManager.DownloadMods(modUriDict, config);
        }
    }

    public class JsonManageer
    {
        public static Dictionary<string, string> JsonModDeserializer(UserConfig config)
        {
            Dictionary<string, string> modUriDict = new Dictionary<string, string>();

            string modConfDir = config.ModConfigDirectory;
            string modConfigName = config.ModConfigName;

            string modConfigJson = $"{modConfDir}\\{modConfigName}";

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
        
        public static UserConfig Config()
        {
            UserConfig config = new UserConfig();

            var workingDir = Directory.GetCurrentDirectory();
            string configDir = $"{workingDir}\\config";
            string configJson = $"{configDir}\\config.json";

            if (!Directory.Exists(configDir))
            {
                Directory.CreateDirectory(configDir);
                Console.WriteLine("Default configuration folder was not present, it has been remade along with a template json config file.\nPlease Create a proper config file using the provided template.");
                return null;
            }
            if (!File.Exists(configJson))
            {
                Console.WriteLine("No config.json found. Please create one using the provided template.");
                return null;
            }

            dynamic jsonObj = JsonConvert.DeserializeObject(File.ReadAllText(configJson));
            jsonObj.Works = true;

            foreach (var option in jsonObj.settings)
            {
                switch(option.Name)
                {
                    case "DefaultDownloadFolder":
                        config.DownloadDirectory = option.First;
                        break;
                    case "DefaultModConfigurationsFolder":
                        config.ModConfigDirectory = option.First;
                        break;
                    case "ModConfigFileName":
                        config.ModConfigName = option.First;
                        break;
                }
                    
            }

            Assert.That(jsonObj, Is.InstanceOf<dynamic>());
            Assert.That(jsonObj, Is.TypeOf<JObject>());

            Console.WriteLine($"{config.DownloadDirectory} : {config.ModConfigDirectory}");

            return config;
        }
    }

    public class DownloadManager
    {
        public static void DownloadMods(Dictionary<string, string> modUriDict, UserConfig config)
        {
            WebClient wc = new WebClient();

            foreach (KeyValuePair<string, string> element in modUriDict)
            {
                Console.WriteLine($"Attempting to download: {element.Key}...");

                try
                {
                    wc.DownloadFile(element.Value, $"{config.DownloadDirectory}\\{element.Key}.jar");
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
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
    }

    public class UserConfig
    {
        public UserConfig() { }
        public UserConfig(string downloadDirectory, string modDirectory, string modConfigName)
        {
            DownloadDirectory = downloadDirectory;
            ModConfigDirectory = modDirectory;
            ModConfigName = modConfigName;
        }

        public string DownloadDirectory { get; set; }
        public string ModConfigDirectory { get; set; }
        public string ModConfigName { get; set; }
    }
}


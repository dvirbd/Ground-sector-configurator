using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace GNGSCT_Gen
{
    public class Config
    {
        private const string ConfigFile = "ColorConfig.json";
        private static Dictionary<string, string> Colors;
        public static void Utilities() //Reads json data and puts it in dictionary called "Colors"
        {
            string json = File.ReadAllText(ConfigFile);
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            Colors = data.ToObject<Dictionary<string, string>>();
        }

        public static string GetColor(string key) //returns the value for a key if the key is avilable
        {
            if (Colors.ContainsKey(key))
            {
                return Colors[key];
            }
            return "";
        }
        public static List<string> GetKeys() //returns List<string> of all the keys
        {
            List<string> keys = new List<string>();
            foreach(var key in Colors.Keys)
            {
                keys.Add(key);
            }
            return keys;
        }
        public static bool StartsWithKey(string line)
        {
            foreach(var key in Colors.Keys)
            {
                if (line.StartsWith(key))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

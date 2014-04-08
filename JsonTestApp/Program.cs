using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;

namespace JsonTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Location of the output file.
            const string filePath = @"c:\temp\store.json";

            // Creating a simple dictionary that has a non-string key
            var dictStore = new Dictionary<DictionaryKey, int>();
            for (var i = 0; i < 800; i++)
            {
                dictStore.Add(new DictionaryKey(i.ToString(CultureInfo.InvariantCulture), i), i);
            }
            var settings = new JsonSerializerSettings { Formatting = Formatting.Indented };
            var jsonSerializer = JsonSerializer.Create(settings);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                var streamWriter = new StreamWriter(stream);
                jsonSerializer.Serialize(streamWriter, dictStore);
                streamWriter.Flush();
            }

            var stopWatch = Stopwatch.StartNew();
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var deserialize = jsonSerializer.Deserialize(new StreamReader(stream), typeof(Dictionary<DictionaryKey, int>));
            }
            stopWatch.Stop();

            Console.WriteLine("Time elapsed: "+stopWatch.ElapsedMilliseconds);
        }
    }

    class DictionaryKey
    {
        private String _name;
        private int _number;

        public DictionaryKey(String name, int number)
        {
            _name = name;
            _number = number;
        }

        public override string ToString()
        {
            return _name + " " + _number;
        }

        public static implicit operator DictionaryKey(string dictionaryKey)
        {
            var strings = dictionaryKey.Split(' ');
            return new DictionaryKey(strings[0], Convert.ToInt32(strings[1]));
        }
    }
}

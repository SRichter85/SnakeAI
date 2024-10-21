using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SnakeAICore
{
    public class StorableData<T> where T:class
    {
        public void Write(string path)
        {
            string data = JsonSerializer.Serialize(this as T, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, data);
        }

        public static T? Read(string path)
        {
            T? retval = default(T);
            if (File.Exists(path))
            {
                string data = File.ReadAllText(path);
                retval = JsonSerializer.Deserialize<T>(data);
            }

            return retval;
        }
    }
}

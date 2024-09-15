using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpBot.Config
{
    internal class jsonreader
    {
        public string token { get; set; }
        public string prefix { get; set; }

        public async Task readjson()
        {
            using (StreamReader sr = new StreamReader("Config.json"))
            {
                string json = await sr.ReadToEndAsync();
                jsonstruct data = JsonConvert.DeserializeObject<jsonstruct>(json);

                this.token = data.token;
                this.prefix = data.prefix;
            }
        }
    }

    internal sealed class jsonstruct
    {
        public string token { get; set; }
        public string prefix { get; set; }
    }
}

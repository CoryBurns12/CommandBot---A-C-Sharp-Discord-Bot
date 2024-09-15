using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;


namespace HelpBot.Games.Jokes
{
    public class DadJokes
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string API = "https://www.icanhazdadjoke.com/";

        public async Task<string> GetJoke()
        {
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            var GetRequest = await client.GetStringAsync(API); // Get request that is sent to the API server
            var serverResponse = JsonConvert.DeserializeObject<Joke>(GetRequest); // Response to the get request

            return serverResponse.joke;

        }
    }

    public class Joke
    {
        [JsonProperty("joke")] // Joke from API
        public string joke { get; set; } // Joke is stored here and is returned above.
    }
}

using System;
using RestSharp;
using System.Threading.Tasks;

namespace HelpBot.Games.RandomFact
{
    public class Fact
    {
        private readonly string APIKey = Environment.GetEnvironmentVariable("API_KEY");

        public async Task<string> GetFact()
        {
            if(string.IsNullOrEmpty(APIKey))
                Console.WriteLine("API KEY IS NOT SET!");

            var client = new RestClient("https://api.api-ninjas.com/v1/facts");
            var request = new RestRequest()
            {
                Method = Method.Get
            };
            request.AddHeader("X-Api-Key", APIKey);

            try
            {
                var response = await client.ExecuteAsync(request);
                if(response.IsSuccessful)
                {
                    Console.WriteLine(response.StatusCode);
                    return response.Content;
                }
                else
                {
                    return $"ERROR: {response.ErrorMessage} - {response.StatusDescription}";
                }
            }
            catch (Exception ex)
            {
                return $"ERROR: {ex.Message}";
            }
        }
    }
}

using System;
using RestSharp;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelpBot.Games.TruthOrDare
{
    public class TruthOrDareConfig
    {
        public async Task<string> GetTruth()
        {
            var client = new RestClient("https://api.truthordarebot.xyz/v1/truth");
            var request = new RestRequest()
            {
                Method = Method.Get
            };

            try
            {
                var response = await client.ExecuteAsync(request);
                if (response.IsSuccessful)
                {
                    JObject json = JObject.Parse(response.Content);
                    string question = json["question"].ToString();
                    return question;
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

        public async Task<string> GetDare()
        {
            var client = new RestClient("https://api.truthordarebot.xyz/api/dare");
            var request = new RestRequest()
            {
                Method = Method.Get
            };

            try
            {
                var response = await client.ExecuteAsync(request);
                if (response.IsSuccessful)
                {
                    JObject json = JObject.Parse(response.Content);
                    string dare = json["question"].ToString();
                    return dare;
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

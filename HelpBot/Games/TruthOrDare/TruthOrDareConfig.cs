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
                    string rating = json["rating"].ToString();
                    string truth = json["question"].ToString();


                    if (rating.Equals("PG", StringComparison.OrdinalIgnoreCase))
                        return truth;
                    else
                        return "Cannot get PG-rated question for you at this time!";

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
                    string rating = json["rating"].ToString();
                    string dare = json["question"].ToString();


                    if (rating.Equals("PG", StringComparison.OrdinalIgnoreCase))
                        return dare;
                    else
                        return "No PG-rated dares available right now!";

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

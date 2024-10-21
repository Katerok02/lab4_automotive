using ClassLibrary1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public class OMDbApiClient
    {
        private readonly string _apiKey;
        private readonly IHttpClient _httpClient;

        public OMDbApiClient(string apiKey, IHttpClient httpClient)
        {
            _apiKey = apiKey;
            _httpClient = httpClient;
        }

        public async Task<OMDbMovieResponse> GetMovieByIdAsync(string imdbId)
        {
            var url = $"http://www.omdbapi.com/?i={imdbId}&apikey={_apiKey}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<OMDbMovieResponse>(content);
        }

        public async Task<string> DefineMovieRating(string imdbId)
        {
            var url = $"http://www.omdbapi.com/?i={imdbId}&apikey={_apiKey}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<OMDbMovieResponse>(content);
            if (obj.Rated == "PG-13")
            {
                return "Family film";
            }
            else if (obj.Rated == "R")
            {
                return "Restricted film";
            }
            else
            {
                return "For all";
            }
        }
    }
}

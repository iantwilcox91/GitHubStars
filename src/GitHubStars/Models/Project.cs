using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace GitHubStars.Models
{
    public class Project
    {
        public string Name { get; set; }
        public string Stargazers_count { get; set; }
        public static List<Project> GetProjects()
        {
            var client = new RestClient("https://api.github.com/search/repositories?page=1&q=user:iantwilcox91&sort=stars:>=1&order=desc");
            var request = new RestRequest("", Method.GET);

            request.AddParameter("Access_token", "4759119685c2f7bf02a64e429b6a9211d0df0e5b");
            request.AddHeader("User-Agent", "iantwilcox91");
            request.AddHeader("Accept", "application/vnd.github.v3.text-match+json");
            client.Authenticator = new HttpBasicAuthenticator("/Itmes.json", "4759119685c2f7bf02a64e429b6a9211d0df0e5b");

            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            var repoList = JsonConvert.DeserializeObject<List<Project>>(jsonResponse["items"].ToString());
            return repoList;
        }
        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response =>
            {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }
    }
}
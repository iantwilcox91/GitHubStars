using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace GitHubStars.Models
{
    public class Project
    {
        public string name { get; set; }
        public string stars { get; set; }
        public static Dictionary<string, string> GetProjects()
        {
            var client = new RestClient("https://api.github.com/users/iantwilcox91/repos");
            var request = new RestRequest("", Method.GET);
            request.AddParameter("access_token", "f6d2a0d542f6b213fb843baf5078d5edc85736ea");
            request.AddHeader("User", "iantwilcox91");
            request.AddHeader("Accept", "application/vnd.github.v3+json");

            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            JArray jsonResponse = JsonConvert.DeserializeObject<JArray>(response.Content);
            var MyProjects = JsonConvert.DeserializeObject<List<Project>>(jsonResponse.ToString());
            Dictionary<string, string> model = new Dictionary<string, string>();
            foreach (var project in MyProjects)
            {
                model.Add(project.name, project.stars);
            }
            return model;

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
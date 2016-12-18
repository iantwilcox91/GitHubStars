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
        public string stargazers_count { get; set; }
        public static List<Project> GetProjects()
        {
            var client = new RestClient("https://api.github.com/users/iantwilcox91/repos");
            var request = new RestRequest(Method.GET);
            //var request = new RestRequest("???maybe i have to tell it a file or something here??" , Method.GET);

            request.AddParameter("access_token", "f9715624905ca91313383c9d9bec097674c2c3f8");
            request.AddHeader("User-Agent", "iantwilcox91");
            request.AddHeader("Accept", "application/vnd.github.v3+json");

            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            JArray jsonResponse = JsonConvert.DeserializeObject<JArray>(response.Content);
            //Unable to cast object of type 'Newtonsoft.Json.Linq.JArray' to type 'Newtonsoft.Json.Linq.JObject'. -- using JArray
            var projectList = JsonConvert.DeserializeObject<List<Project>>(jsonResponse.ToString());
            return projectList;
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
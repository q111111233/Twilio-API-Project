using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TwilioProject
{
    public class Message
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Body { get; set; }
        public string Status { get; set; }
    }

    public class Program
    {

        public static void Main(string[] args)
        {
            ////1
            //var client = new RestClient("https://api.twilio.com/2010-04-01");
            ////2
            //var request = new RestRequest("Accounts/AC025e97a0775a04b665532afbf9e0aaa5/Messages", Method.POST);
            ////3
            //request.AddParameter("To", "+13475753862");
            //request.AddParameter("From", "+17182850735");
            //request.AddParameter("Body", "Hello world!");
            ////4
            //client.Authenticator = new HttpBasicAuthenticator("AC025e97a0775a04b665532afbf9e0aaa5", "acd8103c2537e6b2c2935cc9b33be5e9");
            ////5
            //client.ExecuteAsync(request, response =>
            //{
            //    Console.WriteLine(response);
            //});
            //Console.ReadLine();

            var client = new RestClient("https://api.twilio.com/2010-04-01");
            //1
            var request = new RestRequest("Accounts/AC025e97a0775a04b665532afbf9e0aaa5/Messages.json", Method.GET);
            client.Authenticator = new HttpBasicAuthenticator("AC025e97a0775a04b665532afbf9e0aaa5", "acd8103c2537e6b2c2935cc9b33be5e9");
            //2
            var response = new RestResponse();
            //3a
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            //4
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            var messageList = JsonConvert.DeserializeObject<List<Message>>(jsonResponse["messages"].ToString());

            foreach (var message in messageList)
            {
                Console.WriteLine("To: {0}", message.To);
                Console.WriteLine("From: {0}", message.From);
                Console.WriteLine("Body: {0}", message.Body);
                Console.WriteLine("Status: {0}", message.Status);
            }
            Console.ReadLine();
        }
        //3b
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

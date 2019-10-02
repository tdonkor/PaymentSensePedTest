using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace PaymentSensePedTest
{
    public class RestApi
    {
        private string testAccountUrl;
        private string username;
        private string password;

        public string GetRequest(string url)
        {
 

            username = "acrelec";
            password = "8ffb32c4-8b29-428d-b5e8-896f7ca7890d";

            var client = new RestClient(url)
            {
                Authenticator = new HttpBasicAuthenticator(username, password)
            };
           
            var request = new RestRequest( Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Connection", "keep-alive");

            IRestResponse response = client.Execute(request);

            return response.Content;

        }

        public void Authenticate(string username, string password, string url)
        {

            var restClient = new RestClient(url)
            {
                Authenticator = new HttpBasicAuthenticator(username, password)
            };

        }




        public string PostRequest(string url)
        {

            return string.Empty;
        }

        public void PutRequest()
        {

        }

        public void DeleteRequest()
        {

        }


    }
}

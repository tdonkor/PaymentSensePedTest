using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace PaymentSensePedTest
{
    public class RestApi
    {

        private string username;
        private string password;

        AppConfiguration configFile = AppConfiguration.Instance;

        public RestApi()
        {
            username = configFile.UserName;
            password = configFile.Password;

        }

        public string GetTIDAvailability(string url)

        {

            RestClient client = Authenticate(url + "/pac/terminals");

            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Connection", "keep-alive");

            IRestResponse response = client.Execute(request);

            return response.Content;

        }


        public string PostSaleTransaction(string url, int value)
        {
            RestClient client = Authenticate(url + "/pac/terminals/" + configFile.Tid + "/transactions");

            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Connection", "keep-alive");
            request.AddParameter("undefined", "{\r\n  \"transactionType\": \"SALE\",\r\n  \"amount\": " + value + ",\r\n  \"currency\": \"GBP\"\r\n}", ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            //check reponse isSuccessful
            if( response.IsSuccessful)
            {
               
                //check response is successfully returned.
                GetTransactionData(response.Content, url);

            }
            


            return response.Content;

        }

       

        public void GetTransactionData(string responseStr, string url)
        {
            TransactionResp tranResponse = JsonConvert.DeserializeObject<TransactionResp>(responseStr);
            Console.WriteLine("Location: " + tranResponse.Location);
            Console.WriteLine("Request Id: " + tranResponse.RequestId);

            RestClient client = Authenticate(url+ "/pac/terminals/" + configFile.Tid + "/transactions/" + tranResponse.RequestId);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Connection", "keep-alive");

            IRestResponse response = client.Execute(request);


        }

        public void PutRequest()
        {

        }

        public void DeleteRequest()
        {

        }

        private RestClient Authenticate(string url)
        {
            return new RestClient(url)
            {
                Authenticator = new HttpBasicAuthenticator(username, password)
            };
        }
    }

    class TransactionResp
    {
        public string Location { get; set; }
        public string RequestId { get; set; }
    }

}

using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading;

namespace PaymentSensePedTest
{
    public class RestApi 
    {

        private string username;
        private string password;
        private string requestId;

        AppConfiguration configFile = AppConfiguration.Instance;

        public RestApi()
        {
            username = configFile.UserName;
            password = configFile.Password;

        }


        /// <summary>
        /// Starts a transaction on the terminal with the given TID.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string PostSaleTransaction(string url, int value)
        {

            RestClient client = Authenticate(url + "/pac/terminals/" + configFile.Tid + "/transactions");

            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Connection", "keep-alive");
            request.AddParameter("undefined", "{\r\n  \"transactionType\": \"SALE\",\r\n  \"amount\": " + value + ",\r\n  \"currency\": \"" + configFile.Currency + "\"\r\n}", ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            //check reponse isSuccessful
            if(response.IsSuccessful)
            {
                //deserialise response
                TransactionResp tranResponse = JsonConvert.DeserializeObject<TransactionResp>(response.Content);
                requestId = tranResponse.RequestId;

                //poll for result every 1 seconds block until finish
                while (true)
                {
                    Thread.Sleep(1000);
                    response = GetTransactionData(requestId, url);
                       //Console.WriteLine(response.Content + "\n\n");
                      
                        if (response.Content.Contains("TRANSACTION_FINISHED"))
                        {
                          
                            break;
                        }       
                }



            }
            
            return response.Content;

        }

        /// <summary>
        ///  Gets data for the transaction with the given requestId.
        /// </summary>
        /// <param name="responseStr"></param>
        /// <param name="url"></param>
        public IRestResponse GetTransactionData(string requestId, string url)
        {

            RestClient client = Authenticate(url+ "/pac/terminals/" + configFile.Tid + "/transactions/" + requestId);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Connection", "keep-alive");

            IRestResponse response = client.Execute(request);

              
            return response;
        }


        public void PutRequest()
        {

        }

        public IRestResponse DeleteRequest(string requestId, string url)
        {

            RestClient client = Authenticate(url + "/pac/terminals/" + configFile.Tid + "/transactions/" + requestId);
            var request = new RestRequest(Method.DELETE);
            request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Connection", "keep-alive");

            IRestResponse response = client.Execute(request);


            return response;
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

    class NotificationCheck
    {
        public string Location { get; set; }
        public string Notifications { get; set; }
    }

}

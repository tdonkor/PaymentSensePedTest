using RestSharp;
using RestSharp.Authenticators;
using System;

using Newtonsoft.Json;
using System.Threading;

namespace PaymentSensePedTest
{
    public class PaymentSenseRestApi : IDisposable
    {

        private string username;
        private string password;
        private string requestId;
        private string url;
        private string tid;
        private string currency;
        private string installerId;
        private string softwareHouseId;
        private string mediaType;
       

        AppConfiguration configFile;


        public PaymentSenseRestApi()
        {
            configFile = AppConfiguration.Instance;
            username = configFile.UserName;
            password = configFile.Password;
            url = configFile.UserAccountUrl;
            tid = configFile.Tid;
            currency = configFile.Currency;
            installerId = configFile.InstallerId;
            softwareHouseId = configFile.SoftwareHouseId;
            mediaType = configFile.MediaType;
        }


        /// <summary>
        /// Starts a transaction on the terminal with the given TID.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string PostSaleTransaction(int value)
        {

            RestClient client = Authenticate(url + "/pac/terminals/" + tid + "/transactions");
            bool signatureRequired = false;
            TransactionDetails transactionDetails;

            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", mediaType);
            request.AddHeader("Software-House-Id", softwareHouseId);
            request.AddHeader("Installer-Id", installerId);
            request.AddHeader("Connection", "keep-alive");
            request.AddParameter("undefined", "{\r\n  \"transactionType\": \"SALE\",\r\n  \"amount\": " + value + ",\r\n  \"currency\": \"" + currency + "\"\r\n}", ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            //check reponse isSuccessful
            if(response.IsSuccessful)
            {
                //deserialise response
                TransactionResp tranResponse = JsonConvert.DeserializeObject<TransactionResp>(response.Content);
                requestId = tranResponse.RequestId;


                 Console.ForegroundColor = ConsoleColor.Cyan;
                //poll for result every 1 seconds block until finish
                //int i = 0;

                while (true)
                {
                    Thread.Sleep(1000);
                    response = GetTransactionData(requestId, url);
                    // Console.WriteLine(response.Content + "\n\n");
                    // Console.Write(" " + i++);
                    


                    if ((response.Content.Contains("SIGNATURE_VERIFICATION")) && (signatureRequired == false))
                    {
                        signatureRequired = true;
                        response = SignaturePutRequest(requestId, url);
                        Console.ForegroundColor = ConsoleColor.Red;
                    }

                    if (response.Content.Contains("TRANSACTION_FINISHED"))
                    {
                         break;
                    }                 
                }

            }

            //deserialise response
            transactionDetails = JsonConvert.DeserializeObject<TransactionDetails>(response.Content);

            //var x = JsonConvert.SerializeObject(transactionDetails.ReceiptLines, Formatting.Indented);

            ReceiptDetails(transactionDetails);

            return transactionDetails.TransactionResult;

            //return response.Content;

        }

        /// <summary>
        ///  Gets data for the transaction with the given requestId.
        /// </summary>
        /// <param name="responseStr"></param>
        /// <param name="url"></param>
        public IRestResponse GetTransactionData(string requestId, string url)
        {

            RestClient client = Authenticate(url+ "/pac/terminals/" + tid + "/transactions/" + requestId);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", mediaType);
            request.AddHeader("Software-House-Id", softwareHouseId);
            request.AddHeader("Installer-Id", installerId);
            request.AddHeader("Connection", "keep-alive");

            IRestResponse response = client.Execute(request);

              
            return response;
        }

        /// <summary>
        /// Reverse the swipe card and set the signature to false
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public IRestResponse SignaturePutRequest(string requestId, string url)
        {
            RestClient client = Authenticate(url + "/pac/terminals/" + tid + "/transactions/" + requestId + "/signature");
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", mediaType);
            request.AddHeader("Software-House-Id", softwareHouseId);
            request.AddHeader("Installer-Id", installerId);
            request.AddHeader("Connection", "keep-alive");
            request.AddParameter("undefined", "{\r\n  \"accepted\": false\r\n}", ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            return response;
        }


    

        //public IRestResponse DeleteRequest(string requestId, string url)
        //{

        //    RestClient client = Authenticate(url + "/pac/terminals/" + configFile.Tid + "/transactions/" + requestId);
        //    var request = new RestRequest(Method.DELETE);
        //    request.AddHeader("Content-Type", "application/json");
        //    //request.AddHeader("Connection", "keep-alive");
        //    IRestResponse response = client.Execute(request);

        //    return response;
        //}

        /// <summary>
        /// Get the transaction details and put them into an object 
        /// </summary>
        /// <param name="transactionDetail"></param>
        public void ReceiptDetails(TransactionDetails transactionDetail)
        {
           
            Console.WriteLine("\n\tRECEIPT");
            Console.WriteLine("\t==========\n");
            Console.WriteLine("Amount Total: " + transactionDetail.AmountTotal);
            Console.WriteLine("Application Id: " + transactionDetail.ApplicationId);
            Console.WriteLine("Application Label: " + transactionDetail.ApplicationLabel);
            Console.WriteLine("AuthCode: " + transactionDetail.AuthCode);
            Console.WriteLine("Cardholder Verification Method: " + transactionDetail.CardholderVerificationMethod);
            Console.WriteLine("CardScheme Name: " + transactionDetail.CardSchemeName);
            Console.WriteLine("Currency: " + transactionDetail.Currency);
            Console.WriteLine("Date Of Expiry: " + transactionDetail.DateOfExpiry);
            Console.WriteLine("Date Of Start: " + transactionDetail.DateOfStart);
            Console.WriteLine("Payment Method: " + transactionDetail.PaymentMethod);
            Console.WriteLine("Primary AccountNumber: " + transactionDetail.PrimaryAccountNumber);
            Console.WriteLine("Primary AccountNumberSequence: " + transactionDetail.PrimaryAccountNumberSequence);
            Console.WriteLine("Transaction Id: " + transactionDetail.TransactionId);
            Console.WriteLine("Transaction Number: " + transactionDetail.TransactionNumber);
            Console.WriteLine("Transaction Result: " + transactionDetail.TransactionResult);
            Console.WriteLine("Transaction Time: " + transactionDetail.TransactionTime);
            Console.WriteLine("Transaction Type: " + transactionDetail.TransactionType);
            Console.WriteLine("UserMessage:" + transactionDetail.UserMessage);
            Console.ResetColor();
            Console.WriteLine("\n\n");



        }

        /// <summary>
        /// Authenticate the users username and password
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private RestClient Authenticate(string url)
        {
            return new RestClient(url)
            {
                Authenticator = new HttpBasicAuthenticator(username, password)
            };
        }


        public void Dispose()
        {

        }
    }

    /// <summary>
    /// structure of the transaction response 
    /// </summary>
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

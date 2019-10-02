using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSensePedTest
{
    public class PaymentSenseAPI : IDisposable
    {
        private string availableURL;
        private string testAccountUrl;
        private string transactionUrl;
        private string username;
        private string password;

        RestApi restApi = new RestApi();

        public void Dispose()
        {
          
        }

        //public void Authorisation()
        //{
        //    testAccountUrl = "https://st185l090000.test.connect.paymentsense.cloud";
        //    username = "acrelec";
        //    password = "8ffb32c4-8b29-428d-b5e8-896f7ca7890d";

        //    restApi.Authenticate(username, password, testAccountUrl);
        //}

        public string CheckTerminalAvailability()
        {

            availableURL = "https://st185l090000.test.connect.paymentsense.cloud/pac/terminals";
            return restApi.GetRequest(availableURL);

        }

        //public string Transaction()
        //{
        //    transactionUrl = "https://st185l090000.test.connect.paymentsense.cloud/pac/terminals/22163665/transactions";
        //    return restApi.PostRequest(transactionUrl);
        //}
    }
}

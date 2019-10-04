using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSensePedTest
{
    public class PaymentSenseAPI : IDisposable
    {
       // private string userAccountURL = "https://st185l090000.test.connect.paymentsense.cloud";

        RestApi restApi = new RestApi();
        AppConfiguration configFile = AppConfiguration.Instance;

        public void Dispose()
        {
          
        }


        public string GetValue(int value)
        {
           
            return restApi.PostSaleTransaction(configFile.UserAccountUrl, value);
        }
    }

   
}

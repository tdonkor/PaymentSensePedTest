using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSensePedTest
{
    class Program
    {
       

        static void Main(string[] args)
        {

            using (var payment = new PaymentSenseAPI())
            {
                // get Authentication
               // payment.Authorisation();

                //check availablity
               Console.WriteLine(payment.CheckTerminalAvailability());

               // Console.WriteLine(payment.Transaction());
            }
            Console.WriteLine("Press any key to exit...");

            Console.ReadKey();
        }
    }
}

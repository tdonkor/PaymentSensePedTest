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
     

            Console.WriteLine("\n\tPaymentSense Payment Simulator");
            Console.WriteLine("\t_______________________________\n");
            int amount = 0;

            using (var payment = new PaymentSenseAPI())
            {
                try
                {
                    Console.Write("Enter the Amount(no decimal point allowed): ");
                    amount = int.Parse(Console.ReadLine());

                    //check value
                    amount = Utils.GetNumericAmountValue(amount);

                    Console.WriteLine($"Payment amount is {amount}");


                    //execute the transaction 
                    Console.WriteLine(payment.GetValue(amount));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error " + ex.Message);
                }
               
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}

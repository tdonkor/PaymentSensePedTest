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

            string answer = "Y";
            do
            {
                Console.Clear();

                Console.WriteLine("\n\tPaymentSense Payment Simulator");
                Console.WriteLine("\t_______________________________\n");
                int amount = 0;

                using (var payment = new PaymentSenseRestApi())
                {
                    try
                    {
                        Console.Write("Enter the Amount(no decimal point allowed): ");
                        amount = int.Parse(Console.ReadLine());

                        Console.Clear();


                        //check value
                        amount = Utils.GetNumericAmountValue(amount);

                        if (amount == 0)
                        {
                            throw new Exception("Payment amount value error...");
                        }

                        Console.WriteLine($"\nPayment amount £{amount/100.0} is valid");

                        //execute the transaction 
                        Console.WriteLine("Payment is : " + payment.PostSaleTransaction(amount));

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                    Console.Write("\n\nWould you like to add another payment? (Y/N): ");
                    answer = Console.ReadLine().ToUpper();
                }
            } while (answer == "Y");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}

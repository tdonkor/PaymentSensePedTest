using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSensePedTest
{
    public enum DiagnosticErrMsg : short
    {
        OK = 0,
        NOTOK = 1
    }

    public class Utils
    {

        /// <summary>
        /// Check the numeric value of the amount
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static int GetNumericAmountValue(int amount)
        {

            if (amount <= 0)
            {

                Console.WriteLine("Invalid pay amount");
                amount = 0;
            }

            return amount;
        }

        /// <summary>
        /// Create Customer Ticket to output the reciept
        /// </summary>
        /// <param name="ticket"></param>
        //public static void CreateCustomerTicket(TransactionInfo ticket)
        //{
        //    string ticketStr = string.Empty;

        //    //display transactionInfo
        //    Console.WriteLine("\nTransaction Information");
        //    Console.WriteLine("-----------------------\n");
        //    Console.WriteLine($" AuthCode: {ticket.AuthorisationCode}");
        //    Console.WriteLine($" CardHolder Name: {ticket.CardHolderName}");
        //    Console.WriteLine($" Currency Code: {ticket.CurrencyCode}");
        //    Console.WriteLine($" Data Entry Method: {ticket.DataEntryMethod}");
        //    Console.WriteLine($" Merchant Number: {ticket.MerchantNo}");
        //    Console.WriteLine($" Response Code: {ticket.ResponseCode}");
        //    Console.WriteLine($" Scheme Number.: {ticket.SchemeName}");
        //    Console.WriteLine($" GetValue Amount: {ticket.TransactionAmount}");
        //    Console.WriteLine($" GetValue Ref Number: {ticket.TransactionRefNo.ToString()}");
        //    Console.WriteLine($" TerminalId: {ticket.TerminalId}");

        //    ticketStr = ticket.CustomerReceipt;


        //    //customer receipt
        //    Console.WriteLine("\n\nCustomer Receipt");
        //    Console.WriteLine("===================\n");

        //    Console.WriteLine($" Customer Reciept: {ticketStr}");
        //}
    }
}

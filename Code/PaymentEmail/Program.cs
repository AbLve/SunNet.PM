using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentEmail
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                PaymentCheck paymentCheck = new PaymentCheck();
                paymentCheck.Check();
            }
            catch (Exception ex)
            {
                LogProvider.WriteLog(ex.ToString());
                return;
            }
        }
    }
}

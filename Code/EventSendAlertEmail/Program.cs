using System;
using System.Timers;
using System.Configuration;

namespace EventSendAlertEmail
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                new AlertChecker().Check();
            }
            catch (Exception ex)
            {
                LogProvider.WriteLog(ex.ToString());
                return;
            }

        }


    }
}

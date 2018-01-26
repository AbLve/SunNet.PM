using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace ReminderEmail
{
    class Program
    {
        static ReminderManager _reminderManager = new ReminderManager();
        static void Main(string[] args)
        {
            #region 控制台程序
            //Wellcome();

            //循环执行
            //SetInterval(ReminderConfig.RunTimeInterval());

            #endregion

            #region windows服务程序
            ServiceBase[] servicesToRun = new ServiceBase[] { new ReminderEmailService()};
            ServiceBase.Run(servicesToRun);
            #endregion
        }

        private static void SetInterval(int intervalMinutes)
        {
            try
            {
                
                while (true)
                {

                    string startInfo = string.Format("{0} Reminding the start", DateTime.Now);
                    ConsoleAndLog(startInfo);

                    _reminderManager.ReminderAll();

                    string completeInfo = string.Format("{0} Reminding the end.After {1} minutes, it will run again.", DateTime.Now, ReminderConfig.RunTimeInterval());
                    ConsoleAndLog(completeInfo);

                    //程序暂停一段时间，再次执行
                    Thread.Sleep(intervalMinutes * 1000 * 60);
                }
                
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);

                //异常后，恢复执行
                SetInterval(ReminderConfig.RunTimeInterval());
            }
        }

        private static void Wellcome()
        {
            string info = string.Format("{0} The program starts and starts to run", DateTime.Now.ToString());
            ConsoleAndLog(info);
        }

        private static void RunCompleted()
        {
            string info = string.Format("{0} Run comepled.After {1} minutes, it will run again.", DateTime.Now.ToString(), ReminderConfig.RunTimeInterval());
            ConsoleAndLog(info);
        }

        private static void ConsoleAndLog(string info)
        {
            Console.WriteLine(info);
            LogProvider.WriteLog(info);
        }
    }
}

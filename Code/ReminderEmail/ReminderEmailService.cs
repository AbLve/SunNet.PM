using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace ReminderEmail
{
    partial class ReminderEmailService : ServiceBase
    {
        System.Timers.Timer _reminderEmailTimer;

        public ReminderEmailService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //邮件提醒
            _reminderEmailTimer = new System.Timers.Timer();
            //分钟换毫秒
            _reminderEmailTimer.Interval = ReminderConfig.RunTimeInterval() * 60 * 1000; 
            _reminderEmailTimer.Enabled = true;
            _reminderEmailTimer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Reminder);
            _reminderEmailTimer.Start();
        }

        protected override void OnStop()
        {
            if (_reminderEmailTimer != null)
            {
                _reminderEmailTimer.Close();
                _reminderEmailTimer.Dispose();
            }
        }

        #region 提醒程序


        /// <summary>
        /// 邮件定时提醒：近三天没有处理过的Ticket
        /// </summary>
        void Timer_Reminder(object sender, System.Timers.ElapsedEventArgs e)
        {
            
            try
            {
                string startInfo = string.Format("{0} Reminding the start", DateTime.Now);
                WriteLog(startInfo);

                ReminderManager reminderManager = new ReminderManager();
                reminderManager.ReminderAll();

                string completeInfo = string.Format("{0} Reminding the end.After {1} minutes, it will run again.", DateTime.Now, ReminderConfig.RunTimeInterval());
                WriteLog(completeInfo);

            }
            catch (Exception exception)
            {
                WriteLog("程序异常："+exception.Message);
            }
        }

        private static void WriteLog(string info)
        {
            LogProvider.WriteLog(info);
        }
        #endregion
    }
}

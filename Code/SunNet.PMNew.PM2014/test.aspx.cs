using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.PM2014.Codes;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;
using StructureMap;
using SunNet.PMNew.Framework;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.KPIModel;

namespace SunNet.PMNew.PM2014
{
    public partial class test : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {



            DateTime dat1 = new DateTime(2016, 08, 04, 16, 0, 0); //08/04/2016 4:00PM

            DateTime dat2 = DateTime.Parse("2016/08/04 19:00"); //08/04/2016 9:00PM
            DateTime dat3 = DateTime.Parse("2016/08/04 21:00"); //08/04/2016 9:00PM
            DateTime now = DateTime.Now;
            Response.Write(string.Format("{0} ----dat1<br>",dat1.ToString("MM/dd/yyyy hh:mm t")));
            Response.Write(string.Format("{0} ----dat2<br>",dat2.ToString("MM/dd/yyyy hh:mm t")));
            Response.Write(string.Format("{0} ----dat3<br>",dat3.ToString("MM/dd/yyyy hh:mm t")));
            if (now > dat1 && now < dat2)
            {
                Response.Write("Between dat1 and dat2");
            }
            if (now > dat1 && now < dat3)
            {
                Response.Write("Between dat1 and dat3");
            }
            List<A> list = new List<A>();
            A a = new A();
            for (int i = 0; i < 10; i++)
            {
                a.CreatedOn = DateTime.Now.AddDays(i);
                list.Add(a);
            }

            foreach (A item in list)
            {
                Response.Write(string.Format("{0} ----<br>", a.CreatedOn.ToString("MM/dd/yyyy")));
            }

            Response.Write(DateTime.Now.ToString("MM/dd/yyyy hh:mm t"));
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            SendHandler handler = new SendHandler(SendEamil);
            handler.BeginInvoke(null, null);
            Response.Redirect("/SunnetTicket/MyTicket.aspx");

            Izl zl = new B();
            zl.start();
            Ipb pb = zl as Ipb;
            pb.jasu();
        }

        public delegate void SendHandler();


        protected void SendEamil()
        {
            List<UsersEntity> list = new App.UserApplication().GetActiveUserList().FindAll(r => r.UserID < 30).OrderBy(r => r.UserID).ToList();
            foreach (UsersEntity user in list)
            {
                ObjectFactory.GetInstance<IEmailSender>().SendMail("Lee@sunnet.us", Config.DefaultSendEmail, "测试委托" + user.UserID, "测试委托内容");
            }
        }
    }


    class A
    {
        public A() { }

        public int ID { get; set; }

        public DateTime CreatedOn { get; set; }
    }

    public interface Izl
    {
        void start();
    }

    public interface Ipb
    {
        void jasu();
    }

    public class AA : Izl
    {
        public int ID { get; set; }
        public void start()
        {
            int i = 0;
            i++;
        }
    }

    public class B : AA, Ipb
    {
        public void jasu()
        {
            int j = 1;
            j++;
        }
    }
}
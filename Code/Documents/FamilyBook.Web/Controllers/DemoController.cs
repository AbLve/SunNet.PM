using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using Newtonsoft.Json.Converters;
using SF.Framework;

namespace FamilyBook.Web.Controllers
{
    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public int Age { get { return DateTime.Now.Year - this.BirthDay.Year; } }
        public bool Aldut { get { return DateTime.Now.Year - this.BirthDay.Year > 18; } }
        public DateTime BirthDay { get; set; }
    }
    public class DemoController : Controller
    {
        public DemoController()
        {
            if (list == null || list.Count == 0)
                for (int i = 0; i < 30; i++)
                {
                    list.Add(new Person() { ID = i, Name = "P " + i, BirthDay = System.DateTime.Now.AddYears(-i), Sex = i % 2 == 0 ? "Male" : "Female" });
                }
        }
        //
        // GET: /Demo/

        public ActionResult Index()
        {
            Thread.Sleep(1000);
            return View();
        }
        public ActionResult Modal()
        {
            return View();
        }
        public ActionResult Popover()
        {
            return View();
        }

        public ActionResult Alert()
        {
            return View();
        }
        public ActionResult DateTime()
        {
            IsoDateTimeConverter JSONDateFormat = new IsoDateTimeConverter() { DateTimeFormat = "MM/dd/yyyy HH:mm:ss.fff" };
            ViewBag.json = Newtonsoft.Json.JsonConvert.SerializeObject(System.DateTime.Now, JSONDateFormat);
            return View();
        }

        public ActionResult jQuery()
        {
            return View();
        }

        public ActionResult Svg()
        {
            ViewBag.Domain = SFConfig.FileVirtualPath;
            return View();
        }
        public ActionResult Svg2()
        {
            return View();
        }
        public ActionResult Svg3()
        {
            return View();
        }

        public ActionResult DaveSvg()
        {
            return View();
        }
        
        public ActionResult SelectPhoto()
        {
            return View();
        }
        public ActionResult KnockoutJS()
        {
            return View();
        }
        static List<Person> list = new List<Person>();
        public string GetData()
        {

            IsoDateTimeConverter JSONDateFormat = new IsoDateTimeConverter() { DateTimeFormat = "MM/dd/yyyy" };
            return Newtonsoft.Json.JsonConvert.SerializeObject(list, JSONDateFormat);
        }
        [HttpGet]
        public ActionResult UpdatePerson(int id)
        {
            var person = list.Find(x => x.ID == id);
            return View(person);
        }
        [HttpPost]
        public string UpdatePerson(Person p)
        {
            var person = list.Find(x => x.ID == p.ID);
            var result = new Dictionary<string, object>();
            if (person != null)
            {
                list.Remove(person);
                list.Add(p);
                list = list.OrderBy(x => x.ID).ToList();
                result.Add("success", true);
                result.Add("data", p);
            }
            else
            {
                result.Add("success", false);
            }
            IsoDateTimeConverter JSONDateFormat = new IsoDateTimeConverter() { DateTimeFormat = "MM/dd/yyyy" };
            return Newtonsoft.Json.JsonConvert.SerializeObject(result, JSONDateFormat);
        }

        public ActionResult Buttons()
        {
            return View();
        }
         
    }
}

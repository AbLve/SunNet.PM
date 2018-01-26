using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Framework.Utils;

namespace SunNet.PMNew.Web
{
    [Serializable]
    public class Person
    {
        public Person(string name, string sex, int age)
        {
            Name = name;
            Sex = sex;
            Age = age;
        }
        public int Age;
        public string Name;
        public string Sex;
    }
    public partial class testJson : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<Person> list = new List<Person>();
                list.Add(new Person("Jack", "M", 20));
                list.Add(new Person("Hacks", "M", 20));
                string s = UtilFactory.Helpers.JSONHelper.GetJson<List<Person>>(list);
                TextBox1.Text = s;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            List<Person> list = UtilFactory.Helpers.JSONHelper.GetEntity<List<Person>>(TextBox1.Text);
            TextBox2.Text = list.Count.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
/**************************************************************************
 * Developer: 		Jack Zhang
 * Computer:		JACKZ
 * Domain:			Jackz
 * CreatedOn:		5/21 03:21:45
 * Description:		��չ��DropDownList�ؼ�
 * Version History:	Created,5/21 03:21:45
 * 
 * 
 **************************************************************************/
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.Framework.Controls
{
    public enum Mode
    {
        /// <summary>
        /// List����ģʽ��Ĭ�����ı���ʹ��DefaultAllText�ṩ���ı�
        /// </summary>
        List,
        /// <summary>
        /// Form����ģʽ��Ĭ�����ı���ʹ��DefaultSelectText�ṩ���ı�
        /// </summary>
        Form
    }

    /// <summary>
    /// ExtendedDropdownList�࣬�̳���DropDownList
    /// </summary>
    [ToolboxData(@"<{0}:ExtendedDropdownList runat='server'></{0}:ExtendedDropdownList>")]
    public class ExtendedDropdownList : DropDownList
    {
        private static object GetPropertyValue(string property, object o)
        {
            if (property == null)
                throw new ArgumentNullException("property");
            if (o == null)
                throw new ArgumentNullException("o");
            Type type = o.GetType();
            string[] propPath = property.Split('.');
            var propInfo = type.GetProperty(propPath[0]);
            if (propInfo == null)
                throw new Exception(String.Format("Could not find property '{0}' on type {1}.", propPath[0], type.FullName));

            object value = propInfo.GetValue(o, null);
            if (propPath.Length > 1)
                return GetPropertyValue(string.Join(".", propPath, 1, propPath.Length - 1), value);
            else
                return value;
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        public ExtendedDropdownList()
        {

        }

        [
        Browsable(true),
        Description("�������DropDownList�ķ������������"),
        Category("��չ")
        ]
        public virtual string DataGroupField
        {
            get
            {
                string s = (string)ViewState["DataGroupField"];
                return s ?? "";
            }
            set
            {
                ViewState["DataGroupField"] = value;
            }
        }

        /// <summary>
        /// �������SmartDropDownList�ķ������ListItem��Valueֵ
        /// </summary>
        [
        Browsable(true),
        Description("�������DropDownList�ķ������ListItem��Valueֵ,�԰�Ƕ��ŷָ�"),
        Category("��չ")
        ]
        public virtual string OptionGroupValues
        {
            get
            {
                string s = (string)ViewState["OptionGroupValue"];
                return s ?? "";
            }
            set
            {
                ViewState["OptionGroupValue"] = value;
            }
        }

        private List<string> _optGroups;
        public virtual List<string> OptionGroups
        {
            get
            {
                if (_optGroups == null || _optGroups.Count == 0)
                {
                    var groups = OptionGroupValues.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    _optGroups = groups.ToList();
                }
                return _optGroups ?? (_optGroups = new List<string>());
            }
            set { _optGroups = value; }
        }

        /// <summary>
        /// �û���ʶĬ����Ĺ���ģʽ
        /// </summary>
        public Mode DefaultMode
        {
            get
            {
                Mode s = (Mode)ViewState["DefaultMode"];
                return s;
            }
            set
            {
                ViewState["DefaultMode"] = value;
            }
        }

        /// <summary>
        /// ����Ĭ��ѡ��ʱ���ڱ�ʾѡ�����е���Ŀ�ı�
        /// </summary>
        public string DefaultAllText
        {
            get
            {
                string s = (string)ViewState["DefaultAllText"];
                return s ?? "";
            }
            set
            {
                ViewState["DefaultAllText"] = value;
            }
        }
        /// <summary>
        /// ����Ĭ��ѡ��ʱ���ڱ�ʾ�յ���Ŀ�ı�
        /// </summary>
        public string DefaultSelectText
        {
            get
            {
                string s = (string)ViewState["DefaultSelectText"];
                return s ?? "";
            }
            set
            {
                ViewState["DefaultSelectText"] = value;
            }
        }
        /// <summary>
        /// ����Ĭ��ѡ��ʱ��ֵ
        /// </summary>
        public string DefaultItemValue
        {
            get
            {
                string s = (string)ViewState["DefaultItemValue"];
                return s ?? "";
            }
            set
            {
                ViewState["DefaultItemValue"] = value;
            }
        }

        private string _value;
        public override string SelectedValue
        {
            get
            {
                return base.SelectedValue;
            }
            set
            {
                base.SelectedValue = value;
                _value = value;
            }
        }

        private void AddDefaultItem()
        {
            var defaulttext = this.DefaultMode == Mode.List ? DefaultAllText : DefaultSelectText;
            var needDefaultItem = this.Items.Count == 0 || this.Items.Count - OptionGroups.Count != 1;
            if (needDefaultItem && !string.IsNullOrEmpty(defaulttext))
            {
                // ���Ĭ��ѡ��
                this.Items.Insert(0, new ListItem(defaulttext, DefaultItemValue));
            }
        }

        public override void DataBind()
        {
            base.DataBind();
            this.AddDefaultItem();
        }

        /// <summary>
        /// ��ָ���ķ������Ͷ����������������������з����.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="groupExpression">�������.</param>
        /// <param name="groupRenameExpression">������������������.</param>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/21 21:42
        public void DataBind<T>(Func<T, string, bool> groupExpression, Func<string, string> groupRenameExpression)
        {
            if (string.IsNullOrEmpty(DataGroupField))
            {
                this.DataBind();
                return;
            }
            var field = DataGroupField;
            var datasource = this.DataSource as IEnumerable<T>;
            if (datasource != null)
            {
                IEnumerable<T> enumerable = datasource as T[] ?? datasource.ToArray();
                var groupBy = enumerable.Select((x) => GetPropertyValue(field, x).ToString()).Distinct().ToList<string>();
                groupBy.AddRange(OptionGroups);
                foreach (var groupName in groupBy)
                {
                    var items = (from ds in enumerable
                                 where groupExpression(ds, groupName)
                                 select ds).ToList<T>();
                    if (groupBy.Count > 1)
                    {
                        this.Items.Add(new ListItem(groupRenameExpression(groupName), groupName));
                        OptionGroups.Add(groupName);
                    }
                    foreach (var item in items)
                    {
                        var text = GetPropertyValue(DataTextField, item).ToString();
                        var value = GetPropertyValue(DataValueField, item).ToString();
                        this.Items.Add(new ListItem(text, value)
                        {
                            Selected = value == _value
                        });
                    }
                }
                OptionGroupValues = string.Join(",", OptionGroups.ToArray());
            }
            this.AddDefaultItem();
            base.SelectedValue = _value;
        }

        /// <summary>
        /// ��ָ���ķ���������������з����.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="groupExpression">�������.</param>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/21 21:40
        public void DataBind<T>(Func<T, string, bool> groupExpression)
        {
            this.DataBind(groupExpression, groupName => groupName);
        }

        /// <summary>
        /// ���ؼ������ݳ��ֵ�ָ���ı�д����
        /// </summary>
        /// <param name="writer">writer</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            //this.Items.RemoveAt(3);
            // ����Option��OptionGroup
            OptionGroupRenderContents(writer);
        }

        /// <summary>
        /// ����Option��OptionGroup
        /// </summary>
        /// <param name="writer">writer</param>
        private void OptionGroupRenderContents(HtmlTextWriter writer)
        {
            // �Ƿ���Ҫ����OptionGroup��EndTag
            bool writerEndTag = false;

            foreach (ListItem li in this.Items)
            {
                // ���û��optgroup���������Option
                if (!this.OptionGroups.Contains(li.Value))
                {
                    // ����Option
                    RenderListItem(li, writer);
                }
                // �����optgroup���������OptionGroup
                else
                {
                    if (writerEndTag)
                        // ����OptionGroup��EndTag
                        OptionGroupEndTag(writer);
                    else
                        writerEndTag = true;

                    // ����OptionGroup��BeginTag
                    OptionGroupBeginTag(li, writer);
                }
            }

            if (writerEndTag)
                // ����OptionGroup��EndTag
                OptionGroupEndTag(writer);
        }

        /// <summary>
        /// ����OptionGroup��BeginTag
        /// </summary>
        /// <param name="li">OptionGroup������</param>
        /// <param name="writer">writer</param>
        private void OptionGroupBeginTag(ListItem li, HtmlTextWriter writer)
        {
            writer.WriteBeginTag("optgroup");

            // д��OptionGroup��label
            writer.WriteAttribute("label", li.Text);
            writer.WriteAttribute("class", li.Text);
            foreach (string key in li.Attributes.Keys)
            {
                // д��OptionGroup����������
                writer.WriteAttribute(key, li.Attributes[key]);
            }

            writer.Write(HtmlTextWriter.TagRightChar);
            writer.WriteLine();
        }

        /// <summary>
        /// ����OptionGroup��EndTag
        /// </summary>
        /// <param name="writer">writer</param>
        private void OptionGroupEndTag(HtmlTextWriter writer)
        {
            writer.WriteEndTag("optgroup");
            writer.WriteLine();
        }

        /// <summary>
        /// ����Option
        /// </summary>
        /// <param name="li">Option������</param>
        /// <param name="writer">writer</param>
        private void RenderListItem(ListItem li, HtmlTextWriter writer)
        {
            writer.WriteBeginTag("option");

            // д��Option��Value
            writer.WriteAttribute("value", li.Value, true);

            if (li.Selected)
            {
                // �����Option��ѡ����д��selected
                writer.WriteAttribute("selected", "selected", false);
            }

            foreach (string key in li.Attributes.Keys)
            {
                // д��Option����������
                writer.WriteAttribute(key, li.Attributes[key]);
            }

            writer.Write(HtmlTextWriter.TagRightChar);

            // д��Option��Text
            HttpUtility.HtmlEncode(li.Text, writer);

            writer.WriteEndTag("option");
            writer.WriteLine();
        }
    }
}

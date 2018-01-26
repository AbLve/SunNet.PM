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
 * Description:		扩展的DropDownList控件
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
        /// List工作模式，默认项文本将使用DefaultAllText提供的文本
        /// </summary>
        List,
        /// <summary>
        /// Form工作模式，默认项文本将使用DefaultSelectText提供的文本
        /// </summary>
        Form
    }

    /// <summary>
    /// ExtendedDropdownList类，继承自DropDownList
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
        /// 构造函数
        /// </summary>
        public ExtendedDropdownList()
        {

        }

        [
        Browsable(true),
        Description("用于添加DropDownList的分组项的属性名"),
        Category("扩展")
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
        /// 用于添加SmartDropDownList的分组项的ListItem的Value值
        /// </summary>
        [
        Browsable(true),
        Description("用于添加DropDownList的分组项的ListItem的Value值,以半角逗号分隔"),
        Category("扩展")
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
        /// 用户标识默认项的工作模式
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
        /// 增加默认选项时用于表示选择所有的项目文本
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
        /// 增加默认选项时用于表示空的项目文本
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
        /// 增加默认选项时的值
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
                // 添加默认选项
                this.Items.Insert(0, new ListItem(defaulttext, DefaultItemValue));
            }
        }

        public override void DataBind()
        {
            base.DataBind();
            this.AddDefaultItem();
        }

        /// <summary>
        /// 以指定的分组规则和对组重命名规则对所有项进行分组绑定.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="groupExpression">分组规则.</param>
        /// <param name="groupRenameExpression">组名进行重命名规则.</param>
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
        /// 以指定的分组规则对所有项进行分组绑定.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="groupExpression">分组规则.</param>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/21 21:40
        public void DataBind<T>(Func<T, string, bool> groupExpression)
        {
            this.DataBind(groupExpression, groupName => groupName);
        }

        /// <summary>
        /// 将控件的内容呈现到指定的编写器中
        /// </summary>
        /// <param name="writer">writer</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            //this.Items.RemoveAt(3);
            // 呈现Option或OptionGroup
            OptionGroupRenderContents(writer);
        }

        /// <summary>
        /// 呈现Option或OptionGroup
        /// </summary>
        /// <param name="writer">writer</param>
        private void OptionGroupRenderContents(HtmlTextWriter writer)
        {
            // 是否需要呈现OptionGroup的EndTag
            bool writerEndTag = false;

            foreach (ListItem li in this.Items)
            {
                // 如果没有optgroup属性则呈现Option
                if (!this.OptionGroups.Contains(li.Value))
                {
                    // 呈现Option
                    RenderListItem(li, writer);
                }
                // 如果有optgroup属性则呈现OptionGroup
                else
                {
                    if (writerEndTag)
                        // 呈现OptionGroup的EndTag
                        OptionGroupEndTag(writer);
                    else
                        writerEndTag = true;

                    // 呈现OptionGroup的BeginTag
                    OptionGroupBeginTag(li, writer);
                }
            }

            if (writerEndTag)
                // 呈现OptionGroup的EndTag
                OptionGroupEndTag(writer);
        }

        /// <summary>
        /// 呈现OptionGroup的BeginTag
        /// </summary>
        /// <param name="li">OptionGroup数据项</param>
        /// <param name="writer">writer</param>
        private void OptionGroupBeginTag(ListItem li, HtmlTextWriter writer)
        {
            writer.WriteBeginTag("optgroup");

            // 写入OptionGroup的label
            writer.WriteAttribute("label", li.Text);
            writer.WriteAttribute("class", li.Text);
            foreach (string key in li.Attributes.Keys)
            {
                // 写入OptionGroup的其它属性
                writer.WriteAttribute(key, li.Attributes[key]);
            }

            writer.Write(HtmlTextWriter.TagRightChar);
            writer.WriteLine();
        }

        /// <summary>
        /// 呈现OptionGroup的EndTag
        /// </summary>
        /// <param name="writer">writer</param>
        private void OptionGroupEndTag(HtmlTextWriter writer)
        {
            writer.WriteEndTag("optgroup");
            writer.WriteLine();
        }

        /// <summary>
        /// 呈现Option
        /// </summary>
        /// <param name="li">Option数据项</param>
        /// <param name="writer">writer</param>
        private void RenderListItem(ListItem li, HtmlTextWriter writer)
        {
            writer.WriteBeginTag("option");

            // 写入Option的Value
            writer.WriteAttribute("value", li.Value, true);

            if (li.Selected)
            {
                // 如果该Option被选中则写入selected
                writer.WriteAttribute("selected", "selected", false);
            }

            foreach (string key in li.Attributes.Keys)
            {
                // 写入Option的其它属性
                writer.WriteAttribute(key, li.Attributes[key]);
            }

            writer.Write(HtmlTextWriter.TagRightChar);

            // 写入Option的Text
            HttpUtility.HtmlEncode(li.Text, writer);

            writer.WriteEndTag("option");
            writer.WriteLine();
        }
    }
}

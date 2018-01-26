using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;

namespace SF.Framework.Core.BrokenMessage
{
    public class BrokenMessageEnabled
    {
        private List<BrokenRuleMessage> brokenRuleMessages = new List<BrokenRuleMessage>();
        public void ClearBrokenRuleMessages()
        {
            brokenRuleMessages.Clear();
        }
        public void AddBrokenRuleMessage(string key, string message)
        {
            BrokenRuleMessage rm = new BrokenRuleMessage(key, message);
            this.brokenRuleMessages.Add(rm);
        }
        public void AddBrokenRuleMessage(BrokenRuleMessage msg)
        {
            this.brokenRuleMessages.Add(msg);
        }
        public void AddBrokenRuleMessages(List<BrokenRuleMessage> msgs)
        {
            this.brokenRuleMessages.AddRange(msgs);
        }
        public List<BrokenRuleMessage> BrokenRuleMessages
        {
            get
            {
                return this.brokenRuleMessages.AsReadOnly().ToList();
            }
        }
        public BrokenRuleMessage FirstBrokenRuleMessage
        {
            get
            {
                if (BrokenRuleMessages.Count > 0)
                    return BrokenRuleMessages[0];
                return null;
            }
        }
    }
}

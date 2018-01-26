using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SF.Framework.ResultMessages.ResultMessage;
using SF.Framework.Core.BrokenMessage;

namespace SF.Framework
{
    public enum ActionType
    {
        None = 0,
        Add = 1,
        Edit = 2,
        Ext = 3
    }

    public class ResultEntity
    {
        #region Constructor

        /// <summary>
        /// Default result entity of constructor.
        /// Create a object, the message is "Operation is successful."
        /// </summary>
        public ResultEntity()
        {
            List<IMessageEntity> defaultMsg = new List<IMessageEntity>() { 
                new MessageEntity("SuccessfulMsg","Operation is successful.")
            };

            CreateResultEntity(ActionType.Ext, true, defaultMsg);
        }

        /// <summary>
        /// Expand the result entity of constructor.
        /// Create a object, the message choose by enum ActionType.
        /// 
        /// ActionType.None: Unknown operation.
        /// ActionType.Add: Save is successful.
        /// ActionType.Edit: Update is successful.
        /// ActionType.Ext: Operation is successful.
        /// </summary>
        /// <param name="aType">Action type.</param>
        public ResultEntity(ActionType aType)
        {
            List<IMessageEntity> defaultMsg = new List<IMessageEntity>() { 
                new MessageEntity("SuccessfulMsg","Operation is successful.")
            };
            switch (aType)
            {
                case ActionType.None:
                    defaultMsg = new List<IMessageEntity>(){
                        new MessageEntity("NoneMsg","Unknown operation.")
                    };
                    break;
                case ActionType.Add:
                    defaultMsg = new List<IMessageEntity>(){
                        new MessageEntity("NoneMsg","Save is successful.")
                    };
                    break;
                case ActionType.Edit:
                    defaultMsg = new List<IMessageEntity>(){
                        new MessageEntity("NoneMsg","Update is successful.")
                    };
                    break;
                case ActionType.Ext:
                    defaultMsg = new List<IMessageEntity>(){
                        new MessageEntity("NoneMsg","Operation is successful.")
                    };
                    break;
            }

            CreateResultEntity(aType, true, defaultMsg);
        }

        /// <summary>
        /// Create new ResultEntity object.
        /// Create a object, the message create by parameter string msg.
        /// </summary>
        /// <param name="aType">Action type.</param>
        /// <param name="isSuccessful">Operation is successful or not.</param>
        /// <param name="msg">Operation returned message.</param>
        /// <param name="key">Primary key or what you want transmited. Default value is null.</param>
        public ResultEntity(ActionType aType, bool isSuccessful, string msg,
            Dictionary<string, object> dictionary = null, object key = null)
        {
            CreateResultEntity(aType, isSuccessful, msg, dictionary, key);
        }

        /// <summary>
        /// Create new ResultEntity object.
        /// Create a object, the message create by parameter List<MessageEntity> msgEntityList.
        /// </summary>
        /// <param name="aType">Action type.</param>
        /// <param name="isSuccessful">Operation is successful or not.</param>
        /// <param name="msgEntityList">Operation returned messages.</param>
        /// <param name="key">Primary key or what you want transmited. Default value is null.</param>
        public ResultEntity(ActionType aType, bool isSuccessful, List<MessageEntity> msgEntityList,
            Dictionary<string, object> dictionary = null, object key = null)
        {
            CreateResultEntity(aType, isSuccessful, msgEntityList, dictionary, key);
        }

        /// <summary>
        /// Create new ResultEntity object.
        /// Create a object, the message create by parameter List<BrokenRuleMessage> brokenRuleMsgList.
        /// </summary>
        /// <param name="aType">Action type.</param>
        /// <param name="isSuccessful">Operation is successful or not.</param>
        /// <param name="brokenRuleMsgList">Operation returned messages.</param>
        /// <param name="key">Primary key or what you want transmited. Default value is null.</param>
        public ResultEntity(
            ActionType aType, bool isSuccessful, List<BrokenRuleMessage> brokenRuleMsgList,
            Dictionary<string, object> dictionary = null, object key = null)
        {
            CreateResultEntity(aType, isSuccessful, brokenRuleMsgList, dictionary, key);
        }

        /// <summary>
        /// Create new ResultEntity object.
        /// Create a object, the message create by parameter List<string> strMsgList.
        /// </summary>
        /// <param name="aType">Action type.</param>
        /// <param name="isSuccessful">Operation is successful or not.</param>
        /// <param name="strMsgList">Operation returned messages.</param>
        /// <param name="key">Primary key or what you want transmited. Default value is null.</param>
        public ResultEntity(ActionType aType, bool isSuccessful, List<string> strMsgList,
            Dictionary<string, object> dictionary = null, object key = null)
        {
            CreateResultEntity(aType, isSuccessful, strMsgList, dictionary, key);
        }

        /// <summary>
        /// Expand the result entity of constructor.
        /// Create a object, the message create by parameter List<IMessageEntity> msgList.
        /// </summary>
        /// <param name="aType">Action type.</param>
        /// <param name="isSuccessful">Operation is successful or not.</param>
        /// <param name="msgList">Operation returned messages.</param>
        /// <param name="key">Primary key or what you want transmited. Default value is null.</param>
        public ResultEntity(ActionType aType, bool isSuccessful, List<IMessageEntity> msgList,
            Dictionary<string, object> dictionary = null, object key = null)
        {
            CreateResultEntity(aType, isSuccessful, msgList, dictionary, key);
        }

        #endregion

        #region Properties

        private object _key;

        /// <summary>
        /// Primary key
        /// </summary>
        public object Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
                List<string> msgs = this.AllMsg == null ? null : this.AllMsg.Select(m => m.Message).ToList();
                this.AllJSONResult = ParseJsonString(this.aType, this.IsSuccessful, msgs, false, this.dictionary, _key);
                this.FirstJSONResult = ParseJsonString(this.aType, this.IsSuccessful, msgs, true, this.dictionary, _key);
            }
        }

        /// <summary>
        /// Status
        /// </summary>
        /// 
        public bool IsSuccessful { get; private set; }

        /// <summary>
        /// The first BrokenRuleMessage
        /// </summary>
        public IMessageEntity FirstMsg { get; private set; }

        /// <summary>
        /// The first result, format to Json.
        /// </summary>
        public string FirstJSONResult { get; private set; }

        /// <summary>
        /// All result, format to Json.
        /// </summary>
        public string AllJSONResult { get; private set; }

        /// <summary>
        /// All BrokenRuleMessage.
        /// </summary>
        public List<IMessageEntity> AllMsg { get; private set; }

        /// <summary>
        /// The type for operate.
        /// </summary>
        private ActionType aType { get; set; }

        private Dictionary<string, object> _dictionary;

        /// <summary>
        /// Extend attribute for json result.
        /// </summary>
        Dictionary<string, object> dictionary
        {
            get
            {
                return _dictionary;
            }
            set
            {
                _dictionary = value;
                List<string> msgs = this.AllMsg == null ? null : this.AllMsg.Select(m => m.Message).ToList();
                this.AllJSONResult = ParseJsonString(this.aType, this.IsSuccessful, msgs, false, _dictionary, this.Key);
                this.FirstJSONResult = ParseJsonString(this.aType, this.IsSuccessful, msgs, true, _dictionary, this.Key);
            }
        }

        #endregion

        #region Ext Methods

        /// <summary>
        /// Create new ResultEntity object.
        /// Reset old object, the message create by parameter string msg.
        /// </summary>
        /// <param name="aType">Action type.</param>
        /// <param name="isSuccessful">Operation is successful or not.</param>
        /// <param name="msg">Operation returned message.</param>
        /// <param name="key">Primary key or what you want transmited. Default value is null.</param>
        public void CreateResultEntity(ActionType aType, bool isSuccessful, string msg,
            Dictionary<string, object> dictionary = null, object key = null)
        {
            List<IMessageEntity> list = new List<IMessageEntity>();
            list.Add(new MessageEntity(Enum.GetName(typeof(ActionType), aType) + isSuccessful.ToString(), msg));

            CreateResultEntity(aType, isSuccessful, list, dictionary, key);
        }

        /// <summary>
        /// Create new ResultEntity object.
        /// Reset old object, the message create by parameter List<MessageEntity> msgEntityList.
        /// </summary>
        /// <param name="aType">Action type.</param>
        /// <param name="isSuccessful">Operation is successful or not.</param>
        /// <param name="msgEntityList">Operation returned messages.</param>
        /// <param name="key">Primary key or what you want transmited. Default value is null.</param>
        public void CreateResultEntity(
            ActionType aType, bool isSuccessful, List<MessageEntity> msgEntityList,
            Dictionary<string, object> dictionary = null, object key = null)
        {
            List<IMessageEntity> list = new List<IMessageEntity>();
            list.AddRange(msgEntityList);

            CreateResultEntity(aType, isSuccessful, list, dictionary, key);
        }

        /// <summary>
        /// Create new ResultEntity object.
        /// Reset old object, the message create by parameter List<BrokenRuleMessage> brokenRuleMsgList.
        /// </summary>
        /// <param name="aType">Action type.</param>
        /// <param name="isSuccessful">Operation is successful or not.</param>
        /// <param name="brokenRuleMsgList">Operation returned messages.</param>
        /// <param name="key">Primary key or what you want transmited. Default value is null.</param>
        public void CreateResultEntity(
            ActionType aType, bool isSuccessful, List<BrokenRuleMessage> brokenRuleMsgList,
            Dictionary<string, object> dictionary = null, object key = null)
        {
            List<IMessageEntity> list = new List<IMessageEntity>();
            list.AddRange(brokenRuleMsgList);

            CreateResultEntity(aType, isSuccessful, list, dictionary, key);
        }

        /// <summary>
        /// Create new ResultEntity object.
        /// Reset old object, the message create by parameter List<string> strMsgList.
        /// </summary>
        /// <param name="aType">Action type.</param>
        /// <param name="isSuccessful">Operation is successful or not.</param>
        /// <param name="strMsgList">Operation returned messages.</param>
        /// <param name="key">Primary key or what you want transmited. Default value is null.</param>
        public void CreateResultEntity(
            ActionType aType, bool isSuccessful, List<string> strMsgList,
            Dictionary<string, object> dictionary = null, object key = null)
        {
            List<IMessageEntity> list = new List<IMessageEntity>();

            for (int i = 0; i < strMsgList.Count; i++)
            {
                list.Add(new MessageEntity(
                    Enum.GetName(typeof(ActionType), aType) + isSuccessful.ToString() + i, strMsgList[i]
                ));
            }

            CreateResultEntity(aType, isSuccessful, list, dictionary, key);
        }

        /// <summary>
        /// Create new ResultEntity object.
        /// Reset old object, the message create by parameter List<IMessageEntity> msgList.
        /// </summary>
        /// <param name="aType">Action type.</param>
        /// <param name="isSuccessful">Operation is successful or not.</param>
        /// <param name="iMsgEntityList">Operation returned messages.</param>
        /// <param name="key">Primary key or what you want transmited. Default value is null.</param>
        public void CreateResultEntity(
            ActionType aType, bool isSuccessful, List<IMessageEntity> iMsgEntityList,
            Dictionary<string, object> dictionary = null, object key = null)
        {
            List<string> msgs = iMsgEntityList == null ? null : iMsgEntityList.Select(m => m.Message).ToList();

            this.IsSuccessful = isSuccessful;
            if (iMsgEntityList != null && iMsgEntityList.Count > 0)
            {
                this.FirstMsg = iMsgEntityList[0];
                this.AllMsg = iMsgEntityList;
            }
            this.aType = aType;
            this.Key = key;
            this.AllJSONResult = ParseJsonString(aType, IsSuccessful, msgs, false, dictionary, key);
            this.FirstJSONResult = ParseJsonString(aType, IsSuccessful, msgs, true, dictionary, key);
        }

        /// <summary>
        /// Add one or more message for existing MessageEntity object.
        /// </summary>
        /// <param name="extMsgList">Existing messages.</param>
        public void AddMsgToObject(List<IMessageEntity> extMsgList, Dictionary<string, object> dictionary = null)
        {
            this.AllMsg.AddRange(extMsgList);
            List<string> msgs = this.AllMsg == null ? null : this.AllMsg.Select(m => m.Message).ToList();
            this.AllJSONResult = ParseJsonString(aType, IsSuccessful, msgs, false, dictionary, this.Key);
            this.FirstJSONResult = ParseJsonString(aType, IsSuccessful, msgs, true, dictionary, this.Key);
        }

        #endregion

        #region Tools

        /// <summary>
        /// Parse the result entity to stirng Json format.
        /// </summary>
        /// <param name="aType">Action type.</param>
        /// <param name="isSuccessful">Operation is successful or not.</param>
        /// <param name="msgList">Operation returned messages.</param>
        /// <param name="GetFirst">Get first message or not.</param>
        /// <param name="key">Primary key or what you want transmited. Default value is null.</param>
        private string ParseJsonString(
            ActionType aType, bool isSuccessful, List<string> msgList, bool GetFirst,
            Dictionary<string, object> dictionary = null, object key = null)
        {
            string type = Enum.GetName(typeof(ActionType), aType);

            var result = new Dictionary<string, object>();
            result.Add("success", isSuccessful);
            result.Add("type", type);

            if (dictionary != null && dictionary.Count > 0)
            {
                foreach (var item in dictionary)
                {
                    result.Add(item.Key, item.Value);
                }
            }

            if (key != null)
            {
                result.Add("key", key);
            }

            if (GetFirst)
            {
                result.Add("msg", msgList != null && msgList.Count > 0 ? msgList[0] : "");
            }
            else
            {
                result.Add("msg", msgList != null ? msgList : null);
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        #endregion

    }
}

﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18444
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SunNet.PMNew.EmailSender.TimeSheetNoticeManager {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://pm.sunnet.us/", ConfigurationName="TimeSheetNoticeManager.TimeSheetNotcieSoap")]
    public interface TimeSheetNotcieSoap {
        
        // CODEGEN: 命名空间 http://pm.sunnet.us/ 的元素名称 password 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://pm.sunnet.us/SendTimeSheetEmail", ReplyAction="*")]
        SunNet.PMNew.EmailSender.TimeSheetNoticeManager.SendTimeSheetEmailResponse SendTimeSheetEmail(SunNet.PMNew.EmailSender.TimeSheetNoticeManager.SendTimeSheetEmailRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SendTimeSheetEmailRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SendTimeSheetEmail", Namespace="http://pm.sunnet.us/", Order=0)]
        public SunNet.PMNew.EmailSender.TimeSheetNoticeManager.SendTimeSheetEmailRequestBody Body;
        
        public SendTimeSheetEmailRequest() {
        }
        
        public SendTimeSheetEmailRequest(SunNet.PMNew.EmailSender.TimeSheetNoticeManager.SendTimeSheetEmailRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://pm.sunnet.us/")]
    public partial class SendTimeSheetEmailRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string password;
        
        public SendTimeSheetEmailRequestBody() {
        }
        
        public SendTimeSheetEmailRequestBody(string password) {
            this.password = password;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SendTimeSheetEmailResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SendTimeSheetEmailResponse", Namespace="http://pm.sunnet.us/", Order=0)]
        public SunNet.PMNew.EmailSender.TimeSheetNoticeManager.SendTimeSheetEmailResponseBody Body;
        
        public SendTimeSheetEmailResponse() {
        }
        
        public SendTimeSheetEmailResponse(SunNet.PMNew.EmailSender.TimeSheetNoticeManager.SendTimeSheetEmailResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://pm.sunnet.us/")]
    public partial class SendTimeSheetEmailResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string SendTimeSheetEmailResult;
        
        public SendTimeSheetEmailResponseBody() {
        }
        
        public SendTimeSheetEmailResponseBody(string SendTimeSheetEmailResult) {
            this.SendTimeSheetEmailResult = SendTimeSheetEmailResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface TimeSheetNotcieSoapChannel : SunNet.PMNew.EmailSender.TimeSheetNoticeManager.TimeSheetNotcieSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class TimeSheetNotcieSoapClient : System.ServiceModel.ClientBase<SunNet.PMNew.EmailSender.TimeSheetNoticeManager.TimeSheetNotcieSoap>, SunNet.PMNew.EmailSender.TimeSheetNoticeManager.TimeSheetNotcieSoap {
        
        public TimeSheetNotcieSoapClient() {
        }
        
        public TimeSheetNotcieSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public TimeSheetNotcieSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TimeSheetNotcieSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TimeSheetNotcieSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SunNet.PMNew.EmailSender.TimeSheetNoticeManager.SendTimeSheetEmailResponse SunNet.PMNew.EmailSender.TimeSheetNoticeManager.TimeSheetNotcieSoap.SendTimeSheetEmail(SunNet.PMNew.EmailSender.TimeSheetNoticeManager.SendTimeSheetEmailRequest request) {
            return base.Channel.SendTimeSheetEmail(request);
        }
        
        public string SendTimeSheetEmail(string password) {
            SunNet.PMNew.EmailSender.TimeSheetNoticeManager.SendTimeSheetEmailRequest inValue = new SunNet.PMNew.EmailSender.TimeSheetNoticeManager.SendTimeSheetEmailRequest();
            inValue.Body = new SunNet.PMNew.EmailSender.TimeSheetNoticeManager.SendTimeSheetEmailRequestBody();
            inValue.Body.password = password;
            SunNet.PMNew.EmailSender.TimeSheetNoticeManager.SendTimeSheetEmailResponse retVal = ((SunNet.PMNew.EmailSender.TimeSheetNoticeManager.TimeSheetNotcieSoap)(this)).SendTimeSheetEmail(inValue);
            return retVal.Body.SendTimeSheetEmailResult;
        }
    }
}

﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SunNet.PMNew.PM2014.OperateDocManagements {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="OperateDocManagements.OperateDocManagementSoap")]
    public interface OperateDocManagementSoap {
        
        // CODEGEN: Generating message contract since element name value from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/AddDocManagement", ReplyAction="*")]
        SunNet.PMNew.PM2014.OperateDocManagements.AddDocManagementResponse AddDocManagement(SunNet.PMNew.PM2014.OperateDocManagements.AddDocManagementRequest request);
        
        // CODEGEN: Generating message contract since element name GetFileInfoResult from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetFileInfo", ReplyAction="*")]
        SunNet.PMNew.PM2014.OperateDocManagements.GetFileInfoResponse GetFileInfo(SunNet.PMNew.PM2014.OperateDocManagements.GetFileInfoRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class AddDocManagementRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="AddDocManagement", Namespace="http://tempuri.org/", Order=0)]
        public SunNet.PMNew.PM2014.OperateDocManagements.AddDocManagementRequestBody Body;
        
        public AddDocManagementRequest() {
        }
        
        public AddDocManagementRequest(SunNet.PMNew.PM2014.OperateDocManagements.AddDocManagementRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class AddDocManagementRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string value;
        
        public AddDocManagementRequestBody() {
        }
        
        public AddDocManagementRequestBody(string value) {
            this.value = value;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class AddDocManagementResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="AddDocManagementResponse", Namespace="http://tempuri.org/", Order=0)]
        public SunNet.PMNew.PM2014.OperateDocManagements.AddDocManagementResponseBody Body;
        
        public AddDocManagementResponse() {
        }
        
        public AddDocManagementResponse(SunNet.PMNew.PM2014.OperateDocManagements.AddDocManagementResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class AddDocManagementResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public bool AddDocManagementResult;
        
        public AddDocManagementResponseBody() {
        }
        
        public AddDocManagementResponseBody(bool AddDocManagementResult) {
            this.AddDocManagementResult = AddDocManagementResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetFileInfoRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetFileInfo", Namespace="http://tempuri.org/", Order=0)]
        public SunNet.PMNew.PM2014.OperateDocManagements.GetFileInfoRequestBody Body;
        
        public GetFileInfoRequest() {
        }
        
        public GetFileInfoRequest(SunNet.PMNew.PM2014.OperateDocManagements.GetFileInfoRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetFileInfoRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public int id;
        
        public GetFileInfoRequestBody() {
        }
        
        public GetFileInfoRequestBody(int id) {
            this.id = id;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetFileInfoResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetFileInfoResponse", Namespace="http://tempuri.org/", Order=0)]
        public SunNet.PMNew.PM2014.OperateDocManagements.GetFileInfoResponseBody Body;
        
        public GetFileInfoResponse() {
        }
        
        public GetFileInfoResponse(SunNet.PMNew.PM2014.OperateDocManagements.GetFileInfoResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetFileInfoResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetFileInfoResult;
        
        public GetFileInfoResponseBody() {
        }
        
        public GetFileInfoResponseBody(string GetFileInfoResult) {
            this.GetFileInfoResult = GetFileInfoResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface OperateDocManagementSoapChannel : SunNet.PMNew.PM2014.OperateDocManagements.OperateDocManagementSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class OperateDocManagementSoapClient : System.ServiceModel.ClientBase<SunNet.PMNew.PM2014.OperateDocManagements.OperateDocManagementSoap>, SunNet.PMNew.PM2014.OperateDocManagements.OperateDocManagementSoap {
        
        public OperateDocManagementSoapClient() {
        }
        
        public OperateDocManagementSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public OperateDocManagementSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public OperateDocManagementSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public OperateDocManagementSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SunNet.PMNew.PM2014.OperateDocManagements.AddDocManagementResponse SunNet.PMNew.PM2014.OperateDocManagements.OperateDocManagementSoap.AddDocManagement(SunNet.PMNew.PM2014.OperateDocManagements.AddDocManagementRequest request) {
            return base.Channel.AddDocManagement(request);
        }
        
        public bool AddDocManagement(string value) {
            SunNet.PMNew.PM2014.OperateDocManagements.AddDocManagementRequest inValue = new SunNet.PMNew.PM2014.OperateDocManagements.AddDocManagementRequest();
            inValue.Body = new SunNet.PMNew.PM2014.OperateDocManagements.AddDocManagementRequestBody();
            inValue.Body.value = value;
            SunNet.PMNew.PM2014.OperateDocManagements.AddDocManagementResponse retVal = ((SunNet.PMNew.PM2014.OperateDocManagements.OperateDocManagementSoap)(this)).AddDocManagement(inValue);
            return retVal.Body.AddDocManagementResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SunNet.PMNew.PM2014.OperateDocManagements.GetFileInfoResponse SunNet.PMNew.PM2014.OperateDocManagements.OperateDocManagementSoap.GetFileInfo(SunNet.PMNew.PM2014.OperateDocManagements.GetFileInfoRequest request) {
            return base.Channel.GetFileInfo(request);
        }
        
        public string GetFileInfo(int id) {
            SunNet.PMNew.PM2014.OperateDocManagements.GetFileInfoRequest inValue = new SunNet.PMNew.PM2014.OperateDocManagements.GetFileInfoRequest();
            inValue.Body = new SunNet.PMNew.PM2014.OperateDocManagements.GetFileInfoRequestBody();
            inValue.Body.id = id;
            SunNet.PMNew.PM2014.OperateDocManagements.GetFileInfoResponse retVal = ((SunNet.PMNew.PM2014.OperateDocManagements.OperateDocManagementSoap)(this)).GetFileInfo(inValue);
            return retVal.Body.GetFileInfoResult;
        }
    }
}

﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ophelia.Integration.C2CMarket.GittiGidiyor.CommunityAuthService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://auth.community.ws.listingapi.gg.com", ConfigurationName="CommunityAuthService.CommunityAuthService")]
    public interface CommunityAuthService {
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(baseResponse))]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        Ophelia.Integration.C2CMarket.GittiGidiyor.CommunityAuthService.apiTokenServiceResponse createToken(string apiKey, string sign, long time, string lang);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        System.Threading.Tasks.Task<Ophelia.Integration.C2CMarket.GittiGidiyor.CommunityAuthService.apiTokenServiceResponse> createTokenAsync(string apiKey, string sign, long time, string lang);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(baseResponse))]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        string getServiceName();
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        System.Threading.Tasks.Task<string> getServiceNameAsync();
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://auth.community.ws.listingapi.gg.com")]
    public partial class apiTokenServiceResponse : baseResponse {
        
        private string tokenIdField;
        
        private string nickField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string tokenId {
            get {
                return this.tokenIdField;
            }
            set {
                this.tokenIdField = value;
                this.RaisePropertyChanged("tokenId");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string nick {
            get {
                return this.nickField;
            }
            set {
                this.nickField = value;
                this.RaisePropertyChanged("nick");
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(apiTokenServiceResponse))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://auth.community.ws.listingapi.gg.com")]
    public partial class baseResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string ackCodeField;
        
        private string responseTimeField;
        
        private errorType errorField;
        
        private string timeElapsedField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string ackCode {
            get {
                return this.ackCodeField;
            }
            set {
                this.ackCodeField = value;
                this.RaisePropertyChanged("ackCode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string responseTime {
            get {
                return this.responseTimeField;
            }
            set {
                this.responseTimeField = value;
                this.RaisePropertyChanged("responseTime");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public errorType error {
            get {
                return this.errorField;
            }
            set {
                this.errorField = value;
                this.RaisePropertyChanged("error");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string timeElapsed {
            get {
                return this.timeElapsedField;
            }
            set {
                this.timeElapsedField = value;
                this.RaisePropertyChanged("timeElapsed");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://auth.community.ws.listingapi.gg.com")]
    public partial class errorType : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string errorIdField;
        
        private string errorCodeField;
        
        private string messageField;
        
        private string viewMessageField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string errorId {
            get {
                return this.errorIdField;
            }
            set {
                this.errorIdField = value;
                this.RaisePropertyChanged("errorId");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string errorCode {
            get {
                return this.errorCodeField;
            }
            set {
                this.errorCodeField = value;
                this.RaisePropertyChanged("errorCode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
                this.RaisePropertyChanged("message");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string viewMessage {
            get {
                return this.viewMessageField;
            }
            set {
                this.viewMessageField = value;
                this.RaisePropertyChanged("viewMessage");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface CommunityAuthServiceChannel : Ophelia.Integration.C2CMarket.GittiGidiyor.CommunityAuthService.CommunityAuthService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CommunityAuthServiceClient : System.ServiceModel.ClientBase<Ophelia.Integration.C2CMarket.GittiGidiyor.CommunityAuthService.CommunityAuthService>, Ophelia.Integration.C2CMarket.GittiGidiyor.CommunityAuthService.CommunityAuthService {
        
        public CommunityAuthServiceClient() {
        }
        
        public CommunityAuthServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CommunityAuthServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CommunityAuthServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CommunityAuthServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Ophelia.Integration.C2CMarket.GittiGidiyor.CommunityAuthService.apiTokenServiceResponse createToken(string apiKey, string sign, long time, string lang) {
            return base.Channel.createToken(apiKey, sign, time, lang);
        }
        
        public System.Threading.Tasks.Task<Ophelia.Integration.C2CMarket.GittiGidiyor.CommunityAuthService.apiTokenServiceResponse> createTokenAsync(string apiKey, string sign, long time, string lang) {
            return base.Channel.createTokenAsync(apiKey, sign, time, lang);
        }
        
        public string getServiceName() {
            return base.Channel.getServiceName();
        }
        
        public System.Threading.Tasks.Task<string> getServiceNameAsync() {
            return base.Channel.getServiceNameAsync();
        }
    }
}

﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ophelia.Integration.C2CMarket.GittiGidiyor.IndividualCargoService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://cargo.individual.ws.listingapi.gg.com", ConfigurationName="IndividualCargoService.IndividualCargoService")]
    public interface IndividualCargoService {
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(baseResponse))]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        Ophelia.Integration.C2CMarket.GittiGidiyor.IndividualCargoService.cargoInformationResponse getCargoInformation(string apiKey, string sign, long time, string saleCode, string lang);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        System.Threading.Tasks.Task<Ophelia.Integration.C2CMarket.GittiGidiyor.IndividualCargoService.cargoInformationResponse> getCargoInformationAsync(string apiKey, string sign, long time, string saleCode, string lang);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(baseResponse))]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        string getServiceName();
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        System.Threading.Tasks.Task<string> getServiceNameAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(baseResponse))]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        Ophelia.Integration.C2CMarket.GittiGidiyor.IndividualCargoService.commonSaleResponse sendCargoInformation(string apiKey, string sign, long time, string saleCode, string cargoPostCode, string cargoCompany, string cargoBranch, string followUpUrl, string userType, string lang);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        System.Threading.Tasks.Task<Ophelia.Integration.C2CMarket.GittiGidiyor.IndividualCargoService.commonSaleResponse> sendCargoInformationAsync(string apiKey, string sign, long time, string saleCode, string cargoPostCode, string cargoCompany, string cargoBranch, string followUpUrl, string userType, string lang);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cargo.individual.ws.listingapi.gg.com")]
    public partial class cargoInformationResponse : baseResponse {
        
        private string cargoPostCodeField;
        
        private string cargoContentField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string cargoPostCode {
            get {
                return this.cargoPostCodeField;
            }
            set {
                this.cargoPostCodeField = value;
                this.RaisePropertyChanged("cargoPostCode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string cargoContent {
            get {
                return this.cargoContentField;
            }
            set {
                this.cargoContentField = value;
                this.RaisePropertyChanged("cargoContent");
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(cargoInformationResponse))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(commonSaleResponse))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cargo.individual.ws.listingapi.gg.com")]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cargo.individual.ws.listingapi.gg.com")]
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
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cargo.individual.ws.listingapi.gg.com")]
    public partial class commonSaleResponse : baseResponse {
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IndividualCargoServiceChannel : Ophelia.Integration.C2CMarket.GittiGidiyor.IndividualCargoService.IndividualCargoService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class IndividualCargoServiceClient : System.ServiceModel.ClientBase<Ophelia.Integration.C2CMarket.GittiGidiyor.IndividualCargoService.IndividualCargoService>, Ophelia.Integration.C2CMarket.GittiGidiyor.IndividualCargoService.IndividualCargoService {
        
        public IndividualCargoServiceClient() {
        }
        
        public IndividualCargoServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public IndividualCargoServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IndividualCargoServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IndividualCargoServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Ophelia.Integration.C2CMarket.GittiGidiyor.IndividualCargoService.cargoInformationResponse getCargoInformation(string apiKey, string sign, long time, string saleCode, string lang) {
            return base.Channel.getCargoInformation(apiKey, sign, time, saleCode, lang);
        }
        
        public System.Threading.Tasks.Task<Ophelia.Integration.C2CMarket.GittiGidiyor.IndividualCargoService.cargoInformationResponse> getCargoInformationAsync(string apiKey, string sign, long time, string saleCode, string lang) {
            return base.Channel.getCargoInformationAsync(apiKey, sign, time, saleCode, lang);
        }
        
        public string getServiceName() {
            return base.Channel.getServiceName();
        }
        
        public System.Threading.Tasks.Task<string> getServiceNameAsync() {
            return base.Channel.getServiceNameAsync();
        }
        
        public Ophelia.Integration.C2CMarket.GittiGidiyor.IndividualCargoService.commonSaleResponse sendCargoInformation(string apiKey, string sign, long time, string saleCode, string cargoPostCode, string cargoCompany, string cargoBranch, string followUpUrl, string userType, string lang) {
            return base.Channel.sendCargoInformation(apiKey, sign, time, saleCode, cargoPostCode, cargoCompany, cargoBranch, followUpUrl, userType, lang);
        }
        
        public System.Threading.Tasks.Task<Ophelia.Integration.C2CMarket.GittiGidiyor.IndividualCargoService.commonSaleResponse> sendCargoInformationAsync(string apiKey, string sign, long time, string saleCode, string cargoPostCode, string cargoCompany, string cargoBranch, string followUpUrl, string userType, string lang) {
            return base.Channel.sendCargoInformationAsync(apiKey, sign, time, saleCode, cargoPostCode, cargoCompany, cargoBranch, followUpUrl, userType, lang);
        }
    }
}

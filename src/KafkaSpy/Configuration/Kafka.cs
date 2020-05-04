using System;
using Confluent.Kafka;

namespace KafkaSpy.Configuration
{
    public class KafkaConfiguration //TODO:Cambiar esto por kafkaClientConfig
    {
        public String BootstrapServers { get; set; }

        public SecurityProtocol? SecurityProtocol { get; set; } //Plaintext,Ssl,SaslPlaintext,SaslSsl
        public bool? ApiVersionRequest { get; set; }
        public string SaslKerberosServiceName { get; set; }
        public string SaslKerberosKeytab { get; set; }
        public string SaslKerberosPrincipal { get; set; }

        public ClientConfig BuildClientConfig(){
            var clientConfig = new ClientConfig()
            {
                BootstrapServers = BootstrapServers,
                SecurityProtocol = SecurityProtocol,
                ApiVersionRequest = ApiVersionRequest,                
            };
            if (!string.IsNullOrWhiteSpace(SaslKerberosServiceName))
                clientConfig.SaslKerberosServiceName = SaslKerberosServiceName;
            if (!string.IsNullOrWhiteSpace(SaslKerberosKeytab))
                clientConfig.SaslKerberosKeytab= SaslKerberosKeytab;
            if (!string.IsNullOrWhiteSpace(SaslKerberosPrincipal))
                clientConfig.SaslKerberosPrincipal= SaslKerberosPrincipal;
            
            return clientConfig;
        }
        
    }   

    
}

using System.Runtime.Serialization;

namespace WebServiceClient.Base.WebService
{
    [DataContract]
    public class Request<T>
    {
        public Request()
        {
            
        }
        [DataMember]
        public string UserGuid { get; set; }
        [DataMember]
        public T Data { get; set; }
    }
}
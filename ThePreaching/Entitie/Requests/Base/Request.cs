using System.Runtime.Serialization;

namespace Entitie.Requests.Base
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

    [DataContract]
    public class Request
    {
        [DataMember]
        public string UserGuid { get; set; }
    }
}
using System.ServiceModel;
using System.ServiceModel.Web;
using Entitie.Language;

namespace PreachingServer.Server.WebService.Rest
{
    [ServiceContract(Name="RestService")]
    public interface IRestService
    {
        [OperationContract]
        [WebGet(UriTemplate = Routing.GetClientRoute, BodyStyle = WebMessageBodyStyle.Bare)]
        string GetClientNameById(string id);

        [OperationContract]
        [WebGet(UriTemplate = Routing.GetClientRoute, BodyStyle = WebMessageBodyStyle.Bare)]
        void GetUserDefaultLanguage(LanguageEnum language);
    }
}
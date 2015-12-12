using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using Entitie.Language;
using PreachingServer.Base.Result;
using PreachingServer.Server.WebService.Base;

namespace PreachingServer.Server.WebService.Rest
{
    [ServiceContract(Name="RestService")]
    public interface IRestService
    {
        [OperationContract]
        [WebGet(UriTemplate = Routing.GetClientRoute, BodyStyle = WebMessageBodyStyle.Bare)]
        LanguageEnum GetClientNameById(string id);

        /// <summary>
        /// Saves user default language
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method  = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "GetUserDefaultLanguage")]
        Response GetUserDefaultLanguage(Request<LanguageEnum> request);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "GetMultiCastAddressForLanguage")]
        Response<string> GetMultiCastAddressForLanguage(Request<LanguageEnum> request);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "UpdateTranslatorDate")]
        Response UpdateTranslatorDate(Request<LanguageEnum> request);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "UpdateUserHearingDate")]
        Response UpdateUserHearingDate(Request<LanguageEnum> request);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "GetTranslatedLanguages")]
        Response<IList<LanguageEnum>> GetTranslatedLanguages(Request request);
    }
}
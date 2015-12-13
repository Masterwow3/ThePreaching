using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Entitie.Language;
using Entitie.Requests.Base;
using PreachingServer.Server.BL;

namespace PreachingServer.Server.WebService.Rest
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode =AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestService:IRestService
    {
        public LanguageEnum GetClientNameById(string id)
        {
            return LanguageEnum.Afrikaans;
        }

        public Response GetUserDefaultLanguage(Request<LanguageEnum> request)
        {
            var bl = new UserBL();
            bl.Save(request);
            return new Response();
        }

        public Response<string> GetMultiCastAddressForLanguage(Request<LanguageEnum> request)
        {
            var bl = new MulticastBL();
            return new Response<string>() {Data= bl.GetAddressFromLanguage(request.Data)};
        }

        public Response UpdateTranslatorDate(Request<LanguageEnum> request)
        {
            var bl = new CurrentTranslatorsBL();
            bl.UpdateTranslatorDate(request);
            return new Response();
        }

        public Response UpdateUserHearingDate(Request<LanguageEnum> request)
        {
            var bl = new CurrentTranslatorsBL();
            bl.UpdateUserHearingDate(request);
            return new Response();
        }

        public Response<IList<LanguageEnum>> GetTranslatedLanguages(Request request)
        {
            var bl = new CurrentTranslatorsBL();
            var languages = bl.GetTranslatedLanguages();
            return new Response<IList<LanguageEnum>>() {Data = languages};
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Entitie.Language;
using Entitie.Translator;
using Entitie.User;
using PreachingServer.Server.WebService.Base;

namespace PreachingServer.Server.BL
{
    public class CurrentTranslatorsBL
    {
        public static ObservableCollection<TranslatedLanguage> TranslatedLanguages { get; set; }
        private TimeSpan RemoveHearingUsers;
        private TimeSpan RemoveTranslators;
        public CurrentTranslatorsBL()
        {
            if (TranslatedLanguages == null)
                TranslatedLanguages = new ObservableCollection<TranslatedLanguage>();
            RemoveHearingUsers = new TimeSpan(0,0,0,30);
            RemoveTranslators = new TimeSpan(0,0,0,30);
        }
        
        public void UpdateTranslatorDate(Request<LanguageEnum> request)
        {
            var language = TranslatedLanguages.FirstOrDefault(x => x.Language == request.Data);
            if (language == null)
            {
                language = new TranslatedLanguage()
                {
                    Language = request.Data
                };
                TranslatedLanguages.Add(language);
            }

            var translator = language.Translators.FirstOrDefault(f => f.Id == request.UserGuid);
            if (translator == null)
            {
                translator = new Translator()
                {
                    Id = request.UserGuid
                };
                language.Translators.Add(translator);
            }
            translator.LastAnswer = DateTime.Now;
            RemoveOldUsersAndTranslators();
        }

        public void UpdateUserHearingDate(Request<LanguageEnum> request)
        {

            var language = TranslatedLanguages.FirstOrDefault(x => x.Language == request.Data);
            if(language == null)
                return;
            var user = language.HearingUsers.FirstOrDefault(x => x.Id == request.UserGuid);
            if (user == null)
            {
                user = new User()
                {
                    Id = user.Id,
                    DefaultLanguage = request.Data
                };
                language.HearingUsers.Add(user);
            }
            user.LastLogin = DateTime.Now;

            RemoveOldUsersAndTranslators();
        }

        private void RemoveOldUsersAndTranslators()
        {
            foreach (var translatedLanguage in TranslatedLanguages)
            {
                var oldUsers =
                    translatedLanguage.HearingUsers.Where(x => (DateTime.Now - x.LastLogin) > RemoveHearingUsers);
                var oldTranslators = translatedLanguage.Translators.Where(x => (DateTime.Now - x.LastAnswer) > RemoveTranslators);
                foreach (var oldUser in oldUsers)
                {
                    translatedLanguage.HearingUsers.Remove(oldUser);
                }
                foreach (var oldTranslator in oldTranslators)
                {
                    translatedLanguage.Translators.Remove(oldTranslator);
                }
            }
            var oldLanguages = TranslatedLanguages.Where(x => x.Translators.Count == 0);
            foreach (var translatedLanguage in oldLanguages)
            {
                TranslatedLanguages.Remove(translatedLanguage);
            }
        }

        public IList<LanguageEnum> GetTranslatedLanguages()
        {
            return TranslatedLanguages.Select(x => x.Language).ToList();
        } 
    }
}
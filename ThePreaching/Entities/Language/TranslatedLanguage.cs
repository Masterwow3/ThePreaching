using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Entitie.Language
{
    public class TranslatedLanguage
    {
        public TranslatedLanguage()
        {
            Translators = new ObservableCollection<Translator.Translator>();
            HearingUsers = new ObservableCollection<User.User>();
        }

        public LanguageEnum Language { get; set; }
        public ObservableCollection<Translator.Translator> Translators { get; set; }
        public ObservableCollection<User.User> HearingUsers { get; set; }


    }
}
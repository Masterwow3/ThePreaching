using System;
using Entitie.Language;

namespace Entitie.User
{
    [Serializable]
    public class User
    {
        public LanguageEnum DefaultLanguage { get; set; }
        public string Id { get; set; }
        public DateTime LastLogin { get; set; }
    }
}
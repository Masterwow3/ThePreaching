using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Entitie.Language;
using Entitie.Requests.Base;
using Entitie.Requests.Result;
using Entitie.User;
using PreachingServer.Views.Main.ViewModel;

namespace PreachingServer.Server.BL
{
    public class UserBL
    {
        private string appData;
        private string path;
        private string filePath;
        public UserBL()
        {
            appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path = appData + Properties.Settings.Default.AppDataPath;
            Directory.CreateDirectory(path);
            filePath = path + Properties.Settings.Default.UsersFileName;
        }


        public IResult Save(Request<LanguageEnum> request)
        {
            var user = new User()
            {
                DefaultLanguage = request.Data,
                Id = request.UserGuid,
                LastLogin = DateTime.Now
            };
            var users = GetUsers();
            users.Remove(users.FirstOrDefault(x => x.Id == user.Id));
            users.Add(user);

            FileStream stream;
            stream = new FileStream(filePath, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, users);
            stream.Close();

            return new MethodResult();
        }

        public IList<User> GetUsers()
        {
            if (!File.Exists(filePath))
                return new List<User>();
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Open);
            var users = (IList<User>)formatter.Deserialize(stream);
            return users;
        }
    }
}
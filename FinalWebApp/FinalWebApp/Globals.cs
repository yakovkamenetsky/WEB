using FinalWebApp.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace FinalWebApp
{
    static public class Globals
    {
        public const string ADMIN_SESSION_KEY = "isAdmin";
        public const string USER_SESSION_KEY = "user";

        static public User getConnectedUser(ISession session)
        {
            var userJson = session.GetString(USER_SESSION_KEY);
            if (userJson == null)
                return null;

            return JsonConvert.DeserializeObject<User>(userJson);
;        }

        static public bool isAdminConnected(ISession session)
        {
            return session.GetString(ADMIN_SESSION_KEY) == "true";
        }
    }
}

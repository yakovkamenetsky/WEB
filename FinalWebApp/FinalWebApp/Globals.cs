using Microsoft.AspNetCore.Http;
using System;

namespace FinalWebApp
{
    static public class Globals
    {
        public const string ADMIN_SESSION_KEY = "isAdmin";
        public const string USER_SESSION_KEY = "user";

        static public string getConnectedUser(ISession session)
        {
            return session.GetString(USER_SESSION_KEY);
        }

        static public bool isAdminConnected(ISession session)
        {
            return session.GetString(ADMIN_SESSION_KEY) == "true";
        }
    }
}

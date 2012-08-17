using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlaystationNetworkAPI.Core.Common
{
    public class Login
    {
        string _username;
        string _password;

        public string Username
        {
            get
            {
                return _username;
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
        }

        public Login(string username, string password)
        {
            _username = username;
            _password = password;
        }
    }
}

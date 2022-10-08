using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alkemyapi.Utils
{
    public class UsersConfig
    {
        // Function to remove . from gmail
        public static string CheckGmail(string email)
        {
            string emailverify;
            var auxemail = email.Split("@");
            if (auxemail[1] == "gmail.com")
            {
                var newemail = auxemail[0].Split(".");
                if (newemail.Length != 0)
                {
                    auxemail[0] = string.Join("", newemail);
                }
                emailverify = string.Join("@", auxemail);
            }
            else
            {
                emailverify = email;
            }
            return emailverify;
        }
    }
}

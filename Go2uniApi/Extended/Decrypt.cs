using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Go2uniApi
{
    public static class Decrypt
    {
        public static string DecryptString(string value)
        {
            string DecryptedString = string.Empty;
            try
            {
                DecryptedString = ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(value));
            }
            catch (Exception ex)
            {
                DecryptedString = string.Empty;
            }
            return DecryptedString;
        }
    }
}
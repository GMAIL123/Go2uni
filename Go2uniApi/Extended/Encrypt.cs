using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Go2uniApi
{
    public static class Encrypt
    {
        public static string EncriptString(string Value)
        {
            return Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(Value));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffingApi.BusinessLogic
{
    public class Base64String
    {
        public Base64String ()
        {
        }
        // Function that decodes base64 string
        public static string DecodeBase64String(string encoded)
        {
            try
            {
                string converted = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
                StringBuilder stringBuilder = new StringBuilder();

                foreach (char c in converted)
                {
                    stringBuilder.Append(((int)c).ToString("x"));
                }

                string decodedString = stringBuilder.ToString();
                return decodedString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

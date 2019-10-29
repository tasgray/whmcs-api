using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WHMCS_API
{
    public class Call
    {
        private readonly string Identifier;
        private readonly string Secret;
        private readonly string Url;
        private readonly string AccessKey;



        public Call(string Identifier, string Secret, string Url)
            : this(Identifier, Secret, "", Url)
        {
        }

        public Call(string Identifier, string Secret, string AccessKey, string Url)
        {
            this.Identifier = Identifier;
            this.Secret = Secret;
            this.AccessKey = AccessKey;
            this.Url = Url + "/includes/api.php";
        }

        private NameValueCollection BuildRequestData(NameValueCollection data)
        {

            NameValueCollection request = new NameValueCollection()
            {
                { "identifier", Identifier},
                { "secret", Secret},
                { "responsetype", "json"}
            };

            if(AccessKey != "")
            {
                request.Add("accesskey", AccessKey);
            }

            foreach (string key in data)
            {
                request.Add(key, data[key]);
            }

            return request;
        }

        public string MakeCall(NameValueCollection data)
        {
            byte[] webResponse;

            try
            {
                webResponse = new WebClient().UploadValues(Url, BuildRequestData(data));

            }
            catch (Exception ex)
            {
                throw new Exception("Unable to connect to WHMCS API. " + ex.Message.ToString());
            }

            return Encoding.ASCII.GetString(webResponse);
        }

       
    }
}

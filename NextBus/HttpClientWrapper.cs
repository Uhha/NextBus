using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NextBus
{
    static public class HttpClientWrapper
    {
        private static HttpClient Client;
        public static HttpClient GetClient()
        {
            if (Client == null)
            {
                Client = new HttpClient();
            }
            return Client;
        }

    }
}

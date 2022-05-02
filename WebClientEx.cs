using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TDM
{
    public class WebClientEx : WebClient
    {
        private readonly long position = 0;

        public WebClientEx(long position)
        {
            this.position = position;
        }
        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = (HttpWebRequest)base.GetWebRequest(address);
            request.AddRange(position);
            return request;
        }
    }
    public class WebClientPutEx : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = (HttpWebRequest)base.GetWebRequest(address);
            request.Method = "PUT";
            return request;
        }
    }
}

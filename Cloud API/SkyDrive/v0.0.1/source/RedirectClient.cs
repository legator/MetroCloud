using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SkyDrive
{
    class RedirectClient:WebClient
    {
        Uri _responseUri;

        public Uri ResponseUri
        {
            get { return _responseUri; }
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse response = base.GetWebResponse(request);
            _responseUri = response.ResponseUri;
            return response;
        }

    }
}

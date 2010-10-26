using System;
using System.Runtime.Serialization;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.RelyingParty;

namespace Suteki.TardisBank.Services
{
    public interface IOpenIdService
    {
        IAuthenticationResponse GetResponse();
        ActionResult CreateRequest(string openidUrl);
    }

    public class OpenIdService : IOpenIdService
    {
        private static readonly OpenIdRelyingParty openid = new OpenIdRelyingParty();

        public IAuthenticationResponse GetResponse()
        {
            return openid.GetResponse();
        }

        public ActionResult CreateRequest(string openidUrl)
        {
            if (openidUrl == null)
            {
                throw new ArgumentNullException("openidUrl");
            }

            Identifier id;
            if (Identifier.TryParse(openidUrl, out id))
            {
                try
                {
                    return openid.CreateRequest(openidUrl).RedirectingResponse.AsActionResult();
                }
                catch (ProtocolException ex)
                {
                    throw new OpenIdException(ex.Message, ex);
                }
            }
            throw new OpenIdException(string.Format("Invalid OpenId Identifier: '{0}'", openidUrl));
        }
    }

    [Serializable]
    public class OpenIdException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public OpenIdException()
        {
        }

        public OpenIdException(string message) : base(message)
        {
        }

        public OpenIdException(string message, Exception inner) : base(message, inner)
        {
        }

        protected OpenIdException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
using System.Net;

namespace BCM_Migration_Tool.Objects
{
    public class RestResponse<T> where T : class
    {
        public HttpStatusCode StatusCode;
        public T Content;
        public string ErrorContent;
        public bool FatalError;
        public string StringContent;
    }
}

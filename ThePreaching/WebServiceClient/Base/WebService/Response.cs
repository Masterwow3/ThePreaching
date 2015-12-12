using PreachingServer.Base.Result;

namespace WebServiceClient.Base.WebService
{
    public class Response
    {
        /// <summary>
        /// Default State = Success
        /// </summary>
        public Response()
        {
            State = ResultState.Success;
        }
        public ResultState State { get; set; } 
    }
}
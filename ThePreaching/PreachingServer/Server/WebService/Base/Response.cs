using PreachingServer.Base.Result;

namespace PreachingServer.Server.WebService.Base
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

    public class Response<T>
    {
        public Response()
        {
            State = ResultState.Success;
        }
        public ResultState State { get; set; }
        public T Data { get; set; }
    }
}
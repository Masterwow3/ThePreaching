using Entitie.Requests.Result;

namespace Entitie.Requests.Base
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
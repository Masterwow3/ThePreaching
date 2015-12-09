using System;

namespace PreachingServer.Base.Result
{
    public class MethodResult : IResult
    {
        public MethodResult(ResultState state = ResultState.Success, string message = "", Exception error = null)
        {
            State = state;
            Message = message;
            Error = error;
        }

        public ResultState State { get;private set; }
        public Exception Error { get; private set; }
        public string Message { get; private set; }
    }
}
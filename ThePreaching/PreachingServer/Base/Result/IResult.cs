using System;
using System.Runtime.Serialization;
using System.Threading;

namespace PreachingServer.Base.Result
{
    public interface IResult
    {
        ResultState State { get; }
        Exception Error { get; }
        string Message { get; }
    }
}
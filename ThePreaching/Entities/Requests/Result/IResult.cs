using System;

namespace Entitie.Requests.Result
{
    public interface IResult
    {
        ResultState State { get; }
        Exception Error { get; }
        string Message { get; }
    }
}
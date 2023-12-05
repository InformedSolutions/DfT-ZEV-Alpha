namespace DfT.ZEV.Core.Application.Clients;

public class ApiClientException : Exception
{
    public ApiClientException(string msg) : base(msg) { }
}
namespace DfT.ZEV.Core.Infrastructure.UnitOfWork;

public class UnitOfWorkException : Exception
{
    public UnitOfWorkException(string msg): base(msg) { }
}
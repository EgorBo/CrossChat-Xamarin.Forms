namespace Crosschat.Server.Domain.Seedwork
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }
}
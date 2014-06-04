using Crosschat.Client.Model.Entities;

namespace Crosschat.Client.Model.Contracts
{
    public interface IContactInvitationSender
    {
        void Send(Contact contact);
    }
}

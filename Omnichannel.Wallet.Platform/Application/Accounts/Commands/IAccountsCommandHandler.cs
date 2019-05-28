using Core.Framework.Cqrs.Commands;
using Omnichannel.Wallet.Platform.Application.Accounts.Commands.Actions;

namespace Omnichannel.Wallet.Platform.Application.Accounts.Commands
{
    public interface IAccountsCommandHandler :
        ICommandHandlerAsync<CreateVoucherCommand>
    {
    }
}

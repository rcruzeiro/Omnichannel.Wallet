using Core.Framework.API.Messages;
using Omnichannel.Wallet.Platform.Application.Accounts.Commands.Actions;

namespace Omnichannel.Wallet.API.Messages.Accounts
{
    public class CreateVoucherRequest : BaseRequest
    {
        public CreateVoucherCommand Command { get; set; }
    }
}

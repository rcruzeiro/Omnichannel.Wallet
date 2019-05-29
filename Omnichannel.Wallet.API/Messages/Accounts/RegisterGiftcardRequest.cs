using Core.Framework.API.Messages;
using Omnichannel.Wallet.Platform.Application.Accounts.Commands.Actions;

namespace Omnichannel.Wallet.API.Messages.Accounts
{
    public class RegisterGiftcardRequest : BaseRequest
    {
        public RegisterGiftcardCommand Command { get; set; }
    }
}

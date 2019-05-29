using Core.Framework.API.Messages;
using Omnichannel.Wallet.Platform.Application.Accounts.Commands.Actions;

namespace Omnichannel.Wallet.API.Messages.Accounts
{
    public class CreateGiftcardRequest : BaseRequest
    {
        public CreateGiftcardCommand Command { get; set; }
    }
}

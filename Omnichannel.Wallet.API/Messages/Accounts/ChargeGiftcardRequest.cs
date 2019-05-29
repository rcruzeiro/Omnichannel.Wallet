using Core.Framework.API.Messages;
using Omnichannel.Wallet.Platform.Application.Accounts.Commands.Actions;

namespace Omnichannel.Wallet.API.Messages.Accounts
{
    public class ChargeGiftcardRequest : BaseRequest
    {
        public ChargeGiftcardCommand Command { get; set; }
    }
}

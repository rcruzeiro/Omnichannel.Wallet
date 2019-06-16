using Core.Framework.API.Messages;
using Omnichannel.Wallet.API.Attributes;

namespace Omnichannel.Wallet.API.Messages.Accounts
{
    public class ChargeGiftcardRequest :
        BaseRequest,
        IMultitenantOperation,
        ISecurityOperation
    {
        [SwaggerExclude]
        public string Company { get; set; }

        [SwaggerExclude]
        public string CPF { get; set; }

        public string AccountId { get; set; }

        public decimal Value { get; set; }

        public string Location { get; set; }
    }
}

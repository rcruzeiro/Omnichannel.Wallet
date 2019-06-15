using Core.Framework.API.Messages;

namespace Omnichannel.Wallet.API.Messages.Accounts
{
    public class GetCPFAccountsRequest :
        BaseRequest,
        IMultitenantOperation,
        ISecurityOperation
    {
        public string Company { get; set; }

        public string CPF { get; set; }
    }
}

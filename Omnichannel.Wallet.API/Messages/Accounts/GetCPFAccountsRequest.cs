﻿using Core.Framework.API.Messages;
using Omnichannel.Wallet.API.Attributes;

namespace Omnichannel.Wallet.API.Messages.Accounts
{
    public class GetCPFAccountsRequest :
        BaseRequest,
        IMultitenantOperation,
        ISecurityOperation
    {
        [SwaggerExclude]
        public string Company { get; set; }

        [SwaggerExclude]
        public string CPF { get; set; }
    }
}

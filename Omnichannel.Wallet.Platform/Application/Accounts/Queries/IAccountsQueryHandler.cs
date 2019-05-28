using System.Collections.Generic;
using Core.Framework.Cqrs.Queries;
using Omnichannel.Wallet.Platform.Application.Accounts.Queries.DTOs;
using Omnichannel.Wallet.Platform.Application.Accounts.Queries.Filters;

namespace Omnichannel.Wallet.Platform.Application.Accounts.Queries
{
    public interface IAccountsQueryHandler :
        IQueryHandlerAsync<GetAccountsFilter, List<AccountDTO>>,
        IQueryHandlerAsync<GetAccountFilter, AccountDTO>
    { }
}

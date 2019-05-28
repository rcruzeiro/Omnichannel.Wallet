﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Omnichannel.Wallet.Platform.Application.Accounts.Commands.Actions;
using Omnichannel.Wallet.Platform.Application.Accounts.Queries.DTOs;
using Omnichannel.Wallet.Platform.Application.Accounts.Queries.Filters;

namespace Omnichannel.Wallet.Platform.Application.Accounts
{
    public interface IAccountsAppService
    {
        Task CreateVoucher(CreateVoucherCommand command, CancellationToken cancellationToken = default);
        Task<List<AccountDTO>> GetAccounts(GetAccountsFilter filter, CancellationToken cancellationToken = default);
        Task<AccountDTO> GetAccount(GetAccountFilter filter, CancellationToken cancellationToken = default);
    }
}
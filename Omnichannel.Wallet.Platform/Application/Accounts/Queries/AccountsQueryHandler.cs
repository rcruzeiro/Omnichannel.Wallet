﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Framework.Cqrs.Queries;
using Omnichannel.Wallet.Platform.Application.Accounts.Queries.DTOs;
using Omnichannel.Wallet.Platform.Application.Accounts.Queries.Filters;
using Omnichannel.Wallet.Platform.Domain.Accounts;

namespace Omnichannel.Wallet.Platform.Application.Accounts.Queries
{
    public class AccountsQueryHandler : IAccountsQueryHandler
    {
        readonly IAccountRepository _accountRepository;

        public AccountsQueryHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public async Task<List<AccountDTO>> HandleAsync(GetAccountsFilter filter, CancellationToken cancellationToken)
        {
            try
            {
                // validate filter
                ValidateFilter(filter);

                var dtos = new List<AccountDTO>();
                var accounts = await _accountRepository.GetAsync(filter, cancellationToken);

                accounts.ToList().ForEach(ac =>
                    dtos.Add(ac.ToDTO()));

                return dtos;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task<AccountDTO> HandleAsync(GetAccountFilter filter, CancellationToken cancellationToken = default)
        {
            try
            {
                // validate filter
                ValidateFilter(filter);

                var accounts = await _accountRepository.GetAsync(filter, cancellationToken);

                if (!accounts.Any()) return null;

                return accounts.Single().ToDTO();
            }
            catch (Exception ex)
            { throw ex; }
        }

        private void ValidateFilter<T>(T filter)
            where T : class, IFilter
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            filter.Validate(null);
        }
    }
}

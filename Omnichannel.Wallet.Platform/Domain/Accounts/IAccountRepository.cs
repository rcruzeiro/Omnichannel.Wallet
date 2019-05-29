using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Framework.Repository;

namespace Omnichannel.Wallet.Platform.Domain.Accounts
{
    public interface IAccountRepository : IRepositoryAsync<Account>
    {
        Task<List<VoucherAccount>> GetVouchers(Func<VoucherAccount, bool> predicate, CancellationToken cancellationToken = default);
        Task<List<GiftcardAccount>> GetGiftcards(Func<GiftcardAccount, bool> predicate, CancellationToken cancellationToken = default);
    }
}

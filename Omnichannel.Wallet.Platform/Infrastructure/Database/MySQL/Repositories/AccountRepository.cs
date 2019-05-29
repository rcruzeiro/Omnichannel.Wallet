using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Framework.Repository;
using Omnichannel.Wallet.Platform.Domain.Accounts;

namespace Omnichannel.Wallet.Platform.Infrastructure.Database.MySQL.Repositories
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(IUnitOfWorkAsync unitOfWork)
            : base(unitOfWork)
        { }

        public async Task<List<VoucherAccount>> GetVouchers(Func<VoucherAccount, bool> predicate, CancellationToken cancellationToken = default)
        {
            try
            {
                var query = Context.Set<VoucherAccount>()
                    .Where(predicate);

                return await Task.FromResult(query.ToList());
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task<List<GiftcardAccount>> GetGiftcards(Func<GiftcardAccount, bool> predicate, CancellationToken cancellationToken = default)
        {
            try
            {
                var query = Context.Set<GiftcardAccount>()
                    .Where(predicate);

                return await Task.FromResult(query.ToList());
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}

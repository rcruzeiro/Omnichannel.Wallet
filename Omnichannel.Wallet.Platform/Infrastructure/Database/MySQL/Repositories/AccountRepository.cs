using Core.Framework.Repository;
using Omnichannel.Wallet.Platform.Domain.Accounts;

namespace Omnichannel.Wallet.Platform.Infrastructure.Database.MySQL.Repositories
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(IUnitOfWorkAsync unitOfWork)
            : base(unitOfWork)
        { }
    }
}

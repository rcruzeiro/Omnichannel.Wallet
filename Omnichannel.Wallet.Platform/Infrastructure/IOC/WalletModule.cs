using System;
using Core.Framework.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Omnichannel.Wallet.Platform.Application.Accounts;
using Omnichannel.Wallet.Platform.Application.Accounts.Commands;
using Omnichannel.Wallet.Platform.Application.Accounts.Queries;
using Omnichannel.Wallet.Platform.Domain.Accounts;
using Omnichannel.Wallet.Platform.Infrastructure.Database.MySQL;
using Omnichannel.Wallet.Platform.Infrastructure.Database.MySQL.Repositories;

namespace Omnichannel.Wallet.Platform.Infrastructure.IOC
{
    public sealed class WalletModule
    {
        public WalletModule(IConfiguration configuration)
            : this(new ServiceCollection(), configuration)
        { }

        public WalletModule(IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // data source
            services.AddScoped<IDataSource>(provider =>
                new DefaultDataSource(configuration, "DefaultSQL"));

            // unit of work
            services.AddScoped<IUnitOfWorkAsync, WalletContext>();

            // repositories
            services.AddScoped<IAccountRepository, AccountRepository>();

            // commands
            services.AddScoped<IAccountsCommandHandler, AccountsCommandHandler>();

            // queries
            services.AddScoped<IAccountsQueryHandler, AccountsQueryHandler>();

            // app services
            services.AddScoped<IAccountsAppService, AccountsAppService>();
        }
    }
}

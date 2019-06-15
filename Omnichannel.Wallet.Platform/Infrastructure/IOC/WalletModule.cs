using System;
using Core.Framework.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Omnichannel.Wallet.Platform.Application.Accounts;
using Omnichannel.Wallet.Platform.Application.Accounts.Commands;
using Omnichannel.Wallet.Platform.Application.Accounts.Queries;
using Omnichannel.Wallet.Platform.Domain.Accounts;
using Omnichannel.Wallet.Platform.Infrastructure.Database.Mongo.Repositories;
using Omnichannel.Wallet.Platform.Infrastructure.Database.MySQL;
using Omnichannel.Wallet.Platform.Infrastructure.Database.MySQL.Repositories;
using Omnichannel.Wallet.Platform.Infrastructure.Providers.Blockchain;
using Omnichannel.Wallet.Platform.Infrastructure.Providers.Blockchain.Models;
using Omnichannel.Wallet.Platform.Infrastructure.Providers.Security;
using Omnichannel.Wallet.Platform.Infrastructure.Providers.Security.Models;

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

            // jwt configuration
            var siginConfiguration = new SigninConfiguration(configuration.GetValue<string>("Security:Key"));
            var tokenConfiguration = new TokenConfiguration();
            new ConfigureFromConfigurationOptions<TokenConfiguration>(
                configuration.GetSection("Token")).Configure(tokenConfiguration);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var paramsValidation = options.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = siginConfiguration.Key;
                paramsValidation.ValidAudience = tokenConfiguration.Audience;
                paramsValidation.ValidIssuer = tokenConfiguration.Issuer;

                // validate token signature
                paramsValidation.ValidateIssuerSigningKey = true;

                // validate token lifetime
                paramsValidation.ValidateLifetime = true;

                // set tolerance time for token expiration
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            // configure access authorization
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            // HTTP Context
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // data source
            services.AddScoped<IDataSource>(provider =>
                new DefaultDataSource(configuration, "DefaultSQL"));

            // unit of work
            services.AddScoped<IUnitOfWorkAsync, WalletContext>();

            // repositories
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IWalletBlockchainRepository>(provider =>
                new WalletBlockchainRepository(configuration.GetConnectionString("DefaultNoSQL")));

            // services
            services.AddScoped<IWalletBlockchainService, WalletBlockchainService>();

            // commands
            services.AddScoped<IAccountsCommandHandler, AccountsCommandHandler>();

            // queries
            services.AddScoped<IAccountsQueryHandler, AccountsQueryHandler>();

            // app services
            services.AddScoped<IAccountsAppService, AccountsAppService>();
        }
    }
}

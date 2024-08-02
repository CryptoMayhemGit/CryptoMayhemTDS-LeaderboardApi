using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using FluentValidation;
using Mayhem.Bl.Implementation;
using Mayhem.Bl.Interfaces;
using Mayhem.Bl.Services.Implementations;
using Mayhem.Bl.Services.Interfaces;
using Mayhem.Bl.Validators;
using Mayhem.Configuration;
using Mayhem.Dal.Context;
using Mayhem.Dal.Dto.Request;
using Mayhem.Dal.Dto.Requests;
using Mayhem.Dal.Mappings;
using Mayhem.Dal.Repositories.Implementations;
using Mayhem.Dal.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;

namespace Mayhem.ApplicationSetup
{
    public static class ApplicationConfigurationExtensions
    {
        public static void AddMayhemContext(this IServiceCollection services, string connectionString)
        {
            services
                .AddDbContext<MayhemDataContext>
                (
                    options => options.UseSqlServer(connectionString)
                );
        }

        public static void ConfigureKeyVault(this IConfigurationBuilder configuration)
        {
            bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            string keyVaultEndpoint = Environment.GetEnvironmentVariable("LeaderBoardApiKeyVaultEndpoint");

            if (isDevelopment)
            {
                configuration.AddAzureKeyVault(
                    new Uri(keyVaultEndpoint),
                    new DefaultAzureCredential());
            }
            else
            {
                SecretClient secretClient = new(new(keyVaultEndpoint), new DefaultAzureCredential());
                configuration.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
            }
        }

        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(TableDtoMappings));
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserStatisticsService, UserStatisticsService>();
            services.AddScoped<ITournamentService, TournamentService>();
            services.AddScoped<ITicketEndoceService, TicketEndoceService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
        }

        public static void AddBlockchain(this IServiceCollection services, string web3ProviderEndpoint, string privateKeyWallet)
        {
            var account = new Account(privateKeyWallet);
            services.AddScoped<IWeb3>(x => new Web3(account, web3ProviderEndpoint));
            services.AddScoped<IBlockchainService, BlockchainService>();
        }

        public static void AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IGameCodeRepository, GameCodeRepository>();
            services.AddScoped<ITournamentRepository, TournamentRepository>();
            services.AddScoped<ITournamentUserStatisticRepository, TournamentUserStatisticRepository>();
        }

        public static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<AuthorizationDecodedRequest>, AuthorizationRequestValidator>();
            services.AddScoped<IValidator<LoginRequest>, LoginAuthorizationValidator>();
            services.AddScoped<IValidator<AddTournamentRequest>, AddTournamentValidator>();
            services.AddScoped<IValidator<UpdateTournamentRequest>, UpdateTournamentValidator>();
        }
    }
}

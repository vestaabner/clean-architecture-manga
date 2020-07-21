namespace WebApi.Modules
{
    using Application.Boundaries.CloseAccount;
    using Application.Boundaries.Deposit;
    using Application.Boundaries.GetAccount;
    using Application.Boundaries.GetAccounts;
    using Application.Boundaries.GetCustomer;
    using Application.Boundaries.OnBoardCustomer;
    using Application.Boundaries.OpenAccount;
    using Application.Boundaries.Register;
    using Application.Boundaries.SignUp;
    using Application.Boundaries.Transfer;
    using Application.Boundaries.UpdateCustomer;
    using Application.Boundaries.Withdraw;
    using Application.UseCases;
    using Domain.Accounts;
    using Domain.Customers;
    using Domain.Security;
    using Domain.Services;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    ///     Adds Use Cases classes.
    /// </summary>
    public static class ApplicationExtensions
    {
        /// <summary>
        ///     Adds Use Cases to the ServiceCollection.
        /// </summary>
        /// <param name="services">Service Collection.</param>
        /// <returns>The modified instance.</returns>
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<ICloseAccountUseCase, CloseAccountUseCase>();
            services.AddScoped<IDepositUseCase, DepositUseCase>();
            services.AddScoped<IGetAccountUseCase, GetAccountUseCase>();
            services.AddScoped<IGetAccountsUseCase, GetAccountsUseCase>();
            services.AddScoped<IGetCustomerUseCase, GetCustomerUseCase>();
            services.AddScoped<IOnBoardCustomerUseCase, OnBoardCustomerUseCase>();
            services.AddScoped<IOpenAccountUseCase, OpenAccountUseCase>();
            services.AddScoped<ISignUpUseCase, SignUpUseCase>();
            services.AddScoped<ITransferUseCase, TransferUseCase>();
            services.AddScoped<IUpdateCustomerUseCase, UpdateCustomerUseCase>();
            services.AddScoped<IWithdrawUseCase, WithdrawUseCase>();

            return services;
        }
    }
}

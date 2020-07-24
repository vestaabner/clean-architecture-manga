namespace WebApi.Modules
{
    using Application.UseCases.CloseAccount;
    using Application.UseCases.Deposit;
    using Application.UseCases.GetAccount;
    using Application.UseCases.GetAccounts;
    using Application.UseCases.GetCustomer;
    using Application.UseCases.OnBoardCustomer;
    using Application.UseCases.OpenAccount;
    using Application.UseCases.SignUp;
    using Application.UseCases.Transfer;
    using Application.UseCases.UpdateCustomer;
    using Application.UseCases.Withdraw;
    using Microsoft.Extensions.DependencyInjection;
    using UseCases.V1.Accounts.CloseAccount;
    using UseCases.V1.Accounts.GetAccount;
    using UseCases.V1.Accounts.GetAccounts;
    using UseCases.V1.Accounts.OpenAccount;
    using UseCases.V1.Customers.GetCustomer;
    using UseCases.V1.Customers.OnBoardCustomer;
    using UseCases.V1.Customers.UpdateCustomer;
    using UseCases.V1.SignUp;
    using UseCases.V1.Transactions.Deposit;
    using UseCases.V1.Transactions.Transfer;
    using UseCases.V1.Transactions.Withdraw;

    /// <summary>
    ///     The User Interface V1 Extensions.
    /// </summary>
    public static class UserInterfaceV1Extensions
    {
        /// <summary>
        ///     Inject All V1 Presenters dependencies;
        /// </summary>
        public static IServiceCollection AddPresentersV1(this IServiceCollection services)
        {
            services.AddScoped<CloseAccountPresenter, CloseAccountPresenter>();
            services.AddScoped<ICloseAccountOutputPort>(x =>
                x.GetRequiredService<CloseAccountPresenter>());

            services.AddScoped<DepositPresenter, DepositPresenter>();
            services.AddScoped<IDepositOutputPort>(
                x => x.GetRequiredService<DepositPresenter>());

            services.AddScoped<GetAccountPresenter, GetAccountPresenter>();
            services.AddScoped<IGetAccountOutputPort>(x =>
                x.GetRequiredService<GetAccountPresenter>());

            services.AddScoped<GetAccountsPresenter, GetAccountsPresenter>();
            services.AddScoped<IGetAccountsOutputPort>(x =>
                x.GetRequiredService<GetAccountsPresenter>());

            services.AddScoped<GetCustomerDetailsPresenter, GetCustomerDetailsPresenter>();
            services.AddScoped<IGetCustomerOutputPort>(x =>
                x.GetRequiredService<GetCustomerDetailsPresenter>());

            services.AddScoped<OnBoardCustomerPresenter, OnBoardCustomerPresenter>();
            services.AddScoped<IOnBoardCustomerOutputPort>(x =>
                x.GetRequiredService<OnBoardCustomerPresenter>());

            services.AddScoped<OpenAccountPresenter, OpenAccountPresenter>();
            services.AddScoped<IOpenAccountOutputPort>(x =>
                x.GetRequiredService<OpenAccountPresenter>());

            services.AddScoped<SignUpPresenter, SignUpPresenter>();
            services.AddScoped<ISignUpOutputPort>(x =>
                x.GetRequiredService<SignUpPresenter>());

            services.AddScoped<TransferPresenter, TransferPresenter>();
            services.AddScoped<ITransferOutputPort>(x =>
                x.GetRequiredService<TransferPresenter>());

            services.AddScoped<UpdateCustomerPresenter, UpdateCustomerPresenter>();
            services.AddScoped<IUpdateCustomerOutputPort>(x =>
                x.GetRequiredService<UpdateCustomerPresenter>());

            services.AddScoped<WithdrawPresenter, WithdrawPresenter>();
            services.AddScoped<IWithdrawOutputPort>(x =>
                x.GetRequiredService<WithdrawPresenter>());


            return services;
        }
    }
}

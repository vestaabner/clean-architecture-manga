namespace WebApi.Modules
{
    using Application.Boundaries.CloseAccount;
    using Application.Boundaries.Deposit;
    using Application.Boundaries.GetAccount;
    using Application.Boundaries.GetAccounts;
    using Application.Boundaries.GetCustomer;
    using Application.Boundaries.OnBoardCustomer;
    using Application.Boundaries.OpenAccount;
    using Application.Boundaries.SignUp;
    using Application.Boundaries.Transfer;
    using Application.Boundaries.UpdateCustomer;
    using Application.Boundaries.Withdraw;
    using Microsoft.Extensions.DependencyInjection;
    using UseCases.V1.CloseAccount;
    using UseCases.V1.Deposit;
    using UseCases.V1.GetAccount;
    using UseCases.V1.GetAccounts;
    using UseCases.V1.GetCustomer;
    using UseCases.V1.OnBoardCustomer;
    using UseCases.V1.OpenAccount;
    using UseCases.V1.SignUpCustomer;
    using UseCases.V1.Transfer;
    using UseCases.V1.UpdateCustomer;
    using UseCases.V1.Withdraw;

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

            services.AddScoped<SignUpCustomerPresenter, SignUpCustomerPresenter>();
            services.AddScoped<ISignUpOutputPort>(x =>
                x.GetRequiredService<SignUpCustomerPresenter>());

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

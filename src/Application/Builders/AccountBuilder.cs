namespace Application.Builders
{
    using System;
    using System.Collections.Generic;
    using Domain.Accounts;
    using Domain.Accounts.ValueObjects;
    using Domain.Customers.ValueObjects;

    /// <summary>
    /// 
    /// </summary>
    public sealed class AccountBuilder : IValidator
    {
        private readonly IAccountFactory _accountFactory;

        private Guid _customerId;
        private decimal _initialAmount;
        private string? _currency;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountFactory"></param>
        public AccountBuilder(IAccountFactory accountFactory)
        {
            this._accountFactory = accountFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public AccountBuilder Customer(Guid customerId)
        {
            this._customerId = customerId;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public AccountBuilder InitialAmount(decimal initialAmount, string currency)
        {
            this._initialAmount = initialAmount;
            this._currency = currency;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IAccount Build()
        {
            var customerId = new CustomerId(this._customerId);
            var initialAmount = new PositiveMoney(this._initialAmount, this._currency);

            IAccount account = this._accountFactory
                .NewAccount(customerId);

            account.Deposit(this._accountFactory, initialAmount);

            return account;
        }

        /// <summary>
        /// 
        /// </summary>
        public IList<ErrorMessage> ErrorMessages { get; } = new List<ErrorMessage>();

        /// <summary>
        /// 
        /// </summary>
        public bool IsValid
        {
            get
            {
                return this.ErrorMessages.Count > 0;
            }
        }
    }
}

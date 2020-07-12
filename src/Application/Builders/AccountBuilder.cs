namespace Application.Builders
{
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

        private CustomerId _customerId;
        private PositiveMoney _initialAmount;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountFactory"></param>
        public AccountBuilder(
            IAccountFactory accountFactory)
        {
            this._accountFactory = accountFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public AccountBuilder Customer(CustomerId customerId)
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
            this._initialAmount = new PositiveMoney(initialAmount, currency);

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IAccount Build()
        {
            IAccount account = this._accountFactory
                .NewAccount(this._customerId);

            account.Deposit(this._accountFactory, this._initialAmount);

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

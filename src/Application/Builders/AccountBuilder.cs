namespace Application.Builders
{
    using System;
    using System.Threading.Tasks;
    using Domain.Accounts;
    using Domain.Accounts.ValueObjects;
    using Domain.Customers.ValueObjects;

    /// <summary>
    /// 
    /// </summary>
    public sealed class AccountBuilder
    {
        private readonly Notification _notification;
        private readonly IAccountFactory _accountFactory;
        private readonly string _key;

        private Guid _customerId;
        private decimal _initialAmount;
        private string? _currency;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountFactory"></param>
        /// <param name="notification"></param>
        /// <param name="key"></param>
        public AccountBuilder(
            IAccountFactory accountFactory,
            Notification notification,
            string key)
        {
            this._accountFactory = accountFactory;
            this._notification = notification;
            this._key = key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public AccountBuilder Customer(Guid customerId)
        {
            if (customerId == Guid.Empty)
            {
                this._notification.Add($"{this._key}.Name.First", string.Format(Messages.FirstNameRequired, ssn));
            }

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
        public Task<IAccount> Build()
        {
            if (!this._notification.IsValid)
            {
                return 
            }

            var customerId = new CustomerId(this._customerId);
            var initialAmount = new PositiveMoney(this._initialAmount, this._currency);

            IAccount account = this._accountFactory
                .NewAccount(customerId);

            account.Deposit(this._accountFactory, initialAmount);

            return account;
        }
    }
}

namespace Domain
{
    using Accounts;
    using Accounts.Credits;
    using Accounts.Debits;
    using Customers;
    using Services;

    /// <summary>
    /// 
    /// </summary>
    public sealed class BuilderFactory
    {
        private readonly IAccountFactory _accountFactory;

        private readonly ICustomerFactory _customerFactory;
        private readonly ISSNValidator _ssnValidator;
        private readonly Notification _notification;
        private readonly ICurrencyExchange _currencyExchange;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountFactory"></param>
        /// <param name="customerFactory"></param>
        /// <param name="ssnValidator"></param>
        /// <param name="notification"></param>
        /// <param name="currencyExchange"></param>
        public BuilderFactory(
            IAccountFactory accountFactory,
            ICustomerFactory customerFactory,
            ISSNValidator ssnValidator,
            Notification notification,
            ICurrencyExchange currencyExchange)
        {
            this._accountFactory = accountFactory;
            this._customerFactory = customerFactory;
            this._ssnValidator = ssnValidator;
            this._notification = notification;
            this._currencyExchange = currencyExchange;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public AccountBuilder NewAccountBuilder(string key)
        {
            return new AccountBuilder(
                this._accountFactory,
                this._notification,
                key);
        }

        public CreditBuilder NewCreditBuilder(string key)
        {
            return new CreditBuilder(
                this._accountFactory,
                this._currencyExchange,
                this._notification,
                key);
        }

        public DebitBuilder NewDebitBuilder(string key)
        {
            return new DebitBuilder(
                this._accountFactory,
                this._currencyExchange,
                this._notification,
                key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CustomerBuilder NewCustomerBuilder(string key)
        {
            return new CustomerBuilder(
                this._customerFactory,
                this._ssnValidator,
                this._notification,
                key);
        }
    }
}

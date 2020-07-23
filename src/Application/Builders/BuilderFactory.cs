namespace Domain
{
    using Accounts;
    using Accounts.Credits;
    using Accounts.Debits;
    using Customers;
    using Security;

    /// <summary>
    /// 
    /// </summary>
    public sealed class BuilderFactory
    {
        private readonly IAccountFactory _accountFactory;

        private readonly ICustomerFactory _customerFactory;
        private readonly Notification _notification;
        private readonly IUserFactory _userFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountFactory"></param>
        /// <param name="customerFactory"></param>
        /// <param name="notification"></param>
        /// <param name="userFactory"></param>
        public BuilderFactory(
            IAccountFactory accountFactory,
            ICustomerFactory customerFactory,
            Notification notification,
            IUserFactory userFactory)
        {
            this._accountFactory = accountFactory;
            this._customerFactory = customerFactory;
            this._notification = notification;
            this._userFactory = userFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public AccountBuilder NewAccountBuilder()
        {
            return new AccountBuilder(
                this._accountFactory,
                this._notification);
        }

        public CreditBuilder NewCreditBuilder()
        {
            return new CreditBuilder(
                this._accountFactory,
                this._notification);
        }

        public DebitBuilder NewDebitBuilder()
        {
            return new DebitBuilder(
                this._accountFactory,
                this._notification);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CustomerBuilder NewCustomerBuilder()
        {
            return new CustomerBuilder(
                this._customerFactory,
                this._notification);
        }

        public UserBuilder NewUserBuilder()
        {
            return new UserBuilder(
                this._userFactory,
                this._notification);
        }
    }
}

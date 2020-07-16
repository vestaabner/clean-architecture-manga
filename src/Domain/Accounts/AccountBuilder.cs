namespace Domain.Accounts
{
    using System;
    using Customers.ValueObjects;

    /// <summary>
    /// 
    /// </summary>
    public sealed class AccountBuilder
    {
        private readonly Notification _notification;
        private readonly IAccountFactory _accountFactory;
        private readonly string _key;

        private Guid _customerId;

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
            this._customerId = customerId;

            return this;
        }

        private void Validate()
        {
            if (this._customerId == Guid.Empty)
            {
                this._notification.Add($"{this._key}.Account.CustomerId", Messages.CustomerIdRequired);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IAccount Build()
        {
            this.Validate();

            if (!this._notification.IsValid)
            {
                return AccountNull.Instance;
            }

            var customerId = new CustomerId(this._customerId);

            IAccount account = this._accountFactory
                .NewAccount(customerId);

            return account;
        }
    }
}

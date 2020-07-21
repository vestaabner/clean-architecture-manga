namespace Domain.Accounts.Credits
{
    using System;
    using ValueObjects;

    /// <summary>
    /// 
    /// </summary>
    public sealed class CreditBuilder
    {
        private readonly IAccountFactory _accountFactory;
        private readonly Notification _notification;

        private IAccount? _account;
        private Currency? _currency;
        private PositiveMoney? _positiveMoney;
        private DateTime? _transactionDate;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountFactory"></param>
        /// <param name="notification"></param>
        public CreditBuilder(
            IAccountFactory accountFactory,
            Notification notification)
        {
            this._accountFactory = accountFactory;
            this._notification = notification;
        }

        public CreditBuilder Account(IAccount account)
        {
            this._account = account;
            return this;
        }

        public CreditBuilder Amount(decimal amount, string currencyCode)
        {
            this._currency = Currency.Create(this._notification, currencyCode);
            if (this._currency != null)
            {
                this._positiveMoney = PositiveMoney.Create(
                    this._notification,
                    amount,
                    this._currency.Value);
            }

            return this;
        }

        public CreditBuilder Timestamp()
        {
            this._transactionDate = DateTime.Now;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>ICredit</returns>
        public ICredit Build()
        {
            if (this._account is AccountNull ||
                !this._currency.HasValue ||
                !this._positiveMoney.HasValue ||
                !this._transactionDate.HasValue ||
                !this._notification.IsValid)
            {
                return CreditNull.Instance;
            }

            return this.BuildInternal();
        }

        private ICredit BuildInternal() =>
            this._accountFactory.NewCredit(
                this._account!,
                this._positiveMoney!.Value,
                this._transactionDate!.Value);
    }
}

namespace Domain.Accounts.Credits
{
    using System;
    using Services;
    using ValueObjects;

    /// <summary>
    /// 
    /// </summary>
    public sealed class CreditBuilder
    {
        private readonly IAccountFactory _accountFactory;
        private readonly ICurrencyExchange _currencyExchange;
        private readonly Notification _notification;
        private readonly string _key;

        private decimal? _amount;
        private string? _currency;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountFactory"></param>
        /// <param name="currencyExchange"></param>
        /// <param name="notification"></param>
        /// <param name="key"></param>
        public CreditBuilder(
            IAccountFactory accountFactory,
            ICurrencyExchange currencyExchange,
            Notification notification,
            string key)
        {
            this._accountFactory = accountFactory;
            this._currencyExchange = currencyExchange;
            this._notification = notification;
            this._key = key;
        }

        public CreditBuilder PositiveMoney(decimal amount, string currency)
        {
            this._amount = amount;
            this._currency = currency;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>ICredit</returns>
        public ICredit Build(IAccount account)
        {
            this.Validate();

            if (!this._notification.IsValid)
            {
                return CreditNull.Instance;
            }

            Currency currency = new Currency(this._currency!);
            PositiveMoney positiveMoney = new PositiveMoney(this._amount!.Value, currency);
            DateTime transactionDate = DateTime.Today;

            return this._accountFactory.NewCredit(
                account,
                positiveMoney,
                transactionDate);
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(this._currency))
            {
                this._notification.Add($"{this._key}.Credit.Currency", Messages.AmountIsNegative);
            }
            else if (!this._currencyExchange.IsCurrencyAllowed(this._currency))
            {
                this._notification.Add($"{this._key}.Credit.Amount", Messages.AmountIsNegative);
            }

            if (this._amount <= 0)
            {
                this._notification.Add($"{this._key}.Credit.Amount", Messages.AmountIsNegative);
            }
        }
    }
}

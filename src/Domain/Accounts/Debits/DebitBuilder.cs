namespace Domain.Accounts.Debits
{
    using System;
    using Services;
    using ValueObjects;

    /// <summary>
    /// 
    /// </summary>
    public sealed class DebitBuilder
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
        public DebitBuilder(
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

        public DebitBuilder PositiveMoney(decimal amount, string currency)
        {
            this._amount = amount;
            this._currency = currency;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>IDebit</returns>
        public IDebit Build(IAccount account)
        {
            this.Validate();

            if (!this._notification.IsValid)
            {
                return DebitNull.Instance;
            }

            Currency currency = new Currency(this._currency!);
            PositiveMoney positiveMoney = new PositiveMoney(this._amount!.Value, currency);
            DateTime transactionDate = DateTime.Today;

            return this._accountFactory.NewDebit(
                account,
                positiveMoney,
                transactionDate);
        }

        private void Validate()
        {
            if (!this._currencyExchange.IsCurrencyAllowed(this._currency))
            {
                this._notification.Add($"{this._key}.Debit.Amount", Messages.AmountIsNegative);
            }

            if (this._amount <= 0)
            {
                this._notification.Add($"{this._key}.Debit.Amount", Messages.AmountIsNegative);
            }
        }
    }
}

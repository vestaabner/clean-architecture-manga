namespace Domain.Customers
{
    using Security;
    using Services;
    using ValueObjects;

    /// <summary>
    /// 
    /// </summary>
    public sealed class CustomerBuilder
    {
        private readonly ICustomerFactory _customerFactory;
        private readonly ISSNValidator _ssnValidator;
        private readonly Notification _notification;
        private readonly string _key;

        private string? _ssn;
        private string? _firstName;
        private string? _lastName;
        private IUser? _user;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerFactory"></param>
        /// <param name="ssnValidator"></param>
        /// <param name="notification"></param>
        /// <param name="key"></param>
        public CustomerBuilder(
            ICustomerFactory customerFactory,
            ISSNValidator ssnValidator,
            Notification notification,
            string key)
        {
            this._customerFactory = customerFactory;
            this._ssnValidator = ssnValidator;
            this._notification = notification;
            this._key = key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ssn"></param>
        /// <returns></returns>
        public CustomerBuilder SSN(string ssn)
        {
            this._ssn = ssn;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CustomerBuilder FirstName(string name)
        {
            this._firstName = name;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CustomerBuilder LastName(string name)
        {
            this._lastName = name;

            return this;
        }

        public CustomerBuilder User(IUser user)
        {
            this._user = user;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ICustomer Build()
        {
            this.Validate();

            if (!this._notification.IsValid)
            {
                return CustomerNull.Instance;
            }

            SSN ssn = new SSN(this._ssn!);
            Name firstName = new Name(this._firstName!);
            Name lastName = new Name(this._lastName!);

            return this._customerFactory.NewCustomer(
                ssn,
                firstName,
                lastName,
                this._user!.ExternalUserId);

        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(this._ssn))
            {
                this._notification.Add($"{this._key}.Customer.SSN", Messages.SSNRequired);
            }
            else if (!this._ssnValidator.IsValid(this._ssn))
            {
                this._notification.Add($"{this._key}.Customer.SSN", Messages.InvalidSSN);
            }

            if (string.IsNullOrWhiteSpace(this._firstName))
            {
                this._notification.Add($"{this._key}.Customer.FirstName", Messages.FirstNameRequired);
            }

            if (string.IsNullOrWhiteSpace(this._lastName))
            {
                this._notification.Add($"{this._key}.Customer.LastName", Messages.LastNameRequired);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ICustomer Update(ICustomer existingCustomer)
        {
            this.Validate();

            if (!this._notification.IsValid)
            {
                return CustomerNull.Instance;
            }

            SSN ssn = new SSN(this._ssn!);
            Name firstName = new Name(this._firstName!);
            Name lastName = new Name(this._lastName!);

            existingCustomer.Update(ssn, firstName, lastName);

            return existingCustomer;
        }
    }
}

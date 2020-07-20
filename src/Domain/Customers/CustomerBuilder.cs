namespace Domain.Customers
{
    using System;
    using ValueObjects;

    /// <summary>
    /// 
    /// </summary>
    public sealed class CustomerBuilder
    {
        private readonly ICustomerFactory _customerFactory;
        private readonly Notification _notification;

        private SSN? _ssn;
        private Name? _firstName;
        private Name? _lastName;

        private Guid _userId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerFactory"></param>
        /// <param name="notification"></param>
        public CustomerBuilder(
            ICustomerFactory customerFactory,
            Notification notification)
        {
            this._customerFactory = customerFactory;
            this._notification = notification;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ssn"></param>
        /// <returns></returns>
        public CustomerBuilder SSN(string ssn)
        {
            this._ssn = ValueObjects.SSN.Create(this._notification, ssn);

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstName"></param>
        /// <returns></returns>
        public CustomerBuilder FirstName(string firstName)
        {
            this._firstName = Name.Create(this._notification, firstName);

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public CustomerBuilder LastName(string lastName)
        {
            this._lastName = Name.Create(this._notification, lastName);

            return this;
        }

        public CustomerBuilder UserId(Guid userId)
        {
            this._userId = userId;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>ICredit</returns>
        public ICustomer Build()
        {
            if (!this._ssn.HasValue ||
                !this._firstName.HasValue ||
                !this._lastName.HasValue ||
                !this._notification.IsValid)
            {
                return CustomerNull.Instance;
            }

            return this.BuildInternal();
        }

        private ICustomer BuildInternal() =>
            this._customerFactory.NewCustomer(
                this._ssn!.Value,
                this._firstName!.Value,
                this._lastName!.Value,
                this._userId);


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ICustomer Update(ICustomer existingCustomer)
        {
            if (!this._ssn.HasValue ||
                !this._firstName.HasValue ||
                !this._lastName.HasValue ||
                !this._notification.IsValid)
            {
                return CustomerNull.Instance;
            }

            return this.UpdateInternal(existingCustomer);
        }

        private ICustomer UpdateInternal(ICustomer existingCustomer)
        {
            existingCustomer.Update(this._ssn!.Value, this._firstName!.Value, this._lastName!.Value);
            return existingCustomer;
        }
    }
}

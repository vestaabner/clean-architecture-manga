namespace Application.Builders
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Customers;
    using Domain.Customers.ValueObjects;
    using Domain.Security;
    using Domain.Security.ValueObjects;
    using Services;

    /// <summary>
    /// 
    /// </summary>
    public sealed class CustomerBuilder
    {
        private readonly ICustomerFactory _customerFactory;
        private readonly ISSNValidator _ssnValidator;
        private readonly IUserService _userService;
        private readonly ICustomerRepository _customerRepository;

        private SSN _ssn;
        private Name _firstName;
        private Name _lastName;
        private ExternalUserId _externalUserId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerFactory"></param>
        /// <param name="ssnValidator"></param>
        /// <param name="userService"></param>
        /// <param name="customerRepository"></param>
        public CustomerBuilder(
            ICustomerFactory customerFactory,
            ISSNValidator ssnValidator,
            IUserService userService,
            ICustomerRepository customerRepository)
        {
            this._customerFactory = customerFactory;
            this._ssnValidator = ssnValidator;
            this._userService = userService;
            this._customerRepository = customerRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ssn"></param>
        /// <returns></returns>
        public CustomerBuilder SSN(string ssn)
        {
            if (!this._ssnValidator.IsValid(ssn))
            {
                this.ErrorMessages
                    .Add(new ErrorMessage("Customer.SSN", Messages.InvalidSSN));
            }
            else
            {
                this._ssn = new SSN(ssn);
            }

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CustomerBuilder FirstName(string name)
        {
            this._firstName = new Name(name);

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CustomerBuilder LastName(string name)
        {
            this._lastName = new Name(name);

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ICustomer Build()
        {
            return this._customerFactory.NewCustomer(
                this._ssn,
                this._firstName,
                this._lastName,
                this._externalUserId);
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task AvailableExternalUserId()
        {
            IUser user = this._userService
                .GetCurrentUser();

            ICustomer existingCustomer = await this._customerRepository
                .Find(user.ExternalUserId)
                .ConfigureAwait(false);

            if (existingCustomer == null)
            {
                this._externalUserId = user.ExternalUserId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task ExistingCustomer()
        {
            IUser user = this._userService
                .GetCurrentUser();

            ICustomer existingCustomer = await this._customerRepository
                .Find(user.ExternalUserId)
                .ConfigureAwait(false);

            if (existingCustomer == null)
            {
                this._externalUserId = user.ExternalUserId;
            }
        }
    }
}

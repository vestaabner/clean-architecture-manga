namespace Application.Builders
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Customers;
    using Domain.Customers.ValueObjects;
    using Domain.Security;
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

        private string? _ssn;
        private string? _firstName;
        private string? _lastName;

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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ICustomer Build()
        {
            SSN ssn = new SSN(this._ssn!);
            Name firstName = new Name(this._firstName!);
            Name lastName = new Name(this._lastName!);

            IUser user = this._userService
                .GetCurrentUser();

            return this._customerFactory.NewCustomer(
                ssn,
                firstName,
                lastName,
                user.ExternalUserId);
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
        public async Task<ICustomer> Update()
        {
            IUser user = this._userService
                .GetCurrentUser();

            ICustomer existingCustomer = await this._customerRepository
                .Find(user.ExternalUserId)
                .ConfigureAwait(false);

            SSN ssn = new SSN(this._ssn!);
            Name firstName = new Name(this._firstName!);
            Name lastName = new Name(this._lastName!);

            existingCustomer.Update(ssn, firstName, lastName);

            return existingCustomer;
        }
    }
}

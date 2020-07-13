namespace Application.Builders
{
    using Domain.Accounts;
    using Domain.Customers;
    using Services;

    /// <summary>
    /// 
    /// </summary>
    public sealed class BuilderFactory
    {
        private readonly IAccountFactory _accountFactory;

        private readonly ICustomerFactory _customerFactory;
        private readonly ISSNValidator _ssnValidator;
        private readonly IUserService _userService;
        private readonly ICustomerRepository _customerRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountFactory"></param>
        /// <param name="customerFactory"></param>
        /// <param name="ssnValidator"></param>
        /// <param name="userService"></param>
        /// <param name="customerRepository"></param>
        public BuilderFactory(IAccountFactory accountFactory, ICustomerFactory customerFactory, ISSNValidator ssnValidator, IUserService userService, ICustomerRepository customerRepository)
        {
            this._accountFactory = accountFactory;
            this._customerFactory = customerFactory;
            this._ssnValidator = ssnValidator;
            this._userService = userService;
            this._customerRepository = customerRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public AccountBuilder NewAccountBuilder()
        {
            return new AccountBuilder(this._accountFactory);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CustomerBuilder NewCustomerBuilder()
        {
            return new CustomerBuilder(
                this._customerFactory,
                this._ssnValidator,
                this._userService,
                this._customerRepository);
        }
    }
}

// <copyright file="CustomerRepositoryFake.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Infrastructure.DataAccess.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Customers;
    using Domain.Customers.ValueObjects;
    using Customer = Entities.Customer;

    public sealed class CustomerRepositoryFake : ICustomerRepository
    {
        private readonly MangaContextFake _context;

        public CustomerRepositoryFake(MangaContextFake context) => this._context = context;

        public Task<ICustomer> Find(string externalUserId) => throw new System.NotImplementedException();

        public async Task Add(ICustomer customer)
        {
            this._context
                .Customers
                .Add((Customer)customer);

            await Task.CompletedTask
                .ConfigureAwait(false);
        }

        public async Task<ICustomer> GetBy(CustomerId customerId)
        {
            Customer customer = this._context
                .Customers
                .SingleOrDefault(e => e.Id.Equals(customerId));

            return await Task.FromResult<Domain.Customers.Customer>(customer)
                .ConfigureAwait(false);
        }

        public async Task Update(ICustomer customer)
        {
            Customer customerOld = this._context
                .Customers
                .SingleOrDefault(e => e.Id.Equals(customer.Id));

            if (customerOld != null)
            {
                this._context.Customers.Remove(customerOld);
                this._context.Customers.Add((Customer) customer);
            }

            await Task.CompletedTask
                .ConfigureAwait(false);
        }
    }
}

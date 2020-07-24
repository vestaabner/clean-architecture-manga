// <copyright file="CustomerRepository.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Infrastructure.DataAccess.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Customers;
    using Domain.Security.ValueObjects;
    using Microsoft.EntityFrameworkCore;

    public sealed class CustomerRepository : ICustomerRepository
    {
        private readonly MangaContext _context;

        public CustomerRepository(MangaContext context) => this._context = context ??
                                                                           throw new ArgumentNullException(
                                                                               nameof(context));

        public async Task<ICustomer> Find(UserId userId)
        {
            Customer customer = await this._context
                .Customers
                .Where(c => c.UserId == userId)
                .SingleOrDefaultAsync()
                .ConfigureAwait(false);

            if (customer is null)
            {
                return CustomerNull.Instance;
            }

            var accounts = this._context
                .Accounts
                .Where(e => e.CustomerId == customer.CustomerId)
                .Select(e => e.AccountId)
                .ToList();

            customer.Accounts
                .AddRange(accounts);

            return customer;
        }

        public async Task Add(Customer customer) =>
            await this._context
                .Customers
                .AddAsync((Entities.Customer)customer)
                .ConfigureAwait(false);

        public async Task Update(Customer customer)
        {
            this._context
                .Customers
                .Update((Entities.Customer)customer);

            await Task.CompletedTask
                .ConfigureAwait(false);
        }
    }
}

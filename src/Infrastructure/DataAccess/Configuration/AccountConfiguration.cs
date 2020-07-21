// <copyright file="AccountConfiguration.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Infrastructure.DataAccess.Configuration
{
    using System;
    using Domain.Accounts.ValueObjects;
    using Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    ///     Account Configuration.
    /// </summary>
    public sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        /// <summary>
        ///     Configure Account.
        /// </summary>
        /// <param name="builder">Builder.</param>
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("Account");

            builder.Property(b => b.Id)
                .HasConversion(
                    v => v.Id,
                    v => new AccountId(v))
                .IsRequired();

            builder.Property(b => b.CustomerId)
                .IsRequired();

            builder.Ignore(p => p.Credits);
            builder.Ignore(p => p.Debits);
        }
    }
}

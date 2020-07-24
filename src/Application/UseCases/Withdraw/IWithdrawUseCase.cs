// <copyright file="IWithdrawOutputPort.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.Withdraw
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    ///     Withdraw
    ///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Domain-Driven-Design-Patterns#use-case">
    ///         Use
    ///         Case Domain-Driven Design Pattern
    ///     </see>
    ///     .
    /// </summary>
    public interface IWithdrawUseCase
    {
        Task Execute(Guid accountId, decimal amount, string currency);
    }
}

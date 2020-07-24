namespace WebApi.UseCases.V2.GetAccount
{
    using System;
    using System.Data;
    using Application.Services;
    using Application.UseCases.GetAccount;
    using Domain.Accounts;
    using Microsoft.AspNetCore.Mvc;
    using OfficeOpenXml;
    using V1;

    /// <summary>
    /// </summary>
    public sealed class GetAccountDetailsPresenterV2 : IGetAccountOutputPort
    {
        private readonly Notification _notification;

        public GetAccountDetailsPresenterV2(Notification notification) => this._notification = notification;

        /// <summary>
        /// </summary>
        public IActionResult ViewModel { get; private set; } = new NoContentResult();

        public void Invalid() => this.ViewModel = PresenterUtils.Invalid(this._notification);

        public void NotFound() => this.ViewModel = PresenterUtils.NotFound();

        public void Successful(IAccount account)
        {
            using var dataTable = new DataTable();
            dataTable.Columns.Add("AccountId", typeof(Guid));
            dataTable.Columns.Add("Amount", typeof(decimal));

            var accountEntity = (Account)account;

            dataTable.Rows.Add(accountEntity.AccountId.Id, accountEntity.GetCurrentBalance().Amount);

            byte[] fileContents;

            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(account.AccountId.ToString());
                ws.Cells["A1"].LoadFromDataTable(dataTable, true);
                ws.Row(1).Style.Font.Bold = true;
                ws.Column(3).Style.Numberformat.Format = "dd/MM/yyyy HH:mm";
                fileContents = pck.GetAsByteArray();
            }

            this.ViewModel = new FileContentResult(fileContents,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}

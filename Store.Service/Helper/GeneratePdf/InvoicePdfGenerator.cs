using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Helper.GeneratePdf
{
	public class InvoicePdfGenerator : IInvoicePdfGenerator
	{
		public byte[] GenerateInvoice(Guid orderId, string customerName, decimal totalAmount, List<InvoiceItemDto> items)
		{
			QuestPDF.Settings.License = LicenseType.Community;

			return Document.Create(container =>
			{
				container.Page(page =>
				{
					page.Margin(30);

					page.Header()
						.Text($"Invoice #{orderId}")
						.FontSize(24)
						.Bold();

					page.Content()
						.Column(column =>
						{
							column.Spacing(10);

							column.Item()
								.Text($"Customer: {customerName}");

							column.Item()
								.Text($"Date: {DateTime.UtcNow:yyyy-MM-dd}");

							column.Item().Table(table =>
							{
								table.ColumnsDefinition(columns =>
								{
									columns.RelativeColumn(4);
									columns.RelativeColumn(2);
									columns.RelativeColumn(2);
									columns.RelativeColumn(2);
								});

								table.Header(header =>
								{
									header.Cell().Text("Product");
									header.Cell().Text("Qty");
									header.Cell().Text("Price");
									header.Cell().Text("Total");
								});

								foreach (var item in items)
								{
									table.Cell().Text(item.ProductName);

									table.Cell().Text(
										item.Quantity.ToString());

									table.Cell().Text(
										item.Price.ToString("C"));

									table.Cell().Text(
										(item.Price * item.Quantity)
										.ToString("C"));
								}
							});

							column.Item()
								.PaddingTop(20)
								.Text($"Total Amount: {totalAmount:C}")
								.Bold()
								.FontSize(16);
						});

					page.Footer()
						.AlignCenter()
						.Text("Thank you for your purchase.");
				});
			})
			.GeneratePdf();
		}
	}
}

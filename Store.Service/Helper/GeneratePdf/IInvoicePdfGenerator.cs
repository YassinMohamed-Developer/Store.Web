using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Helper.GeneratePdf
{
	public interface IInvoicePdfGenerator
	{
		byte[] GenerateInvoice(
		Guid orderId,
		string customerName,
		decimal totalAmount,
		List<InvoiceItemDto> items);
	}
}

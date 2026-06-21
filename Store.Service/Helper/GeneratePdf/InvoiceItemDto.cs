namespace Store.Service.Helper.GeneratePdf
{
	public class InvoiceItemDto
	{
		public string? ProductName { get; set; }

		public int Quantity { get; set; }

		public decimal Price {  get; set; }
	}
}
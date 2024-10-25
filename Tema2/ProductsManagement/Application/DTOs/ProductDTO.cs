namespace Application.DTOs
{
	public class ProductDTO
	{
		public Guid ID { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
		public int TVA { get; set; }
	}
}

using MediatR;

namespace Application.UseCases.Commands
{
	public class CreateProductCommand : IRequest<Guid>
	{
		public string Name { get; set; }
		public decimal Price { get; set; }
		public int TVA { get; set; }
	}
}

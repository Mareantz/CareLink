using MediatR;

namespace Application.Commands
{
    public class UpdateProductCommand : IRequest
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int TVA { get; set; }
    }
}

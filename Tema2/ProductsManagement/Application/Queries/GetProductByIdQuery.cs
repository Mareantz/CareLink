using Application.DTOs;
using MediatR;
namespace Application.Queries
{
    public class GetProductByIdQuery : IRequest<ProductDTO>
    {
        public Guid ID { get; set; }
    }
}
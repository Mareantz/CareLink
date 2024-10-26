using Application.DTOs;
using MediatR;

namespace Application.Queries
{
    public class GetProductsQuery : IRequest<List<ProductDTO>>
    {
    }
}

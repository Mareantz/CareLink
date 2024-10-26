using Application.UseCases.Commands;
using Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
namespace Application.UseCases.CommandHandlers
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository repository;
        public DeleteProductCommandHandler(IProductRepository repository)
        {
            this.repository = repository;
        }
        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            await repository.DeleteProductAsync(request.Id);
        }
    }
}

using Application.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.CommandHandlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IProductRepository repository;
        private readonly IMapper mapper;

        public UpdateProductCommandHandler(IProductRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = mapper.Map<Book>(request);
            return repository.UpdateAsync(product);
        }
    }
}

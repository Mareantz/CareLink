using Application.UseCases.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers
{
	public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
	{
		private readonly IProductRepository repository;
		private readonly IMapper mapper;

        public CreateProductCommandHandler(IProductRepository repository, IMapper mapper)
        {
            this.repository = repository;
			this.mapper = mapper;

		}

		public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
		{
			var product = mapper.Map<Product>(request);
			return await repository.AddProductAsync(product);
		}
	}
}

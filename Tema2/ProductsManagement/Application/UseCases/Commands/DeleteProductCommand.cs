using MediatR;

namespace Application.UseCases.Commands 
{ 
    public class DeleteProductCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}


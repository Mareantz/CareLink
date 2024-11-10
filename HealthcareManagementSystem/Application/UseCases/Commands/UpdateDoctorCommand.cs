using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands
{
    public class UpdateDoctorCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Specialization { get; set; }
        public required string Bio { get; set; }
    }
}

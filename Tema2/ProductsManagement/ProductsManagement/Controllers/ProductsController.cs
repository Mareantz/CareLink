using Application.UseCases.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ProductsManagement.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly IMediator mediator;

		public ProductsController(IMediator mediator)
		{
			this.mediator = mediator;
		}

		[HttpPost]
		public async Task<ActionResult<Guid>> CreateProduct(CreateProductCommand command)
		{
			return await mediator.Send(command);
		}

        [HttpPut("id")]
        public async Task<IActionResult> Update(Guid id, UpdateProductCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            await mediator.Send(command);
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}

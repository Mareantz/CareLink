using Application.DTOs;
using Application.Queries;
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

        [HttpGet]
        public async Task<ActionResult<List<ProductDTO>>> GetProducts()
        {
            return await mediator.Send(new GetProductsQuery());
        }

        [HttpGet("id")]
        public async Task<ActionResult<ProductDTO>> GetById(Guid id)
        {
            return await mediator.Send(new GetProductByIdQuery { ID = id });
        }

        [HttpPost]
		public async Task<ActionResult<Guid>> CreateProduct(CreateProductCommand command)
		{
			return await mediator.Send(command);
		}
		[HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            await mediator.Send(new DeleteProductCommand { Id = id });
            return NoContent();
        }
    }
}

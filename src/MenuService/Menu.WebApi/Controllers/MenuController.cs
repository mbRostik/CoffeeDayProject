using MediatR;
using Menu.Application.Contracts.ChangeDTO;
using Menu.Application.Contracts.GetDTOs;
using Menu.Application.UseCases.Commands;
using Menu.Application.UseCases.Queries;
using Menu.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Menu.WebApi.Controllers
{

    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class MenuController : ControllerBase
    {
        private readonly IMediator mediator;
        public MenuController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("GetAllCategories")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Category>>> GetAllCategories()
        {
          

            var result = await mediator.Send(new GetAllCategoriesQuery());

            if (result == null)
            {
                return NotFound("There is no information.");
            }
            return Ok(result);
        }

        [HttpGet("GetUserBag")]
        public async Task<ActionResult<List<GetUserBagDTO>>> GetUserBag()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("User ID not found.");
            }


            var result = await mediator.Send(new GetUserBagQuery(userId));
            return Ok(result);
        }

        [HttpGet("GetAllProducts")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            var result = await mediator.Send(new GetAllProductsQuery());

            if (result == null)
            {
                return NotFound("There is no information.");
            }
            return Ok(result);
        }

        [HttpPost("GetProductsByCategory")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Category>>> GetProductsByCategory([FromBody] GetProductsByCategoryDTO request)
        {
            var result = await mediator.Send(new GetProductsByCategoryQuery(request.Id));

            if (result == null)
            {
                return NotFound("There is no information.");
            }
            return Ok(result);
        }

        [HttpPost("AddProductToTheBag")]
        public async Task<ActionResult<bool>> AddProductToTheBag([FromBody] AddProductToTheBagDTO request)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("User ID not found.");
            }


            var result = await mediator.Send(new AddProductToTheBagCommand(request, userId));

            return Ok(result);
        }

        [HttpPost("RemoveProductFromTheBag")]
        public async Task<ActionResult<bool>> RemoveProductFromTheBag([FromBody] RemoveProductFromTheBagDTO request)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("User ID not found.");
            }

            var result = await mediator.Send(new RemoveProductFromTheBagCommand(request, userId));

            return Ok(result);
        }

        [HttpPost("RemoveAllProductsFromTheBag")]
        public async Task<ActionResult<bool>> RemoveAllProductsFromTheBag([FromBody] RemoveAllProductsFromTheBagDTO request)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("User ID not found.");
            }

            var result = await mediator.Send(new RemoveAllProductsFromTheBagCommand(request, userId));

            return Ok(result);
        }

        [HttpPost("PayOrder")]
        public async Task<ActionResult<bool>> PayOrder()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("User ID not found.");
            }
             
            var result = await mediator.Send(new PayOrderCommand(userId));

            return Ok(result);
        }
    }
}
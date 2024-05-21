using ContactUs.Application.Contracts.ChangeDTO;
using ContactUs.Application.UseCases.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ContactUs.WebApi.Controllers
{

    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ContactUsController : ControllerBase
    {
        private readonly IMediator mediator;
        public ContactUsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("SendUsMessage")]
        public async Task<ActionResult> ChangeUserSettings([FromBody] SendMessageDTO model)
        {

            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrWhiteSpace(userId))
                {
                    Console.WriteLine("SendUsMessage was called but no user ID was found in the claims.");
                    return Unauthorized("User ID not found.");
                }

                Console.WriteLine("Starting SendUsMessage for user.");

                model.UserId = userId;
                await mediator.Send(new CreateMessageCommand(model));

                Console.WriteLine("SuccessfullySendUsMessage for user.");
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while SendUsMessage.");
            }

        }
    }
}

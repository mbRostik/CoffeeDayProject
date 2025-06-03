using ContactUs.Application.Contracts.ChangeDTO;
using ContactUs.Application.UseCases.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Text;

namespace ContactUs.WebApi.Controllers
{

    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ContactUsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly HttpClient _httpClient;
        public ContactUsController(IMediator mediator, HttpClient httpClient)
        {
            _httpClient = httpClient;
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

                var spamCheckPayload = new { text = model.User_Message };
                var jsonPayload = JsonSerializer.Serialize(spamCheckPayload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var spamResponse = await _httpClient.PostAsync("http://127.0.0.1:5000/predict_spam", content);

                if (spamResponse.IsSuccessStatusCode)
                {
                    var responseString = await spamResponse.Content.ReadAsStringAsync();
                    var responseJson = JsonDocument.Parse(responseString);
                    var isSpam = responseJson.RootElement.GetProperty("prediction").GetString() == "spam";

                    if (isSpam)
                    {
                        Console.WriteLine("Message identified as spam.");
                        return BadRequest("The message was detected as spam.");
                    }
                }
                else
                {
                    Console.WriteLine("Spam detection service is unavailable.");
                    return StatusCode(500, "Spam detection service is unavailable.");
                }

                model.UserId = userId;
                await mediator.Send(new CreateMessageCommand(model));

                Console.WriteLine("Successfully sent message for user.");
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in SendUsMessage: {ex.Message}");
                return StatusCode(500, "An error occurred while SendUsMessage.");
            }

        }
    }
}

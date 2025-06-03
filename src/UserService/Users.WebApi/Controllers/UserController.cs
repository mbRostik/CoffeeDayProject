using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Users.Application.Contracts.ChangeDTOs;
using Users.Application.Contracts.GetDTOs;
using Users.Application.Contracts.SendDTOs;
using Users.Application.UseCases.Commands;
using Users.Application.UseCases.Queries;

namespace Users.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly HttpClient _httpClient;
        public UserController(IMediator mediator, HttpClient httpClient)
        {
            this.mediator = mediator;
            _httpClient = httpClient;
        }

        [HttpGet("GetUserProfile")]
        public async Task<ActionResult<GetUserProfileDTO>> GetUser()
        {

            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("User ID not found.");
            }

            var result = await mediator.Send(new GetUserProfileQuery(userId));

            if (result == null)
            {
                return NotFound("There is no information for the given user ID.");
            }
            return Ok(result);
        }

        [HttpPost("ChangeUserSettings")]
        public async Task<ActionResult> ChangeUserSettings([FromBody] ChangeProfileInformationDTO model)
        {
            
            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrWhiteSpace(userId))
                {
                    Console.WriteLine("ChangeUserSettings was called but no user ID was found in the claims.");
                    return Unauthorized("User ID not found.");
                }

                Console.WriteLine("Starting ChangeUserSettings for user.");

                model.Id = userId;
                await mediator.Send(new ChangeUserInformationCommand(model));

                Console.WriteLine("Successfully changed settings for user.");
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex + "Error changing settings for user.");
                return StatusCode(500, "An error occurred while changing user settings.");
            }

        }


        [HttpGet("GetUserOrders")]
        public async Task<ActionResult<List<GetUserOrdersDTO>>> GetUserOrders()
        {

            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrWhiteSpace(userId))
                {
                    Console.WriteLine("GetUserOrders was called but no user ID was found in the claims.");
                    return Unauthorized("User ID not found.");
                }

                Console.WriteLine("Starting GetUserOrders for user.");

                
                var result = await mediator.Send(new GetUserOrdersQuery(userId));

                Console.WriteLine("Successfully GetUserOrders for user.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex + "Error GetUserOrders for user.");
                return StatusCode(500, "An error occurred while GetUserOrders.");
            }

        }


        [HttpPost("GetUserOrderDetails")]
        public async Task<ActionResult<List<GetUserOrderDetailsDTO>>> GetUserOrderDetails([FromBody] GetUserOrderDetailsByIdDTO model )
        {

            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrWhiteSpace(userId))
                {
                    Console.WriteLine("GetUserOrderDetails was called but no user ID was found in the claims.");
                    return Unauthorized("User ID not found.");
                }

                Console.WriteLine("Starting GetUserOrderDetails for user.");


                var result = await mediator.Send(new GetUserOrderDetailsQuery(model.Id));

                Console.WriteLine("Successfully GetUserOrderDetails for user.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex + "Error GetUserOrderDetails for user.");
                return StatusCode(500, "An error occurred while GetUserOrderDetails user settings.");
            }

        }



        [HttpPost("UploadProfilePhoto")]
        public async Task<ActionResult<GetUserProfileDTO>> UploadProfilePhoto([FromBody] ChangeProfilePhotoDTO model)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("UploadProfilePhoto called but user ID is missing.");
                return Unauthorized("User ID is required.");
            }

            try
            {
                Console.WriteLine("Attempting to upload profile photo for user");

                // Convert base64 string to image file
                byte[] imageBytes = Convert.FromBase64String(model.Avatar);
                using (var imageContent = new ByteArrayContent(imageBytes))
                {
                    imageContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    using (var formData = new MultipartFormDataContent())
                    {
                        formData.Add(imageContent, "image", "avatar.jpg");

                        // Send request to detect_person endpoint
                        var response = await _httpClient.PostAsync("http://127.0.0.1:5000/detect_person", formData);
                        var responseString = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            var jsonResponse = JsonDocument.Parse(responseString);
                            bool isPerson = jsonResponse.RootElement.GetProperty("is_person").GetBoolean();

                            if (!isPerson)
                            {
                                Console.WriteLine("The uploaded image does not contain a person.");
                                return BadRequest("The uploaded photo must contain a person.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Person detection service is unavailable.");
                            return StatusCode(500, "Person detection service is unavailable.");
                        }
                    }
                }

                // Proceed with updating the profile photo if person detection passed
                model.Id = userId;
                await mediator.Send(new ChangeUserAvatarCommand(model));

                Console.WriteLine("Profile photo updated successfully for user. Fetching updated user profile.");

                var result = await mediator.Send(new GetUserProfileQuery(model.Id));

                if (result == null)
                {
                    Console.WriteLine("Failed to fetch updated profile for user after uploading photo.");
                    return NotFound("User profile not found.");
                }

                Console.WriteLine("Successfully retrieved updated profile for user after photo upload.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while uploading profile photo for user." + ex);
                return BadRequest("Something went wrong.");
            }
        }
    }
}

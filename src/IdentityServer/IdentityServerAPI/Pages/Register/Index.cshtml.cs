﻿// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using IdentityServerHost.Pages.Login;
using MailKit.Net.Smtp;
using MailKit.Security;
using MassTransit;
using MassTransit.RabbitMqTransport;
using MessageBus.Messages.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using static System.Net.WebRequestMethods;

namespace IdentityServerHost.Pages.Register;

[SecurityHeaders]
[AllowAnonymous]
public class RegisterModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IPublishEndpoint _publisher;

    public RegisterModel(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IConfiguration configuration, IPublishEndpoint _publisher
)
    {
        _userManager = userManager;
        this._publisher = _publisher;
        _configuration = configuration;
        _signInManager = signInManager;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }
    public async Task<IActionResult> OnGet(string returnUrl = null)
    {
        if (User.Identity.IsAuthenticated)
        {
            return LocalRedirect("~/");
        }
        ReturnUrl = returnUrl;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        if (User.Identity.IsAuthenticated)
        {
            return LocalRedirect("~/");
        }

        if (ModelState.IsValid)
        {
            var chechkEmail = await _userManager.FindByEmailAsync(Input.Email);
            if (chechkEmail != null)
            {
                ModelState.AddModelError(string.Empty, "This email is already taken");
                return Page();
            }
            else
            {
                var user = new IdentityUser { UserName = Input.Username, Email = Input.Email, EmailConfirmed = false, TwoFactorEnabled = false };
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    var CreatedUser = await _userManager.FindByEmailAsync(user.Email);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);


                    var returnUrlQuery = !string.IsNullOrEmpty(ReturnUrl) ? $"&returnUrl={Uri.EscapeDataString(ReturnUrl)}" : string.Empty;
                    var baseUrl = _configuration.GetValue<string>("Links:BaseUrl");

                    var callbackUrl = $"{baseUrl}ConfirmEmail?id={CreatedUser.Id}&token={Uri.EscapeDataString(code)}{returnUrlQuery}";

                    MailMessage message = new MailMessage();
                    message.From = new MailAddress(_configuration.GetValue<string>("EmailSettings:Email"));
                    message.Subject = "TryIt verification";
                    message.To.Add(new MailAddress(user.Email));
                    message.Body = $"Please confirm your account by <a href='{callbackUrl}'>clicking here</a>.";
                    message.IsBodyHtml = true;

                    var resultConfirmation = await _userManager.ConfirmEmailAsync(user, code);

                    UserCreationEvent creationEvent = new UserCreationEvent
                    {
                        UserId = user.Id,
                        Email = user.Email,
                        UserName = user.UserName
                    };
                    await _publisher.Publish(creationEvent);

                    if (ReturnUrl != null && ReturnUrl != "")
                    {
                        return LocalRedirect(ReturnUrl);
                    }
                    return Redirect("~/");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
           
        }

        return Page();
    }

}




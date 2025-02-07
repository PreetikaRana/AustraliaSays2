using AustraliaSays2_DataAccess.Repository.IRepository;
using AustraliaSays2_Models.DTO;
using AustraliaSays2_Utility;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;

namespace AustraliaSays2.Areas.Admin.Contollers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly IUserRegisterRepository _userRegisterRepository;
        private readonly ILoginUserRepository _loginUserRepository;
        private readonly IEmailSender _emailSender;
        public AuthController(IUserRegisterRepository userRegisterRepository, ILoginUserRepository loginUserRepository, IEmailSender emailSender)
        {
            _userRegisterRepository = userRegisterRepository;
            _loginUserRepository = loginUserRepository;
            _emailSender = emailSender;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Register(RegisterUserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Try adding the user
                    var result = await _userRegisterRepository.AddUserAsync(userDTO);

                    if (result != null)
                    {
                        // Send email only if user was successfully added
                        string subject = "Registration Successful";
                        string body = $"Dear {userDTO.FirstName} {userDTO.LastName},\n\nYour registration to Australia Says is successful.";

                        try
                        {
                            // Send confirmation email
                            await _emailSender.SendEmailAsync(userDTO.Email, subject, body);
                        }
                        catch (Exception emailEx)
                        {
                            // Handle email-specific errors (e.g., email delivery issues)
                            TempData["ErrorMessage"] = "User registration was successful, but there was an issue sending the confirmation email: " + emailEx.Message;
                            return View(nameof(Index)); // Redirect or return view with an email issue
                        }

                        // Show success message
                        TempData["SuccessMessage"] = "Registration Successful! A confirmation email has been sent to your registered email.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "An error occurred while creating your account. Please try again.";
                    }
                }
                catch (Exception ex)
                {
                    // Handle general registration errors
                    if (ex.Message.Contains("Email Already Exists"))
                    {
                        TempData["ErrorMessage"] = "The provided email is already registered.";
                    }
                    else if (ex.Message.Contains("Phone Number Already Exists"))
                    {
                        TempData["ErrorMessage"] = "The provided phone number is already registered.";
                    }
                    else if (ex.Message.Contains("Invalid role"))
                    {
                        TempData["ErrorMessage"] = "The provided role is invalid. Only 'Commenter' and 'Reader' are allowed.";
                    }
                    else
                    {
                        // General error handling
                        TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
                    }
                }
            }
            else
            {
                // Collect all model validation errors and show them
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                TempData["ErrorMessage"] = string.Join(" ", errorMessages);
            }

            return View(nameof(Index));
        }
        [HttpGet]
      public IActionResult Login()
        {
            return View();
        }
        public async Task<IActionResult> Login(LoginUserDTO loginUserDTO)
        {



            //if (string.IsNullOrEmpty(loginUserDTO.Email) || string.IsNullOrEmpty(loginUserDTO.Password))
            //{
            //     TempData["ErrorMessage"] = "Email or Phone Number must be provided.";
            //    return View(loginUserDTO);
            //}
            //if (string.IsNullOrEmpty(loginUserDTO.Password))
            //{
            //    TempData["ErrorMessage"] = "Password must be provided.";
            //    return View(loginUserDTO);
            //}


            try
            {
                var user = await _loginUserRepository.Loginasync(loginUserDTO);

                if (user == null)
                {
                    TempData["ErrorMessage"] = "User not found with the provided credentials.";
                    return View(loginUserDTO);
                }

                TempData["SuccessMessage"] = "Login successful!";
                return View("Login");
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(loginUserDTO);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(loginUserDTO);
            }
        }
        
    }
}

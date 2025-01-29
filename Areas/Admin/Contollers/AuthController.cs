using AustraliaSays2_DataAccess.Repository.IRepository;
using AustraliaSays2_Models.DTO;
using AustraliaSays2_Utility;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AustraliaSays2.Areas.Admin.Contollers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly IUserRegisterRepository _userRegisterRepository;
        private readonly ILoginUserRepository _loginUserRepository;
        public AuthController(IUserRegisterRepository userRegisterRepository, ILoginUserRepository loginUserRepository)
        {
            _userRegisterRepository = userRegisterRepository;
            _loginUserRepository = loginUserRepository;
        }
        public async Task<IActionResult> Index()
        {


            return View();
        }

        public async Task<IActionResult> Register(RegisterUserDTO userDTO)
        {
            var response = new APIResponse();


            if (!ModelState.IsValid)
            {

                response.StatusCode = HttpStatusCode.BadRequest;
                response.Success = false;
                response.Message = "There are validation errors in the form.";
                response.Data = userDTO;


                return View(response);
            }

            try
            {

                var user = await _userRegisterRepository.AddUserAsync(userDTO);
                if (user == null)
                {

                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Success = false;
                    response.Message = "Something went wrong during registration.";
                }
                else
                {

                    response.StatusCode = HttpStatusCode.OK;
                    response.Success = true;
                    response.Message = "User registered successfully.";
                    response.Data = userDTO;
                }
            }
            catch (Exception ex)
            {

                response.StatusCode = HttpStatusCode.BadRequest;
                response.Success = false;
                response.Message = ex.Message;
            }


            return View(response);
        }

        public async Task<IActionResult> Login(LoginUserDTO loginUserDTO)
        {
            // 1. Validate the model state

            if (string.IsNullOrEmpty(loginUserDTO.Email) || string.IsNullOrEmpty(loginUserDTO.Password))
            {
                ModelState.AddModelError(string.Empty, "Email and Password must be provided.");
                return View(loginUserDTO);
            }


            try
            {
                // 2. Check if the user exists in the database
                var user = await _loginUserRepository.Loginasync(loginUserDTO);

                // 3. If user is null (not found), throw an exception
                if (user == null)
                {
                    throw new ArgumentException("User not found with the provided credentials.");
                }

                // 4. If user is found, redirect to Home Index page
                return RedirectToAction("Index", "Home");
            }
            catch (ArgumentException ex)
            {
                // 5. Specific error handling for user not found
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(loginUserDTO);
            }
            catch (Exception ex)
            {
                // 6. Generic exception handling for other errors
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");


                return View(loginUserDTO);
            }
        }

    }
}

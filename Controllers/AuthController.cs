using AustraliaSays2_DataAccess.Repository.IRepository;
using AustraliaSays2_Models.DTO;
using AustraliaSays2_Utility;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AustraliaSays2.Controllers
{
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
            var response = new APIResponse();

            if (!ModelState.IsValid)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Success = false;
                response.Message = "There are validation errors in the form.";
                // Make sure to return the same model so the view can handle it accordingly
                response.Data = loginUserDTO;
                return View(response);
            }

            try
            {
                var user = await _loginUserRepository.Loginasync(loginUserDTO);

                if (user == null)
                {
                    // User was not found or login failed
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Success = false;
                    response.Message = "Login failed. Invalid credentials.";
                }
                else
                {
                    // Successful login
                    response.StatusCode = HttpStatusCode.OK;
                    response.Success = true;
                    response.Message = "Login successful.";
                    response.Data = loginUserDTO; // Send back the login user details in response
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Success = false;
                response.Message = ex.Message;
            }

            return View(response); // Return the response, so the view can render accordingly
        }
    }


}

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
        public AuthController(IUserRegisterRepository userRegisterRepository)
        {
            _userRegisterRepository = userRegisterRepository; 
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

        public async Task<IActionResult> Login()
        {
            return View();
        }


    }
}

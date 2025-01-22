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
            var model = new RegisterUserDTO
            {
                FirstName = "", // Initialize with actual data or empty
                LastName = "",
                Email = "",
                PhoneNumber = "",
                Address = "",
                PostalCode = "",
                Role = ""
            };

            return View();
        }

        public async Task<IActionResult> Register(RegisterUserDTO userDTO)
        {

            var response = new APIResponse();
            try
            {
                var user = await _userRegisterRepository.AddUserAsync(userDTO);
                if (user == null)
                {
                    
                        response.StatusCode= HttpStatusCode.BadRequest;
                    response.Success = false;
                    response.Message = "Their went something wrong durng registration";
                }
               
                response.StatusCode = HttpStatusCode.OK;
                response.Success = true;
                response.Message = "User Register Successfully";
                response.Data = userDTO;
            }
             

            catch (Exception ex)
            { 
                response.StatusCode=HttpStatusCode.BadRequest;
                response.Success= false;
                response.Message = ex.Message;
            }
            return View(response);
        }
             
    }
}

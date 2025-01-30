using AustraliaSays2_DataAccess.Repository.IRepository;
using AustraliaSays2_Models.DTO;
using AustraliaSays2_Utility;
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
            if(ModelState.IsValid)
            {
                await _userRegisterRepository.AddUserAsync(userDTO);
                return View(Index); 
            }
            return RedirectToAction("Index");


        }

        public async Task<IActionResult> Login(LoginUserDTO loginUserDTO)
        {

            if (string.IsNullOrEmpty(loginUserDTO.Email) || string.IsNullOrEmpty(loginUserDTO.Password))
            {
                ModelState.AddModelError(string.Empty, "Email and Password must be provided.");
                return View(loginUserDTO);
            }


            try
            {
                var user = await _loginUserRepository.Loginasync(loginUserDTO);

                if (user == null)
                {
                    throw new ArgumentException("User not found with the provided credentials.");
                }

                return RedirectToAction("Index", "Home");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(loginUserDTO);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");


                return View(loginUserDTO);
            }
        }

    }
}

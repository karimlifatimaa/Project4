using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project4.DTOs;
using System.Runtime.InteropServices;

namespace Project4.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Register()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            User user = new User()
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Surname = registerDto.Surname,
                UserName = registerDto.Username,
            };
            var result=await _userManager.CreateAsync(user, registerDto.Password);
            if(!result.Succeeded)
            {
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                    return View();
                }
            }
            await _userManager.AddToRoleAsync(user, "User");
            return RedirectToAction("Login");
        }
        public async Task<IActionResult> Login()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            
            var user = await _userManager.FindByEmailAsync(loginDto.EmailOrUserName);
            if(user == null)
            {
                user =await _userManager.FindByNameAsync(loginDto.EmailOrUserName);
                if(user == null)
                {
                    ModelState.AddModelError("", "Password or email/username is not valid");
                    return View();
                }
            }
           var result= await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password,true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Try agin later");
                return View();
            }
            if(!result.Succeeded)
            {
                ModelState.AddModelError("", "Password or email/username is not valid");
                return View();
            }
            await _signInManager.SignInAsync(user, loginDto.IsRemember);
            return RedirectToAction("Index", "Home");

           
        }
        public async Task<IActionResult> CreateRole()
        {
            IdentityRole identityRole = new IdentityRole("Admin");
            IdentityRole identityRole1 = new IdentityRole("User");
            await _roleManager.CreateAsync(identityRole);
            await _roleManager.CreateAsync(identityRole1);
            return Ok();

        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

using JobBoard.Models;
using JobBoard.Models.DataContext;
using JobBoard.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Controllers
{
	public class AccountController : Controller
	{
		private DataContext _dataContext;
		private UserManager<AppUser> _userManager;
		private SignInManager<AppUser> _signInManger;
		private RoleManager<IdentityRole> _roleManager;

		public AccountController(DataContext dataContext,UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            _dataContext = dataContext;
			_userManager = userManager;
			_signInManger = signInManager;
			_roleManager = roleManager;
        }
        public IActionResult Index()
		{
			return View();
		}

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel registerVM)
		{
			if (!ModelState.IsValid) return View(registerVM);

			AppUser user = await _userManager.FindByNameAsync(registerVM.Username);

			if(user != null)
			{
				ModelState.AddModelError("Username", "Username has taken!");
				return View(registerVM);
			}

			user = await _userManager.FindByEmailAsync(registerVM.Email);

			if(user != null)
			{
				ModelState.AddModelError("Email", "Email has taken!");
				return View(registerVM);
			}

			AppUser newUser = new AppUser
			{
				UserName = registerVM.Username,
				Fullname = registerVM.Fullname,
				Email = registerVM.Email
			};

			var userResult = await _userManager.CreateAsync(newUser,registerVM.Password);

			if (!userResult.Succeeded)
			{
				foreach (var err in userResult.Errors)
				{
					ModelState.AddModelError("", err.Description);
				}
				return RedirectToAction("Index", "Home");
			}

			//add role 

			var roleResult = await _userManager.AddToRoleAsync(newUser, "Admin");
			if (!roleResult.Succeeded)
			{
				foreach (var err in roleResult.Errors)
				{
					ModelState.AddModelError("", err.Description);
				}
				return View(registerVM);
			}


			await _signInManger.SignInAsync(newUser, false);

			return RedirectToAction("Index","Home");
		}

		public IActionResult Login()
		{
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginVM)
		{
			if (!ModelState.IsValid) return View(loginVM);

			AppUser user = await _userManager.FindByEmailAsync(loginVM.Email);
			if(user is null)
			{
				ModelState.AddModelError("", "Email or password incorrect!");
				return View(loginVM);
			}

			var result = await _userManager.CheckPasswordAsync(user,loginVM.Password);
			if (!result)
			{
				ModelState.AddModelError("", "Email or password incorrect!");
				return View();
			}

			await _signInManger.SignInAsync(user, false);

			return RedirectToAction("Index", "Home");

		}
		
		public async Task<IActionResult> Logout()
		{
			if (User.Identity.IsAuthenticated)
			{
				await _signInManger.SignOutAsync();
			}

			return RedirectToAction("Index", "Home");
		}
		//public async Task<IActionResult> CreateRole()
		//{
		//	IdentityRole newRole = new IdentityRole("Company");

		//	await _roleManager.CreateAsync(newRole);

		//	return Content("Created");
		//}
	}
}

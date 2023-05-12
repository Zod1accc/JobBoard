using JobBoard.Models;
using Microsoft.AspNetCore.Identity;

namespace JobBoard.Services
{
	public class LayoutService
	{
		private IHttpContextAccessor _httpContextAccessor;
		private UserManager<AppUser> _userManager;

		public LayoutService(IHttpContextAccessor httpContextAccessor,UserManager<AppUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
			_userManager = userManager;
        }

		public async Task<string> GetUserId()
		{
			string id = null;
			AppUser user = null;

			if ( _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
			{
				user = await _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
				id = user.Id;
			}

			return id;
		}
    }
}

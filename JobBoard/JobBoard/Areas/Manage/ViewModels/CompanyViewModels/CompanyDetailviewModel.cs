using JobBoard.Models;
using System.ComponentModel.DataAnnotations;

namespace JobBoard.Areas.Manage.ViewModels.CompanyViewModels
{
	public class CompanyDetailviewModel
	{
        public Company Company { get; set; }
        public AppUser User { get; set; }
    }
}

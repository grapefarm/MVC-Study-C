using System.ComponentModel.DataAnnotations;

namespace WebApplication4.ViewModels
{
	public class EmployeeCreateViewModel
	{
		[Required(ErrorMessage = "姓氏必填")]
		[StringLength(50)]
		public string LastName { get; set; }

		[Required(ErrorMessage = "名字必填")]
		[StringLength(50)]
		public string FirstName { get; set; }

		public string Title { get; set; }
	}
}

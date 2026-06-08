using System.ComponentModel.DataAnnotations;

namespace WebApplication4.ViewModels
{
	public class EmployeeEditViewModel
	{
		public int EmployeeId { get; set; } // 隱藏欄位，確保更新正確的 ID
		
		[Required(ErrorMessage = "姓氏必填")]
		[StringLength(50)]
		public string LastName { get; set; }

		[Required(ErrorMessage = "名字必填")]
		[StringLength(50)]
		public string FirstName { get; set; }
		public string Title { get; set; }
	}
}

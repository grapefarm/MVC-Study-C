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

		[Required]
		[MinLength(4, ErrorMessage = "帳號至少 4 碼")]
		public string Username { get; set; }

		[Required(ErrorMessage = "密碼必填")]
		[DataType(DataType.Password)] // 這是關鍵！它會讓網頁自動把文字變成星號或黑點
		[StringLength(20, MinimumLength = 8, ErrorMessage = "密碼長度需在 8-20 碼之間")]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,20}$",ErrorMessage = "密碼必須包含大小寫字母與數字")]
		public string Password { get; set; }
	}
}

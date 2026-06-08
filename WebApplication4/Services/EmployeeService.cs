using WebApplication4.Models;

namespace WebApplication4.Services
{
	public class EmployeeService : IEmployeeService
	{
		private readonly NorthwindContext _context;
		public EmployeeService(NorthwindContext context) => _context = context;

		public async Task<(bool CanDelete, string Message)> CheckDelete(int id)
		{
			var employee = await _context.Employees.FindAsync(id);
			
			if (employee == null)
			{ 
				return (false, "找不到員工");
			}

			if (employee.Title == "Sales Representative")
			{
				return (false, "安全提示：Sales Representative不可刪除");
			}

			return (true, "成功");
		}
	}
}

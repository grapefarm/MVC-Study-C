using Microsoft.EntityFrameworkCore;
using WebApplication4.Common;
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

		public async Task<PagedResult<Employee>> GetEmployeesAsync(string keyword, string sort, int page, int pageSize)
		{
			var query = _context.Employees.AsQueryable();

			// 搜尋
			if (!string.IsNullOrEmpty(keyword))
			{
				query = query.Where(e => e.LastName.Contains(keyword) || e.FirstName.Contains(keyword));
			}

			// 總筆數 (計算分頁用)
			int totalCount = await query.CountAsync();

			// 排序
			query = sort switch
			{
				"title_desc" => query.OrderByDescending(e => e.Title),
				"title_asc" => query.OrderBy(e => e.Title),
				_ => query.OrderBy(e => e.EmployeeId)
			};

			// 分頁
			var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

			return new PagedResult<Employee>
			{
				Items = items,
				PageNumber = page,
				TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
			};
		}
	}
}

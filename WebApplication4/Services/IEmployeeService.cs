using WebApplication4.Common;
using WebApplication4.Models;

namespace WebApplication4.Services
{
	public interface IEmployeeService
	{
		Task<(bool CanDelete, string Message)> CheckDelete(int id);
	}
}

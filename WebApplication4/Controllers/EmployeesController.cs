using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;
using WebApplication4.Services;
using WebApplication4.ViewModels;

namespace WebApplication4.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly NorthwindContext _context;
		private readonly IEmployeeService _employeeService;

		public EmployeesController(NorthwindContext context, IEmployeeService employeeService)
        {
            _context = context;
            _employeeService = employeeService;
		}

		// GET: Employees
		public async Task<IActionResult> Index(string keyword, string sort, int page = 1)
		{
			int pageSize = 5;

			// 呼叫 Service 取得封裝好的資料
			var model = await _employeeService.GetEmployeesAsync(keyword, sort, page, pageSize);

			// 儲存狀態給 View 使用
			ViewData["Keyword"] = keyword;
			ViewData["CurrentSort"] = sort;
			ViewData["Sort"] = (sort == "title_asc") ? "title_desc" : "title_asc";

			return View(model); // 直接傳入 PagedResult
		}

		// GET: Employees/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
				var employee = new Employee
				{
					LastName = vm.LastName,
					FirstName = vm.FirstName,
					Title = vm.Title
				};

				_context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

			// 把撈出來的資料庫映射給vm
			var vm = new EmployeeEditViewModel
			{
				EmployeeId = employee.EmployeeId,
				LastName = employee.LastName,
				FirstName = employee.FirstName,
				Title = employee.Title
			};

			return View(vm);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeEditViewModel vm)
        {
            if (id != vm.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
				var employee = await _context.Employees.FindAsync(id);
                if (employee == null) 
                { 
                    return NotFound();
                }

                employee.LastName = vm.LastName;
                employee.FirstName = vm.FirstName;
                employee.Title = vm.Title;

                await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		[HttpPost]
		public async Task<IActionResult> CanDelete(int id)
		{
			var result = await _employeeService.CheckDelete(id);
			return Json(new { canDelete = result.CanDelete, message = result.Message });
		}

		private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }

    }
}

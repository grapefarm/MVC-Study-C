using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;
using WebApplication4.Services;

namespace WebApplication4
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// 註冊DbContext
			builder.Services.AddDbContext<NorthwindContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("NorthwindContext")));		
			//註冊Employee服務
			builder.Services.AddScoped<IEmployeeService, EmployeeService>();

			// Add services to the container.
			builder.Services.AddControllersWithViews();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}

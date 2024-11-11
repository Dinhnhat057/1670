using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();
app.UseEndpoints(endpoints =>
{
    if (endpoints != null)
    {
        endpoints.MapAreaControllerRoute(
            name: "NhaTuyenDung",
            areaName: "NhaTuyenDung",
            pattern: "nha-tuyen-dung/{controller=Login}/{action=Index}/{id?}"
        );

        endpoints.MapAreaControllerRoute(
            name: "Admin",
            areaName: "Admin",
            pattern: "Admin/{controller= Account}/{action=Login}/{id?}"
        );

        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=ClientHome}/{action=Index}/{id?}"
        );

        endpoints.MapControllerRoute(
            name: "TinTuyenDung",
            pattern: "tin-tuyen-dung/{id?}",
            defaults: new { controller = "ClientHome", action = "Detail" },
            constraints: new { id = @"\d+" }
            );
        endpoints.MapControllers();
    }

});
app.Run();

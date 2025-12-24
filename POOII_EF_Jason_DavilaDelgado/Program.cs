using Microsoft.EntityFrameworkCore;
using POOII_EF_Jason_DavilaDelgado.Data;

var builder = WebApplication.CreateBuilder(args);

// 1 Conexión a la BD
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3 Add services to the container (Agregar soporte MVC)
builder.Services.AddControllersWithViews();

var app = builder.Build();

// 4 Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// 5 Rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

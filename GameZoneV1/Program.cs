var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") //1
    ?? throw new InvalidOperationException("No Connection String Was Found");
builder.Services.AddDbContext<ApplicationDBContext>(options =>
options.UseSqlServer(connectionString)); 


builder.Services.AddIdentity<IdentityUser , IdentityRole> (Options => Options.Password.RequireDigit=true)
 .AddEntityFrameworkStores<ApplicationDBContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICategoriesService, CategoriesService>();
builder.Services.AddScoped<IDevicesService ,DevicesService>();
builder.Services.AddScoped<IGamesService, GamesService>();
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

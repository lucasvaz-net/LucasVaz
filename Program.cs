using LucasVaz.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<DataConnection>();
builder.Services.AddScoped<ContatoDal>();
builder.Services.AddScoped<PessoaDal>();
builder.Services.AddScoped<PrivacidadeDal>();
builder.Services.AddScoped<ProjetoDal>();
builder.Services.AddScoped<EstudoDal>();
builder.Services.AddScoped<ExperienciaDal>();
   


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


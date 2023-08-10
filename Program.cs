using LucasVaz.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies; // Adicione isso

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Adicione o serviço de Sessão
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Adicione o serviço de autenticação baseado em Cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login"; // Seu caminho de login, ajuste conforme necessário
        options.AccessDeniedPath = "/AcessoNegado"; // Seu caminho de acesso negado, ajuste conforme necessário
    });

builder.Services.AddScoped<DataConnection>();
builder.Services.AddScoped<ContatoDal>();
builder.Services.AddScoped<PessoaDal>();
builder.Services.AddScoped<PrivacidadeDal>();
builder.Services.AddScoped<ProjetoDal>();
builder.Services.AddScoped<EstudoDal>();
builder.Services.AddScoped<ExperienciaDal>();
builder.Services.AddScoped<HabilidadeDal>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Use Session middleware
app.UseSession();

// Use Authentication middleware - deve vir antes de UseAuthorization
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

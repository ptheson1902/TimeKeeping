using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UNN_Ki_001;
using UNN_Ki_001.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


// Identityを追加
builder.Services.AddIdentity<AppUser, AppRole>(options => options.Stores.MaxLengthForKeys = 128)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddErrorDescriber<IdentityErrorDescriberJP>();

// KintaiDbContext
builder.Services.AddDbContext<KintaiDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<KinmuManager>();

// 承認設定
builder.Services.AddAuthorization(options =>
{
    // フォールバックポリシーの設定
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireRole("Admin")
        .Build();

    // Admin許可用のポリシー
    options.AddPolicy("Admin", builder =>
    {
        builder.RequireRole("Admin");
    });

    // ついでにManager用も用意しておく
    options.AddPolicy("Manager", builder =>
    {
        builder.RequireRole("Manager");
    });

    // 一般ユーザー用
    options.AddPolicy("Rookie", builder =>
    {
        builder.RequireAuthenticatedUser();
    });
});

// Cookie設定
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = ".UNN.Session";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.LoginPath = "/Account/Login";
});

// セッションの使用を有効化
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

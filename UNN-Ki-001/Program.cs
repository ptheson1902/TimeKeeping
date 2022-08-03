using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UNN_Ki_001;
using UNN_Ki_001.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Connection"))
);

// Identity��ǉ�
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.Stores.MaxLengthForKeys = 128)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddErrorDescriber<IdentityErrorDescriberJP>();

// ���F�ݒ�
builder.Services.AddAuthorization(options =>
{
    // �t�H�[���o�b�N�|���V�[�̐ݒ�
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireRole("Admin")
        .Build();

    // Admin���p�̃|���V�[
    options.AddPolicy("Admin", builder =>
    {
        builder.RequireRole("Admin", "Manager");
    });

    // ���ł�Manager�p���p�ӂ��Ă���
    options.AddPolicy("Manager", builder =>
    {
        builder.RequireRole("Manager");
    });

    // ��ʃ��[�U�[�p
    options.AddPolicy("Rookie", builder =>
    {
        builder.RequireAuthenticatedUser();
    });
});

// Cookie�ݒ�
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.LoginPath = "/Identity/Account/Login";
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

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

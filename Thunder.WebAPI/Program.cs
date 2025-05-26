using Microsoft.AspNetCore.Authentication.Cookies;
using Thunder.Application.Interfaces;
using Thunder.Infrastructure.Generic;
using Thunder.Infrastructure.Identity;
using Thunder.Infrastructure.Repositories.References;
using Thunder.Infrastructure.Repositories.Users;
using Thunder.Persistence.AutoMapper;
using Thunder.Persistence.Context;
using Thunder.Persistence.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.IgnoreObsoleteActions();
	options.IgnoreObsoleteProperties();
	options.CustomSchemaIds(type => type.FullName);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.LoginPath = "/Auth/Login";
		options.LogoutPath = "/Auth/Logout";
		options.AccessDeniedPath = "/Auth/AccessDenied";
		options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
		options.SlidingExpiration = true;
		options.Cookie.HttpOnly = true;
		options.Cookie.Name = "thunder-pms-project";
	});

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<PasswordHasher>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IReferenceRepository, ReferenceRepository>();
builder.Services.AddScoped<IMapper, Mapper>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

using AspNetCore.ExternalLogin;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(config =>
    {
        config.DefaultScheme = TwitterDefaults.AuthenticationScheme;
    })
    .AddTwitter(config =>
    {
        config.CallbackPath = new PathString("/auth_callback");
        config.RetrieveUserDetails = true;

        // keys
        config.ConsumerKey = builder.Configuration["Authentication:Twitter:ConsumerKey"];
        config.ConsumerSecret = builder.Configuration["Authentication:Twitter:ConsumerSecret"];
    });

builder.Services.AddDbContext<ApplicationDataContext>(config =>
{
    config.UseInMemoryDatabase("contoso");
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddSignInManager()
    .AddEntityFrameworkStores<ApplicationDataContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
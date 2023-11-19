using System.Text;
using API.Data;
using API.Extensions;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add AppDbContext as a service
// So that will be able to inject to other parts of the application


builder.Services.AddControllers();

// extended with own extension method
builder.Services.AddApplicationServices(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// Moved to ApplicationServiceExtensions
// builder.Services.AddDbContext<AppDbContext>(options =>
// {
//     options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
// });

//builder.Services.AddCors();
// lifetime of how long do we want this service to be available for
//builder.Services.AddScoped<ITokenService, TokenService>();


builder.Services.AddIdentityServices(builder.Configuration);
// Add authentication service and specify the type of authentication scheme
// This gives server enough information to take a look at the token and validate it just based on the issuer signing key
// In order for this service to be used, we need to add the middleware to authenticate the request
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//                 // specify all of the rules about how our server should validate
//                 .AddJwtBearer(options =>
//                 {
//                     options.TokenValidationParameters = new TokenValidationParameters
//                     {
//                         ValidateIssuerSigningKey = true,
//                         // specify the exact same key that we used in the token service.
//                         IssuerSigningKey = new SymmetricSecurityKey(Encoding
//                             .UTF8.GetBytes(builder.Configuration["TokenKey"])),
//                         ValidateIssuer = false,
//                         ValidateAudience = false
//                     };
//                 });


var app = builder.Build();

// Configure the HTTP request pipeline.
// Order is important
// builder = Cors policy builder
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

// order is important
app.UseAuthentication(); // ask you have valid token
app.UseAuthorization();  // what are you allow to do

app.MapControllers();

app.Run();

using MongoDB.Driver;
using EjerciciosProgramacion.Util;
using EjerciciosProgramacion.Models;
using FluentValidation;
using EjerciciosProgramacion.Dto;
using EjerciciosProgramacion.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options => {
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
  options.TokenValidationParameters = new TokenValidationParameters {
    ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "ejercicios.com",
    ValidAudience = builder.Configuration["Jwt:Audience"] ?? "ejercicios.com",
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "sdsadsdsdsdsadsadsadasddsa")),
    ValidateIssuer = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
  };
});
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IValidator<User>, RegisterDto>();

var connectionString = Environment.GetEnvironmentVariable("MONGODB_URI");

if (connectionString == null)
{
  connectionString = "mongodb://localhost:27017";
}

var client = new MongoClient(connectionString);

var userService = new UserService(client);

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", [AllowAnonymous] () => {
  return Results.Ok(new SimpleResponse() { message = "Success", code = 200 });
});

app.MapPost("/login", [AllowAnonymous] (LoginDto login) => {
  var issuer = "ejercicios.com";
  var audience = "ejercicios.com";
  var key = Encoding.ASCII.GetBytes("ijurkbdlhmklqacwqzdxmkkhvqowlyqa");
  var jwtTokenHandler = new JwtSecurityTokenHandler();
  var tokenDescriptor = new SecurityTokenDescriptor
  {
    Subject = new ClaimsIdentity(new[]
    {
      new Claim("id", "1"),
      new Claim(JwtRegisteredClaimNames.Sub, "username"),
      new Claim(JwtRegisteredClaimNames.Email, "email@email.comd"),
      new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    }),
    Expires = DateTime.UtcNow.AddDays(30),
    Audience = audience,
    Issuer = issuer,
    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),
  };
  var token = jwtTokenHandler.CreateToken(tokenDescriptor);
  var jwtToken = jwtTokenHandler.WriteToken(token);
  return Results.Ok(jwtToken);
});

app.MapPost("/register", [AllowAnonymous] async (IValidator<User> validator, User user) => {
  var results = await validator.ValidateAsync(user);
  if (!results.IsValid)
  {
    return Results.ValidationProblem(results.ToDictionary());
  }
  user.password = Func.Sha256(user.password);
  await userService.CreateAsync(user);
  return Results.Created("User created successfully", user);
});

app.MapGet("/test", [Authorize] () => Results.Ok(new SimpleResponse { message = "success", code = 200 }));


app.Run();

record LoginDto(string email, string password);

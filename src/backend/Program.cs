using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

builder.Services
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.Authority = configuration["AUTHORITY"];
    options.Audience = configuration["AUDIENCE"];
    options.IncludeErrorDetails = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        //ValidAudiences = new[] { "master-realm", "account" },
        ValidateIssuer = false,
        //ValidIssuer = configuration["AUTHORITY"],
        ValidateLifetime = true, 
        RequireExpirationTime = true                        
    };
});

builder.Services.AddCors();


// Scoped access handler, attribute will not work without it: Authorize("my_scope")
builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();


// Restrict access by scope for the entire application

builder.Services.AddAuthorization();

// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("my_scope", policy => policy.Requirements.Add(new HasScopeRequirement("my_scope", configuration["AUTHORITY"])));
// });


var app = builder.Build();

 app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);
app.UseAuthentication();
app.UseAuthorization();


app.MapGet("/healthcheck", () => "I'm alive!");


app.MapGet("/userinfo", (HttpContext context) => {
    var claims = context.User.Claims.ToDictionary(x => x.Type, x => x.Value);
    return Results.Json(claims);
})
.RequireAuthorization();

app.Run();

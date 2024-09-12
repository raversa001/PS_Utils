using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PS_Utils.Components;
using PS_Utils.Services;
using System.Text;

namespace PS_Utils
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to DI container
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddSingleton<JwtTokenService>();

            builder.Services.AddRazorComponents().AddInteractiveServerComponents(); // Include Interactive Components support


            // Add authentication services for JWT Bearer
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtTokenService.SecretKey))
                    };
                });

            builder.Services.AddAuthorization(); // Add authorization

            builder.Services.AddAntiforgery(); // Add Antiforgery services

            var app = builder.Build();

            // Ensure middleware is in the correct order
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();            

            // Add Antiforgery middleware between authentication and routing            

            app.UseRouting();
            app.UseAntiforgery();
            app.UseAuthorization();

            app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}

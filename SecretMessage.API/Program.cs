using FirebaseAdmin;
using FirebaseAdminAuthentication.DependencyInjection.Extensions;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authorization;
using SecretMessage.Core;

namespace SecretMessage.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Add Firebase Authentication
            builder.Services.AddSingleton(FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromJson(builder.Configuration.GetValue<string>("FIREBASE_CONFIG"))
            }));
            builder.Services.AddFirebaseAuthentication();
            builder.Services.AddAuthorization();

            var app = builder.Build();


            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            // Minimal endpoint with authentication user 
            app.MapGet("/", [Authorize] () =>
            {
                return Results.Json(new SecretMessageResponse() { SecretMessage = "You are authenticate or ?" });
            });


            // Start the application.
            app.Run();
        }
    }
}

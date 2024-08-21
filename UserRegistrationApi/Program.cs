using Microsoft.EntityFrameworkCore;
using UserRegistrationApi.Infrastructure.Extensions;
using UserRegistrationApi.Services;

namespace UserRegistrationApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddInfrastructureServices(builder.Configuration.GetConnectionString("DefaultConnection"));

            builder.Services.AddScoped<IUserCredentialService, UserCredentialService>();
            builder.Services.AddScoped<IProfilePictureService, ProfilePictureService>();
            builder.Services.AddScoped<ICreateUserMapper, CreateUserMapper>();
            builder.Services.AddScoped<IUserService, UserService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

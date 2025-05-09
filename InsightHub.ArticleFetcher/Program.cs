
using InsightHub.ArticleFetcher.Services;

namespace InsightHub.ArticleFetcher
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
            builder.Services.AddSingleton<LocalArticleIndex>();
            builder.Services.AddHttpClient<SemanticScholarService>(client =>
            {
                client.DefaultRequestHeaders.Add("User-Agent", "InsightHub/1.0 (mytrados@gmail.com)");
            });
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors();

            app.UseAuthorization();

            app.UseDefaultFiles();
            app.UseStaticFiles();


            app.MapControllers();

            // Fallback to index.html for React SPA
            app.MapFallbackToFile("index.html");


            app.Run();
        }
    }
}

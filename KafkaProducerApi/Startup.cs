using Confluent.Kafka;
using Kafka.Library;
using Microsoft.Extensions.Configuration;

namespace KafkaProducerApi
{
    public class Startup
    {
        public IConfiguration configRoot {get;}

        public Startup(IConfiguration configuration)
        {
            configRoot = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddRazorPages();
            //dependecy Injection service for producer
       
            services.AddTransient<KafkaProducer>();
        }
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
     
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();

        }

    }
}

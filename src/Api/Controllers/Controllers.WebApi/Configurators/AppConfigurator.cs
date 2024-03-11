using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Rhyous.EntityAnywhere.WebApi
{
    public class AppConfigurator : IAppConfigurator
    {
        public void Configure(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v2/swagger.json", "Rhyous.EntityAnywhere v2"));
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors("MyAllowedOrigins");

            app.MapControllers();
        }
    }
}

using Swashbuckle.Application;
using System.Web.Http;
using WebActivatorEx;
using WebApi;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace WebApi
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "Web Api Lanchonete");
                        c.IncludeXmlComments(GetXmlCommentsPath());
                    });
        }

        protected static string GetXmlCommentsPath()
        {
            return string.Format(@"{0}\SwaggerDocs\WebApi.XML", System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}

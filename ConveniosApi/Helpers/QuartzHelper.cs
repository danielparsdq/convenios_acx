using Quartz;

namespace ConveniosApi.Helpers
{
    public class QuartzHelper
    {
        public static void Configure(IServiceCollectionQuartzConfigurator q)
        {
            //q.UseMicrosoftDependencyInjectionScopedJobFactory();
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            var listQuartzItems = configuration.GetSection("Quartz").GetChildren();

            foreach (var quartzItem in listQuartzItems)
            {

                
            }
        }
    }
}

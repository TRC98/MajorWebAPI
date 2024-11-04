namespace MajorWebAPI.Extensions
{
    public static class ConfigureService
    {
        private static IConfiguration _Configuration;

        public static IServiceCollection Configrue(IServiceCollection services,IConfiguration configuration)
        {
            _Configuration = configuration;

            return services;
        }
    }
}

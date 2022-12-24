using Bussiness.Mapping;

namespace CustomerManagement.Extension
{
    public static class RegisterServiceExtension
    {
        public static void DIService(this IServiceCollection service)
        {
            service.AddAutoMapper(typeof(MappingProfile));
            
        }
    }
}

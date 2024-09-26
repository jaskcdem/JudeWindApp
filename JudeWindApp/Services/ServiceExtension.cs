using Common.Interfaces;
using JudeWind.Service;
using System.Reflection;

namespace JudeWindApp.Services
{
    /// <summary>  </summary>
    public static class ServiceExtension
    {
        /// <summary> import customs services </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddDataService(this IServiceCollection services)
        {
            #region 依賴註入
            var baseType = typeof(BaseService);
            var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var referencedAssemblies = Directory.GetFiles(path, "*.dll").Select(Assembly.LoadFrom).ToArray();
            var serviceTypes = referencedAssemblies.SelectMany(a => a.DefinedTypes).Select(ti => ti.AsType()).Where(t => t != baseType && !t.IsAbstract && baseType.IsAssignableFrom(t)).ToArray();
            var implementTypes = serviceTypes.Where(x => x.IsClass).ToArray();
            foreach (var implementType in implementTypes)
            {
                services.AddScoped(implementType);
            }
            #endregion
            return services;
        }
        /// <summary> import customs services </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddIDataService(this IServiceCollection services)
        {
            #region 依賴註入
            var baseType = typeof(IApiService);
            var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var referencedAssemblies = System.IO.Directory.GetFiles(path, "*.dll").Select(Assembly.LoadFrom).ToArray();
            var types = referencedAssemblies.SelectMany(a => a.DefinedTypes).Select(type => type.AsType()).Where(x => x != baseType && baseType.IsAssignableFrom(x)).ToArray();
            var implementTypes = types.Where(x => x.IsClass).ToArray();
            var interfaceTypes = types.Where(x => x.IsInterface).ToArray();
            foreach (var implementType in implementTypes)
            {
                var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
                if (interfaceType != null)
                    services.AddScoped(interfaceType, implementType);
            }
            #endregion
            return services;
        }
    }
}

using System.Linq;
using System.Reflection;
using AutoMapper;

namespace GameStore.WEB.AutoMapper
{
    public class AutoMapperConfiguration
    {
        public void Config()
        {
            var assembliesToScan = System.AppDomain.CurrentDomain.GetAssemblies();
            var allTypes = assembliesToScan.SelectMany(a => a.ExportedTypes).ToArray();

            var profiles =
                allTypes
                    .Where(t => typeof(Profile).GetTypeInfo().IsAssignableFrom(t.GetTypeInfo()))
                    .Where(t => !t.GetTypeInfo().IsAbstract);

            Mapper.Initialize(cfg =>
            {
                foreach (var profile in profiles)
                {
                    cfg.AddProfile(profile);
                }
            });
        }
    }
}
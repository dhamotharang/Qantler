using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Services
{
    public static class CustomConfigProviderExtensions
    {
        public static IConfigurationBuilder AddEncryptedProvider(this IConfigurationBuilder builder, IConfiguration configuration, byte[] Key, byte[] IV)
        {
            return builder.Add(new CustomConfigProvider(configuration, Key, IV));
        }
    }
}

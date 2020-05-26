using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NuciDAL.Repositories;

using DynamicNamesModGenerator.Configuration;
using DynamicNamesModGenerator.DataAccess.DataObjects;
using DynamicNamesModGenerator.Service.ModBuilders.CrusaderKings2;

namespace DynamicNamesModGenerator
{
    public class Program
    {
        static DataStoreSettings dataStoreSettings;

        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">CLI arguments</param>
        public static void Main(string[] args)
        {
            IConfiguration config = LoadConfiguration();
            dataStoreSettings = new DataStoreSettings();
            config.Bind(nameof(DataStoreSettings), dataStoreSettings);

            IServiceProvider serviceProvider = new ServiceCollection()
                .AddSingleton(dataStoreSettings)
                .AddSingleton<IRepository<LocationEntity>>(s => new XmlRepository<LocationEntity>(dataStoreSettings.TitleStorePath))
                .AddSingleton<ICK2ModBuilder, CK2ModBuilder>()
                .BuildServiceProvider();
            
            ICK2ModBuilder ck2Builder = serviceProvider.GetService<ICK2ModBuilder>();
            ck2Builder.Build();
        }
        
        static IConfiguration LoadConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
        }
    }
}
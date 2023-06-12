﻿using Frame.Core;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceControllerExtentions
    {

        /// <summary>
        /// 根据模组自动注入对应模组
        /// 【此方式性能好，但需要一个一个注入进来】
        /// </summary>
        /// <typeparam name="TMoudle"></typeparam>
        /// <param name="app"></param>
        /// <returns></returns>
        /// <exception cref="WebException"></exception>
        public static IServiceCollection AddModule<TMoudle>(this IServiceCollection app)
            where TMoudle : IModule, new()
        {
            Assembly? assembly = Assembly.GetAssembly(typeof(TMoudle));

            if (assembly is null) throw new ApplicationException("程序集为空无法注入");

            AutoDependencyInjection(app, assembly);
            return app;
        }
        /// <summary>
        /// 遍历所有的程序集注入
        /// 【此方式性能不好，遍历了所有的引用程序集】
        /// </summary>
        /// <typeparam name="TMoudle"></typeparam>
        /// <param name="app"></param>
        /// <returns></returns>
        /// <exception cref="WebException"></exception>
        public static IServiceCollection AddModuleAll(this IServiceCollection app)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            if (assembly is null) throw new ApplicationException("程序集为空无法注入");

            AutoDependencyInjection(app, assembly);

            var referencedAssemblies = assembly.GetReferencedAssemblies();
            foreach (var item in referencedAssemblies)
            {
                Assembly itemAssembly = Assembly.Load(item);
                AutoDependencyInjection(app, itemAssembly);
            }

            return app;
        }

        /// <summary>
        /// 注入类
        /// </summary>
        /// <param name="app"></param>
        /// <param name="assembly"></param>
        /// <exception cref="ArgumentNullException"></exception>
        private static void AutoDependencyInjection(IServiceCollection app, Assembly assembly)
        {
            if (assembly is null) throw new ArgumentNullException(nameof(assembly));

            Type[] types = assembly.GetExportedTypes();
            foreach (Type type in types)
            {
                if (type.IsPublic || type.IsClass || type.IsAbstract)
                {
                    var imps = type.GetInterfaces();
                    if (imps.Any(t => t.Equals(typeof(ITransientInstance))))
                    {
                        app.AddTransient(type);
                    }
                    else if (imps.Any(t => t.Equals(typeof(ISingletonInstance))))
                    {
                        app.AddSingleton(type);
                    }
                    else if (imps.Any(t => t.Equals(typeof(IScopedInstance))))
                    {
                        app.AddScoped(type);
                    }
                }
            }
        }
    }
}

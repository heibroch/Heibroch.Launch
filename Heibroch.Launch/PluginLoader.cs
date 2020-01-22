using Heibroch.Common;
using Heibroch.Launch.Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Heibroch.Launch
{
    public interface IPluginLoader
    {
        List<ILaunchPlugin> Plugins { get; }

        void Load();
    }

    public class PluginLoader : IPluginLoader
    {
        private List<Assembly> loadedAssemblies = new List<Assembly>();

        public PluginLoader()
        {
            Plugins = new List<ILaunchPlugin>();
        }

        public List<ILaunchPlugin> Plugins { get; }

        public void Load()
        {
            var pluginDirectories = Directory.GetDirectories("Plugins");

            foreach(var pluginDirectory in pluginDirectories)
            {
                try
                {
                    //var appDomain = AppDomain.CreateDomain(pluginDirectory, null, appDomainSetup);
                    var assemblyFiles = Directory.GetFiles(pluginDirectory, "*.dll");

                    var pluginDirectoryAssemblies = new List<Assembly>();
                    //Load assemblies
                    foreach (var assemblyFilePath in assemblyFiles)
                    {
                        var assembly = Assembly.LoadFile(Path.GetFullPath(assemblyFilePath));//LoadDependencies(assemblyFilePath);
                        loadedAssemblies.Add(assembly);
                        pluginDirectoryAssemblies.Add(assembly);
                    }

                    //Load types
                    foreach (var assembly in pluginDirectoryAssemblies)
                    {
                        if (assembly.FullName.Contains("Microsoft")) continue;
                        if (assembly.FullName.StartsWith("System")) continue;
                        //AppDomain.CurrentDomain.Load
                        //if (assemblyFile.Contains("Heibroch.Launch.Plugin.dll")) continue;
                        //var assembly = AppDomain.CurrentDomain.LoadFile(Path.GetFullPath(assemblyFile)); //LoadDependencies(assemblyFile);//Assembly.Load(assemblyFile);
                        var types = assembly.GetExportedTypes();

                        foreach (var type in types)
                        {
                            if (!type.GetInterfaces().Any(x => x == typeof(ILaunchPlugin))) continue;
                            var constructors = type.GetConstructors();
                            if (constructors.Length <= 0) continue;
                            var constructor = constructors[0];
                            var parameters = constructor.GetParameters();
                            var resolvedParameters = new List<object>();
                            foreach (var parameter in parameters)
                            {
                                var currentContainer = Container.Current;
                                var method = currentContainer.GetType().GetMethod("Resolve");
                                var genericMethod = method.MakeGenericMethod(parameter.ParameterType);
                                var resolvedParameter = genericMethod.Invoke(currentContainer, null);
                                resolvedParameters.Add(resolvedParameter);
                            }
                            var resolvedPlugin = constructor.Invoke(resolvedParameters.ToArray());
                            Plugins.Add((ILaunchPlugin)resolvedPlugin);
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"Could not load plugin from: \"{pluginDirectory}\"\r\n{ex}");
                }                                
            }
        }

        //public Assembly LoadDependencies(string assemblyPath)
        //{
        //    var mainAssembly = Assembly.LoadFile(Path.GetFullPath(assemblyPath));
        //    loadedAssemblies.Add((mainAssembly.GetName(), mainAssembly));
            


        //    //var assemblyNames = mainAssembly.GetReferencedAssemblies();
            
        //    //foreach(var assemblyDependencyName in assemblyNames)
        //    //{
        //    //    if (loadedAssemblies.Any(x => x.Item1 == assemblyDependencyName)) continue;
                
        //    //    var loadedDependency = Assembly.LoadFile(assemblyDependencyName);
        //    //    loaded

        //    //    loadedAssemblies.Add(assemblyDependencyName);
        //    //    //loadedAssembly.nam
        //    //    //LoadDependencies(assembly);
        //    //}

        //    //var loadedAssembly = Assembly.Load(assemblyPath);
        //    //loadedAssemblies.Add(mainAssembly.GetName());
        //    //return loadedAssembly;
        //}
    }
}

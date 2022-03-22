using Heibroch.Common;
using Heibroch.Infrastructure.Interfaces.MessageBus;
using Heibroch.Launch.Events;
using Heibroch.Launch.Plugin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly IInternalMessageBus internalMessageBus;
        private readonly IContainer container;
        private List<Assembly> loadedAssemblies = new List<Assembly>();

        private List<string> exceptions = new List<string>()
        {
            "\\PresentationCore.dll",
            "\\System.Drawing.Common.dll",
            "\\System.IO.Packaging.dll",
            "\\System.Security.Permissions.dll",
            "\\System.Windows.Extensions.dll",
            "\\System.Windows.Input.Manipulations.dll",
            "\\System.Xaml.dll",
            "\\UIAutomationTypes.dll"
        };

        public PluginLoader(IInternalMessageBus internalMessageBus, IContainer container)
        {
            Plugins = new List<ILaunchPlugin>();
            this.internalMessageBus = internalMessageBus;
            this.container = container;
        }

        public List<ILaunchPlugin> Plugins { get; }

        public void Load()
        {
            var path = Environment.CurrentDirectory + "\\Plugins";
            internalMessageBus.Publish(new LogEntryPublished("Heibroch.Launch - Initializing", $"Listing plugin directories...\r\nPath: {path}", EventLogEntryType.Information));
            var pluginDirectories = Directory.GetDirectories(Environment.CurrentDirectory + "\\Plugins");
            var currentPluginDirectory = string.Empty;

            foreach (var pluginDirectory in pluginDirectories)
            {
                try
                {
                    currentPluginDirectory = pluginDirectory;

                    //var appDomain = AppDomain.CreateDomain(pluginDirectory, null, appDomainSetup);
                    var assemblyFiles = Directory.GetFiles(pluginDirectory, "*.dll");

                    var pluginDirectoryAssemblies = new List<Assembly>();
                    //Load assemblies
                    foreach (var assemblyFilePath in assemblyFiles)
                    {
                        if (exceptions.Any(x => assemblyFilePath.EndsWith(x)))
                            continue;

                        var assembly = Assembly.LoadFile(Path.GetFullPath(assemblyFilePath));//LoadDependencies(assemblyFilePath);
                        loadedAssemblies.Add(assembly);
                        pluginDirectoryAssemblies.Add(assembly);
                    }

                    //Load types
                    foreach (var assembly in pluginDirectoryAssemblies)
                    {
                        if (assembly.FullName.Contains("Microsoft")) continue;
                        if (assembly.FullName.StartsWith("System")) continue;

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
                                var method = container.GetType().GetMethod("Resolve");
                                var genericMethod = method.MakeGenericMethod(parameter.ParameterType);
                                var resolvedParameter = genericMethod.Invoke(container, null);
                                resolvedParameters.Add(resolvedParameter);
                            }
                            var resolvedPlugin = constructor.Invoke(resolvedParameters.ToArray());
                            Plugins.Add((ILaunchPlugin)resolvedPlugin);
                        }
                    }
                }
                catch (Exception ex)
                {
                    internalMessageBus.Publish(new LogEntryPublished("Heibroch.Launch - Initializing", $"Initializing plugin failed...\r\n \r\n {ex.StackTrace}", EventLogEntryType.Error));
                    MessageBox.Show($"Could not load plugin from: \"{pluginDirectory}\"\r\n{ex}");
                }
            }
        }
    }
}

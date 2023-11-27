﻿using System.Reflection;
using Autofac;
using LastfmDiscordRPC2.Models.API;
using LastfmDiscordRPC2.ViewModels;
using LastfmDiscordRPC2.ViewModels.Panes;

namespace LastfmDiscordRPC2;

public static class ContainerConfigurator
{
    public static IContainer Configure()
    {
        ContainerBuilder builder = new ContainerBuilder();

        builder.RegisterAssemblyTypes(Assembly.Load(nameof(LastfmDiscordRPC2)))
            .Where(t => t.Namespace.Contains("Panes"))
            .As(typeof(IPaneViewModel))
            .InstancePerLifetimeScope();

        builder.RegisterType<MainViewModel>().As<IWindowViewModel>();
        builder.RegisterType<LastfmAPIClient>().SingleInstance();
        builder.RegisterType<SignatureLocalClient>().As<ISignatureAPIClient>().SingleInstance();
            
        return builder.Build();
    }
}
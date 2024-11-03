using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyExceptions.Yaml.Serialization
{
    internal static class LazyComponentRegistrationListExtensions
    {
        public static TComponent BuildComponentChain<TComponent>(this LazyComponentRegistrationList<TComponent, TComponent> registrations, TComponent innerComponent)
        {
            var outerComponent = registrations.InReverseOrder.Aggregate(
                innerComponent,
                (inner, factory) => factory(inner)
            );

            return outerComponent;
        }

        public static TComponent BuildComponentChain<TArgument, TComponent>(this LazyComponentRegistrationList<TArgument, TComponent> registrations, TComponent innerComponent, Func<TComponent, TArgument> argumentBuilder)
        {
            var outerComponent = registrations.InReverseOrder.Aggregate(
                innerComponent,
                (inner, factory) => factory(argumentBuilder(inner))
            );

            return outerComponent;
        }

        public static List<TComponent> BuildComponentList<TComponent>(this LazyComponentRegistrationList<Nothing, TComponent> registrations)
        {
            return registrations
                .Select(factory => factory(default))
                .ToList();
        }

        public static List<TComponent> BuildComponentList<TArgument, TComponent>(this LazyComponentRegistrationList<TArgument, TComponent> registrations, TArgument argument)
        {
            return registrations
                .Select(factory => factory(argument))
                .ToList();
        }
    }
}

using System;
using System.Text;
using System.Reflection;
using Microsoft.Practices.Unity;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PlusUltra.MefResolver
{
    public static class ComponentLoader
    {
        public static void LoadContainer(IUnityContainer container, string path, string pattern)
        {
            var directoryCatalog = new DirectoryCatalog(path, pattern);
            var importDefinition = BuildImportDefinition();

            try
            {
                using(var aggregateCatalog = new AggregateCatalog())
                {
                    aggregateCatalog.Catalogs.Add(directoryCatalog);

                    using(var compositionContainer = new CompositionContainer(aggregateCatalog))
                    {
                        IEnumerable<Export> exports = compositionContainer.GetExports(importDefinition);
                        IEnumerable<IComponent> modules = exports.Select(exp => exp.Value as IComponent).Where(m => m != null);

                        var registerComponent = new RegisterComponent(container);

                        foreach(var module in modules)
                        {
                            module.Setup(registerComponent);
                        }
                    }
                }
            }
            catch(ReflectionTypeLoadException exc)
            {
                var builder = new StringBuilder();

                foreach(var exception in exc.LoaderExceptions)
                {
                    builder.AppendFormat("{0}\n", exception.Message);
                }

                throw new TypeLoadException(builder.ToString(), exc);
            }
        }

        private static ImportDefinition BuildImportDefinition()
        {
            return new ImportDefinition(def => true, typeof(IComponent).FullName, ImportCardinality.ZeroOrMore, false, false);
        }
    }

    internal class RegisterComponent : IRegisterComponent
    {
        private readonly IUnityContainer _container;

        public RegisterComponent(IUnityContainer container)
        {
            _container = container;
        }

        public void RegisterType<TFrom, TTo>(bool withInterception = false) where TTo : TFrom
        {
            if (withInterception)
            {
                // register with interception
            }
            else
            {
                _container.RegisterType<TFrom, TTo>();
            }
        }

        public void RegisterTypeWithControlledLifeTime<TFrom, TTo>(bool withInterception = false) where TTo : TFrom
        {
            _container.RegisterType<TFrom, TTo>(new ContainerControlledLifetimeManager());
        }
    }
}

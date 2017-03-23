# PlusUltra.MefResolver
Project to include in solution for resolving objects within assemblies, based on MEF.

##Usage

For the assembly which you want to be resolved automatically by the MefResolver, create a class in the project which implements the ```IComponent``` interface and put the ```Export``` attribute on it. Consequently, register the types to be resolved in the ```Setup``` method

###Example
```C#
[Export(typeof(IComponent))]
class DependencyResolver : IComponent
{
    public void Setup(IRegisterComponent registerComponent)
    {
        registerComponent.RegisterType<IUnitOfWork, UnitOfWork>();
    }
}
```

Secondly, in the unity config class of your startup project, load the container.

###Example

```C#
ComponentLoader.LoadContainer(container, ".\\bin", "DataService.BusinessServices.dll");
ComponentLoader.LoadContainer(container, ".\\bin", "DataService.Data.dll");
```

```LoadContainer``` also accepts wildcards, so the above example can be replaced by:

```C#
ComponentLoader.LoadContainer(container, ".\\bin", "DataService.*.dll");
```

So that any other assemblies which are being dropped in the specified folder and which export the IComponent will be automatically resolved without having to recompile the application.


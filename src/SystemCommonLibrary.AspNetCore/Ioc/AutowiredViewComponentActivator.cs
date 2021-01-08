using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace SystemCommonLibrary.AspNetCore.IoC
{
    public class AutowiredViewComponentActivator : ServiceBasedViewComponentActivator, IViewComponentActivator
    {

        public AutowiredViewComponentActivator() : base() { }

        public new object Create(ViewComponentContext context)
        {
            //default create controller function
            var controllerInstance = base.Create(context);
            DependencyInjection.Resolve(context.ViewContext?.HttpContext?.RequestServices, controllerInstance);
            return controllerInstance;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace SystemCommonLibrary.AspNetCore.IoC
{
    public class AutowiredControllerActivator : ServiceBasedControllerActivator, IControllerActivator
    {

        public AutowiredControllerActivator() : base() { }

        public new object Create(ControllerContext actionContext)
        {
            //default create controller function
            var controllerInstance = base.Create(actionContext);
            DependencyInjection.Resolve(actionContext.HttpContext.RequestServices, controllerInstance);
            return controllerInstance;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;

namespace SystemCommonLibrary.AspNetCore.IoC
{
    public class AutowiredControllerActivator : ServiceBasedControllerActivator, IControllerActivator
    {

        public AutowiredControllerActivator() : base() { }

        public new object Create(ControllerContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(nameof(actionContext));
            }

            var controllerType = actionContext.ActionDescriptor.ControllerTypeInfo.AsType();
            var controllerInstance = DependencyInjection.GetRequiredService(controllerType, actionContext.HttpContext.RequestServices);

            DependencyInjection.Resolve(actionContext.HttpContext.RequestServices, controllerInstance);
            return controllerInstance;
        }
    }
}

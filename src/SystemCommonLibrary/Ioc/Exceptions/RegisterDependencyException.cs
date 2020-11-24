using System;

namespace SystemCommonLibrary.IoC.Exceptions
{
    public class RegisterDependencyException : SystemException
    {
        public RegisterDependencyException()
        {

        }

        public RegisterDependencyException(string message) : base(message) { }

    }
}

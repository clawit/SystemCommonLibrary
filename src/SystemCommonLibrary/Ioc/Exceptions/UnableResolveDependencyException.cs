using System;

namespace SystemCommonLibrary.IoC.Exceptions
{
    public class UnableResolveDependencyException : SystemException
    {
        public UnableResolveDependencyException()
        {

        }

        public UnableResolveDependencyException(string message) : base(message) { }

    }
}

using System;
using System.Diagnostics.CodeAnalysis;

namespace Molder.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class DirectoryException : Exception
    {
        public DirectoryException(string message) : base(message) { }
    }
}
using System;

namespace Molder.Web.Models.Mediator
{
    public interface IMediator
    {
        void Execute(Action action);
        object Execute<TResult>(Func<TResult> action);
        object Wait<TResult>(Func<TResult> action);
    }
}

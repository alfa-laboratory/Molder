using System;

namespace EvidentInstruction.Web.Models.PageObject.Models.Mediator.Interfaces
{
    public interface IMediator
    {
        void Execute(Action action);
        object Execute<TResult>(Func<TResult> action);
        object Wait<TResult>(Func<TResult> action);
    }
}

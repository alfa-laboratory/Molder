using System;

namespace EvidentInstruction.Web.Models.PageObject.Models.Interfaces
{
    public interface IMediator
    {
        void Execute(object sender, Action action);
        object Execute<TResult>(object sender, Func<TResult> action);
        object Wait<TResult>(object sender, Func<TResult> action);
    }
}

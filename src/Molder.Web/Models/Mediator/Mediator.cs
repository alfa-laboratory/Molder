using Polly.Retry;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Molder.Web.Models.Mediator
{
    [ExcludeFromCodeCoverage]
    public abstract class Mediator : IMediator
    {
        protected RetryPolicy retryPolicy = null;

        protected RetryPolicy waitAndRetryPolicy = null;

        public virtual void Execute(Action action)
        {
            retryPolicy.Execute(action);
        }

        public virtual object Execute<TResult>(Func<TResult> action)
        {
            return retryPolicy.Execute(action);
        }

        public virtual object Wait<TResult>(Func<TResult> action)
        {
            return waitAndRetryPolicy.Execute(action);
        }
    }
}
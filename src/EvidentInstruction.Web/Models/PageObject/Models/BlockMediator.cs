using EvidentInstruction.Web.Models.PageObject.Models.Interfaces;
using System;

namespace EvidentInstruction.Web.Models.PageObject.Models
{
    public class BlockMediator : IMediator
    {
        public void Execute(object sender, Action action)
        {
            throw new NotImplementedException();
        }

        public object Execute<TResult>(object sender, Func<TResult> action)
        {
            throw new NotImplementedException();
        }

        public object Wait<TResult>(object sender, Func<TResult> action)
        {
            throw new NotImplementedException();
        }
    }
}

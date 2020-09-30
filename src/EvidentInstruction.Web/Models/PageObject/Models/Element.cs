using EvidentInstruction.Web.Models.PageObject.Models.Interfaces;
using EvidentInstruction.Web.Models.Providers;
using System;

namespace EvidentInstruction.Web.Models.PageObject.Models
{
    public class Element : IElement
    {
        protected IMediator _mediator;

        protected string _xpath;
        protected string _name;

        [ThreadStatic]
        public ElementProvider _provider;

        public Element(ElementProvider provider) => _provider = provider;

        public string Name => _name;

        public string Text =>  _provider.Text;

        public object Value => _mediator.Execute(this, () => _provider.GetAttribute("value"));

        public bool Loaded => throw new NotImplementedException();

        public bool Enabled => Convert.ToBoolean(_mediator.Execute(this, () => _provider.Enabled));

        public bool Displayed => Convert.ToBoolean(_mediator.Execute(this, () => _provider.Displayed));

        public bool Selected => Convert.ToBoolean(_mediator.Execute(this, () => _provider.Selected));

        public bool Editabled => Convert.ToBoolean(_mediator.Execute(this, () => _provider.GetAttribute("readonly")));

        public void SetMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public string GetAttribute(string name)
        {
            return _mediator.Execute(this, () => _provider.GetAttribute(name)).ToString();
        }
    }
}

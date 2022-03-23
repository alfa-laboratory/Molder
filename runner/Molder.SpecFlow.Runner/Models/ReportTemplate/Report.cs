using System;
using System.Collections.Generic;

namespace Molder.SpecFlow.Runner.Models.ReportTemplate
{
    public class Report : IDisposable
    {
        private IEnumerable<Feature> _reportTemplates;
        private readonly Lazy<Report> _reportLazy;
        private bool _isDisposed;
        
        public Report Current => _reportLazy.Value;
        
        public Report()
        {
            _reportLazy = new Lazy<Report>(CreateReport);
        }
        
        private Report CreateReport()
        {
            return new Report()
            {
                _reportTemplates = new List<Feature>()
            };
        }

        public IEnumerable<Feature> ReportTemplates() => _reportTemplates;

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            if (_reportLazy.IsValueCreated)
            {
                foreach (var obj in Current._reportTemplates)
                {
                    if (obj is IDisposable disp) { disp.Dispose(); }
                }
            }

            _isDisposed = true;;
        }
    }
}
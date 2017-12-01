using GridForms.BindingsExample.Abstract;
using GridForms.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GridForms.BindingsExample
{
    class BindingsExampleViewModel : BindableObject
    {
        public BindingsExampleViewModel()
        {
            AsyncHelper.Run(() => Thread.Sleep(5000), b => UiIsVisible = !b);
        }

        private bool _uiIsVisible;

        public virtual bool UiIsVisible
        {
            get => _uiIsVisible;
            set => SetProperty(ref _uiIsVisible, value);
        }
    }
}

using GridForms.WPF.Examples.BindingsExample.Abstract;
using GridForms.WPF.Examples.Helpers;
using System.Threading;

namespace GridForms.WPF.Examples.BindingsExample
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

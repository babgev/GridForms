using System;
using System.Threading.Tasks;

namespace GridForms.Helpers
{
    public static class AsyncHelper
    {
        public delegate void UiUpdate(bool isBusy, bool requestClose = false);

        public static async void Run(Action serviceCall, Action<bool> uiUpdate = null)
        {
            uiUpdate?.Invoke(true);
            await Task.Run(serviceCall);
            uiUpdate?.Invoke(false);
        }

        public static async void Run(Action serviceCall, UiUpdate uiUpdate = null, bool requestClose = false)
        {
            uiUpdate?.Invoke(true);
            await Task.Run(serviceCall);
            uiUpdate?.Invoke(false, requestClose);
        }

        public static async void RunWithResult<T>(Func<T> serviceCall, Action<T> uiUpdate = null, Action<bool> loadingUpdate = null)
        {
            loadingUpdate?.Invoke(true);

            await Task.Run(serviceCall).ContinueWith((r) =>
            {
                if (r.Exception != null) return;
                if (r.Result != null)
                {
                    uiUpdate?.Invoke(r.Result);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());

            loadingUpdate?.Invoke(false);
        }
    }
}


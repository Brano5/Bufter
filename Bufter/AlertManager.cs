using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.JSInterop;

namespace Bufter
{
	public class AlertManager
	{
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public AlertManager(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "./_content/TcOpen.Inxton.Local.Security.Blazor/BlazorAlert.js").AsTask());
        }

        public async void addAlert(string type, string message)
        {
            var module = await moduleTask.Value;

            await module.InvokeAsync<string>("alert", new string[] { message, type });
        }
    }
}

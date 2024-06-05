using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using ServerAppSchule.Interfaces;
using ServerAppSchule.Models;
using System.Security.Claims;

namespace ServerAppSchule.Components
{
    partial class SettingsDesign
    {
        [CascadingParameter]
        Task<AuthenticationState> _authenticationState { get; set; }
        [Parameter]
        public UserSettings UsrSettings { get; set; }
        [Inject]
        NavigationManager _navManager { get; set; }
        [Inject]
        ISettingsService _settingsService { get; set; }
        HubConnection _hubConnection;
        string _uid;
        bool darkmode;
        protected override async Task OnInitializedAsync()
        {
            var currentauth = await _authenticationState;
            _uid = currentauth.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            _hubConnection = new HubConnectionBuilder()
             .WithUrl(_navManager.ToAbsoluteUri("/serverappschulehub"))
             .WithAutomaticReconnect()
             .Build();
            darkmode = _settingsService.GetTheme(_uid);
            await _hubConnection.StartAsync();
            await base.OnInitializedAsync();
        }
        async Task OnThemeChanged(bool theme)
        {
            string uid = UsrSettings.UserId ?? _uid;
            await _hubConnection.InvokeAsync("ThemeChanged", theme, uid);
        }
    }
}

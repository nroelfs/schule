using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ServerAppSchule.Models;
using ServerAppSchule.Interfaces;
using System.Security.Claims;

namespace ServerAppSchule.Pages
{
    partial class Settings
    {
        [CascadingParameter]
        Task<AuthenticationState> _authenticationState { get; set; }
        [Inject]
        NavigationManager _navManager { get; set; }
        [Inject]
        ISettingsService _settingsService { get; set; }
        string _uid { get; set; }
        UserSettings? _usrSettings { get; set; }

        protected override async Task OnInitializedAsync()
        {
            AuthenticationState? currentauth = await _authenticationState;
            if (!currentauth.User.Identity.IsAuthenticated)
            {
                _navManager.NavigateTo("/login", true);
            }
            _uid = currentauth.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            _usrSettings = _settingsService.GetSettings(_uid);
            if(_usrSettings == null)
            {
                _usrSettings = new UserSettings();
            }

        }

    }
}

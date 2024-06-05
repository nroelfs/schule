using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ServerAppSchule.Models;
using ServerAppSchule.Interfaces;

namespace ServerAppSchule.Components
{
    public partial class ProfileSlim
    {
        [CascadingParameter]
        Task<AuthenticationState> _authenticationState { get; set; }
        UserSlim _user { get; set; }
        string? _profilePic { get; set; }
        string _shortName { get; set; }
        string _uid { get; set; }
        [Inject]
        ISettingsService _settingsService { get; set; }
        
        protected override async Task OnInitializedAsync()
        {
            AuthenticationState? currentAuth = await _authenticationState;
            _uid = currentAuth.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (_uid == null)
                _profilePic = "";
            else
                GetProfilePic();
            _shortName = currentAuth.User.Identity.Name.Substring(0, 1);
        }
        
        /// <summary>
        /// ruft das Profilbild aus der Datenbank ab
        /// </summary>
        void GetProfilePic()
        {
             _profilePic = _settingsService.GetPicture(_uid);
        }
    }
}

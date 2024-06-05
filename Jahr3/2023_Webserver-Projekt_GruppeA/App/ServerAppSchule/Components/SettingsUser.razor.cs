using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using ServerAppSchule.Interfaces;
using ServerAppSchule.Models;

namespace ServerAppSchule.Components
{
    partial class SettingsUser : ComponentBase
    {
        [Parameter]
        public string uid { get; set; }
        [Parameter]
        public UserSettings? usrSettings { get; set; } = new UserSettings();
        RegisterUser? _usr { get; set; }
        [Inject]
        private IUserService _userService { get; set; }
        [Inject]
        private ISettingsService _settingsService { get; set; }
        [Inject]
        private IDialogService _dialogService { get; set; }
        private string _profilepic { get; set;}
        protected override async Task OnInitializedAsync()
        {
            _usr = _userService.GetUserById(uid);
            if(usrSettings == null)
            {
                usrSettings = _settingsService.GetSettings(uid) ?? new();
            }
            if(usrSettings.ProfilePicture != null)
            {
                _profilepic = _settingsService.GetPicture(uid);
            }
        }
        /// <summary>
        /// Ändert das Profilbild
        /// </summary>
        /// <param name="file">Profilbild das hochgeladen wird</param>
        /// <returns></returns>
        async Task UploadProfilePicture(IBrowserFile file)
        {
             await _settingsService.UpdateProfilePictureAsync(file, _usr.Id);
        }
        /// <summary>
        /// Öffnet das Dialogfenster zum Ändern des Passworts
        /// </summary>
        void OpenDialog()
        {
            DialogParameters<ChangePasswordDialog> parameters = new DialogParameters<ChangePasswordDialog>();
            parameters.Add(x => x.uid, uid);
            _dialogService.Show<ChangePasswordDialog>("Passwort Ändern",parameters);
           
        }

    }
}

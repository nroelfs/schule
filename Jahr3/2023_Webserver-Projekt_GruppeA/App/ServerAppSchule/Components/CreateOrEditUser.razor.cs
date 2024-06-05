using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using ServerAppSchule.Interfaces;
using ServerAppSchule.Models;
using System.Text.RegularExpressions;

namespace ServerAppSchule.Components
{
    public partial class CreateOrEditUser : ComponentBase
    {
        [Parameter]
        public RegisterUser? InputUser { get; set; }

        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; }

        [Parameter]
        public List<string> _roles { get; set; }
        [Inject]
        private IRoleService _roleService { get; set; }
        [Inject]
        private IUserService _userService { get; set; }
        [Inject]
        private IJSRuntime _jsRuntime { get; set; }

        protected override void OnInitialized()
        {
            if (InputUser == null)
                InputUser = new RegisterUser();
                InputUser.Role = "User";
            _roles = _roleService.GetRoleNames();
            base.OnInitialized();
        }

        /// <summary>
        ///  Überprüft ob das Passwort den Anforderungen entspricht
        /// </summary>
        /// <param name="pw">Password</param>
        /// <returns>Anforderung die erfüllt werden muss</returns>
        private IEnumerable<string> PasswordStrength(string pw)
        {
            if (string.IsNullOrWhiteSpace(pw))
            {
                yield return "Password is required!";
                yield break;
            }
            if (pw.Length < 8)
                yield return "Password must be at least of length 8";
            if (!Regex.IsMatch(pw, @"[A-Z]"))
                yield return "Password must contain at least one capital letter";
            if (!Regex.IsMatch(pw, @"[a-z]"))
                yield return "Password must contain at least one lowercase letter";
            if (!Regex.IsMatch(pw, @"[0-9]"))
                yield return "Password must contain at least one digit";
            if (!Regex.IsMatch(pw, @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]"))
                yield return "Password must contain at least one special character";
        }

        /// <summary>
        /// Überprüft ob das Passwort und die Passwortwiederholung übereinstimmen
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private string PasswordMatch(string arg)
        {
            if (InputUser == null)
                return "";
            if (InputUser.Password != arg)
                return "Passwörter stimmen nicht über ein";
            return "";
        }

        /// <summary>
        /// Bricht den Dialog ab
        /// </summary>
        private void Cancel() => MudDialog.Cancel();

        /// <summary>
        /// erstellt einen neuen Benutzer
        /// </summary>
        /// <returns></returns>
        private async Task SubmitAsync()
        {
            if (!InputUser.IsValid())
            {
                await _jsRuntime.InvokeVoidAsync("alert", "Benutzer kann nicht erstellt werden! Ungültige Angaben1");
            }
            Task create = _userService.CreateNewUserAsync(InputUser);
            await create;
            if (create.IsCompletedSuccessfully)
            {
                MudDialog.Close(DialogResult.Ok(true));
            }
            else
            {
                await _jsRuntime.InvokeVoidAsync("alert", "Benutzer kann nicht erstellt werden! Ungültige Angaben2");
            }

        }
    }
}

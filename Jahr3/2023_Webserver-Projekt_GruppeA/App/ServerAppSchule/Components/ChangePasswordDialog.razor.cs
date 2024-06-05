using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using ServerAppSchule.Interfaces;
using ServerAppSchule.Models;
using System.Text.RegularExpressions;

namespace ServerAppSchule.Components
{
    partial class ChangePasswordDialog : ComponentBase
    {
        [Parameter]
        public string uid { get; set;}
        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; }

        [Inject]
        private IUserService _userService { get; set; }
        [Inject]
        private IJSRuntime _jsRuntime { get; set; }

        RegisterUser InputUser = new RegisterUser();
        string PasswordRepeat = string.Empty;
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
        /// Ändert das Passwort des Benutzers
        /// </summary>
        /// <returns></returns>
        private async Task SubmitAsync()
        {
            Task change = _userService.ChangePassword(uid, InputUser.Password);
            await change;
            if (change.IsCompletedSuccessfully)
            {
                MudDialog.Close(DialogResult.Ok(true));
            }
            else
            {
                await _jsRuntime.InvokeVoidAsync("alert", "Benutzer kann nicht erstellt werden! Ungültige Angaben2");
            }

        }
        /// <summary>
        /// Überprüft ob der Speichern Button aktiviert werden kann
        /// </summary>
        /// <returns> true: ist deaktiviert | false: ist aktiv </returns>
        private bool disableSave()
        {
            return (String.IsNullOrEmpty(InputUser.Password)
                || String.IsNullOrEmpty(PasswordRepeat)
                || InputUser.Password != PasswordRepeat);
        }
        
    }
}

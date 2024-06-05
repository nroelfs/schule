using Microsoft.AspNetCore.Components.Forms;
using ServerAppSchule.Models;

namespace ServerAppSchule.Interfaces
{
    public interface ISettingsService
    {
        /// <summary>
        /// Fetched Die Settings eines Users
        /// </summary>
        /// <param name="uid">User Id </param>
        /// <returns>einstellungen eines Users</returns>
        UserSettings? GetSettings(string uid);
        /// <summary>
        /// Updated das Profilbild eines Users
        /// </summary>
        /// <param name="profilePic">Profilbild as IBrowserfile</param>
        /// <param name="uid">User Id</param>
        /// <returns>true</returns>
        Task<bool> UpdateProfilePictureAsync(IBrowserFile profilePic, string uid);
        /// <summary>
        /// Ruft das ProfilBild eines Users ab 
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetPicture(string uid, string type = "png");
        /// <summary>
        /// Updated das Theme eines Users
        /// </summary>
        /// <param name="uid">Id des Users</param>
        /// <param name="theme">true: Darkmode | false: lightmode </param>
        /// <returns></returns>
        Task UpdateTheme(string uid, bool theme);
        /// <summary>
        /// Ruft das Theme eines Users ab
        /// </summary>
        /// <param name="uid">User Id</param>
        /// <returns></returns>
        bool GetTheme(string uid);
    }
}

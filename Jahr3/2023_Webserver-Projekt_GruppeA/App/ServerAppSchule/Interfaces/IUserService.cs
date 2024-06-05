using ServerAppSchule.Models;

namespace ServerAppSchule.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// erstellt einen User
        /// </summary>
        /// <param name="user">Daten aus dem Formular</param>
        /// <returns>Task Status ob erstellen geklappt hat</returns>
        Task<Task> CreateNewUserAsync(RegisterUser user);
        /// <summary>
        /// Gibt Alle User im gemappten Zustand wieder
        /// </summary>
        /// <returns>Liste an Usern</returns>
        Task<IEnumerable<UserSlim>> GetAllMappedUsersAsync();
        /// <summary>
        /// Sucht den User anhand der ID und gibt ihn im gemappten Zustand wieder
        /// </summary>
        /// <param name="id">Id des Users der Herausgesucht werden soll</param>
        /// <returns>User</returns>
        RegisterUser GetUserById(string id);
        /// <summary>
        /// Updated den letzten Login Refresh
        /// </summary>
        /// <param name="uid">User Id</param>
        /// <returns></returns>
        Task UpdateLastLoginRefresh(string uid);
        /// <summary>
        /// Updated das Passwort eines Users
        /// </summary>
        /// <param name="uid">User Id</param>
        /// <param name="pwd">Neues  Passwort</param>
        /// <returns></returns>
        Task<Task> ChangePassword(string uid, string pwd);
        /// <summary>
        /// Holt den Usernamen anhand der Id
        /// </summary>
        /// <param name="uid">Id des Users der gesucht werden soll</param>
        /// <returns>Benutzernamen des Users</returns>
        string GetUsernameById(string uid);
        /// <summary>
        /// Überprüft ob sich der Benutzer das erste mal anmeldet
        /// </summary>
        /// <param name="uid">user id</param>
        /// <returns>True => User hat sich bereits ein mal angemeldet | false User hat sich zum ersten mal angemeldet</returns>
       Task<bool> IsFirstSignInDone(string uid);
    }
}

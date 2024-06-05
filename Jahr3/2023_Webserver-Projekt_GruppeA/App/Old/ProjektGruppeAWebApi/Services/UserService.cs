using ProjektGruppeAWebApi.Models;

namespace ProjektGruppeAWebApi.Services
{
    public class UserService
    {
        #region Public Method
        public UserSlim ToSlimModel(User user)
        {
            return new UserSlim 
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UID = user.Id,
                Role = user.Role,
                UserName = user.UserName,
            };
        }
        #endregion
    }
}

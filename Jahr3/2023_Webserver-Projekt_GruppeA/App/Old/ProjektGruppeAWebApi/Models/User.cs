using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ProjektGruppeAWebApi.Models
{
    public class User : IdentityUser
    {
        #region public fields
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        [NotMapped]
        public string RefreshToken { get; set; }
        [NotMapped]
        public DateTime RefreshTokenExpiryTime { get; set; }
        #endregion
    }
}

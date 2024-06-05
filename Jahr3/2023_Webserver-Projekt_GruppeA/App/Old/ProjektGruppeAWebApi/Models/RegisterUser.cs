using System.ComponentModel.DataAnnotations;

namespace ProjektGruppeAWebApi.Models
{
    public class RegisterUser
    {
        #region public fields
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string RoleName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        #endregion
    }
}

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ProjektGruppeAWebApi.Models
{
    [Obsolete("IdentityRole wird stattdessen genutzt")]
    public class AppRole : IdentityRole
    {
        #region public fields
        public string description { get; set; }
        #endregion
    }
}

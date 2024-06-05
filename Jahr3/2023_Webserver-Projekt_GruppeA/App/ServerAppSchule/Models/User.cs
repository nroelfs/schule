using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerAppSchule.Models
{

    public class User : IdentityUser
    {
        public string LastHomeRefresh { get; set; }
        public bool HasFirstLoginDone { get; set; }
    }
}

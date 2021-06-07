using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Snackis.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the SnackisUser class
    public class SnackisUser : IdentityUser
    {
        ///     Lägg till i register
        [PersonalData]//Data tas bort om användaren raderar sitt konto
        public int BirthYear { get; set; }
        [PersonalData]
        public string NickName { get; set; }
        [PersonalData]
        public Byte[] Image { get; set; }
    }
}

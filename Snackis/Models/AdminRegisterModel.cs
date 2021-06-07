using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Models
{
    public class AdminRegisterModel
    {
        //[Required]
        //[MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        //[Compare(nameof(Password))]
        public string Confirm { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}

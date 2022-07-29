using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWalletAPITask.Authentication
{
    public class RegisterModel
    {
        //These are the properties to be used to create an account - Username, password, etc
        public string FirstName { get; set; }
        public string LastName { get; set; }


        [Required(ErrorMessage = "USERNAME IS REQUIRED")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "EMAIL IS REQUIRED")]
        public string Email { get; set; }


        [Required(ErrorMessage = "PASSWORD IS REQUIRED")]
        public string Password { get; set; }



    }
}

using System.ComponentModel.DataAnnotations;

namespace GameStore.WEB.Models.IdentityModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Account.AccountResource), ErrorMessageResourceName = "EnterLogin")]
        [Display(Name = "Login", ResourceType = typeof(Resources.Account.AccountResource))]
        public string Login { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Account.AccountResource), ErrorMessageResourceName = "EnterPassword")]
        [Display(Name = "Password", ResourceType = typeof(Resources.Account.AccountResource))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsPersistent { get; set; }
    }
}
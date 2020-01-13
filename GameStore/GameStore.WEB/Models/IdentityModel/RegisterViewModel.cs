using System.ComponentModel.DataAnnotations;

namespace GameStore.WEB.Models.IdentityModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Account.AccountResource), ErrorMessageResourceName = "LoginRequired")]
        [Display(Name = "Login", ResourceType = typeof(Resources.Account.AccountResource))]
        [MinLength(5, ErrorMessageResourceType = typeof(Resources.Account.AccountResource), ErrorMessageResourceName = "LoginMinError")]
        public string Login { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Account.AccountResource), ErrorMessageResourceName = "PasswordRequired")]
        [Display(Name = "Password", ResourceType = typeof(Resources.Account.AccountResource))]
        [MinLength(5, ErrorMessageResourceType = typeof(Resources.Account.AccountResource), ErrorMessageResourceName = "PasswordMinError")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "IsPublisher", ResourceType = typeof(Resources.Account.AccountResource))]
        public bool IsPublisher { get; set; }

        [Display(Name = "CompanyName", ResourceType = typeof(Resources.Account.AccountResource))]
        public string CompanyName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Account.AccountResource), ErrorMessageResourceName = "ConfirmPasswordRequired")]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resources.Account.AccountResource))]
        [Compare("Password", ErrorMessageResourceType = typeof(Resources.Account.AccountResource), ErrorMessageResourceName = "CompareError")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Account.AccountResource), ErrorMessageResourceName = "NameRequired")]
        [MinLength(3, ErrorMessageResourceType = typeof(Resources.Account.AccountResource), ErrorMessageResourceName = "NameMinError")]
        [Display(Name = "Name", ResourceType = typeof(Resources.Account.AccountResource))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Account.AccountResource), ErrorMessageResourceName = "AddressRequired")]
        [Display(Name = "Address", ResourceType = typeof(Resources.Account.AccountResource))]
        public string Address { get; set; }
    }
}
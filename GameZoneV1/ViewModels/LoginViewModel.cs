namespace GameZoneV1.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Display(Name = "Remember Me")]
        public bool rememberMe { get; set; }


    }
}

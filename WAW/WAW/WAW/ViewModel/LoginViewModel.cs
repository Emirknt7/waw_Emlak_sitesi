using System.ComponentModel.DataAnnotations;

namespace WAW.ViewModel
{

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı gerekli.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Şifre gerekli.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
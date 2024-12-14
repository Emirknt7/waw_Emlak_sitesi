using System.ComponentModel.DataAnnotations;

namespace WAW.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı gerekli.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Ad gerekli.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad gerekli.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Şifre gerekli.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "E-posta adresi gerekli.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hesap türü gerekli.")]
        public string AccountType { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile AvatarFile { get; set; } 
    }
}

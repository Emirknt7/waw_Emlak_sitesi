using System.ComponentModel.DataAnnotations;

namespace WAW.ViewModel
{
    public class EditProfileViewModel
    {
        public string Username { get; set; } // عرض فقط، لا يتم التعديل عليه

        [Required(ErrorMessage = "Ad gerekli.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad gerekli.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "E-posta gerekli.")]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi.")]
        public string Email { get; set; }
    }
}

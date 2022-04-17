using System.ComponentModel.DataAnnotations;

namespace Blog_rev02.ViewModels {
    public class RegisterViewModel {

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email é inválido.")]
        public string Email { get; set; }
    }
}

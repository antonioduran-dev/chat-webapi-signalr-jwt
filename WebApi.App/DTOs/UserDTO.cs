using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApi.App.DTOs
{
    public class UserDTO
    {
        [Required]
        [RegularExpression(@"^.{3,12}$", ErrorMessage = "Usuario debe contener entre 3 y 12 caracteres, y ser único.")]
        public string UserName { get; set; }
        [Required]
        // [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$", ErrorMessage = "La contraseña debe contener al menos 6 dígitos, incluidos 1 numero, 1 mayuscula y 1 caracter especial.")]
        public string Password { get; set; }
    }
}

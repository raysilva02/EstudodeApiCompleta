using System.ComponentModel.DataAnnotations;

namespace EstudoApiCompleta.Model
{
    public class Cliente
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage ="O campo {0} é obrigatório")]
        [StringLength (100, ErrorMessage = "O campo {0} deve possuir entre {1} e {2} caracteres",  MinimumLength = 6)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int TipoCliente { get; set; }

        public bool Ativo { get; set; }

        public Guid UsuarioId { get; set; }

    }
}

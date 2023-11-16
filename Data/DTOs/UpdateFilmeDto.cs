
using System.ComponentModel.DataAnnotations;

namespace FilmesApi.Data.Dtos;

public class UpdateFilmeDto
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required(ErrorMessage = "O título do filme é obrigatório")]
    public string Titulo { get; set; }
    [Required(ErrorMessage = "O gênero do filme é obrigatório")]
    [StringLength(50, ErrorMessage = "O tamanho do gênero não pode ser maior que 50 caracteres")]
    public string Genero { get; set; }
    [Required(ErrorMessage = "A duração do filme é obrigatório")]
    [Range(70, 600, ErrorMessage = "A duração do filme deve ter entre 70 a 600 minutos")]
    public int Duracao { get; set; }
}

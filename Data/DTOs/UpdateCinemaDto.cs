
using System.ComponentModel.DataAnnotations;

namespace FilmesApi.Data.DTOs;

public class UpdateCinemaDto
{
    [Required(ErrorMessage = "O campo de nome é obrigatorio")]
    public string Nome { get; set; }
    public ReadEnderecoDto ReadEnderecoDto { get; set; }
}
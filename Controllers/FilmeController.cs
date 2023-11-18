
using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.Dtos;
using FilmesApi.Data.DTOs;
using FilmesApi.Model;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{

    //private static List<Filme> filmes = new List<Filme>();
    //private static int id = 0;

    private FilmeContext _context;
    private IMapper _mapper;

    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    /// <summary>
    /// Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="filmeDto">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionaFilme(
    [FromBody] CreateFilmeDto filmeDto)
    {
        Filme filme = _mapper.Map<Filme>(filmeDto);
        _context.Filmes.Add(filme);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperaFilmePorId),
        new { id = filme.Id },
        filme);
    }

    /// <summary>
    /// Recupera todos os filmes cadastrados no banco de dados
    /// </summary>
    /// <param name="skip">Número de registros a serem ignorados</param>
    /// <param name="take">Número máximo de registros a serem retornados</param>
    /// <returns>IEnumerable de ReadFilmeDto</returns>
    [HttpGet]
    public IEnumerable<ReadFilmeDto> RecuperaFilmes([FromQuery] int skip = 0,
    [FromQuery] int take = 50,
    [FromQuery] string? nomeCinema = null)
    {
        if (nomeCinema == null)
        {
            return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take).ToList());
        }
        return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).
            Take(take).Where(filme => filme.Sessoes.
            Any(sessao => sessao.Cinema.Nome == nomeCinema)).ToList());

    }

    /// <summary>
    /// Recupera um filme pelo id
    /// </summary>
    /// <param name="id">Identificador único do filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso o filme seja encontrado</response>
    /// <response code="404">Caso o filme não seja encontrado</response>
    [HttpGet("{id}")]
    public IActionResult RecuperaFilmePorId(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();
        var filmeDto = _mapper.Map<ReadFilmeDto>(filme);
        return Ok(filmeDto);
    }

    /// <summary>
    /// Atualiza as informações de um filme
    /// </summary>
    /// <param name="id">Identificador único do filme</param>
    /// <param name="filmeDto">Objeto com os campos a serem atualizados</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso a atualização seja bem-sucedida</response>
    /// <response code="404">Caso o filme não seja encontrado</response>
    [HttpPut("{id}")]
    public IActionResult AtualizaFilme(int id,
    [FromBody] UpdateFilmeDto filmeDto)
    {
        var filme = _context.Filmes.FirstOrDefault(
            filme => filme.Id == id);
        if (filme == null) return NotFound();
        _mapper.Map(filmeDto, filme);
        _context.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Atualiza parcialmente as informações de um filme
    /// </summary>
    /// <param name="id">Identificador único do filme</param>
    /// <param name="patch">Objeto JsonPatchDocument contendo as atualizações parciais</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso a atualização parcial seja bem-sucedida</response>
    /// <response code="400">Caso haja um erro de validação</response>
    /// <response code="404">Caso o filme não seja encontrado</response>
    [HttpPatch]
    public IActionResult AtualizaFilmeParcial(int id, JsonPatchDocument<UpdateFilmeDto> patch)
    {
        var filme = _context.Filmes.FirstOrDefault(
            filme => filme.Id == id);
        if (filme == null) return NotFound();

        var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);
        patch.ApplyTo(filmeParaAtualizar, ModelState);

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(filmeParaAtualizar, filme);
        _context.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Deleta um filme pelo id
    /// </summary>
    /// <param name="id">Identificador único do filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso a exclusão seja bem-sucedida</response>
    /// <response code="404">Caso o filme não seja encontrado</response>
    [HttpDelete("{id}")]
    public IActionResult DeletaFilme(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(
            filme => filme.Id == id);
        if (filme == null) return NotFound();
        _context.Remove(filme);
        _context.SaveChanges();
        return NoContent();

    }
}
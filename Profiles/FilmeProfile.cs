
using AutoMapper;
using FilmesApi.Data.Dtos;
using FilmesApi.Data.DTOs;
using FilmesApi.Model;

namespace FilmesApi.Profiles;

public class FilmeProfile : Profile
{
    public FilmeProfile() 
    {
        CreateMap<CreateFilmeDto, Filme>();
        CreateMap<UpdateFilmeDto, Filme>();
        CreateMap<Filme, UpdateFilmeDto>();
        CreateMap<Filme, ReadFilmeDto>();
    }
}
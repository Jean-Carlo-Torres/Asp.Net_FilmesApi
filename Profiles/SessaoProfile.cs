using AutoMapper;
using FilmesApi.Data.DTOs;
using FilmesApi.Model;

namespace FilmesApi.Profiles;

public class SessaoProfile : Profile
{
    public SessaoProfile(){
        CreateMap<CreateSessaoDto, Sessao>();
        CreateMap<Sessao, ReadSessaoDto>();
    }
}
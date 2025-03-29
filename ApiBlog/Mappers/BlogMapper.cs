using ApiBlog.Models;
using ApiBlog.Models.DTO;
using AutoMapper;

namespace ApiBlog.Mappers
{
    public class BlogMapper : Profile
    {
        public BlogMapper()
        {
            CreateMap<Post, PostDTO>().ReverseMap();
            CreateMap<Post, PostCrearDTO>().ReverseMap();
            CreateMap<Post, PostActualizarDTO>().ReverseMap();
        }
    }
}

using AutoMapper;
using Company.API.Models;
using Company.API.ViewModels;

namespace Company.API.Mapping
{
    public class PreviewMap : Profile
    {
        public PreviewMap()
        {
            CreateMap<CompanyPreview, PreviewViewModel>()
                .ForMember(dest => 
                        dest.Founded, 
                    opt => opt.MapFrom(src => src.CompanyFunding.Founded))
                .ForMember(dest => 
                        dest.Goal, 
                    opt => opt.MapFrom(src => src.CompanyFunding.Goal));
        }
    }
}
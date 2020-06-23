using AutoMapper;
using Funding.API.Models;
using Funding.API.ViewModels;

namespace Funding.API.Mapping
{
    public class FundingMap : Profile
    {
        public FundingMap()
        {
            CreateMap<FundingItem, FundingInputModel>()
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndFunding));

            CreateMap<FundingItem, FundingViewModel>();
        }
    }
}
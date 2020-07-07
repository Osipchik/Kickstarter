using System;
using AutoMapper;
using Company.Models;
using Company.ViewModels;

namespace Company.Infrastructure
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Funding, FundingViewModel>();

            CreateMap<Preview, PreviewViewModel>();
            CreateMap<Preview, CompanyProgress>()
                .ForMember(dest => dest.Progress, opt => opt.MapFrom(src => GetProgress(src.Funding)));

            CreateMap<Reward, RewardViewModel>();
        }

        private float GetProgress(Funding funding)
        {
            var progress = funding.Goal != 0 ? funding.Funded * 100 / funding.Goal : 0;

            return (float) Math.Round(progress, 3);
        }
    }
}
using System;
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
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndFundingDate))
                .ForMember(dest => dest.Progress, opt => opt.MapFrom(src => CalculateProgress(src.Funded, src.Goal)));
            
            CreateMap<CompanyPreview, PreviewInputModel>();
            CreateMap<CompanyPreview, UserPreviewViewModel>()
                .ForMember(dest => dest.Condition, opt => opt.MapFrom(src => GetCondition(src)));
        }

        private float CalculateProgress(float funded, float goal)
        {
            var progress = funded / goal * 100;
            return (float)Math.Round(progress, 3);
        }
        
        private string GetCondition(CompanyPreview preview)
        {
            var condition = preview.EndFundingDate > DateTime.Now 
                ? "ended" 
                : CalculateProgress(preview.Funded, preview.Goal).ToString();

            return condition;
        }
    }
}
using AutoMapper;
using Frisbee.ApiModels;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.Common;

namespace FrisbeeApp.Controllers.Mappers
{
    public class SearchCriteriaProfile : Profile
    {
        public SearchCriteriaProfile()
        {
            CreateMap<SearchCriteriaApiModel, SearchCriteria>();
        }
    }
}

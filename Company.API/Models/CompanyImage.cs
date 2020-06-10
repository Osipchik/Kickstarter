using System.ComponentModel.DataAnnotations;
using Company.API.Infrastructure.Interfaces;

namespace Company.API.Models
{
    public class CompanyImage : IEntity
    {
        public string Id { get; set; }
        
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }
    }
}
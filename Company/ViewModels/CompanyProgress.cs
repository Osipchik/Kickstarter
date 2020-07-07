using Company.Data;

namespace Company.ViewModels
{
    public class CompanyProgress
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public CompanyStatus Status { get; set; }

        public float Progress { get; set; }
    }
}
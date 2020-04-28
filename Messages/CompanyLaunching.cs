using System;

namespace Messages
{
    public class CompanyLaunching
    {
        public string CompanyId;
        public DateTime EndDate;
        public float Goal;

        public CompanyLaunching(string companyId, DateTime endDate, float goal)
        {
            CompanyId = companyId;
            EndDate = endDate;
            Goal = goal;
        }
    }
}
using Company.Data;

namespace CompanyTests.IntegrationTests.Helpers
{
    public static class Utilities
    {
        public static void InitializeDbForTests(AppDbContext db)
        {
            var fundings = TestEntityBuilder.GetFakeFundings();
            db.Fundings.AddRange(fundings);
            db.Previews.AddRange(TestEntityBuilder.GetFakePreviews(fundings));
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(AppDbContext db)
        {
            db.Previews.RemoveRange(db.Previews);
            db.Fundings.RemoveRange(db.Fundings);
            InitializeDbForTests(db);
        }
    }
}
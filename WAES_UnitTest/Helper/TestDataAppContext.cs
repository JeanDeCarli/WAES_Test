using System.Data.Entity;
using WAES_Test.Models;

namespace WAES_UnitTest.Helper
{
    public class TestDataAppContext : IWAESAssignmentDBEntities
    {
        public TestDataAppContext()
        {
            this.Data = new TestDataDbSet();
        }

        public DbSet<Data> Data { get; set; }

        public int SaveChanges()
        {
            return 0;
        }

        public void MarkAsModified(Data item) { }
        public void Dispose() { }
    }
}
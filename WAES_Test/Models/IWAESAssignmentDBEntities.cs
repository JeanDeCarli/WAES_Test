using System;
using System.Data.Entity;

namespace WAES_Test.Models
{
    public interface IWAESAssignmentDBEntities : IDisposable
    {
        DbSet<Data> Data { get; }
        int SaveChanges();
        void MarkAsModified(Data data);
    }
}
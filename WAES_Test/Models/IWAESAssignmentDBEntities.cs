using System;
using System.Data.Entity;
using WAES_Test.Models;

namespace WAES_Test.Models
{
    public interface IWAESAssignmentDBEntities : IDisposable
    {
        DbSet<Data> Data { get; }
        int SaveChanges();
        void MarkAsModified(Data data);
    }
}
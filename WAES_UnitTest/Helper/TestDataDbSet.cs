using System;
using System.Linq;
using WAES_Test.Models;

namespace WAES_UnitTest.Helper
{
    class TestDataDbSet : TestDbSet<Data>
    {
        public override Data Find(params object[] keyValues)
        {
            return this.SingleOrDefault(product => product.Id == (int)keyValues.Single());
        }
    }
}
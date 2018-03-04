using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WAES_Test.Models;

namespace WAES_Test.Helper
{
    public static class APIsHelper
    {
        #region Enums

        public enum Side
        {
            Left,
            Right
        }

        #endregion

        #region Methods

        public static void InsertData(Data data)
        {
            var db = new WAESAssignmentDBEntities();
            var existentData = db.Data.Find(data.Id);

            if (existentData != null)
            {
                db.Data.Remove(existentData);
                db.SaveChanges();
            }

            db.Data.Add(data);
            db.SaveChanges();
        }

        #endregion
    }
}
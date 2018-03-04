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

        public static void InsertData(Data data, Side side)
        {
            var db = new WAESAssignmentDBEntities();
            var existentData = db.Data.Find(data.Id);

            if (existentData != null)
            {
                switch (side)
                {
                    case Side.Left:
                        existentData.LeftSide = data.LeftSide;
                        break;
                    case Side.Right:
                        existentData.RightSide = data.RightSide;
                        break;
                }
            }
            else
            {
                db.Data.Add(data);
            }
            db.SaveChanges();
        }

        #endregion
    }
}
﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WAES_Test.Models;

namespace WAES_Test.Controllers
{
    [RoutePrefix("v1/diff")]
    public class DataController : ApiController
    {
        private WAESAssignmentDBEntities db = new WAESAssignmentDBEntities();

        [Route("all")]
        public IQueryable<Data> GetData()
        {
            return db.Data;
        }

        [Route("{id}")]
        [ResponseType(typeof(Data))]
        public IHttpActionResult GetData(int id)
        {
            Data data = db.Data.Find(id);
            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        // PUT: api/Data/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutData(int id, Data data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != data.Id)
            {
                return BadRequest();
            }

            db.Entry(data).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DataExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("{id}/left")]
        [ResponseType(typeof(Data))]
        public IHttpActionResult InsertLeft(int id, [FromBody] string content)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var data = new Data() {Id = id, EncodedJSON = content, Side = "Left" };

            db.Data.Add(data);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (DataExists(data.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = data.Id }, data);
        }

        // DELETE: api/Data/5
        [ResponseType(typeof(Data))]
        public IHttpActionResult DeleteData(int id)
        {
            Data data = db.Data.Find(id);
            if (data == null)
            {
                return NotFound();
            }

            db.Data.Remove(data);
            db.SaveChanges();

            return Ok(data);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DataExists(int id)
        {
            return db.Data.Count(e => e.Id == id) > 0;
        }
    }
}
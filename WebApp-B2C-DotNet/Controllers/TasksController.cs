using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApp_B2C_DotNet.Models;

namespace WebApp_B2C_DotNet.Controllers
{
    public class TasksController : ApiController
    {
        private SandboxContext db = new SandboxContext();
        private static string ObjectIdClaimType = "oid";

        // GET: api/Tasks
        [HttpGet]
        [HostAuthentication("AADBearer")]
        [Authorize]
        public IQueryable<Models.Task> GetTasks()
        {
            string userId = ClaimsPrincipal.Current.FindFirst(ObjectIdClaimType).Value;
            return db.Tasks.Where(t => t.Owner.Equals(userId));
        }

        // GET: api/Tasks/5
        [HttpGet]
        [HostAuthentication("AADBearer")]
        [Authorize]
        [ResponseType(typeof(Models.Task))]
        public async Task<IHttpActionResult> GetTask(int id)
        {
            string userId = ClaimsPrincipal.Current.FindFirst(ObjectIdClaimType).Value;
            Models.Task task = await db.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            if (task.Owner != userId)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            return Ok(task);
        }

        // PUT: api/Tasks/5
        [HttpPut]
        [HostAuthentication("AADBearer")]
        [Authorize]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTask(int id, Models.Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != task.Id)
            {
                return BadRequest();
            }

            string userId = ClaimsPrincipal.Current.FindFirst(ObjectIdClaimType).Value;

            if (task.Owner != userId)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            task.DateModified = DateTime.UtcNow;
            db.Entry(task).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(task);
        }

        // POST: api/Tasks
        [HttpPost]
        [HostAuthentication("AADBearer")]
        [Authorize]
        [ResponseType(typeof(Models.Task))]
        public async Task<IHttpActionResult> PostTask(Models.Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string userId = ClaimsPrincipal.Current.FindFirst(ObjectIdClaimType).Value;
            task.Owner = userId;
            task.DateModified = DateTime.UtcNow;
            task.Completed = false;

            db.Tasks.Add(task);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = task.Id }, task);
        }

        // DELETE: api/Tasks/5
        [HttpDelete]
        [HostAuthentication("AADBearer")]
        [Authorize]
        [ResponseType(typeof(Models.Task))]
        public async Task<IHttpActionResult> DeleteTask(int id)
        {
            Models.Task task = await db.Tasks.FindAsync(id);
            string userId = ClaimsPrincipal.Current.FindFirst(ObjectIdClaimType).Value;
            if (task == null)
            {
                return NotFound();
            }
            if (task.Owner != userId)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            db.Tasks.Remove(task);
            await db.SaveChangesAsync();

            return Ok(task);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TaskExists(int id)
        {
            return db.Tasks.Count(e => e.Id == id) > 0;
        }
    }
}
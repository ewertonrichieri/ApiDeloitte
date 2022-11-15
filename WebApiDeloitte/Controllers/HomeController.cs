using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApiDeloitte.Model;

namespace WebApiDeloitte.Controllers {
    [Route("api/deloitte")]
    [ApiController]
    public class HomeController : ControllerBase {
        private Context _ctx;

        public HomeController(Context dbContext) {
            _ctx = dbContext;
        }

        [HttpGet]
        [Route("get/schoolrecords")]
        public IActionResult GetSchoolRecords() {
            try {
                ContextModel ctxModel = new ContextModel();
                Response res = ctxModel.GetAllSchoolRecord(_ctx);
                return StatusCode((int)res.StatusCode, !string.IsNullOrEmpty(res.Body) ? res.Body : res.Error);
            }
            catch (Exception ex) {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("post/schoolrecords")]
        public async Task<IActionResult> PostSchoolRecords() {
            try {
                using var reader = new StreamReader(HttpContext.Request.Body);
                string body = await reader.ReadToEndAsync();
                SchoolRecord schoolRec = JsonConvert.DeserializeObject<SchoolRecord>(body);

                ContextModel ctxModel = new ContextModel();
                Response res = ctxModel.PostSchoolRecord(_ctx, schoolRec);

                return StatusCode((int)res.StatusCode, !string.IsNullOrEmpty(res.Body) ? res.Body : res.Error);
            }
            catch (Exception ex) {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [HttpPut]
        [Route("put/schoolrecords")]
        public IActionResult PutSchoolRecords() {
            Student student1 = _ctx.Set<Student>().Where(s => s.Id == 1).FirstOrDefault();
            student1.Name = "Airton Lopes";
            _ctx.Set<Student>().Update(student1);

            _ctx.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("del/schoolrecords")]
        public IActionResult DelSchoolRecords(int id) {
            Student student = _ctx.Set<Student>().Where(s => s.Id == id).FirstOrDefault();
            _ctx.Set<Student>().Remove(student);
            _ctx.SaveChanges();

            return Ok(student);
        }
    }
}

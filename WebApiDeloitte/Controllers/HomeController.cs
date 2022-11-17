using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;
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
                return StatusCode((int)res.StatusCode, !string.IsNullOrEmpty(res.Body) ? res : res.Error);
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
                if (body.Contains("body")) body = GetNewBody(body);

                SchoolRecord schoolRec = JsonConvert.DeserializeObject<SchoolRecord>(body);

                ContextModel ctxModel = new ContextModel();
                Response res = ctxModel.PostSchoolRecord(_ctx, schoolRec);

                return StatusCode((int)res.StatusCode, !string.IsNullOrEmpty(res.Body) ? res : res.Error);
            }
            catch (Exception ex) {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [HttpPut]
        [Route("put/schoolrecords")]
        public async Task<IActionResult> PutSchoolRecords() {
            try {
                using var reader = new StreamReader(HttpContext.Request.Body);
                string body = await reader.ReadToEndAsync();
                SchoolRecord schoolRec = JsonConvert.DeserializeObject<SchoolRecord>(body);

                ContextModel ctxModel = new ContextModel();
                Response res = ctxModel.PutSchoolRecord(_ctx, schoolRec);
                return StatusCode((int)res.StatusCode, !string.IsNullOrEmpty(res.Body) ? res : res.Error);
            }
            catch (Exception ex) {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("del/schoolrecords")]
        public IActionResult DelSchoolRecords(int idBulletinGrade) {
            try {
                ContextModel ctxModel = new ContextModel();
                Response res = ctxModel.DeleteSchoolRecordByIdBulletinGrade(_ctx, idBulletinGrade);
                return StatusCode((int)res.StatusCode, !string.IsNullOrEmpty(res.Body) ? res : res.Error);
            }
            catch (Exception ex) {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private string GetNewBody(string body)
        {
           return body.Replace("\"body\":[", string.Empty).Replace("]", string.Empty).Replace("{{", "{").Replace("}}", "}");
        }
    }
}

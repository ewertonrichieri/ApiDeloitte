using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WebApiDeloitte.Model;

namespace WebApiDeloitte.Controllers
{
    [Route("api/deloitte")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private Context _dbContext;

        public HomeController(Context dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("get/schoolrecords")]
        public IActionResult GetSchoolRecords()
        {
            try
            {
                IList<Student> schoolRecord = _dbContext.Students.ToList();
                return Ok(schoolRecord);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("post/schoolrecords")]
        public IActionResult PostSchoolRecords()
        {
            Student student = new Student() { Name = "Ernesto", Email = "teste@gmail.com", BirthDate = new DateTime() };
            _dbContext.Set<Student>().Add(student);
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("put/schoolrecords")]
        public IActionResult PutSchoolRecords()
        {
            //Student student = new Student() { Name = "Ernesto", Email = "teste@gmail.com", BirthDate = new DateTime() };
            Student student1 = _dbContext.Set<Student>().Where(s => s.Id == 1).FirstOrDefault();
            student1.Name = "Airton Lopes";
            _dbContext.Set<Student>().Update(student1);

            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("del/schoolrecords")]
        public IActionResult DelSchoolRecords()
        {
            Student student1 = _dbContext.Set<Student>().Where(s => s.Id == 5).FirstOrDefault();
            student1.Name = "Airton Lopes";
            _dbContext.Set<Student>().Remove(student1);

            _dbContext.SaveChanges();

            return Ok(student1);
        }
    }
}

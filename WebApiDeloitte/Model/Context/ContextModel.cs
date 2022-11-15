using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace WebApiDeloitte.Model {
    public class ContextModel {
        private Context _ctx;
        public Response GetAllSchoolRecord(Context ctx) {
            ResponseModel respModel = new ResponseModel();
            try {
                List<SchoolRecord> schoolRecords = new List<SchoolRecord>();
                List<BulletinGrade> bulletinGrades = ctx.BulletinGrades.ToList();
                List<Bulletin> bulletins = ctx.Bulletins.ToList();
                List<Discipline> disciplines = ctx.Disciplines.ToList();
                List<Student> students = ctx.Students.ToList();

                foreach (BulletinGrade blGrade in bulletinGrades) {
                    SchoolRecord schRec = new SchoolRecord();
                    schRec.Discipline = disciplines.Where(d => d.Id == blGrade.IdDiscipline).FirstOrDefault();
                    schRec.Bulletin = bulletins.Where(b => b.Id == blGrade.IdBulletin).FirstOrDefault();
                    schRec.Student = students.Where(s => s.Id == schRec.Bulletin.IdStudenty).FirstOrDefault();
                    schRec.BulletinGrade = blGrade;
                    if (schRec.Student != null)
                        schoolRecords.Add(schRec);
                }
                return respModel.GetResponse(JsonConvert.SerializeObject(schoolRecords), HttpStatusCode.OK);
            }
            catch (Exception ex) {
                return respModel.GetResponse(string.Empty, HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public Response PostSchoolRecord(Context ctx, SchoolRecord schoolRec) {
            ResponseModel respModel = new ResponseModel();
            try {
                _ctx = ctx;
                int idStudent = PostStudentAndGetId(schoolRec.Student);
                schoolRec.Bulletin.IdStudenty = idStudent;

                int idBulletin = PostBulletinAndGetId(schoolRec.Bulletin);
                schoolRec.BulletinGrade.IdBulletin = idBulletin;

                int idDiscipline = PostDisciplineAndGetId(schoolRec.Discipline);

                schoolRec.BulletinGrade.IdBulletin = idBulletin;
                schoolRec.BulletinGrade.IdDiscipline = idDiscipline;

                PostBulletinGradeBySchoolRecord(schoolRec);
                ctx.SaveChanges();

                return respModel.GetResponse(Msg.registerOk, HttpStatusCode.Created);
            }
            catch (Exception ex) {
                return respModel.GetResponse(string.Empty, HttpStatusCode.BadRequest, ex.Message);
            }
        }

        private int PostStudentAndGetId(Student std) {
            _ctx.Set<Student>().Add(std);
            _ctx.SaveChanges();
            return _ctx.Set<Student>().Where(s => s.Name == std.Name
            && s.Email == std.Email && s.BirthDate == std.BirthDate).FirstOrDefault().Id;
        }

        private int PostBulletinAndGetId(Bulletin bullt) {
            _ctx.Set<Bulletin>().Add(bullt);
            _ctx.SaveChanges();
            return _ctx.Bulletins.Where(b => b.IdStudenty == bullt.IdStudenty).FirstOrDefault().Id;
        }

        private int PostDisciplineAndGetId(Discipline dp) {
            _ctx.Set<Discipline>().Add(dp);
            _ctx.SaveChanges();
            return _ctx.Set<Discipline>().Where(d => d.Name == dp.Name && d.Workload == dp.Workload).FirstOrDefault().Id;
        }

        private void PostBulletinGradeBySchoolRecord(SchoolRecord schoolRec) {
            _ctx.Set<BulletinGrade>().Add(schoolRec.BulletinGrade);
        }
    }
}

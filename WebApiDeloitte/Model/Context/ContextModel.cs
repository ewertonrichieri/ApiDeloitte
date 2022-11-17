using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace WebApiDeloitte.Model
{
    public class ContextModel
    {
        private Context _ctx;
        public Response GetAllSchoolRecord(Context ctx)
        {
            ResponseModel respModel = new ResponseModel();
            try {
                List<SchoolRecord> schoolRecords = new List<SchoolRecord>();
                List<BulletinGrade> bulletinGrades = ctx.BulletinGrades.ToList();

                foreach (BulletinGrade blGrade in bulletinGrades) {
                    SchoolRecord schRec = new SchoolRecord();
                    schRec.Discipline = GetDiscipline(ctx, blGrade);
                    schRec.Bulletin = ctx.Bulletins.Where(b => b.Id == blGrade.IdBulletin).FirstOrDefault();
                    if (schRec.Bulletin != null)
                        schRec.Student = ctx.Students.Where(s => s.Id == schRec.Bulletin.IdStudenty).FirstOrDefault();
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

        private Discipline GetDiscipline(Context ctx, BulletinGrade blGrade)
        {
            Discipline discipline = ctx.Disciplines.Where(d => d.Id == blGrade.IdDiscipline).FirstOrDefault();
            return discipline == null ? new Discipline() { Name = String.Empty } : discipline;
        }

        public Response PutSchoolRecord(Context ctx, SchoolRecord newSchoolRec)
        {
            ResponseModel respModel = new ResponseModel();
            try {
                _ctx = ctx;
                int idBulletinGrade = newSchoolRec.BulletinGrade.Id;
                BulletinGrade blGrade = _ctx.Set<BulletinGrade>().Where(s => s.Id == idBulletinGrade).FirstOrDefault();
                if (blGrade == null)
                    return respModel.GetResponse(string.Empty, HttpStatusCode.NotModified, Msg.BulletinGradeNotModified);

                SchoolRecord oldSchoolRec = GetSchoolRecordByBulletinGrade(blGrade);
                UpdateSchoolRecord(oldSchoolRec, newSchoolRec);

                return respModel.GetResponse(Msg.registerChangedOk, HttpStatusCode.OK);
            }
            catch (Exception ex) {
                return respModel.GetResponse(string.Empty, HttpStatusCode.BadRequest, ex.Message);
            }
        }

        private SchoolRecord GetSchoolRecordByBulletinGrade(BulletinGrade blGrade)
        {
            SchoolRecord schRec = new SchoolRecord();
            schRec.Discipline = _ctx.Disciplines.Where(d => d.Id == blGrade.IdDiscipline).FirstOrDefault();
            schRec.Bulletin = _ctx.Bulletins.Where(b => b.Id == blGrade.IdBulletin).FirstOrDefault();
            schRec.Student = _ctx.Students.Where(s => s.Id == schRec.Bulletin.IdStudenty).FirstOrDefault();
            schRec.BulletinGrade = blGrade;
            return schRec;
        }

        private void UpdateSchoolRecord(SchoolRecord oldSchoolRec, SchoolRecord newSchoolRec)
        {
            oldSchoolRec.Student.Name = newSchoolRec.Student.Name;
            oldSchoolRec.Student.Email = newSchoolRec.Student.Email;
            oldSchoolRec.Student.BirthDate = newSchoolRec.Student.BirthDate;
            _ctx.Set<Student>().Update(oldSchoolRec.Student);

            oldSchoolRec.Discipline.Name = newSchoolRec.Discipline.Name;
            oldSchoolRec.Discipline.Workload = newSchoolRec.Discipline.Workload;
            _ctx.Set<Discipline>().Update(oldSchoolRec.Discipline);

            oldSchoolRec.Bulletin.DeliveryDate = newSchoolRec.Bulletin.DeliveryDate;
            _ctx.Set<Bulletin>().Update(oldSchoolRec.Bulletin);

            oldSchoolRec.BulletinGrade.Grade = newSchoolRec.BulletinGrade.Grade;
            _ctx.Set<BulletinGrade>().Update(oldSchoolRec.BulletinGrade);

            _ctx.SaveChanges();
        }

        public Response PostSchoolRecord(Context ctx, SchoolRecord schoolRec)
        {
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

        private int PostStudentAndGetId(Student std)
        {
            _ctx.Set<Student>().Add(std);
            _ctx.SaveChanges();
            return _ctx.Set<Student>().Where(s => s.Name == std.Name
            && s.Email == std.Email && s.BirthDate == std.BirthDate).FirstOrDefault().Id;
        }

        private int PostBulletinAndGetId(Bulletin bullt)
        {
            _ctx.Set<Bulletin>().Add(bullt);
            _ctx.SaveChanges();
            return _ctx.Bulletins.Where(b => b.IdStudenty == bullt.IdStudenty).FirstOrDefault().Id;
        }

        private int PostDisciplineAndGetId(Discipline dp)
        {
            _ctx.Set<Discipline>().Add(dp);
            _ctx.SaveChanges();
            return _ctx.Set<Discipline>().Where(d => d.Name == dp.Name && d.Workload == dp.Workload).FirstOrDefault().Id;
        }

        private void PostBulletinGradeBySchoolRecord(SchoolRecord schoolRec)
        {
            _ctx.Set<BulletinGrade>().Add(schoolRec.BulletinGrade);
        }

        public Response DeleteSchoolRecordByIdBulletinGrade(Context ctx, int idBulletinGrade)
        {
            ResponseModel respModel = new ResponseModel();
            try {
                _ctx = ctx;
                BulletinGrade blGrade = _ctx.Set<BulletinGrade>().Where(s => s.Id == idBulletinGrade).FirstOrDefault();
                if (blGrade == null)
                    return respModel.GetResponse(string.Empty, HttpStatusCode.NotModified, Msg.BulletinGradeNotModified);

                SchoolRecord schoolRec = GetSchoolRecordByBulletinGrade(blGrade);
                DeleteSchoolRecord(schoolRec);

                return respModel.GetResponse(Msg.registerDeletedOk, HttpStatusCode.OK);
            }
            catch (Exception ex) {
                return respModel.GetResponse(string.Empty, HttpStatusCode.BadRequest, ex.Message);
            }
        }

        private void DeleteSchoolRecord(SchoolRecord schRec)
        {
            if(schRec.Student != null) _ctx.Set<Student>().Remove(schRec.Student);
            if(schRec.Discipline != null) _ctx.Set<Discipline>().Remove(schRec.Discipline);
            if(schRec.Bulletin != null) _ctx.Set<Bulletin>().Remove(schRec.Bulletin);
            if(schRec.BulletinGrade != null) _ctx.Set<BulletinGrade>().Remove(schRec.BulletinGrade);
            _ctx.SaveChanges();
        }
    }
}

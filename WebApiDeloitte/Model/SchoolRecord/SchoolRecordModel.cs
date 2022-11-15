using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebApiDeloitte.Model
{
    public class SchoolRecordModel
    {
        public SchoolRecord GetAllSchoolRecords(Context ctx)
        {
            List<SchoolRecord> liSchoolRec = new List<SchoolRecord>();
            //liSchoolRec.Student.addr ctx.Students.ToList();
            //liSchoolRec.Discipline = ctx.Disciplines.ToList();
            //liSchoolRec.Bulletin = ctx.Bulletins.ToList();
            //liSchoolRec.BulletinGrade = ctx.BulletinGrades.ToList();

            return liSchoolRec.FirstOrDefault();    
        }
    }
}

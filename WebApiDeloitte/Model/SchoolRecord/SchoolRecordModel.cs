using System.Linq;

namespace WebApiDeloitte.Model
{
    public class SchoolRecordModel
    {
        public SchoolRecord GetAllSchoolRecords(Context ctx)
        {
            SchoolRecord schoolRecord = new SchoolRecord();
            schoolRecord.Student = ctx.Students.ToList();
            schoolRecord.Discipline = ctx.Disciplines.ToList();
            schoolRecord.Bulletin = ctx.Bulletins.ToList();
            schoolRecord.BulletinGrade = ctx.BulletinGrades.ToList();

            return schoolRecord;    
        }
    }
}

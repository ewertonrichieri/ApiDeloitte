using System.Collections.Generic;

namespace WebApiDeloitte.Model
{
    public class SchoolRecord
    {
        public List<Bulletin> Bulletin { get; set; }
        public List<BulletinGrade> BulletinGrade { get; set; }
        public List<Discipline> Discipline { get; set; }
        public List<Student> Student { get; set; }
    }
}

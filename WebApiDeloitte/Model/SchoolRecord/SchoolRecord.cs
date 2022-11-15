using System.Collections.Generic;

namespace WebApiDeloitte.Model
{
    public class SchoolRecord
    {
        public Student Student { get; set; }
        public Discipline Discipline { get; set; }
        public Bulletin Bulletin { get; set; }
        public BulletinGrade BulletinGrade { get; set; }
    }
}

using System;

namespace WebApiDeloitte.Model {
    public class ContextModel {

        public void GetAllSchoolRecord() {

        }

        public void PostSchoolRecord(Context ctx, SchoolRecord schoolRec) {
            try {
                //AQUI TEM INPUT UM DE CADA VEZ PARA PEGAR O ID
                ctx.Set<Student>().Add(schoolRec.Student);
                ctx.Set<Discipline>().Add(schoolRec.Discipline);
                ctx.Set<Bulletin>().Add(schoolRec.Bulletin);
                ctx.Set<BulletinGrade>().Add(schoolRec.BulletinGrade);
                ctx.SaveChanges();
            }
            catch (Exception ex) {

            }
        }
    }
}

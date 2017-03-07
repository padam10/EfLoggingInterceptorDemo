using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

namespace EF6DBFirstTutorials
{
    public class Logger
    {
        public static void Log(string message)
        {
            Console.WriteLine("EF Message: {0} ", message);
        }
    }

    class EF6Demo
    {

        public static void AddRange()
        {

            IList<Student> newStudents = new List<Student>();
            newStudents.Add(new Student() { StudentName = "Student1 by addrange" });
            newStudents.Add(new Student() { StudentName = "Student2 by addrange" });
            newStudents.Add(new Student() { StudentName = "Student3 by addrange" });
               
            Console.WriteLine("*** AddRange Start ***");
            using (var context = new SchoolDBEntities())
            {
                context.Students.AddRange(newStudents);
                context.SaveChanges();
            }
            Console.WriteLine("*** AddRange Finished ***");

        }
        public static void DBCommandLogging()
        {
            Console.WriteLine("*** DBCommandLogging Start ***");
            using (var context = new SchoolDBEntities())
            {
                context.Database.Log =   Logger.Log;
                var student = context.Students
                                    .Where(s => s.StudentID == 1).FirstOrDefault<Student>();

                student.StudentName = "Edited Name";
                context.SaveChanges();
            }
            Console.WriteLine("*** DBCommandLogging Finished ***");
        }

        public static void DbCommandInterceptor()
        {
            Console.WriteLine("*** DbCommandInterceptor Start ***");

            IDbCommandInterceptor consoleInterceptor = new EFCommandInterceptor();
            System.Data.Entity.Infrastructure.Interception.DbInterception.Add(consoleInterceptor);

            using (var context = new SchoolDBEntities())
            {
                var student = context.Students
                                    .Where(s => s.StudentID == 1).FirstOrDefault<Student>();

                student.StudentName = "Edited Name for interception demo";
                context.SaveChanges();
            }
            System.Data.Entity.Infrastructure.Interception.DbInterception.Remove(consoleInterceptor);
            Console.WriteLine("*** DbCommandInterceptor Finished ***"); // this one won't get logged 
        }


        public static void AsyncQueryAndSave()
        {
            Console.WriteLine("*** AsyncQueryAndSave Start ***");
            var student = GetStudent();
            Console.WriteLine("Let's do something else till we get student..");
           
            ////check how many times it loops through till it is waiting to complete GetStudent
            //int i = 0;
            //while (!student.IsCompleted)
            //{
            //    Console.WriteLine("Let's do something else till we get student.." + (i += 1).ToString());
            //}

            student.Wait();

            
            var studentSave = SaveStudent(student.Result);
            Console.WriteLine("Let's do something else till we save student.." );

            ////check how many times it loops through till it is waiting to complete GetStudent
            //i = 0;
            //while (!studentSave.IsCompleted)
            //{
            //    Console.WriteLine("Let's do something else till we get student.." + (i += 1).ToString());
            //}

           // Console.WriteLine("Let's do something else till we Save student..");
            studentSave.Wait();

            Console.WriteLine("*** AsyncQueryAndSave Finished ***");
        }

        private static async Task SaveStudent(Student editedStudent)
        {

            using (var context = new SchoolDBEntities())
            {
                context.Entry(editedStudent).State = EntityState.Modified;
                
                Console.WriteLine("Start SaveStudent...");
                
                int x = await (context.SaveChangesAsync());
                
                Console.WriteLine("Finished SaveStudent...");
            }
        
        }
        private static async Task<Student> GetStudent()
        {
            Student student = null;

            using (var context = new SchoolDBEntities())
            {
                Console.WriteLine("Start GetStudent...");
                student = await (context.Students.Where(s => s.StudentID == 1).FirstOrDefaultAsync<Student>());
                Console.WriteLine("Finished GetStudent...");
            }

            return student;
        }

        public static void TransactionSupport()
        {
            Console.WriteLine("*** TransactionSupport Start ***");
            using (var context = new SchoolDBEntities())
            {
                
                using (System.Data.Entity.DbContextTransaction dbTran = context.Database.BeginTransaction( ))
                {
                    try
                    {
                        Student std1 = new Student() { StudentName = "newstudent" };
                        context.Students.Add(std1);
                        context.Database.ExecuteSqlCommand(
                           @"UPDATE Student SET StudentName = 'Edited Student Name'" +
                               " WHERE StudentID =1"
                           );
                        context.Students.Remove(std1);

                        //saves all above operations within one transaction
                        context.SaveChanges();

                        //commit transaction
                        dbTran.Commit();
                    }
                    catch (Exception ex)
                    {
                        //Rollback transaction if exception occurs
                        dbTran.Rollback();
                    }

                }
            }
            Console.WriteLine("*** TransactionSupport Finished ***");
        }

     
    }
}

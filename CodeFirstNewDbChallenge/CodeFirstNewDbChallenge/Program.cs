using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.SymbolStore;

namespace CodeFirstNewDbChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new SchoolContext())
            {

                DateTime dob = new DateTime(1990, 09, 27);
                var student = new Student() { FirstName = "Laura", LastName = "Ha", DateOfBirth = dob, EmailAddress = "laura@laura.com", StudentId = 1 };

                db.Students.Add(student);
                db.SaveChanges();


            }
        }
    }
    
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string EmailAddress { get; set; }
        
        public Grade Grade { get; set; }

    }

    public class Grade
    {
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public string Section { get; set; }

        public ICollection<Student> Students { get; set; }
    }

    public class SchoolContext : DbContext
    {
        public SchoolContext() : base()
        {

        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }
    }


}

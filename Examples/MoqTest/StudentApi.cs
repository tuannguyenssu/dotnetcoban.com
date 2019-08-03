using System;
using System.Collections.Generic;
using System.Text;

namespace MoqTest
{
    public class Student
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public interface IStudentRepository
    {
        List<Student> GetAll();
        Student GetById(int id);
    }
}

using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace MoqTest
{
    public class StudentApiTest
    {
        [Fact]
        public void GetAll()
        {
            var mock = new Mock<IStudentRepository>();
            mock.Setup(m => m.GetAll()).Returns(new List<Student>()
            {
                new Student(){Name = "Tuan Nguyen 1", Age = 20},
                new Student(){Name = "Tuan Nguyen 2", Age = 21},
                new Student(){Name = "Tuan Nguyen 3", Age = 22},
            }
            );
            var students = mock.Object.GetAll();
            Assert.NotNull(students);
            Assert.Equal("Tuan Nguyen 2", students[1].Name);
            Assert.Equal(20, students[0].Age);
        }
    }
}

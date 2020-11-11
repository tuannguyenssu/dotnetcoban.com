using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace AttributeTest
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method)]
    public class VietnameseInfoAttribute : Attribute
    {
        public string Description { get; set; }

        public VietnameseInfoAttribute(string s) => Description = s;
    }

    [VietnameseInfo("Lớp biểu diễn đối tượng học sinh")]
    public class Student
    {
        [VietnameseInfo("Thuộc tính tên học sinh")]
        public string Name { get; set; }
        [VietnameseInfo("Thuộc tính tuổi học sinh")]
        public int Age { get; set; }

        [VietnameseInfo("Phương thức hiển thị thông tin học sinh")]
        public override string ToString()
        {
            return $"{Name} {Age}";
        }

        public static Type GetCurrentType()
        {
            var student = new Student();
            return student.GetType();
        }
    }


    public class User
    {
        [Required(ErrorMessage = "Bắt buộc phải có trường tên")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tên từ 3 đến  100 ký tự")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Range(18, 99, ErrorMessage = "Tuổi phải từ 18 đến 99")]
        public int Age { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { set; get; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }
    }

    public class AttributeTest
    {
        public static void Run()
        {
            Console.OutputEncoding = Encoding.UTF8;

            foreach (var attribute in Student.GetCurrentType().GetCustomAttributes(false))
            {
                if (attribute != null && attribute is VietnameseInfoAttribute vietnameseInfo)
                {
                    Console.WriteLine($"{Student.GetCurrentType().Name} {vietnameseInfo.Description}");
                }
            }

            foreach (var property in Student.GetCurrentType().GetProperties())
            {
                foreach (var attribute in property.GetCustomAttributes(false))
                {
                    if (attribute != null && attribute is VietnameseInfoAttribute vietnameseInfo)
                    {
                        Console.WriteLine($"{property.Name} {vietnameseInfo.Description}");
                    }
                }
            }

            foreach (var method in Student.GetCurrentType().GetMethods())
            {
                foreach (var attribute in method.GetCustomAttributes(false))
                {
                    if (attribute != null && attribute is VietnameseInfoAttribute vietnameseInfo)
                    {
                        Console.WriteLine($"{method.Name} {vietnameseInfo.Description}");
                    }
                }
            }
        }

        public static void RunDataAnotationValidationTest()
        {
            var user = new User {Age = 6, PhoneNumber = "abc", Email = "abc"};

            var context = new ValidationContext(user, null, null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(user, context, results, true);
            if (!isValid)
            {
                Console.OutputEncoding = Encoding.UTF8;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                foreach (var result in results)
                {
                    Console.WriteLine($" {result.MemberNames.First()}");
                    Console.WriteLine($"{result.ErrorMessage}");
                }
            }
        }
    }
}

using System;
using FluentEmail.Core;

namespace FluentEmailTest
{
    //https://github.com/lukencode/FluentEmail
    class Program
    {
        static void Main(string[] args)
        {
            var email = Email
                .From("john@email.com")
                .To("bob@email.com", "bob")
                .Subject("hows it going bob")
                .Body("yo dawg, sup?")
                .Send();
        }
    }
}

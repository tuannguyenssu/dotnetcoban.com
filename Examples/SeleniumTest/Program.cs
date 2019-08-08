using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Đường dẫn tới thư mục chứa Chrome Driver
            // Tải về tại đây https://chromedriver.chromium.org/downloads
            // Thay đổi cho đúng với trường hợp của bạn
            string chromeDriverPath = @"D:\ChromeDriver";
            var options = new ChromeOptions();
            options.AddArgument("no-sandbox");
            // Chạy ngầm không pop up trình duyệt ra ngoài 
            //options.AddArgument("headless");
            var driver = new ChromeDriver(chromeDriverPath, options, TimeSpan.FromDays(20));
            string url = "https://www.dotnetcoban.com/";
            driver.Url = url;

            driver.Navigate().GoToUrl(url);
            Console.ReadKey();
        }
    }
}

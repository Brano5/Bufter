using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace BufterSeleniumTests
{
    public class UserTest
    {
        private WebDriver WebDriver { get; set; } = null;
        private string DriverPath { get; set; } = @"D:\Robota\MTS\Bufter\BufterSeleniumTests";
        private string BaseUrl { get; set; } = "https://localhost:7005/";

        [SetUp]
        public void Setup()
        {
            WebDriver = GetChromeDriver();
            WebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
        }

        [TearDown]
        public void TearDown()
        {
            WebDriver.Quit();
        }

        [Order(1)]
        [Test]
        public void CreateUserTestSuccess()
        {
            Login();

            //Navigate to user page
            WebDriver.Navigate().GoToUrl(BaseUrl + "User");

            //Click on create
            var input = WebDriver.FindElement(By.CssSelector("div[class='fixed-table-toolbar'] div[class='bs-bars float-left'] div[id='toolbar'] button"));
            input.Click();

            //Enter Name
            input = WebDriver.FindElement(By.Id("NameCreateModal"));
            input.Clear();
            input.SendKeys("testUser");

            //Enter Password
            input = WebDriver.FindElement(By.Id("PasswordCreateModal"));
            input.Clear();
            input.SendKeys("testPassword");

            //Click on Create button
            input = WebDriver.FindElement(By.CssSelector("form[action='/User/Create'] button[type='submit']"));
            input.Click();

            //Validate Alert
            var inputs = WebDriver.FindElements(By.XPath("//div[text()='Succesfull created!']"));
            Assert.True(inputs.Count != 0);

            //Validate
            input = WebDriver.FindElement(By.XPath("//p[text()='testUser']"));
            Assert.IsNotNull(input);
        }

        [Order(2)]
        [Test]
        public void CreateExistedUserTest()
        {
            Login();

            //Navigate to user page
            WebDriver.Navigate().GoToUrl(BaseUrl + "User");

            //Click on create
            var input = WebDriver.FindElement(By.CssSelector("div[class='fixed-table-toolbar'] div[class='bs-bars float-left'] div[id='toolbar'] button"));
            input.Click();

            //Enter Name
            input = WebDriver.FindElement(By.Id("NameCreateModal"));
            input.Clear();
            input.SendKeys("testUser");

            //Enter Password
            input = WebDriver.FindElement(By.Id("PasswordCreateModal"));
            input.Clear();
            input.SendKeys("testPassword");

            //Click on Create button
            input = WebDriver.FindElement(By.CssSelector("form[action='/User/Create'] button[type='submit']"));
            input.Click();

            //Validate Alert
            var inputs = WebDriver.FindElements(By.XPath("//div[text()='Wrong name!']"));
            Assert.True(inputs.Count != 0);

            //Validate
            input = WebDriver.FindElement(By.XPath("//p[text()='testUser']"));
            Assert.IsNotNull(input);
        }

        [Order(3)]
        [Test]
        public void EditUserTestSuccessChangePassword()
        {
            Login();

            //Navigate to user page
            WebDriver.Navigate().GoToUrl(BaseUrl + "User");

            //Find user
            var inputRow = WebDriver.FindElement(By.XPath("//p[text()='testUser']/../.."));

            //Click on edit
            var input = inputRow.FindElement(By.CssSelector("td[class='text-center'] a[title='Edit']"));
            input.Click();

            //Enter new Password
            input = inputRow.FindElement(By.CssSelector("td input[class='form-control editInput password']"));
            input.Clear();
            input.SendKeys("newTestPassword");

            //Click on edit
            input = inputRow.FindElement(By.CssSelector("td[class='text-center'] a[title='Save']"));
            input.Click();

            //Validate Alert
            var inputs = WebDriver.FindElements(By.XPath("//div[text()='successfully edited!']"));
            Assert.True(inputs.Count != 0);
        }

        [Order(4)]
        [Test]
        public void DeleteUserTestSuccess()
        {
            Login();

            //Navigate to user page
            WebDriver.Navigate().GoToUrl(BaseUrl + "User");

            //Find delete user
            var input = WebDriver.FindElement(By.XPath("//p[text()='testUser']/../.."));

            //Click on delete
            input = input.FindElement(By.CssSelector("td[class='text-center'] a[title='Remove']"));
            input.Click();

            //Click on Delete button
            input = WebDriver.FindElement(By.CssSelector("form[action='/User/Delete'] button[type='submit']"));
            input.Click();

            //Validate
            WebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            var inputs = WebDriver.FindElements(By.XPath("//p[text()='testUser']"));
            Assert.True(inputs.Count == 0);
            WebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
        }

        private void Login()
        {
            //Navigate to login page
            WebDriver.Navigate().GoToUrl(BaseUrl + "Login");

            //Enter Name
            var input = WebDriver.FindElement(By.Id("Name"));
            input.Clear();
            input.SendKeys("admin");

            //Enter Password
            input = WebDriver.FindElement(By.Id("Password"));
            input.Clear();
            input.SendKeys("admin");

            //Click on Login button
            input = WebDriver.FindElement(By.CssSelector("form[name='myForm'] button[type='submit']"));
            input.Click();

            //Validate Url
            Assert.That(WebDriver.Url, Is.EqualTo("https://localhost:7005/User"));

            //Validate Log Out button
            input = WebDriver.FindElement(By.PartialLinkText("Log Out"));
            Assert.IsNotNull(input);
        }


        private WebDriver GetChromeDriver()
        {
            var options = new ChromeOptions();

            return new ChromeDriver(DriverPath, options, TimeSpan.FromSeconds(300));
        }
    }
}
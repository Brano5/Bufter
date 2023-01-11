using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BufterSeleniumTests
{
    public class LoginTest
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

        [Test]
        public void LoginTestSuccess()
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

            //Validate Alert
            var inputs = WebDriver.FindElements(By.XPath("//div[text()='User successfuly logged in!']"));
            Assert.True(inputs.Count != 0);

            //Validate Log Out button
            input = WebDriver.FindElement(By.PartialLinkText("Log Out"));
            Assert.IsNotNull(input);
        }

        [Test]
        public void LoginTestWrongCredentials()
        {
            //Navigate to login page
            WebDriver.Navigate().GoToUrl(BaseUrl + "Login");

            //Enter Wrong Name
            var input = WebDriver.FindElement(By.Id("Name"));
            input.Clear();
            input.SendKeys("hsfusdf");

            //Enter Wrong Password
            input = WebDriver.FindElement(By.Id("Password"));
            input.Clear();
            input.SendKeys("sdffdsfsd");

            //Click on Login button
            input = WebDriver.FindElement(By.CssSelector("form[name='myForm'] button[type='submit']"));
            input.Click();

            //Validate Url
            Assert.That(WebDriver.Url, Is.EqualTo("https://localhost:7005/Login"));

            //Validate Alert
            var inputs = WebDriver.FindElements(By.XPath("//div[text()='User was not logged in!']"));
            Assert.True(inputs.Count != 0);

            //Validate Log In button
            input = WebDriver.FindElement(By.CssSelector("form[name='myForm'] button[type='submit']"));
            Assert.IsNotNull(input);
        }

        [Test]
        public void LoginTestEmptyCredentials()
        {
            //Navigate to login page
            WebDriver.Navigate().GoToUrl(BaseUrl + "Login");

            //Click on Login button
            var input = WebDriver.FindElement(By.CssSelector("form[name='myForm'] button[type='submit']"));
            input.Click();

            //Validate Url
            Assert.That(WebDriver.Url, Is.EqualTo("https://localhost:7005/Login"));

            //Validate Login button
            input = WebDriver.FindElement(By.CssSelector("form[name='myForm'] button[type='submit']"));
            Assert.IsNotNull(input);
        }

        [Test]
        public void LoginTestSQLInjection()
        {
            //Navigate to login page
            WebDriver.Navigate().GoToUrl(BaseUrl + "Login");

            //Enter SQL injection
            var input = WebDriver.FindElement(By.Id("Name"));
            input.Clear();
            input.SendKeys("'OR 1=1--");

            //Enter Password
            input = WebDriver.FindElement(By.Id("Password"));
            input.Clear();
            input.SendKeys("sdffdsfsd");

            //Click on Login button
            input = WebDriver.FindElement(By.CssSelector("form[name='myForm'] button[type='submit']"));
            input.Click();

            //Validate Url
            Assert.That(WebDriver.Url, Is.EqualTo("https://localhost:7005/Login"));

            //Validate Alert
            var inputs = WebDriver.FindElements(By.XPath("//div[text()='User was not logged in!']"));
            Assert.True(inputs.Count != 0);

            //Validate Login button
            input = WebDriver.FindElement(By.CssSelector("form[name='myForm'] button[type='submit']"));
            Assert.IsNotNull(input);
        }

        private WebDriver GetChromeDriver()
        {
            var options = new ChromeOptions();

            return new ChromeDriver(DriverPath, options, TimeSpan.FromSeconds(300));
        }
    }
}
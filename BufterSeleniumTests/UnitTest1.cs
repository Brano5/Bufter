using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BufterSeleniumTests
{
    public class Tests
    {
        private WebDriver WebDriver { get; set; } = null;
        private string DriverPath { get; set; } = @"D:\Robota\MTS\Bufter\BufterSeleniumTests";
        private string BaseUrl { get; set; } = "https://localhost:7005/";

        [SetUp]
        public void Setup()
        {
            WebDriver = GetChromeDriver();
            WebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(120);
        }

        [TearDown]
        public void TearDown()
        {
            WebDriver.Quit();
        }

        [Test]
        public void IsPageTitle()
        {
            WebDriver.Navigate().GoToUrl(BaseUrl);
            Assert.AreEqual("Home - Bufter", WebDriver.Title);
        }

        [Test]
        public void LoginTest()
        {
            // Navigate to login page
            WebDriver.Navigate().GoToUrl(BaseUrl + "Login");

            // Enter Name
            Thread.Sleep(5000);
            var input = WebDriver.FindElement(By.Id("Name"));
            input.Clear();
            input.SendKeys("admin");

            // Enter Password
            Thread.Sleep(5000);
            input = WebDriver.FindElement(By.Id("Password"));
            input.Clear();
            input.SendKeys("admin");

            // Click on Login button
            Thread.Sleep(5000);
            input = WebDriver.FindElement(By.LinkText("Login"));
            input.Click();

            // Validate login message
            var startTimestamp = DateTime.Now.Millisecond;
            var endTimstamp = startTimestamp + 15000;

            while (true)
            {
                try
                {
                    input = WebDriver.FindElement(By.Id("p_welcome_message"));
                    Assert.AreEqual("Hello, julius. caesar@gmail.com", input.Text);
                    break;
                }
                catch
                {
                    var currentTimestamp = DateTime.Now.Millisecond;
                    if (currentTimestamp > endTimstamp)
                    {
                        throw;
                    }
                    Thread.Sleep(2000);
                }
            }
        }

        private WebDriver GetChromeDriver()
        {
            var options = new ChromeOptions();

            return new ChromeDriver(DriverPath, options, TimeSpan.FromSeconds(300));
        }
    }
}
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Globalization;

namespace BufterSeleniumTests
{
    public class BuyItemTest
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
        public void BuyItemTestSuccess()
        {
            //Arrange
            string room = "Programatori";
            string person = "Braòo";
            string item = "Horalka";

            //Navigate to room page
            WebDriver.Navigate().GoToUrl(BaseUrl + "Home/Room");

            //Click on room
            var input = WebDriver.FindElement(By.PartialLinkText(room));
            input.Click();

            //Click on person
            input = WebDriver.FindElement(By.PartialLinkText(person));
            input.Click();

            //Find Bill
            input = WebDriver.FindElement(By.CssSelector("div[class='col-md-4 p-4 mt-4'] div[class='card mt-2 text-center'] div[class='card-body'] p[class='card-text']"));
            var bill = input.Text.Substring(input.Text.LastIndexOf(':') + 2, input.Text.IndexOf('€') - input.Text.LastIndexOf(':') - 2);

            //Find Price
            input = WebDriver.FindElement(By.PartialLinkText(item));
            input = input.FindElement(By.CssSelector("div[class='card'] div[class='card-body'] p[class='card-text']"));
            var price = input.Text.Substring(input.Text.LastIndexOf(':') + 2, input.Text.IndexOf('€') - input.Text.LastIndexOf(':') - 2);

            //Click on Item
            input = WebDriver.FindElement(By.PartialLinkText(item));
            input.Click();

            //Validate Url
            Assert.That(WebDriver.Url, Is.EqualTo("https://localhost:7005/"));

            //Find Bill
            input = WebDriver.FindElement(By.CssSelector("div[class='col-md-4 p-4 mt-4'] div[class='card mt-2 text-center'] div[class='card-body'] p[class='card-text']"));
            var billAfter = input.Text.Substring(input.Text.LastIndexOf(':') + 2, input.Text.IndexOf('€') - input.Text.LastIndexOf(':') - 2);

            //Validate Bill
            Assert.That(double.Parse(bill.Replace(',', '.'), CultureInfo.InvariantCulture.NumberFormat) - double.Parse(price.Replace(',', '.'), CultureInfo.InvariantCulture.NumberFormat), Is.EqualTo(double.Parse(billAfter.Replace(',', '.'), CultureInfo.InvariantCulture.NumberFormat)));
        }

        [Test]
        public void AddMoneyTestSuccess()
        {
            //Navigate to room page
            WebDriver.Navigate().GoToUrl(BaseUrl + "Manage/ManagePerson");

            //Find user
            var inputUser = WebDriver.FindElement(By.XPath("//h5[text()='Braòo']/.."));

            //Find Bill
            var input = inputUser.FindElement(By.CssSelector("p[class='card-text']"));
            var bill = input.Text.Substring(input.Text.IndexOf(':') + 2, input.Text.IndexOf('€') - input.Text.IndexOf(':') - 2);

            //Click on add money
            input = inputUser.FindElement(By.CssSelector("a[data-bs-target='#addMoneyModal']"));
            input.Click();

            //Click on 5
            input = WebDriver.FindElement(By.PartialLinkText("5"));
            input.Click();

            //Validate Alert
            var inputs = WebDriver.FindElements(By.XPath("//div[text()='Successfully added money!']"));
            Assert.True(inputs.Count != 0);

            //Find user
            inputUser = WebDriver.FindElement(By.XPath("//h5[text()='Braòo']/.."));

            //Find Bill
            input = inputUser.FindElement(By.CssSelector("p[class='card-text']"));
            var billAfter = input.Text.Substring(input.Text.IndexOf(':') + 2, input.Text.IndexOf('€') - input.Text.IndexOf(':') - 2);

            //Validate Bill
            Assert.That(double.Parse(bill.Replace(',', '.'), CultureInfo.InvariantCulture.NumberFormat) + 5d, Is.EqualTo(double.Parse(billAfter.Replace(',', '.'), CultureInfo.InvariantCulture.NumberFormat)));
        }

        [Test]
        public void GoBackFromPersonToRoomTestSuccess()
        {
            //Arrange
            string room = "Programatori";

            //Navigate to room page
            WebDriver.Navigate().GoToUrl(BaseUrl + "Home/Room");

            //Click on room
            var input = WebDriver.FindElement(By.PartialLinkText(room));
            input.Click();

            //Click on room - back
            input = WebDriver.FindElement(By.PartialLinkText("Room:"));
            input.Click();

            //Validate Url
            Assert.That(WebDriver.Url, Is.EqualTo("https://localhost:7005/Home/Room"));
        }

        [Test]
        public void GoBackFromItemToRoomTestSuccess()
        {
            //Arrange
            string room = "Programatori";
            string person = "Braòo";

            //Navigate to room page
            WebDriver.Navigate().GoToUrl(BaseUrl + "Home/Room");

            //Click on room
            var input = WebDriver.FindElement(By.PartialLinkText(room));
            input.Click();

            //Click on person
            input = WebDriver.FindElement(By.PartialLinkText(person));
            input.Click();

            //Click on room - back
            input = WebDriver.FindElement(By.PartialLinkText("Room:"));
            input.Click();

            //Validate Url
            Assert.That(WebDriver.Url, Is.EqualTo("https://localhost:7005/Home/Room"));
        }

        [Test]
        public void GoBackFromItemToPersonTestSuccess()
        {
            //Arrange
            string room = "Programatori";
            string person = "Braòo";

            //Navigate to room page
            WebDriver.Navigate().GoToUrl(BaseUrl + "Home/Room");

            //Click on room
            var input = WebDriver.FindElement(By.PartialLinkText(room));
            input.Click();

            //Click on person
            input = WebDriver.FindElement(By.PartialLinkText(person));
            input.Click();

            //Click on person - back
            input = WebDriver.FindElement(By.PartialLinkText("Person:"));
            input.Click();

            //Validate Url
            Assert.That(WebDriver.Url, Is.EqualTo("https://localhost:7005/Home/PersonHard?Room=" + room));
        }


        private WebDriver GetChromeDriver()
        {
            var options = new ChromeOptions();

            return new ChromeDriver(DriverPath, options, TimeSpan.FromSeconds(300));
        }
    }
}
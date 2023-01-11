using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Globalization;
using System.Xml.Linq;

namespace BufterSeleniumTests
{
    public class ManageItemTest
    {
        private object l;
        private readonly object element;

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
        public void CreateItemTestSuccess()
        {
            //Navigate to manage page
            WebDriver.Navigate().GoToUrl(BaseUrl + "Manage/ManageItem");

            //Click on create
            var input = WebDriver.FindElement(By.CssSelector("a[data-bs-target='#createModal']"));
            input.Click();

            //Enter Name
            input = WebDriver.FindElement(By.Id("NameCreateModal"));
            input.Clear();
            input.SendKeys("testItem");

            //Select Room
            input = WebDriver.FindElement(By.Id("RoomIdCreateModal"));
            input.SendKeys("Every");

            //Enter Price
            input = WebDriver.FindElement(By.Id("PriceCreateModal"));
            input.Clear();
            input.SendKeys("5.51");

            //Click on Create button
            input = WebDriver.FindElement(By.CssSelector("form[action='/Manage/CreateItem'] button[type='submit']"));
            input.Click();

            //Validate Alert
            var inputs = WebDriver.FindElements(By.XPath("//div[text()='Successfully created!']"));
            Assert.True(inputs.Count != 0);

            //Validate
            input = WebDriver.FindElement(By.XPath("//h5[text()='testItem']"));
            Assert.IsNotNull(input);
        }

        [Order(2)]
        [Test]
        public void DeleteItemTestSuccess()
        {
            //Navigate to manage page
            WebDriver.Navigate().GoToUrl(BaseUrl + "Manage/ManageItem");

            //Find delete user
            var input = WebDriver.FindElement(By.XPath("//h5[text()='testItem']/.."));

            //Click on delete
            input = input.FindElement(By.CssSelector("a[data-bs-target='#deleteModal']"));
            input.Click();

            //Click on Delete button
            input = WebDriver.FindElement(By.CssSelector("form[action='/Manage/DeleteItem'] button[type='submit']"));
            input.Click();

            //Validate Alert
            var inputs = WebDriver.FindElements(By.XPath("//div[text()='Successfully deleted!']"));
            Assert.True(inputs.Count != 0);

            //Validate
            WebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            inputs = WebDriver.FindElements(By.XPath("//p[text()='testItem']"));
            Assert.True(inputs.Count == 0);
            WebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
        }


        private WebDriver GetChromeDriver()
        {
            var options = new ChromeOptions();

            return new ChromeDriver(DriverPath, options, TimeSpan.FromSeconds(300));
        }
    }
}
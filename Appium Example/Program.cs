using System;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace Appium_Example
{
    internal class Program
    {
        static async Task<AndroidDriver<AndroidElement>> Appium_Driver(string device_name, string os_version, string appium_port)
        {
            AppiumOptions options = new AppiumOptions
            {
                PlatformName = "Android"
            };
            options.AddAdditionalCapability("deviceName", device_name);
            options.AddAdditionalCapability("platformVersion", os_version);
            options.AddAdditionalCapability("automationName", "UIautomator2");
            options.AddAdditionalCapability("appPackage", "com.instagram.android");
            options.AddAdditionalCapability("appActivity", "com.instagram.mainactivity.MainActivity");
            options.AddAdditionalCapability("newCommandTimeout", 5000);

            var driver = new AndroidDriver<AndroidElement>(new Uri($"http://127.0.0.1:{appium_port}/wd/hub"), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            await Task.Delay(3000);

            return driver;
        }

        static async Task<bool> Login(AndroidDriver<AndroidElement> driver, string username, string password)
        {
            try
            {
                driver.FindElement(By.XPath("(//android.widget.EditText[@class=\"android.widget.EditText\"])[1]")).SendKeys(username);
                await Task.Delay(200);
                driver.FindElement(By.XPath("(//android.widget.EditText[@class=\"android.widget.EditText\"])[2]")).SendKeys(password);
                await Task.Delay(200);
                driver.FindElements(By.XPath("//android.widget.Button"))[1].Click();
                await Task.Delay(10000);

                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

                driver.FindElements(By.XPath("//android.widget.Button"))[1].Click();
                await Task.Delay(5000);

                try { var check_element = driver.FindElementById("row_feed_photo_profile_name"); }
                catch
                {
                    driver.FindElements(By.XPath("//android.widget.Button"))[1].Click(); // geç
                    await Task.Delay(3000);
                }

                return true;
            }
            catch { return false; }
        }

        static async Task Main()
        {
            Console.Title = "Appium Sample Project | @hiddenexe";
            
            string device_name = "emulator-5556", os_version = "7.1.2", port = "4725";
            string username = "test", password = "test123456";

            AndroidDriver<AndroidElement> driver = await Appium_Driver(device_name, os_version, port);
            if (await Login(driver, username, password))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[+] Giriş başarılı");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[-] Giriş başarısız");
            }

            driver.Quit();

            Console.ReadKey();
        }
    }
}

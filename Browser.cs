using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace dodo {

	public class Browser {
		private FirefoxDriver driver;
		private string name;

		private int state;

		public void startBrowser(string url, string name) {
			this.name = name;
			var options = new FirefoxOptions();
			options.SetPreference("permissions.default.microphone", 1);
			options.SetPreference("permissions.default.camera", 2);
			driver = new FirefoxDriver(options);
			driver.Navigate().GoToUrl(url);
			var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60)) {
				PollingInterval = TimeSpan.FromSeconds(5)
			};
			wait.IgnoreExceptionTypes(typeof(NoSuchElementException));

			new Thread(() => { handle(wait); }).Start();
		}

		private void handle(WebDriverWait wait) {
			try {
				if (state == 0) {
					Console.WriteLine(state);
					var foo = wait.Until(drv => drv.FindElement(By.Name("guestnameform")));
					var input = foo.FindElement(By.Id("guest-name"));
					input.SendKeys(name);
					var btn = driver.FindElement(By.Id("launch-html-guest"));
					btn.Click();
					state++;
				}

				if (state == 1) {
					var micTuto = wait.Until(drv => drv.FindElement(By.ClassName("techcheck-controls")))
						.FindElement(By.TagName("button"));

					if (!micTuto.Enabled)
						micTuto = driver.FindElement(By.ClassName("techcheck-audio-skip"))
							.FindElement(By.TagName("button"));

					micTuto.Click();
					state++;
				}

				if (state == 2) {
					var camTuto = wait.Until(drv => drv.FindElement(By.Id("techcheck-video-ok-button")));
					camTuto.Click();
					state++;
				}

				if (state == 3) {
					var tuto = wait.Until(drv => drv.FindElement(By.Id("announcement-modal-page-wrap")))
						.FindElement(By.ClassName("close"));
					tuto.Click();
					state++;
				}


			} catch (Exception e) {
				Console.WriteLine(e);
				driver.Navigate().Refresh();
				handle(wait);
			}

		}


		public void stop() {
			driver.Close();
		}
	}

}
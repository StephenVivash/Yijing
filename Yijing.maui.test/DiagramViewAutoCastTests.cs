using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using Xunit;
using Xunit.Sdk;

namespace Yijing.Maui.Test;

public sealed class DiagramViewAutoCastTests : IAsyncLifetime
{
	private readonly string? _skipReason;
	private readonly Uri? _serverUri;
	private readonly AppiumOptions? _options;
	private readonly string? _platformName;
	private AppiumDriver? _driver;

	public DiagramViewAutoCastTests()
	{
		var serverUrl = Environment.GetEnvironmentVariable("YIJING_APPIUM_SERVER_URL");
		var platformName = Environment.GetEnvironmentVariable("YIJING_APPIUM_PLATFORM");

		if (string.IsNullOrWhiteSpace(serverUrl) || string.IsNullOrWhiteSpace(platformName))
		{
			_skipReason = "Set YIJING_APPIUM_SERVER_URL and YIJING_APPIUM_PLATFORM to run Appium UI tests.";
			return;
		}

		_serverUri = new Uri(serverUrl);
		_platformName = platformName.ToLowerInvariant();
		_options = new AppiumOptions
		{
			PlatformName = platformName,
		};

		var automationName = Environment.GetEnvironmentVariable("YIJING_APPIUM_AUTOMATION_NAME");
		if (string.IsNullOrWhiteSpace(automationName))
		{
			automationName = _platformName switch
			{
				"android" => "uiautomator2",
				"ios" => "XCUITest",
				"windows" => "Windows",
				_ => null,
			};
		}

		if (!string.IsNullOrWhiteSpace(automationName))
		{
			_options.AddAdditionalAppiumOption("automationName", automationName);
		}

		foreach (var (capability, variable) in new (string capability, string variable)[]
		{
			("app", "YIJING_APPIUM_APP_ID"),
			("appPackage", "YIJING_APPIUM_APP_PACKAGE"),
			("appActivity", "YIJING_APPIUM_APP_ACTIVITY"),
			("deviceName", "YIJING_APPIUM_DEVICE_NAME"),
			("platformVersion", "YIJING_APPIUM_PLATFORM_VERSION"),
		})
		{
			var value = Environment.GetEnvironmentVariable(variable);
			if (!string.IsNullOrWhiteSpace(value))
			{
				_options.AddAdditionalAppiumOption(capability, value);
			}
		}
	}

	public Task InitializeAsync()
	{
		if (_skipReason is not null)
		{
			return Task.CompletedTask;
		}

		_driver = _platformName switch
		{
			"android" => new AndroidDriver(_serverUri!, _options!, TimeSpan.FromSeconds(180)),
			"ios" => new IOSDriver(_serverUri!, _options!, TimeSpan.FromSeconds(180)),
			"windows" => new WindowsDriver(_serverUri!, _options!, TimeSpan.FromSeconds(180)),
			_ => throw new NotSupportedException($"Unsupported platform '{_platformName}'."),
		};

		_driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
		return Task.CompletedTask;
	}

	public Task DisposeAsync()
	{
		_driver?.Quit();
		_driver?.Dispose();
		_driver = null;
		return Task.CompletedTask;
	}

	[Fact(DisplayName = "AutoCast")]
	public void AutoCast()
	{
		if (_skipReason is not null)
		{
			//throw new SkipException(_skipReason);
			throw SkipException.ForSkip(_skipReason);
		}

		if (_driver is null)
		{
			throw new InvalidOperationException("Appium driver was not initialised.");
		}

		var driver = _driver!;
		var wait = new WebDriverWait(new SystemClock(), driver, TimeSpan.FromSeconds(60), TimeSpan.FromMilliseconds(500));

		var diagramModePicker = wait.Until(_ =>
			FindElement(driver, new[]
			{
				MobileBy.AccessibilityId("picDiagramMode"),
				MobileBy.Id("picDiagramMode"),
				By.XPath("//*[contains(@text,'Diagram Mode')]/following::*[1]")
			}) ?? throw new NoSuchElementException("Diagram mode picker not found."));

		diagramModePicker.Click();

		var autoCastOption = wait.Until(_ =>
			FindElement(driver, new[]
			{
				MobileBy.AccessibilityId("Auto Cast"),
				MobileBy.Name("Auto Cast"),
				By.XPath("//*[contains(@text,'Auto Cast') or contains(@name,'Auto Cast')]")
			}) ?? throw new NoSuchElementException("Unable to locate the Auto Cast option."));

		autoCastOption.Click();

		wait.Until(_ =>
		{
			var selected = GetElementText(diagramModePicker);
			return selected?.Contains("Auto Cast", StringComparison.OrdinalIgnoreCase) == true;
		});

		var selectedText = GetElementText(diagramModePicker);
		Assert.Contains("Auto Cast", selectedText, StringComparison.OrdinalIgnoreCase);
	}

	private static AppiumElement? FindElement(AppiumDriver driver, IEnumerable<By> selectors)
	{
		foreach (var selector in selectors)
		{
			try
			{
				var element = driver.FindElement(selector);
				if (element is not null)
				{
					return element;
				}
			}
			catch (NoSuchElementException)
			{
				// Try the next selector.
			}
		}

		return null;
	}

	private static string? GetElementText(AppiumElement element)
	{
		var text = element.Text;
		if (!string.IsNullOrWhiteSpace(text))
		{
			return text;
		}

		var value = element.GetAttribute("value");
		if (!string.IsNullOrWhiteSpace(value))
		{
			return value;
		}

		var name = element.GetAttribute("Name");
		if (!string.IsNullOrWhiteSpace(name))
		{
			return name;
		}

		return null;
	}

	ValueTask IAsyncLifetime.InitializeAsync()
	{
		throw new NotImplementedException();
	}

	ValueTask IAsyncDisposable.DisposeAsync()
	{
		throw new NotImplementedException();
	}
}

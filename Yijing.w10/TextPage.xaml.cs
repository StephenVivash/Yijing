using System;
using System.Text.RegularExpressions;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using ValueSequencer;

namespace Yijing
{
	public sealed partial class TextPage : Page
	{
		public static TextPage m_tp;
		private String m_strCurrentText;

		public TextPage()
		{
			m_tp = this;
			this.InitializeComponent();
			NavigationCacheMode = NavigationCacheMode.Enabled;
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			SetTextUrl(DiagramPage.m_hvsCurrent,0);
		}

		private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
		{

		}
		
		private void btnFirst_Click(object sender, RoutedEventArgs e)
		{
			DiagramPage.SetFirst();
			SetTextUrl(DiagramPage.m_hvsCurrent, 0);
		}

		private void btnPrevious_Click(object sender, RoutedEventArgs e)
		{
			DiagramPage.SetPrevious();
			SetTextUrl(DiagramPage.m_hvsCurrent, 0);
		}

		private void btnNext_Click(object sender, RoutedEventArgs e)
		{
			DiagramPage.SetNext();
			SetTextUrl(DiagramPage.m_hvsCurrent, 0);
		}

		private void btnLast_Click(object sender, RoutedEventArgs e)
		{
			DiagramPage.SetLast();
			SetTextUrl(DiagramPage.m_hvsCurrent, 0);
		}

		private void btnHome_Click(object sender, RoutedEventArgs e)
		{
			DiagramPage.SetHome();
			SetTextUrl((CHexagramValueSequencer) DiagramPage.m_hvsCurrent, 0);
		}

		private async void webText_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
		{
			if (args.Uri != null)
				if (args.Uri.AbsolutePath.StartsWith("Hexagram"))
				{
					DiagramPage.SetHexagramValue(Convert.ToInt32(args.Uri.AbsolutePath.Substring(8)));
					SetTextUrl(DiagramPage.m_hvsCurrent, 0);
				}
				else
				if (args.Uri.AbsolutePath.StartsWith("Trigram"))
				{
					CTrigramValueSequencer tvs = new CTrigramValueSequencer(0);
					tvs.Value = Convert.ToInt32(args.Uri.AbsolutePath.Substring(7));
					webText.Navigate(new Uri("https://en.wikipedia.org/wiki/" + tvs.Label));
				}
				else
				if (args.Uri.AbsoluteUri == "http://hermetica.info/")
				{
					args.Cancel = true;
					await Launcher.LaunchUriAsync(new Uri("http://hermetica.info/"));
				}
		}

		public void SetTextUrl(CHexagramValueSequencer hvsPrimary, int nHexagramLabel)
		{
			String strText = Sequences.strDiagramSettings[16, Sequences.HexagramText + 1];

			String strUrl = "<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>Yijing</title></head><body>" +
				"<h2>" + Sequences.strSymbols[hvsPrimary.Value] + "  ";
/*
			strUrl +=
			"Hatcher: " + Sequences.strHexagramLabels[2, hvsPrimary.Value] + "</br>" +
			"Heyboer: " + Sequences.strHexagramLabels[3, hvsPrimary.Value] + "</br>" +
			"Karcher: " + Sequences.strHexagramLabels[4, hvsPrimary.Value] + "</br>" +
			"Legge: " + Sequences.strHexagramLabels[5, hvsPrimary.Value] + "</br>" +
			"Machovec: " + Sequences.strHexagramLabels[6, hvsPrimary.Value] + "</br>" +
			"Marshall: " + Sequences.strHexagramLabels[7, hvsPrimary.Value] + "</br>" +
			"Rutt: " + Sequences.strHexagramLabels[8, hvsPrimary.Value] + "</br>" +
			"Wilhelm: " + Sequences.strHexagramLabels[10, hvsPrimary.Value] + "</br>";
*/
			strUrl += DiagramPage.DescribeCast();
			strUrl += "</h2>\n";

			if (strText == "Andrade")
			{
				strUrl += "<center><img src=\"" + Andrade.strText[hvsPrimary.Value] + "\" alt=\"" +
				hvsPrimary.Label + "\"></center>" +
				"</body></html>";
				webText.NavigateToString(strUrl);
			}
			else
			if (strText == "Chinese")
			{
				strUrl += Chinese.strText[hvsPrimary.Value] +
				"</body></html>";
				webText.NavigateToString(strUrl);
			}
			else
			if (strText == "Legge")
			{
				strUrl += Legge.strText[hvsPrimary.Value];
				for (int nLine = 0; nLine < 6; ++nLine)
				{
					strUrl += "<h2>Line " + (nLine + 1).ToString() + "</h2>" +
					Legge.strLine[nLine,hvsPrimary.Value];
				}
				strUrl += "</body></html>";
				webText.NavigateToString(strUrl);
			}
			else
			if ((strText == "Hatcher")/* && (m_strCurrentText != "Hatcher")*/)
			{
				strUrl += "You can download Bradford Hatcher's Yijing translation as a zipped PDF from ";
				strUrl += "<a href=\"http://hermetica.info/\">Hermetica.info</a>"; // /Yijing-One.zip
				strUrl += "<br/><br/>You can also purchase a hardcopy of the book from the site";
				strUrl += "<br/><br/>This application can not currently open the document to the selected hexagram";
				strUrl += "<br/><br/>You can view the document in a <a href=\"ms-windows-store://pdp/?productid=9wzdncrfj2gc\">PDF Reader</a> ";
				strUrl += "</body></html>";
				webText.NavigateToString(strUrl);
/*
				try
				{
					LaunchUriResult b;
					//Windows.ApplicationModel.Package.Current.Launch("");

					LauncherOptions options = new Windows.System.LauncherOptions();
					options.DisplayApplicationPicker = true;

					StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
					StorageFile file = await folder.GetFileAsync("Yijing-One.pdf");
					//await Launcher.LaunchFileAsync(file, options); // - "/A \"zoom=1000\"" page=pagenum search=wordList

					Uri uri = new Uri(folder.Path + "/Yijing-One.pdf");
					//Uri uri = new Uri("http://google.com");
					//b  = await Launcher.LaunchUriForResultsAsync(uri, options);

					//webText.Navigate(uri);
				}
				catch (Exception e) {
					String s = e.Message;
				}
*/
			}
			else
			if (strText == "Heyboer")
			{
				strUrl = Heyboer.strText[hvsPrimary.Value];
				webText.Navigate(new Uri(strUrl));
			}
			else
			if (strText == "Wilhelm")
			{
				strUrl += Wilhelm.strText[hvsPrimary.Value] +
					"<h2>The Image</h2>" +
					Wilhelm.strImage[hvsPrimary.Value] +
					"<h2>The Judgement</h2>" +
					Wilhelm.strJudgement[hvsPrimary.Value];
				for (int nLine = 0; nLine < 6; ++nLine)
				{
					strUrl += "<h2>Line " + (nLine + 1).ToString() + "</h2>" +
					Wilhelm.strLine[nLine,hvsPrimary.Value];
				}
				strUrl += "</body></html>";
				for (int i = 0; i < 64; ++i)
					if (i != hvsPrimary.Value)
					{
						String strHref = "<a href=\"Hexagram" + i + "\">" + Sequences.strHexagramLabels[9,i] + "</a>";
						Regex rgx = new Regex("\\b(?i)" + Sequences.strHexagramLabels[9, i] +
							"(s)?(t)?(y)?(ty)?(ing)?(ed)?(ous)?(ment)?(ate)?(in)?\\b");
						strUrl = rgx.Replace(strUrl, strHref + "$1$2$3$4$5$6$7$8$9$10");
					}
				/*
				for (int i = 0; i < 8; ++i)
				{
					String strHref = "<a href=\"Trigram" + i + "\">" + Sequences.strTrigramLabels[2,i] + "</a>";
					Regex rgx = new Regex("\\b(?i)" + Sequences.strTrigramLabels[2, i] +
						"(s)?(t)?(y)?(ty)?(ing)?(ed)?(ous)?(ment)?(ate)?(in)?\\b");
					strUrl = rgx.Replace(strUrl, strHref + "$1$2$3$4$5$6$7$8$9$10");
				}
				*/
				webText.NavigateToString(strUrl);
			}
			else
			if (strText == "YellowBridge")
			{
				strUrl = YellowBridge.strText[hvsPrimary.Value];
				webText.Navigate(new Uri(strUrl));
			}
			else
			if (strText == "Regis")
			{
				strUrl = Regis.strText[hvsPrimary.Value];
				webText.Navigate(new Uri(strUrl));
			}
			m_strCurrentText = strText;

			//webText.setBackground(display.getSystemColor(SWT.COLOR_BLUE));
			//strUrl = webText.getUrl(); 
			//webText.traverse(SWT.TRAVERSE_PAGE_NEXT);
			// C:/Src/aframe-boilerplate-master/index.html 
			// C:/Src/IChing.308/HomePage/index.html 
		}

		public static void EnableSettings(bool bEnable)
		{
			m_tp.btnFirst.IsEnabled = bEnable;
			m_tp.btnPrevious.IsEnabled = bEnable;
			m_tp.btnNext.IsEnabled = bEnable;
			m_tp.btnLast.IsEnabled = bEnable;
			m_tp.btnHome.IsEnabled = bEnable;
		}
	}
}

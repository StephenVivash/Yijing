package org.yijing;

import android.content.Intent;
import android.graphics.Bitmap;
import android.net.Uri;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.view.Menu;
import android.view.MenuItem;
import android.webkit.WebResourceRequest;
import android.webkit.WebResourceResponse;
import android.webkit.WebView;
import android.webkit.WebViewClient;

public class TextActivity extends AppCompatActivity {

	private WebView m_webText;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_text);

		m_webText = (WebView) findViewById(R.id.webText);
		m_webText.setWebViewClient(new TextWebViewClient());
		//m_webText.getSettings().setJavaScriptEnabled(true);
	}

	@Override
	protected void onPostResume() {
		super.onPostResume();
	}

	@Override
	protected void onStart() {
		super.onStart();
		SetTextUrl(DiagramCanvas.m_hvsCurrent);
	}

	@Override
	protected void onStop() {
		super.onStop();
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		getMenuInflater().inflate(R.menu.diagram, menu);
		menu.findItem(R.id.action_text).setEnabled(false);
		return true;
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
		YijingActivity.startMenuActivity(item);
		return super.onOptionsItemSelected(item);
	}

	public void SetTextUrl(final CHexagramValueSequencer hvsPrimary) {
		String strText = Sequences.DiagramSetting("Hexagram Text",DiagramCanvas.m_nHexagramText);
		String strUrl = "<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>Yijing</title></head><body>" +
				"<h2>" + // Sequences.strSymbols[hvsPrimary.Value()] + "  " +
/*
			"Hatcher: " + Sequences.strHexagramLabels[DiagramSetting(15,"Hatcher")][hvsPrimary.Value()] + "</br>" +
			"Heyboer: " + Sequences.strHexagramLabels[DiagramSetting(15,"Heyboer")][hvsPrimary.Value()] + "</br>" +
			"Karcher: " + Sequences.strHexagramLabels[DiagramSetting(15,"Karcher")][hvsPrimary.Value()] + "</br>" +
			"Legge: " + Sequences.strHexagramLabels[DiagramSetting(15,"Legge")][hvsPrimary.Value()] + "</br>" +
			"Machovec: " +Sequences.strHexagramLabels[DiagramSetting(15,"Machovec")][hvsPrimary.Value()] + "</br>" +
			"Marshall: " + Sequences.strHexagramLabels[DiagramSetting(15,"Marshall")][hvsPrimary.Value()] + "</br>" +
			"Rutt: " + Sequences.strHexagramLabels[DiagramSetting(15,"Rutt")][hvsPrimary.Value()] + "</br>" +
			"Wilhelm: " + Sequences.strHexagramLabels[DiagramSetting(15,"Wilhelm")][hvsPrimary.Value()] + "</br>" +
*/
				DiagramCanvas.DescribeCast() + "</h2>\n";

		if (strText.equals("Andrade")) {
			strUrl += "<center><img src=\"" + Andrade.strText[hvsPrimary.Value()] + "\" alt=\"" +
					hvsPrimary.Label() + "\"></center>" +
					"</body></html>";
			m_webText.loadData(strUrl, "text/html", "UTF-8");
		}
		else
		if (strText.equals("Chinese")) {
			//strUrl += Chinese.strText[hvsPrimary.Value()] +
			strUrl += "Sorry, this text source is currently unavailable" +
					"</body></html>";
			m_webText.loadData(strUrl, "text/html", "UTF-8");
		}
		else
		if (strText.equals("Legge")) {
			strUrl += Legge.strText[hvsPrimary.Value()];
			for(int nLine = 0; nLine < 6; ++nLine) {
				strUrl += "<h2>Line " + Integer.toString(nLine + 1) + "</h2>" +
						Legge.strLine[nLine][hvsPrimary.Value()];
			}
			strUrl += "</body></html>";
			m_webText.loadData(strUrl, "text/html", "UTF-8");
		}
		else
		if (strText.equals("Hatcher")) {
			//strUrl = "C:/Src/Yijing/Text/Hatcher/Yijing-One.pdf#page=" + Sequences.nHatcherPage[hvsPrimary.Sequence()];
			//m_webText.dispose();
			//NewBrowser();
			m_webText.loadUrl(strUrl);

			strUrl += "You can download Bradford Hatcher's Yijing translation as a zipped PDF from ";
			strUrl += "<a href=\"http://hermetica.info/\">Hermetica.info</a>";
			strUrl += "<br/><br/>You can also purchase a hardcopy of the book from the site";
			strUrl += "<br/><br/>This application can not currently open the document to the selected hexagram";
			strUrl += "<br/><br/>You can view the document in a <a href=\"https://play.google.com/store/apps/details?id=com.adobe.reader\">PDF Reader</a> "; // https://play.google.com/store/apps/details?id=org.yijing
			strUrl += "</body></html>";
			m_webText.loadData(strUrl, "text/html", "UTF-8");
		}
		else
		if (strText.equals("Heyboer")) {
			strUrl = Heyboer.strText[hvsPrimary.Value()];
			m_webText.loadUrl(strUrl);
		}
		else
		if (strText.equals("Wilhelm")) {
			strUrl += Wilhelm.strText[hvsPrimary.Value()] +
					"<h2>The Image</h2>" +
					Wilhelm.strImage[hvsPrimary.Value()] +
					"<h2>The Judgement</h2>" +
					Wilhelm.strJudgement[hvsPrimary.Value()];
			for(int nLine = 0; nLine < 6; ++nLine) {
				strUrl += "<h2>Line " + Integer.toString(nLine + 1) + "</h2>" +
						Wilhelm.strLine[nLine][hvsPrimary.Value()];
			}
			strUrl += "</body></html>";
			for (int i = 0; i < 64; ++i)
				if (i != hvsPrimary.Value()) {
					String strHref = "<a href=\"yijing://hexagram" + i + ".com\">" + Sequences.strHexagramLabels[9][i] + "</a>";
					strUrl = strUrl.replaceAll("\\b(?i)" + Sequences.strHexagramLabels[9][i] + "\\b", strHref);
					//strTemp = strTemp.replaceAll("\\b(?i)" + Sequences.strHexagramLabels[9][i] +
					//		"(s)?(t)?(y)?(ty)?(ing)?(ed)?(ous)?(ment)?(ate)?(in)?\\b", strHref + "$1$2$3$4$5$6$7$8$9$10");
				}
			for (int i = 0; i < 8; ++i) {
				String strHref = "<a href=\"yijing://trigram" + i + ".com\">" + Sequences.strTrigramLabels[2][i] + "</a>";
				strUrl = strUrl.replaceAll("\\b(?i)" + Sequences.strTrigramLabels[2][i] + "\\b", strHref);
				//strTemp = strTemp.replaceAll("\\b(?i)" + Sequences.strTrigramLabels[2][i] +
				//		"(s)?(t)?(y)?(ty)?(ing)?(ed)?(ous)?(ment)?(ate)?(in)?\\b", strHref + "$1$2$3$4$5$6$7$8$9$10");
			}
			m_webText.loadData(strUrl, "text/html", "UTF-8");
		}
		else
		if (strText.equals("YellowBridge")) {
			strUrl = YellowBridge.strText[hvsPrimary.Value()];
			m_webText.loadUrl(strUrl);
		}
		else
		if (strText.equals("Regis")) {
			strUrl = Regis.strText[hvsPrimary.Value()];
			m_webText.loadUrl(strUrl);
		}
	}

	private class TextWebViewClient extends WebViewClient {

		TextWebViewClient() {
		}

		public boolean shouldOverrideUrlLoading(WebView view, WebResourceRequest request) {
			String strUrl = request.getUrl().toString();
			if (strUrl.startsWith("yijing://hexagram")) {
				String[] strTemp = strUrl.split("yijing://hexagram");
				strTemp = strTemp[1].split(".com");
				DiagramCanvas.SetHexagramValue(Integer.valueOf(strTemp[0]));
				SetTextUrl(DiagramCanvas.m_hvsCurrent);
				return true;
			}
			else
			if (strUrl.startsWith("yijing://trigram")) {
				String[] strTemp = strUrl.split("yijing://trigram");
				strTemp = strTemp[1].split(".com");
				CTrigramValueSequencer tvs = new CTrigramValueSequencer(0);
				tvs.Value(Integer.valueOf(strTemp[0]));
				m_webText.loadUrl("https://en.wikipedia.org/wiki/" + tvs.Label());
				return true;
			}
			else
			if (strUrl.startsWith("http://hermetica.info/")) {
				startActivity(new Intent(Intent.ACTION_VIEW, Uri.parse(strUrl)));
				return false;
			}
			else
			if (strUrl.startsWith("https://play.google.com/")) {
				startActivity(new Intent(Intent.ACTION_VIEW, Uri.parse(strUrl)));
				return false;
			}
			return false;
		}

		public void onPageStarted(WebView view, String url, Bitmap favicon) {
		}

		public void onLoadResource(WebView view, String url) {
		}

		public WebResourceResponse shouldInterceptRequest(WebView view, WebResourceRequest request) {
			return super.shouldInterceptRequest(view,request);
		}
	}
}

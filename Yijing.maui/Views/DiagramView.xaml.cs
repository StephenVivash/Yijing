
/************************************************************************************************
*************************************************************************************************
*************************************************************************************************
*************************************************************************************************
*************************************************************************************************
*************************************************************************************************
*************************************************************************************************
*************************************************************************************************
*************************************************************************************************
*************************************************************************************************

Generating an AppStoreConnect API Key
If you've enrolled in Apple's AppStoreConnect Program, you'll need to:

Sign-in to your Apple Developer Account.
Select Users and Access.
Select the Integrations tab.
Select the Team Keys tab.
Press the + button.
Enter a descriptive name in the Name field.
Enter Admin in the Access field.
Press OK.

*************************************************************************************************
*************************************************************************************************
*************************************************************************************************
*************************************************************************************************
*************************************************************************************************
*************************************************************************************************
*************************************************************************************************
*************************************************************************************************
*************************************************************************************************
*************************************************************************************************/

//using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Controls.Shapes;
using System.Text.RegularExpressions;

#if WINDOWS
using Windows.Win32;
using Windows.Win32.Foundation;
#endif

using ValueSequencer;
//using YijingDb;

using Yijing.Pages;
using Yijing.Services;

namespace Yijing.Views;

//public record Dimension(int Width, int Height, int Padding, int FontSize);

// //////////////////////////////////////////////////////////////////////////////////////////////////
// //////////////////////////////////////////////////////////////////////////////////////////////////
// //////////////////////////////////////////////////////////////////////////////////////////////////

public partial class DiagramView : ContentView
{
	private Random m_rand = new Random(DateTime.Now.Millisecond);

	private CHexagramValueSequencer m_hvsCurrent;
	private CHexagramValueSequencer m_hvsCast;
	private CHexagramValueSequencer m_hvsPrimary = null;
	private CValueSequencer m_vsCurrent;

	private int[] m_nSpeeds = { 2000, 600, 5 };

	private int m_nDiagramMode = (int)eDiagramMode.eExplore;
	private int m_nDiagramType = (int)eDiagramType.eHexagram;
	private int m_nDiagramColor = (int)eDiagramColor.eTrigram;
	private int m_nDiagramSpeed = (int)eDiagramSpeed.eMedium;

	private bool m_bTimerOn = false;
	private bool m_bForward = true;

	private int m_nTouchLine = 0;
	private int m_nCurrentLine = 0;
	private int m_nCurrentTrigram = 0;

	private int m_nTriggerSpeed = 1;

	private int m_nDiagramHeight;
	private int m_nDiagramWidth;
	private int m_nDiagramPadding;
	//private int m_nDiagramFontSize;

	private RoundRectangle[,] m_recLine = new RoundRectangle[6, 2];
	private Ellipse[] m_ellLine = new Ellipse[6];

	private Timer m_timDiagram = new Timer(DiagramTimer, null, Timeout.Infinite, 0);

	//private Task m_tskAutoCast = null;

	private SolidColorBrush m_brMonoColor;
	private SolidColorBrush m_brMovingYang;
	private SolidColorBrush m_brSelectStoke;
	private SolidColorBrush m_brBackStoke;
	private SolidColorBrush m_brDarkGray = new SolidColorBrush(Color.FromRgba(0x40, 0x40, 0x40, 0xFF));
	private SolidColorBrush m_brLightGray = new SolidColorBrush(Colors.LightGray);

	public DiagramView()
	{
		var b = new RegisterInViewDirectoryBehavior(); // { Key = "DiagramView1" };
		Behaviors.Add(b);
		InitializeComponent();
		BindingContext = this;

		m_brMonoColor = App.Current.RequestedTheme == AppTheme.Dark ? Brush.LightGray : Brush.DarkGray;
		m_brMovingYang = App.Current.RequestedTheme == AppTheme.Dark ? Brush.Black : Brush.White;
		// Brush.DarkSlateGray /*new SolidColorBrush(Color.FromArgb("#272727"))*/ (SolidColorBrush) Background
		m_brSelectStoke = Brush.DeepPink;// App.Current.RequestedTheme == AppTheme.Dark ? Brush.DarkGray : Brush.LightGray;
		m_brBackStoke = App.Current.RequestedTheme == AppTheme.Dark ? Brush.White : Brush.Black;

		Sequences.Initialise();
		m_hvsCurrent = new CHexagramValueSequencer(1);

		//AppPreferences.Save();

		SetLineRatio(1); // 0 - 5 "Equal", "Coin", "Yarrow", "Marbles", "Yin", "Yang"
		SetDiagramLsb(AppPreferences.DiagramLsb);

		m_hvsCurrent.First();
		m_vsCurrent = m_hvsCurrent;

		m_recLine[5, 0] = lin5L;
		m_recLine[5, 1] = lin5R;
		m_ellLine[5] = ell5;

		m_recLine[4, 0] = lin4L;
		m_recLine[4, 1] = lin4R;
		m_ellLine[4] = ell4;

		m_recLine[3, 0] = lin3L;
		m_recLine[3, 1] = lin3R;
		m_ellLine[3] = ell3;

		m_recLine[2, 0] = lin2L;
		m_recLine[2, 1] = lin2R;
		m_ellLine[2] = ell2;

		m_recLine[1, 0] = lin1L;
		m_recLine[1, 1] = lin1R;
		m_ellLine[1] = ell1;

		m_recLine[0, 0] = lin0L;
		m_recLine[0, 1] = lin0R;
		m_ellLine[0] = ell0;

		SetAppTitle("Yijing - The classic of change");
		LoadDiagramSettings();

		picDiagramMode.SelectedIndex = (int)eDiagramMode.eExplore;
		picDiagramType.SelectedIndex = (int)eDiagramType.eHexagram;
		picDiagramSpeed.SelectedIndex = (int)eDiagramSpeed.eMedium;
		picDiagramColor.SelectedIndex = (int)eDiagramColor.eTrigram;
		picDiagramLsb.SelectedIndex = 0;

		//YijingDB();
		UpdateText();
	}

	protected void Page_Loaded(object sender, EventArgs e)
	{
		//UpdateText();

#if WINDOWS
		HWND hWnd = PInvoke.GetActiveWindow();
		hWnd = PInvoke.GetDesktopWindow();
		//PInvoke.SetWindowText(hWnd, "Yijing");
		//string b = Kernel32Extensions.GetMessage(Win32ErrorCode.ERROR_CREATE_FAILED);
#endif
		/*
		#if WINDOWS || MACCATALYST
				btnMove.IsVisible = false;
				btnHome.IsVisible = false;
				btnFirst.IsVisible = false;
				btnLast.IsVisible = false;
				btnInverse.IsVisible = false;
				btnOpposite.IsVisible = false;
				btnTransverse.IsVisible = false;
				btnNuclear.IsVisible = false;
		#endif
		*/
	}

	protected override void OnSizeAllocated(double width, double height)
	{
		if ((width == -1) || (height == -1))
			return;

		double w = width - 10;
		ResizeDiagram(w, height);

		//w = width - 40;
		lblHexagram.WidthRequest = w;

		w /= 2;
		lblDiagramMode.WidthRequest = w;
		lblDiagramType.WidthRequest = w;
		lblDiagramSpeed.WidthRequest = w;
		lblDiagramColor.WidthRequest = w;
		lblDiagramLsb.WidthRequest = w;

		lblHexagramText.WidthRequest = w;
		lblHexagramLabel.WidthRequest = w;
		lblHexagramSequence.WidthRequest = w;
		lblHexagramRatio.WidthRequest = w;
		lblTrigramText.WidthRequest = w;
		lblTrigramLabel.WidthRequest = w;
		lblTrigramSequence.WidthRequest = w;
		lblTrigramRatio.WidthRequest = w;
		lblLineText.WidthRequest = w;
		lblLineLabel.WidthRequest = w;
		lblLineSequence.WidthRequest = w;
		lblLineRatio.WidthRequest = w;

		picDiagramMode.WidthRequest = w;
		picDiagramType.WidthRequest = w;
		picDiagramSpeed.WidthRequest = w;
		picDiagramColor.WidthRequest = w;
		picDiagramLsb.WidthRequest = w;

		picHexagramText.WidthRequest = w;
		picHexagramLabel.WidthRequest = w;
		picHexagramSequence.WidthRequest = w;
		picHexagramRatio.WidthRequest = w;
		picTrigramText.WidthRequest = w;
		picTrigramLabel.WidthRequest = w;
		picTrigramSequence.WidthRequest = w;
		picTrigramRatio.WidthRequest = w;
		picLineText.WidthRequest = w;
		picLineLabel.WidthRequest = w;
		picLineSequence.WidthRequest = w;
		picLineRatio.WidthRequest = w;

		w -= 10;

		w /= 2;
		btnMove.WidthRequest = w;
		btnHome.WidthRequest = w;
		btnFirst.WidthRequest = w;
		btnLast.WidthRequest = w;
		btnInverse.WidthRequest = w;
		btnOpposite.WidthRequest = w;
		btnTransverse.WidthRequest = w;
		btnNuclear.WidthRequest = w;

		base.OnSizeAllocated(width, height);
	}
	/*
	protected void lin_PointerEntered(object sender, PointerEventArgs e)
	{
	}

	protected void lin_PointerExited(object sender, PointerEventArgs e)
	{
	}

	protected void lin_PointerMoved(object sender, PointerEventArgs e)
	{
	}
	*/
	protected void lin_Tapped(object sender, TappedEventArgs e)
	{
		string s = ((RoundRectangle)sender).StyleId;
		Point? p = e.GetPosition((View)sender);
		if ((s[4] == 'L') && (p?.X < m_nDiagramWidth))
			m_bForward = false;
		else
			m_bForward = true;
		if ((m_nDiagramMode == (int)eDiagramMode.eExplore) ||
			(m_nDiagramMode == (int)eDiagramMode.eAnimate) ||
			(m_nDiagramMode == (int)eDiagramMode.eTouchCast))
		{
			SetCurrentLine(int.Parse(s.Substring(3, 1)), false);
			picDiagramType_ValueChanged(null, null);
			if (m_nDiagramMode == (int)eDiagramMode.eTouchCast)
			{
				if (m_nTouchLine == m_nCurrentLine)
				{
					m_bTimerOn = false;
					m_timDiagram.Change(Timeout.Infinite, 0);
					if (m_nCurrentLine == 5)
						EndCast();
				}
				else
				{
					m_bTimerOn = true;
					m_timDiagram.Change(0, m_nSpeeds[m_nDiagramSpeed]);
				}
				m_nTouchLine = m_nCurrentLine;
			}
			else
			{
				if (m_bForward)
					m_vsCurrent.Next();
				else
					m_vsCurrent.Previous();
				if (m_nDiagramMode == (int)eDiagramMode.eExplore)
					UpdateText();
			}
			UpdateDiagram(false);
		}
	}

	protected void btnFirst_Clicked(object sender, EventArgs e)
	{
		//System.Diagnostics.Debug.WriteLine(ViewDirectory.DebugDump());
		SetFirst();
		//Test6502();
	}

	protected void btnLast_Clicked(object sender, EventArgs e)
	{
		SetLast();
	}

	protected void btnMove_Clicked(object sender, EventArgs e)
	{
		SetMove();
	}

	protected void btnHome_Clicked(object sender, EventArgs e)
	{
		SetHome();
	}

	protected void btnInverse_Clicked(object sender, EventArgs e)
	{
		SetInverse();
	}

	protected void btnOpposite_Clicked(object sender, EventArgs e)
	{
		SetOpposite();
	}

	protected void btnTransverse_Clicked(object sender, EventArgs e)
	{
		SetTransverse();
	}

	protected void btnNuclear_Clicked(object sender, EventArgs e)
	{
		SetNuclear();
	}

	protected void picSession_SelectedIndexChanged(object sender, EventArgs e)
	{
		/*
		if (picSession.SelectedIndex == -1)
			return;

		if (picSession.SelectedIndex == 0)
		{
			DateTime start = DateTime.Now;
			StartChat();
			EegView._strSession = AppSettings.ReverseDateString();
			LoadChat("", _ai._contextSessions);
			UpdateText(false);
		}
		else
			LoadChat((string)picSession.SelectedItem, _ai._contextSessions);
		*/
	}

	protected void btnDeleteSession_Clicked(object sender, EventArgs e)
	{

		//var x1 = Window.Parent as Yijing.App;
		//x1.Test1();
		//var x2 = Window.Page.GetParentWindow();
		/*
		if (picSession.SelectedIndex > 0)
		{
			bool delete = await Window.Page.DisplayAlert("Delete Session",
				"Are you sure you want to delete session " +
				(string)picSession.SelectedItem + " ?", "Yes", "No");
			if (delete)
			{
				string s = (string)picSession.SelectedItem;
				File.Delete(System.IO.Path.Combine(AppSettings.DocumentHome(), "Questions", s + ".txt"));
				File.Delete(System.IO.Path.Combine(AppSettings.DocumentHome(), "Answers", s + ".txt"));
				LoadSessions(0);
			}
		}
		*/
	}

	protected void picDiagramMode_ValueChanged(object sender, EventArgs e)
	{
		m_nDiagramMode = (int)picDiagramMode.SelectedIndex;
		AppPreferences.DiagramMode = m_nDiagramMode;
		if (m_bTimerOn)
		{
			m_timDiagram.Change(Timeout.Infinite, 0);
			UpdateText();
		}
		m_bTimerOn = false;
		if (m_nDiagramMode == (int)eDiagramMode.eExplore)
			EnableDiagramControls(true, false);
		else
		if (m_nDiagramMode == (int)eDiagramMode.eAnimate)
		{
			EnableDiagramControls(true, false);
			m_timDiagram.Change(0, m_nSpeeds[m_nDiagramSpeed]);
			m_bTimerOn = true;
		}
		else
		if (m_nDiagramMode == (int)eDiagramMode.eTouchCast)
		{
			EnableDiagramControls(false, true);
			picDiagramMode.IsEnabled = true;
			picDiagramSpeed.SelectedIndex = (int)eDiagramSpeed.eFast;
			picDiagramType.SelectedIndex = (int)eDiagramType.eLine;
			SetCurrentLine(0, true);
			m_nTouchLine = -1;
		}
		else
		if (m_nDiagramMode == (int)eDiagramMode.eAutoCast)
		{
			EnableDiagramControls(false, true);
			AutoCast();
			//m_tskAutoCast = new Task(AutoCast);
			//m_tskAutoCast.Start();
		}
		else
		if (m_nDiagramMode == (int)eDiagramMode.eMindCast)
			if (UI.Get<EegView>().EegChannel(0) is not null)
			{
				EnableDiagramControls(false, true);
				MindCast();
				//m_tskAutoCast = new Task(MindCast);
				//m_tskAutoCast.Start();
			}
			else
				picDiagramMode.SelectedIndex = (int)eDiagramMode.eExplore;
	}

	protected void picDiagramType_ValueChanged(object sender, EventArgs e)
	{
		m_nDiagramType = picDiagramType.SelectedIndex;
		if (m_nDiagramType == (int)eDiagramType.eHexagram)
			m_vsCurrent = m_hvsCurrent;
		else
		if (m_nDiagramType == (int)eDiagramType.eTrigram)
			m_vsCurrent = m_hvsCurrent.Trigram(m_nCurrentTrigram);
		else
		if (m_nDiagramType == (int)eDiagramType.eLine)
			m_vsCurrent = m_hvsCurrent.Trigram(m_nCurrentTrigram).Line(m_nCurrentLine % 3);
		UpdateDiagram(false);
	}

	protected void picDiagramSpeed_ValueChanged(object sender, EventArgs e)
	{
		m_nDiagramSpeed = picDiagramSpeed.SelectedIndex;
		if (m_bTimerOn)
			m_timDiagram.Change(0, m_nSpeeds[m_nDiagramSpeed]);
	}

	protected void picDiagramColor_ValueChanged(object sender, EventArgs e)
	{
		m_nDiagramColor = (int)picDiagramColor.SelectedIndex;
		UpdateDiagram(false);
	}

	private void picDiagramLsb_ValueChanged(object sender, EventArgs e)
	{
		if (picDiagramLsb != null)
			SetDiagramLsb(picDiagramLsb.SelectedIndex);
	}

	private void picHexagramText_ValueChanged(object sender, EventArgs e)
	{
		if (picHexagramText != null)
			SetHexagramText(picHexagramText.SelectedIndex);
	}

	private void picHexagramLabel_ValueChanged(object sender, EventArgs e)
	{
		if (picHexagramLabel != null)
			SetHexagramLabel(picHexagramLabel.SelectedIndex);
	}

	private void picHexagramSequence_ValueChanged(object sender, EventArgs e)
	{
		if (picHexagramSequence != null)
			SetHexagramSequence(picHexagramSequence.SelectedIndex);
	}

	private void picHexagramRatio_ValueChanged(object sender, EventArgs e)
	{
		if (picHexagramRatio != null)
			SetHexagramRatio(picHexagramRatio.SelectedIndex);
	}

	private void picTrigramText_ValueChanged(object sender, EventArgs e)
	{
		if (picTrigramText != null)
			SetTrigramText(picTrigramText.SelectedIndex);
	}

	private void picTrigramLabel_ValueChanged(object sender, EventArgs e)
	{
		if (picTrigramLabel != null)
			SetTrigramLabel(picTrigramLabel.SelectedIndex);
	}

	private void picTrigramSequence_ValueChanged(object sender, EventArgs e)
	{
		if (picTrigramSequence != null)
			SetTrigramSequence(picTrigramSequence.SelectedIndex);
	}

	private void picTrigramRatio_ValueChanged(object sender, EventArgs e)
	{
		if (picTrigramRatio != null)
			SetTrigramRatio(picTrigramRatio.SelectedIndex);
	}
	private void picLineText_ValueChanged(object sender, EventArgs e)
	{
		if (picLineText != null)
			SetLineText(picLineText.SelectedIndex);
	}

	private void picLineLabel_ValueChanged(object sender, EventArgs e)
	{
		if (picLineLabel != null)
			SetLineLabel(picLineLabel.SelectedIndex);
	}

	private void picLineSequence_ValueChanged(object sender, EventArgs e)
	{
		if (picLineSequence != null)
			SetLineSequence(picLineSequence.SelectedIndex);
	}

	private void picLineRatio_ValueChanged(object sender, EventArgs e)
	{
		if (picLineRatio != null)
			SetLineRatio(picLineRatio.SelectedIndex);
	}

	private void xxxxxxxxxMainPage_File_Open_Clicked(object sender, EventArgs e)
	{
		//DisplayAlert("File", "Open", "OK");
	}

	// //////////////////////////////////////////////////////////////////////////////////////////////////
	// //////////////////////////////////////////////////////////////////////////////////////////////////
	// //////////////////////////////////////////////////////////////////////////////////////////////////

	private void Test6502()
	{
		String s1 = "uint8_t CodeC64[]={";
		using (FileStream fs = File.OpenRead("C:\\Src\\Arduino\\Esp32_Rtos_6502\\target\\text1.c64"))
		{
			byte[] buffer = new byte[fs.Length];
			//////////////////////////////////////////////fs.Read(buffer, 0, buffer.Length);
			fs.ReadExactly(buffer);
			for (int i = 0; i < buffer.Length; i++) // 0x41
			{
				if (i % 8 == 0)
					s1 += "\n";
				String s2 = $"0x{buffer[i]:X2}, ";
				s1 += s2;
			}
			s1 += "};\n\n";
		}
		/*
		s1 += "byte CodeA2[]={";
		using (FileStream fs = File.OpenRead("C:\\Src\\Arduino\\Esp32_Rtos_6502\\target\\text1.ap2"))
		{
			byte[] buffer = new byte[fs.Length];
			fs.Read(buffer, 0, buffer.Length);
			for (int i = 0; i < buffer.Length; i++) // 0x41
			{
				if (i % 8 == 0)
					s1 += "\n";
				String s2 = $"0x{buffer[i]:X2}, ";
				s1 += s2;
			}
			s1 += "};\n\n";
		}
		*/
		using (FileStream fs = new FileStream("C:\\Src\\Arduino\\Esp32_Rtos_6502\\src\\code.h", FileMode.Truncate, FileAccess.Write))
		{
			byte[] val = System.Text.Encoding.UTF8.GetBytes(s1);
			fs.Write(val, 0, val.Length);
		}
	}

	public void LoadDiagramSettings()
	{
		int nLength = Sequences.strDiagramSettings.Length / 17;
		for (int i = 1; i < nLength; ++i)
			if (Sequences.strDiagramSettings[16, i].Length > 0)
				picHexagramText.Items.Add(Sequences.strDiagramSettings[16, i]);
		picHexagramText.SelectedIndex = AppPreferences.HexagramText;

		for (int i = 1; i < nLength; ++i)
			if (Sequences.strDiagramSettings[15, i].Length > 0)
				picHexagramLabel.Items.Add(Sequences.strDiagramSettings[15, i]);
		picHexagramLabel.SelectedIndex = AppPreferences.HexagramLabel;

		for (int i = 1; i < nLength; ++i)
			if (Sequences.strDiagramSettings[13, i].Length > 0)
				picHexagramSequence.Items.Add(Sequences.strDiagramSettings[13, i]);
		picHexagramSequence.SelectedIndex = AppPreferences.HexagramSequence;

		for (int i = 1; i < nLength; ++i)
			if (Sequences.strDiagramSettings[14, i].Length > 0)
				picHexagramRatio.Items.Add(Sequences.strDiagramSettings[14, i]);
		picHexagramRatio.SelectedIndex = AppPreferences.HexagramRatio;

		for (int i = 1; i < nLength; ++i)
			if (Sequences.strDiagramSettings[12, i].Length > 0)
				picTrigramText.Items.Add(Sequences.strDiagramSettings[12, i]);
		picTrigramText.SelectedIndex = AppPreferences.TrigramText;

		for (int i = 1; i < nLength; ++i)
			if (Sequences.strDiagramSettings[11, i].Length > 0)
				picTrigramLabel.Items.Add(Sequences.strDiagramSettings[11, i]);
		picTrigramLabel.SelectedIndex = AppPreferences.TrigramLabel;

		for (int i = 1; i < nLength; ++i)
			if (Sequences.strDiagramSettings[9, i].Length > 0)
				picTrigramSequence.Items.Add(Sequences.strDiagramSettings[9, i]);
		picTrigramSequence.SelectedIndex = AppPreferences.TrigramSequence;

		for (int i = 1; i < nLength; ++i)
			if (Sequences.strDiagramSettings[10, i].Length > 0)
				picTrigramRatio.Items.Add(Sequences.strDiagramSettings[10, i]);
		picTrigramRatio.SelectedIndex = AppPreferences.TrigramRatio;

		for (int i = 1; i < nLength; ++i)
			if (Sequences.strDiagramSettings[8, i].Length > 0)
				picLineText.Items.Add(Sequences.strDiagramSettings[8, i]);
		picLineText.SelectedIndex = AppPreferences.LineText;

		for (int i = 1; i < nLength; ++i)
			if (Sequences.strDiagramSettings[7, i].Length > 0)
				picLineLabel.Items.Add(Sequences.strDiagramSettings[7, i]);
		picLineLabel.SelectedIndex = AppPreferences.LineLabel;

		for (int i = 1; i < nLength; ++i)
			if (Sequences.strDiagramSettings[5, i].Length > 0)
				picLineSequence.Items.Add(Sequences.strDiagramSettings[5, i]);
		picLineSequence.SelectedIndex = AppPreferences.LineSequence;

		for (int i = 1; i < nLength; ++i)
			if (Sequences.strDiagramSettings[6, i].Length > 0)
				picLineRatio.Items.Add(Sequences.strDiagramSettings[6, i]);
		picLineRatio.SelectedIndex = AppPreferences.LineRatio;
	}

	public void SetDiagramMode(eDiagramMode eMode)
	{
		picDiagramMode.SelectedIndex = (int)eMode;
	}

	public void SetDiagramColor(eDiagramColor eColor)
	{
		picDiagramColor.SelectedIndex = (int)eColor;
	}

	public void SetDiagramSpeed(eDiagramSpeed eSpeed)
	{
		picDiagramSpeed.SelectedIndex = (int)eSpeed;
	}

	public void SetDiagramLsb(int nLsb)
	{
		Sequences.DiagramLsb = nLsb;
		Sequences.SetLSB(Sequences.DiagramLsb == 0);
		UpdateDiagram(true);
	}

	public void SetLineSequence(int nSequence)
	{
		Sequences.LineSequence = nSequence;
		CLineValueSequencer.SetCurrentSequence(nSequence);
		UpdateDiagram(true);
	}

	public void SetLineRatio(int nRatio)
	{
		Sequences.LineRatio = nRatio;
		CLineValueSequencer.SetCurrentRatio(nRatio);
		UpdateDiagram(true);
	}

	public void SetLineLabel(int nLabel)
	{
		Sequences.LineLabel = nLabel;
		CLineValueSequencer.SetCurrentLabel(nLabel);
		UpdateDiagram(false);
	}

	public void SetLineText(int nText)
	{
		Sequences.LineText = nText;
	}

	public void SetTrigramSequence(int nSequence)
	{
		Sequences.TrigramSequence = nSequence;
		CTrigramValueSequencer.SetCurrentSequence(nSequence);
		UpdateDiagram(true);
	}

	public void SetTrigramRatio(int nRatio)
	{
		Sequences.TrigramRatio = nRatio;
		CTrigramValueSequencer.SetCurrentRatio(nRatio);
		UpdateDiagram(true);
	}

	public void SetTrigramLabel(int nLabel)
	{
		Sequences.TrigramLabel = nLabel;
		CTrigramValueSequencer.SetCurrentLabel(nLabel);
		UpdateDiagram(false);
	}

	public void SetTrigramText(int nText)
	{
		Sequences.TrigramText = nText;
	}

	public void SetHexagramSequence(int nSequence)
	{
		Sequences.HexagramSequence = nSequence;
		CHexagramValueSequencer.SetCurrentSequence(nSequence);
		UpdateDiagram(true);
	}

	public void SetHexagramRatio(int nRatio)
	{
		Sequences.HexagramRatio = nRatio;
		CHexagramValueSequencer.SetCurrentRatio(nRatio);
		UpdateDiagram(true);
	}

	public void SetHexagramLabel(int nLabel)
	{
		Sequences.HexagramLabel = nLabel;
		CHexagramValueSequencer.SetCurrentLabel(nLabel);
		UpdateDiagram(false);
	}

	public void SetHexagramText(int nText)
	{
		Sequences.HexagramText = nText;
	}

	private void Transition()
	{
		m_hvsPrimary = null;
		UpdateDiagram(false);
		UpdateText();
	}

	public void SetFirst()
	{
		//UpdateSessionLog("KernelFunction SetFirst", true, true);
		m_vsCurrent.First();
		Transition();
	}

	public void SetPrevious()
	{
		//UpdateSessionLog("KernelFunction SetPrevious", true, true);
		m_vsCurrent.Previous();
		Transition();
	}

	public void SetNext()
	{
		//UpdateSessionLog("KernelFunction SetNext", true, true);
		m_vsCurrent.Next();
		Transition();
	}

	public void SetLast()
	{
		//UpdateSessionLog("KernelFunction SetLast", true, true);
		m_vsCurrent.Last();
		Transition();
	}

	public void SetMove()
	{
		//UpdateSessionLog("KernelFunction SetMove", true, true);
		SetMove(0);
	}

	public void SetHome()
	{
		//UpdateSessionLog("KernelFunction SetHome", true, true);
		SetHome(0);
	}

	public void SetInverse()
	{
		//UpdateSessionLog("KernelFunction SetInverse", true, true);
		m_vsCurrent.Inverse();
		Transition();
	}

	public void SetOpposite()
	{
		//UpdateSessionLog("KernelFunction SetOpposite", true, true);
		m_vsCurrent.Opposite();
		Transition();
	}

	public void SetTransverse()
	{
		//UpdateSessionLog("KernelFunction SetTransverse", true, true);
		m_vsCurrent.Transverse();
		Transition();
	}

	public void SetNuclear()
	{
		//UpdateSessionLog("KernelFunction SetNuclear", true, true);
		m_vsCurrent.Nuclear();
		Transition();
	}

	public string DescribrCastHexagram()
	{
		//UpdateSessionLog("KernelFunction DescribeCastHexagram", true, true);
		return m_hvsCurrent.DescribeCast();
	}

	public async Task<string> AutoCastHexagram()
	{
		//UpdateSessionLog("KernelFunction AutoCastHexagram", true, true);
		//picDiagramMode.SelectedIndex = (int)eDiagramMode.eAutoCast;
		return await AutoCastAsync();
	}

	public void SetHexagramValue(int nValue)
	{
		//UpdateSessionLog($"KernelFunction SetHexagram {sequence}", true, true);
		m_hvsCurrent.Value = nValue;
		UpdateDiagram(true);
		UpdateText();
	}

	public void SetHexagramCast(string cast)
	{
		//UpdateSessionLog($"KernelFunction SetHexagram {sequence}", true, true);
		string[] s1 = cast.Split(' ', '.');
		if (s1.Length > 0)
			if (int.TryParse(s1[0], out int sequence))
			{
				m_hvsCurrent.Sequence = sequence - 1;
				m_hvsCurrent.Update();
				if (s1.Length > 1)
					for (int i = 0; i < s1[1].Length; ++i)
						if (int.TryParse(s1[1][i].ToString(), out int line))
							m_hvsCurrent.Trigram((line - 1) / 3).Line((line - 1) % 3).Next();
						else
							break;
				m_hvsCast = new CHexagramValueSequencer(ref m_hvsCurrent);
				m_vsCurrent = m_hvsCurrent;
				UpdateDiagram(false);
				UpdateText();
			}
	}
	public void SetHexagram(int sequence)
	{
		//UpdateSessionLog($"KernelFunction SetHexagram {sequence}", true, true);
		m_vsCurrent.Sequence = sequence - 1;
		Transition();
		UpdateDiagram(true);
	}

	public int GetHexagram()
	{
		//UpdateSessionLog($"KernelFunction GetHexagram {_this.m_vsCurrent.Sequence + 1}", true, true);
		return m_vsCurrent.Sequence + 1;
	}

	// //////////////////////////////////////////////////////////////////////////////////////////////////
	// //////////////////////////////////////////////////////////////////////////////////////////////////
	// //////////////////////////////////////////////////////////////////////////////////////////////////

	private void ResizeDiagram(double width, double height)
	{
		bool bLandscape = false;
		Rect rec = new();
		int l = 6;
		rec.Y = 0;

#if ANDROID || IOS
		//if (width > height)
		//	bLandscape = true;
#endif

		if (width > 1800)
			width /= 4.2;
		else
		if (width > 1500)
			width /= 4;
		else
		if (width > 1200)
			width /= 4.5;
		else
		if (bLandscape || (width > 800))
			width /= 3;
		else
		 if (width > 600)
			width /= 2;

		m_nDiagramHeight = (int)(width / 5.2); // 5.2
		m_nDiagramHeight = m_nDiagramHeight - m_nDiagramHeight % 2;
		m_nDiagramWidth = m_nDiagramHeight * 2;
		m_nDiagramPadding = m_nDiagramHeight / 60;
		m_nDiagramPadding = m_nDiagramPadding == 0 ? 2 : m_nDiagramPadding * 3;
		//m_nDiagramFontSize = m_nDiagramPadding * 6;

		for (int i = 0; i < 6; ++i)
		{
			rec.X = 0;
			rec.Width = m_nDiagramWidth;
			rec.Height = m_nDiagramHeight;
			layDiagram.SetLayoutBounds(m_recLine[--l, 0], rec);

			rec.X = m_nDiagramWidth;
			rec.Width = m_nDiagramHeight;
			rec.Height = m_nDiagramHeight;
			layDiagram.SetLayoutBounds(m_ellLine[l], rec);

			rec.X = m_nDiagramWidth + m_nDiagramHeight;
			rec.Width = m_nDiagramWidth;
			rec.Height = m_nDiagramHeight;
			layDiagram.SetLayoutBounds(m_recLine[l, 1], rec);

			rec.Y += m_nDiagramHeight + m_nDiagramPadding;
		}
		//lblHexagram.FontSize = m_nDiagramFontSize;
		UpdateDiagram(false);
	}

	private void SetCurrentLine(int nLine, bool bCurrent)
	{
		m_nCurrentLine = nLine;
		m_nCurrentTrigram = m_nCurrentLine / 3;
		if (bCurrent)
			m_vsCurrent = m_hvsCurrent.Trigram(nLine / 3).Line(nLine % 3);
	}

	private void SetMove(int dummy)
	{
		if (m_hvsCurrent.IsMoving)
		{
			m_hvsPrimary = new CHexagramValueSequencer(ref m_hvsCurrent);
			m_vsCurrent.Move();
		}
		else
		if (m_hvsPrimary != null)
		{
			m_hvsCurrent = new CHexagramValueSequencer(ref m_hvsPrimary);
			m_vsCurrent = m_hvsCurrent;
			m_hvsPrimary = null;
		}
		UpdateDiagram(false);
		UpdateText();
	}

	private void SetHome(int dummy)
	{
		if (m_hvsCast != null)
		{
			m_hvsCurrent = new CHexagramValueSequencer(ref m_hvsCast);
			m_vsCurrent = m_hvsCurrent;
			UpdateDiagram(false);
			UpdateText();
		}
	}

	private static SolidColorBrush TrigramColor(int nValue)
	{
		switch (nValue)
		{
			case 0: // earth
				return Brush.Black;
			case 1: // thunder
				return Brush.Green;
			case 2: // water
				return Brush.Blue;
			case 3: // lake
				return Brush.Cyan;
			case 4: // mountain
				return Brush.Magenta;
			case 5: // fire
				return Brush.Red;
			case 6: // wind
				return Brush.Yellow;
			case 7: // heaven
				break;
		}
		return Brush.LightGray;
	}

	private SolidColorBrush RgbColor()
	{
		bool bLower, bUpper;
		int[] nRgb = { 0, 0, 0 };
		for (int i = 0; i < 3; ++i)
		{
			bUpper = m_hvsCurrent.Trigram(1).Line(i).Value % 2 == 1 ? true : false;
			bLower = m_hvsCurrent.Trigram(0).Line(i).Value % 2 == 1 ? true : false;
			nRgb[i] = (byte)(bLower ? bUpper ? 0xFF : 0x55 : bUpper ? 0xAA : 0x00);
		}
		if ((nRgb[0] == 0) && (nRgb[1] == 0) && (nRgb[2] == 0))
			return new SolidColorBrush(Color.FromRgba(0x40, 0x40, 0x40, 0xFF));
		if ((nRgb[0] == 255) && (nRgb[1] == 255) && (nRgb[2] == 255))
			return new SolidColorBrush(Colors.LightGray);
		return new SolidColorBrush(Color.FromRgba(nRgb[0], nRgb[1], nRgb[2], 0xFF));
	}

	private void UpdateDiagram()
	{
		try
		{
			SolidColorBrush brDiagram = RgbColor();
			for (int l = 0; l < 6; ++l)
			{
				bool bSpecific = ((m_nDiagramType == (int)eDiagramType.eLine) && (m_nCurrentLine == l)) ||
					((m_nDiagramType == (int)eDiagramType.eTrigram) && (m_nCurrentTrigram == (l / 3))) ||
					(m_nDiagramType == (int)eDiagramType.eHexagram);
				bool bCurrentLine = (m_nDiagramType == (int)eDiagramType.eLine) && (m_nCurrentLine == l);

				int nValue = m_hvsCurrent.Trigram(l / 3).Line(l % 3).Value;
				bool bYang = nValue % 2 == 1;
				bool bOld = (nValue == 0) || (nValue == 3);

				if (m_nDiagramColor == (int)eDiagramColor.eTrigram)
					brDiagram = TrigramColor(m_hvsCurrent.Trigram(l / 3).Value);
				else
					brDiagram = (m_nDiagramColor == (int)eDiagramColor.eMono) ? m_brMonoColor :
						(m_nDiagramColor == (int)eDiagramColor.eDual) ? bYang ? m_brLightGray : m_brDarkGray : brDiagram;

				var rec = layDiagram.GetLayoutBounds(m_recLine[l, 0]);
				rec.Width = bYang ? (m_nDiagramWidth * 2) + m_nDiagramHeight : m_nDiagramWidth;
				layDiagram.SetLayoutBounds(m_recLine[l, 0], rec);

				m_recLine[l, 0].Fill = brDiagram;
				m_recLine[l, 0].Stroke = bSpecific && bCurrentLine ? m_brSelectStoke : m_brBackStoke;
				m_recLine[l, 0].StrokeThickness = bCurrentLine ? 8 : 2;

				m_recLine[l, 1].Fill = brDiagram;
				m_recLine[l, 1].Stroke = bSpecific && bCurrentLine ? m_brSelectStoke : m_brBackStoke;
				m_recLine[l, 1].StrokeThickness = bCurrentLine ? 8 : 2;
				m_recLine[l, 1].IsVisible = !bYang;

				m_ellLine[l].Fill = bYang ? m_brMovingYang : brDiagram;
				m_ellLine[l].Stroke = bSpecific && bCurrentLine ? m_brSelectStoke : m_brBackStoke;
				m_ellLine[l].StrokeThickness = bCurrentLine ? 8 : 2;
				m_ellLine[l].IsVisible = bOld;
			}

			lblHexagram.Text = m_hvsCurrent.DescribeCast(true);
		}
		catch { }
	}

	private void UpdateDiagram(bool bCurrent)
	{
		if (bCurrent)
			m_hvsCurrent.Update();
		UpdateDiagram();
	}

	private void UpdateText(bool resetSession = true)
	{
		//if (resetSession)
		//	picSession.SelectedIndex = 0;
		UpdateText(m_hvsCurrent);
	}

	private void DiagramTimer()
	{
		bool bRatio = (m_nDiagramMode == (int)eDiagramMode.eTouchCast) || (m_nDiagramMode == (int)eDiagramMode.eAutoCast) || (m_nDiagramMode == (int)eDiagramMode.eMindCast);
		if (m_bForward)
			m_vsCurrent.Next(bRatio);
		else
			m_vsCurrent.Previous(bRatio);
		UpdateDiagram(false);
	}

	private static void DiagramTimer(object state)
	{
		UI.Call<DiagramView>(v => v.DiagramTimer());
	}


	private async Task<string> AutoCastAsync()
	{
		await AutoCast();
		return m_hvsCurrent.DescribeCast();
	}


	private async Task AutoCast()
	{
		//Random r = true ? Sequences.m_ranSession : new Random(DateTime.Now.Millisecond);
		for (int i = 0; i < 6; ++i)
		{
			SetCurrentLine(i, true);
			m_timDiagram.Change(0, m_nSpeeds[(int)eDiagramSpeed.eFast]);
			await Task.Delay((m_rand.Next(5) + 2) * 1000 + m_rand.Next(1000));
			m_timDiagram.Change(Timeout.Infinite, 0);
		}
		void action() => EndCast();
		Dispatcher.Dispatch(action);
	}

	public async Task SoundTrigger(float fBand, float fTrigger)
	{
#if WINDOWS

		TimeSpan ts = DateTime.Now - AppSettings._lastEegDataTime;

		if (ts.TotalSeconds > 30)
		{
			fBand = 0.01f;
			fTrigger = 0.01f;
		}

		//PInvoke.Beep(261, 2000); // Middle C
		uint freq = (uint)((fBand + 11.0) * 23.7);
		PInvoke.Beep(freq, 200);
		freq = (uint)((fTrigger + 11.0) * 23.7);
		if (fTrigger != 0.0f)
			PInvoke.Beep(freq, 200);
#endif
	}

	private async Task MindCast()
	{
		//Random r = true ? Sequences.m_ranSession : new Random(DateTime.Now.Millisecond);
		EegView ev = UI.Get<EegView>();
		ev.EegSetTriggers(true, true);
		for (int i = 0; i < 6; ++i)
		{
			SetCurrentLine(i, true);
			UpdateDiagram(false);

			//App.EegChannel(AppPreferences.TriggerIndex).m_fMinValue = 1000.0f;
			//App.EegChannel(AppPreferences.TriggerIndex).m_fMaxValue = -1000.0f;

			//App.EegChannel(23).m_fMinValue = 1000.0f;
			//App.EegChannel(23).m_fMaxValue = -1000.0f;

			DateTime start = DateTime.Now;
			TimeSpan ts = DateTime.Now - start;
			float[] amount = new float[] { 0.0f, 0.01f, 0.01f, 0.01f, 0.02f, 0.02f, 0.02f, 0.03f, 0.03f, 0.03f, 0.04f, 0.04f, 0.04f, 0.05f, 0.05f, 0.05f, 0.06f, 0.06f, 0.06f, 0.07f, 0.07f, 0.07f, 0.08f, 0.08f, 0.08f };

			int count = 0;
			//App.EegSetTriggers(true, true);
			while (ev.EegIsConnected() && !ev.EegIsTriggerOn())
			{
				await Task.Delay(200);
				if (AppPreferences.TriggerSounding && (ev.EegReplaySpeed() == 1) && (++count % 50 == 0))
					await SoundTrigger(ev.EegChannel(AppPreferences.TriggerIndex).m_fCurrentValue * AppPreferences.AudioScale,
						ev.EegChannel(AppPreferences.TriggerIndex).m_fHigh * AppPreferences.AudioScale);
				ts = DateTime.Now - start;
				int speed = m_nTriggerSpeed * ev.EegReplaySpeed();
				speed = speed > 50 ? 50 : speed;
				int index = (int)ts.TotalSeconds * speed / 60;
				if (!AppPreferences.TriggerFixed && (index < 25))
				{
					ev.EegDecreaseTriggers(amount[index]);
					amount[index] = 0.0f;
				}
			}
			if (!ev.EegIsConnected())
				break;
			if (AppPreferences.TriggerSounding)
				await SoundTrigger(ev.EegChannel(AppPreferences.TriggerIndex).m_fCurrentValue * AppPreferences.AudioScale, 0.0f);

			m_timDiagram.Change(0, m_nSpeeds[(int)eDiagramSpeed.eFast]);

			start = DateTime.Now;
			ts = DateTime.Now - start;
			amount = new float[] { 0.0f, 0.01f, 0.01f, 0.01f, 0.02f, 0.02f, 0.02f, 0.03f, 0.03f, 0.03f, 0.04f, 0.04f, 0.04f, 0.05f, 0.05f, 0.05f, 0.06f, 0.06f, 0.06f, 0.07f, 0.07f, 0.07f, 0.08f, 0.08f, 0.08f };

			count = 0;
			//App.EegSetTriggers(true, false);
			while (ev.EegIsConnected() && !ev.EegIsTriggerOff())
			{
				await Task.Delay(200);
				if (AppPreferences.TriggerSounding && (ev.EegReplaySpeed() == 1) && (++count % 50 == 0))
					await SoundTrigger(ev.EegChannel(AppPreferences.TriggerIndex).m_fCurrentValue * AppPreferences.AudioScale,
						ev.EegChannel(AppPreferences.TriggerIndex).m_fLow * AppPreferences.AudioScale);
				ts = DateTime.Now - start;
				int speed = m_nTriggerSpeed * ev.EegReplaySpeed();
				speed = speed > 50 ? 50 : speed;
				int index = (int)ts.TotalSeconds * speed / 60;
				if (!AppPreferences.TriggerFixed && (index < 25))
				{
					ev.EegIncreaseTriggers(amount[index]);
					amount[index] = 0.0f;
				}
			}

			//App.EegCalculateTriggers();
			m_timDiagram.Change(Timeout.Infinite, 0);
			if (!ev.EegIsConnected())
				break;
			if (AppPreferences.TriggerSounding)
				await SoundTrigger(ev.EegChannel(AppPreferences.TriggerIndex).m_fCurrentValue * AppPreferences.AudioScale, 0.0f);
		}
		await Task.Delay(100);
		if (AppPreferences.TriggerSounding)
			AudioPlayer.PlayHexagramEnd(Dispatcher);
		void action() => EndCast();
		Dispatcher.Dispatch(action);
	}

	private void EndCast()
	{
		UpdateText();
		EnableDiagramControls(true, true);
		UpdateDiagram(false);
		m_hvsCast = new CHexagramValueSequencer(ref m_hvsCurrent);
		m_vsCurrent = m_hvsCurrent;
	}

	private void EnableDiagramControls(bool bEnable, bool bExplore)
	{
		//MainPage.EnableDiagramControls(bEnable);
		//SettingsPage.EnableDiagramControls(bEnable);

		picDiagramMode.IsEnabled = bEnable;
		picDiagramType.IsEnabled = bEnable;
		picDiagramSpeed.IsEnabled = bEnable;
		//picDiagramColor.IsEnabled = bEnable;

		btnMove.IsEnabled = bEnable;
		btnHome.IsEnabled = bEnable;
		btnFirst.IsEnabled = bEnable;
		btnLast.IsEnabled = bEnable;
		btnInverse.IsEnabled = bEnable;
		btnOpposite.IsEnabled = bEnable;
		btnTransverse.IsEnabled = bEnable;
		btnNuclear.IsEnabled = bEnable;

		mnuMove.IsEnabled = bEnable;
		mnuHome.IsEnabled = bEnable;
		mnuFirst.IsEnabled = bEnable;
		mnuLast.IsEnabled = bEnable;
		mnuInverse.IsEnabled = bEnable;
		mnuOpposite.IsEnabled = bEnable;
		mnuTransverse.IsEnabled = bEnable;
		mnuNuclear.IsEnabled = bEnable;

		if (bEnable)
		{
			if (bExplore)
				picDiagramMode.SelectedIndex = (int)eDiagramMode.eExplore;
			picDiagramType.SelectedIndex = (int)eDiagramType.eHexagram;
			picDiagramSpeed.SelectedIndex = (int)eDiagramSpeed.eMedium;
		}
		else
		{
			picDiagramType.SelectedIndex = (int)eDiagramType.eLine;
			picDiagramSpeed.SelectedIndex = (int)eDiagramSpeed.eFast;
		}
	}

	public bool IsExploreMode()
	{
		return m_nDiagramMode == (int)eDiagramMode.eExplore;
	}

	public void UpdateText(CHexagramValueSequencer hvsPrimary)
	{

		////////////////////////////////////////////////////////////////////////////////////
		////////////////////////////////////////////////////////////////////////////////////
		// https://www.youtube.com/live/hM4ifrqF_lQ?si=eyV0DSBPuay9JV1d&t=21795 - RegEx Span
		////////////////////////////////////////////////////////////////////////////////////
		////////////////////////////////////////////////////////////////////////////////////

		String strText = Sequences.strDiagramSettings[16, Sequences.HexagramText + 1];
		string strBC = App.Current.RequestedTheme == AppTheme.Dark ? "black" : "white";
		string strFC = App.Current.RequestedTheme == AppTheme.Dark ? "white" : "black";
		string strAC = App.Current.RequestedTheme == AppTheme.Dark ? "gray" : "gray";

		String strHtml =
		"<html><head><meta charset=\"utf-8\"/><style> " +
		"body{ " +
		$"background-color:{strBC};color:{strFC};font-family:'Open Sans',sans-serif;font-size:16px;line-height:1.5; " +
		"} " +
		"strong{color:" + strFC + ";} " +
		"a {" + $" color: {strAC};" + "} " +
		"h2 {" + $" color: {strAC};" + "} " +
		"h2 {" + $" color: {strAC};" + "} " +
		"h4 {" + $" color: {strAC};" + "} " +
		"</style></head><body><h2>";

		strHtml += hvsPrimary.DescribeCast() + "</h2>";

		if (strText == "Andrade")
		{
			strHtml += "<center><img src=\"" + Andrade.strText[hvsPrimary.Value] + "\" alt=\"" +
			hvsPrimary.Label + "\"></center>" +
			"</body></html>";
		}
		else
		if (strText == "Chinese")
		{
			strHtml += Chinese.strText[hvsPrimary.Value] +
			"</body></html>";
		}
		else
		if (strText == "Legge")
		{
			strHtml += Legge.strText[hvsPrimary.Value];
			for (int nLine = 0; nLine < 6; ++nLine)
			{
				CHexagramValueSequencer hvs = new CHexagramValueSequencer(ref hvsPrimary);
				hvs.Young();
				hvs.Trigram(nLine / 3).Line(nLine % 3).Old();
				hvs.Move();
				String strHref = "<a href=\"Hexagram" + hvs.Value + "\">" + Sequences.strHexagramLabels[9, hvs.Value] + "</a>";
				strHtml += "<h2>Line " + (nLine + 1).ToString() + " - " + strHref + "</h2>" +
				Legge.strLine[nLine, hvsPrimary.Value];
			}
			strHtml += "</body></html>";
		}
		else
		if (strText == "Hatcher")
		{
			strHtml += "You can download Bradford Hatcher's Yijing translation as a zipped PDF from ";
			strHtml += "<a href=\"http://hermetica.info/\">Hermetica.info</a>"; // /Yijing-One.zip
			strHtml += "<br/><br/>You can also purchase a hardcopy of the book from the site";
			strHtml += "<br/><br/>This application can not currently open the document to the selected hexagram";
			strHtml += "<br/><br/>You can view the document in a <a href=\"ms-windows-store://pdp/?productid=9wzdncrfj2gc\">PDF Reader</a> ";
			strHtml += "</body></html>";
		}
		else
		if (strText == "Wilhelm")
		{
			String strHtml1 = Wilhelm.strText[hvsPrimary.Value] +
				"<h2>The Image</h2>" +
				Wilhelm.strImage[hvsPrimary.Value] +
				"<h2>The Judgement</h2>" +
				Wilhelm.strJudgement[hvsPrimary.Value];
			for (int nLine = 0; nLine < 6; ++nLine)
			{
				CHexagramValueSequencer hvs = new CHexagramValueSequencer(ref hvsPrimary);
				hvs.Young();
				hvs.Trigram(nLine / 3).Line(nLine % 3).Old();
				hvs.Move();
				String strHref = "<a href=\"Hexagram" + hvs.Value + "\">" + Sequences.strHexagramLabels[9, hvs.Value] + "</a>";
				strHtml1 += "<h2>Line " + (nLine + 1).ToString() + " - " + strHref + "</h2>" +
				Wilhelm.strLine[nLine, hvsPrimary.Value];
			}
			for (int i = 0; i < 64; ++i)
				if (i != hvsPrimary.Value)
					if (strHtml1.Contains(Sequences.strHexagramLabels[9, i], StringComparison.CurrentCultureIgnoreCase))
					{
						String strHref = "<a href=\"Hexagram" + i + "\">" + Sequences.strHexagramLabels[9, i] + "</a>";
						Regex rgx = new Regex("\\b(?i)" + Sequences.strHexagramLabels[9, i] +
							"(s)?(t)?(y)?(ty)?(ing)?(ed)?(ous)?(ment)?(ate)?(in)?\\b",
							RegexOptions.Compiled | RegexOptions.NonBacktracking | RegexOptions.IgnoreCase); // RegexOptions.ExplicitCapture
						strHtml1 = rgx.Replace(strHtml1, strHref + "$1$2$3$4$5$6$7$8$9$10");
					}
			strHtml += strHtml1 + "</body></html>";

			/*
			for (int i = 0; i < 8; ++i)
			{
				String strHref = "<a href=\"Trigram" + i + "\">" + Sequences.strTrigramLabels[2,i] + "</a>";
				Regex rgx = new Regex("\\b(?i)" + Sequences.strTrigramLabels[2, i] +
					"(s)?(t)?(y)?(ty)?(ing)?(ed)?(ous)?(ment)?(ate)?(in)?\\b");
				strUrl = rgx.Replace(strUrl, strHref + "$1$2$3$4$5$6$7$8$9$10");
			}
			*/
		}
		else
		if (strText == "Heyboer")
			UI.Call<DiagramPage>(p => p.WebView().Source = Heyboer.strText[hvsPrimary.Value]);
		else
		if (strText == "YellowBridge")
			UI.Call<DiagramPage>(p => p.WebView().Source = YellowBridge.strText[hvsPrimary.Value]);
		else
		if (strText == "Regis")
			UI.Call<DiagramPage>(p => p.WebView().Source = Regis.strText[hvsPrimary.Value]);

		UI.Call<DiagramPage>(p => p.WebView().Source = new HtmlWebViewSource { Html = strHtml });

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

		webText.traverse(SWT.TRAVERSE_PAGE_NEXT);

		Uri uri = new Uri(folder.Path + "/Yijing-One.pdf");
		Uri uri = new Uri("http://google.com");
		b  = await Launcher.LaunchUriForResultsAsync(uri, options);
		webText.Navigate(uri);
		*/
	}

	// //////////////////////////////////////////////////////////////////////////////////////////////////
	// //////////////////////////////////////////////////////////////////////////////////////////////////
	// //////////////////////////////////////////////////////////////////////////////////////////////////

	public void UpdateSessionLog(string str, bool append, bool newline)
	{
		//if (append)
		//	UI.Call<DiagramPage>(p => p.SessionLog().Text += str + (newline ? "\n" : ""));
		//else
		//	UI.Call<DiagramPage>(p => p.SessionLog().Text = str + (newline ? "\n" : ""));
	}
	/*
	public void AiChat(bool includeCast)
	{
		string s = "";
		UI.Call<DiagramPage>(p => s = p.SessionLog().Text);

		if (includeCast)
			s += " I consulted the oracle and the Yijing responded with hexagram " + m_hvsCurrent.DescribeCast();

		bool reload = false;
		if (_ai._userPrompts[1].Count() == 0)
			if (picSession.FindByName(EegView._strSession) == null)
				reload = true;
		_saveChat = true;
		AiChat(s, reload);
		UpdateSessionLog("", false, false);
	}

	public async void AiChat(string msg, bool reload)
	{
		await _ai.ChatAsync(AppPreferences.AiChatService, msg);
		if (reload)
			LoadSessions(-1);
		else
		{
			SaveChat(picSession.SelectedItem as string);
			UpdateChat();
		}
	}

	public void LoadSessions(int index)
	{
		String strTemp = System.IO.Path.Combine(AppSettings.DocumentHome(), "Questions");
		if (!Directory.Exists(strTemp))
			Directory.CreateDirectory(strTemp);

		// wait Task.Delay(1000);

		IEnumerable<string> ief = Directory.EnumerateFiles(strTemp, "*.txt", SearchOption.TopDirectoryOnly);
		List<string> lf = new();
		foreach (string f in ief)
			lf.Add(System.IO.Path.GetFileNameWithoutExtension(f));

		if (index == -1)
			lf.Add(EegView._strSession);

		lf.Sort(new DescendingOrder());

		lf.Insert(0, "New Session");

		picSession.SelectedIndex = -1;
		picSession.ItemsSource = lf;

		if (index == -1)
			if ((index = picSession.Items.IndexOf(EegView._strSession)) == -1)
				index = 0;

		picSession.SelectedIndex = index;
		picSession.Focus();
	}

	public void StartNewChat()
	{
		UI.Call<DiagramView>(v => v.StartChat());
	}

	public void SelectChat(string name)
	{
		int i = -1;
		UI.Call<DiagramView>(v => i = v.picSession.Items.IndexOf(name));
		if (i != -1)
			UI.Call<DiagramView>(v => v.picSession.SelectedItem = name);
		else
		{
			UI.Call<DiagramView>(v => v.picSession.SelectedIndex = 0);
			EegView._strSession = name;
		}
	}

	public void StartChat()
	{
		SaveChat(EegView._strSession);
		_saveChat = false;

		//_ai.NewChat();

		_ai._userPrompts = [[], []];
		_ai._chatReponses = [[], []];
		UI.Call<DiagramView>(v => v.UpdateChat());
	}

	public void SaveChat(string name)
	{
		if (_saveChat)
			if ((_ai._userPrompts[1].Count() > 0) || (_ai._chatReponses[1].Count() > 0))
			{
				SaveChat(name, "Question", _ai._userPrompts[1]);
				SaveChat(name, "Answer", _ai._chatReponses[1]);
				_saveChat = false;
			}
	}

	public void SaveChat(string name, string type, List<string> list)
	{
		string str = System.IO.Path.Combine(AppSettings.DocumentHome(), $"{type}s", name + ".txt");
		using (FileStream fs = new(str, FileMode.Create, FileAccess.Write))
			foreach (string s in list)
			{
				byte[] val = System.Text.Encoding.UTF8.GetBytes($"$({type})\n" + s + "\n");
				fs.Write(val, 0, val.Length);
			}
	}

	public void LoadChat(string session, List<string> contexts)
	{
		StartChat();
		for (int i = 0; i < contexts.Count(); ++i)
		{
			LoadChat(contexts[i], "Question", _ai._userPrompts[0]);
			LoadChat(contexts[i], "Answer", _ai._chatReponses[0]);
		}
		if (!string.IsNullOrEmpty(session))
		{
			EegView._strSession = session;
			LoadChat(session, "Question", _ai._userPrompts[1]);
			LoadChat(session, "Answer", _ai._chatReponses[1]);
			if ((_ai._userPrompts[1].Count() > 0) || (_ai._chatReponses[1].Count() > 0))
				UpdateChat();
		}
	}

	public void LoadChat(string name, string type, List<string> list)
	{
		string str = System.IO.Path.Combine(AppSettings.DocumentHome(), $"{type}s", name + ".txt");
		string entry = "";
		if (File.Exists(str))
			using (StreamReader sr = File.OpenText(str))
			{
				while ((str = sr.ReadLine()) != null)
					if (!string.IsNullOrEmpty(str))
						if (str == $"$({type})")
						{
							if (!string.IsNullOrEmpty(entry))
							{
								list.Add(entry);
								entry = "";
							}
						}
						else
							entry += str + "\n";
				if (!string.IsNullOrEmpty(entry))
					list.Add(entry);
			}
		else
			UpdateSessionLog("Failed to load " + str, true, true);
	}

	public bool UpdateChat()
	{
		String strText = Sequences.strDiagramSettings[16, Sequences.HexagramText + 1];
		string strBC = App.Current.RequestedTheme == AppTheme.Dark ? "black" : "white";
		string strFC = App.Current.RequestedTheme == AppTheme.Dark ? "white" : "black";
		string strAC = App.Current.RequestedTheme == AppTheme.Dark ? "gray" : "gray";
		String strHtml = "<html xmlns=\"http://www.w3.org/1999/xhtml\">" +
			"<head><title>Yijing</title>" +
			"<link href=\"https://fonts.googleapis.com/css?family=Open+Sans\" rel=\"stylesheet\"/>" +
			"<style>" +
			"body {" +
			$" background-color: {strBC};" +
			$" color: {strFC};" +
			"} " +
			"html {" +
			" font-size: 16px;" +
			" font-family: \"Open Sans\", sans-serif;" +
			"} " +
			"a {" +
			$" color: {strAC};" +
			"} " +
			"h2 {" +
			$" color: {strAC};" +
			"} " +
			"h2 {" +
			$" color: {strAC};" +
			"} " +
			"h4 {" +
			$" color: {strAC};" +
			"} " +
			"</style></head><body><h2>" + " Chat Session: " + (picSession.SelectedItem as string);

		if (_ai._contextSessions.Count() > 0)
			strHtml += "</p>Context Sessions: ";
		foreach (var s in _ai._contextSessions)
			strHtml += s + " ";

		strHtml += "</h2>";

		int count = int.Max(_ai._chatReponses[1].Count(), _ai._userPrompts[1].Count());
		for (int i = 0; i < count; ++i)
		{
			if (i < _ai._userPrompts[1].Count())
				strHtml += "<p><h4>" + _ai._userPrompts[1][i].Replace("\n", "</p>") + "</h4></p>";
			if (i < _ai._chatReponses[1].Count())
				strHtml += "<p>" + _ai._chatReponses[1][i].Replace("\n", "</p>") + "</p>";
		}
		for (int i = 0; i < 64; ++i)
			if (strHtml.Contains(Sequences.strHexagramLabels[9, i], StringComparison.CurrentCultureIgnoreCase))
			{
				String strHref = "<a href=\"Hexagram" + i + "\">" + Sequences.strHexagramLabels[9, i] + "</a>";
				Regex rgx = new Regex("\\b(?i)" + Sequences.strHexagramLabels[9, i] +
					"(s)?(t)?(y)?(ty)?(ing)?(ed)?(ous)?(ment)?(ate)?(in)?\\b",
					RegexOptions.Compiled | RegexOptions.NonBacktracking | RegexOptions.IgnoreCase); // RegexOptions.ExplicitCapture
				strHtml = rgx.Replace(strHtml, strHref + "$1$2$3$4$5$6$7$8$9$10");
			}

		strHtml += "</body></html>";
		UI.Try<DiagramPage>(p => p.WebView().Source = new HtmlWebViewSource { Html = strHtml });
		return true;
	}
	*/
	// //////////////////////////////////////////////////////////////////////////////////////////////////
	// //////////////////////////////////////////////////////////////////////////////////////////////////
	// //////////////////////////////////////////////////////////////////////////////////////////////////

	public void SetAppTitle(string title)
	{
		void action()
		{
			Window[] w = Application.Current.Windows.ToArray();
			w[0].Title = title;
		}
		Dispatcher.Dispatch(action);
	}
	/*
	void YijingDB()
	{
		String path = System.IO.Path.Combine(AppSettings.DocumentHome(), "Yijing.db");
		YijingData yd = new YijingData(path);
		yd.InitialseDB();

		using (YijingEntities ye = new YijingEntities())
		{

			Text txt = ye.Texts.First();

			YijingDb.Type t = ye.Types.Find(3);
			LabelSery ls2 = ye.LabelSeries.Where(ls1 => ls1.Name == "Vivash" &&
				ls1.TypeId == (int)Sequences.ValueType.Hexagram).First(); // .OrderBy(ls1 => ls1.Name)
			IEnumerable<LabelSery> iels = from ls3 in ye.LabelSeries
										  where ls3.TypeId == (int)Sequences.ValueType.Line
										  orderby ls3.Name
										  select ls3;
			List<LabelSery> lls = iels.ToList();
			LabelSery ls4 = ye.LabelSeries.Include(ls => ls.Type).Include(ls => ls.Labels).Single(ls5 => ls5.Name == "Vivash");
			YijingDb.Label l1 = ls2.Labels.Single(l => l.LabelData == "Return");
			t = l1.Type;
		}

		//lblValues.Text = "";
		//CHexagramArray ha = new CHexagramArray();
		//ha.MultiCast(10000);
		//foreach (CHexagram h in ha.HexagramArray())
		//	if (h.Count > 0)
		//		lblValues.Text += $"{h.Count,4:D} {h.DescribeCast}\n";
	}
	*/
	public static readonly BindableProperty CardTitleProperty = BindableProperty.Create(nameof(CardTitle),
		typeof(string), typeof(DiagramView), string.Empty);

	public static readonly BindableProperty CardColorProperty = BindableProperty.Create(nameof(CardColor),
		typeof(Color), typeof(DiagramView), App.Current.RequestedTheme == AppTheme.Dark ? Colors.Black : Colors.White);

	public string CardTitle
	{
		get => (string)GetValue(CardTitleProperty);
		set => SetValue(CardTitleProperty, value);
	}

	public Color CardColor
	{
		get => (Color)GetValue(CardColorProperty);
		set => SetValue(CardColorProperty, value);
	}
}

// //////////////////////////////////////////////////////////////////////////////////////////////////
// //////////////////////////////////////////////////////////////////////////////////////////////////
// //////////////////////////////////////////////////////////////////////////////////////////////////
/*

	// https://github.com/ollama/ollama/blob/main/docs/api.md
	//string request = "http://localhost:11434/api/generate";
	//string content = $"{{" +
	//	"\"model\": \"llama3.1\"," +
	//	"\"prompt\": \"Why is the sky blue?\"" +
	//	"}";

	//{
	//	"model": "llama3.1",
	//	"created_at": "2024-09-16T06:07:39.2728134Z",
	//	"response": "Hello! How are you today? Is there something I can help you with, or would you like to just chat?",
	//	"done": true,
	//	"done_reason": "stop",
	//	"context": [128006, 882, 128007],
	//	"total_duration": 3520390800,
	//	"load_duration": 16736600,
	//	"prompt_eval_count": 11,
	//	"prompt_eval_duration": 329277000,
	//	"eval_count": 25,
	//	"eval_duration": 3173195000
	//}

public class OllamaRequest
{
	public string model;
	public string prompt;
	public bool stream;
}

public class OllamaResponse
{
	public string model;
	public DateTime created_at;
	public string response;
	public bool done;
	public string done_reason;
	public int[] context;
	public long total_duration;
	public long load_duration;
	public long prompt_eval_count;
	public long prompt_eval_duration;
	public long eval_count;
	public long eval_duration;
}
	
	public async Task<HttpResponseMessage> OllamaPostRequest(string model, string prompt)
	{
		var request = new OllamaRequest
		{
			model = model,
			prompt = prompt,
			stream = false
		};

		var jsonContent = JsonConvert.SerializeObject(request);
		var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
		string uri = "http://localhost:11434/api/generate"; // "https://api.ollama.com/v1/your-endpoint"
		var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
		{
			Content = content
		};

		// Optionally, add headers if needed
		// httpRequestMessage.Headers.Add("Authorization", "Bearer your-token");

		using (var httpClient = new HttpClient())
			return await httpClient.SendAsync(httpRequestMessage);
	}
	

		HttpResponseMessage response1 = await OllamaPostRequest("llama3.1", s);
		response1.EnsureSuccessStatusCode();
		string content = await response1.Content.ReadAsStringAsync();
		OllamaResponse or = JsonConvert.DeserializeObject<OllamaResponse>(content);
		_aiUserPrompts.Add(DiagramPage.SessionLog().Text);
		_aiChatReponses.Add(or.response);
		UpdateChat();
		UpdateSessionLog("", false, false);
		return;
		
		// Brain dead Ollama

		IOllamaApiClient _ollamaApiClient = new OllamaApiClient(new Uri("http://localhost:11434"), "llama3.1:latest");
		var chatRequest = new ChatRequest
		{
			Messages = new List<Message>
			{
				new Message { Content = s}
			}
		};
		string r = "";
		await foreach (var response2 in _ollamaApiClient.ChatAsync(chatRequest))
			r += response2.Message.Content;

		_aiUserPrompts.Add(DiagramPage.SessionLog().Text);
		_aiChatReponses.Add(r);
		UpdateChat();
		UpdateSessionLog("", false, false);
		return;

		string seq = "";
		if (m_hvsCurrent.Sequence < 8)
			seq = $"{m_hvsCurrent.Sequence + 1:0#}";
		else
			seq = $"{m_hvsCurrent.Sequence + 1:#}";

		s = $"Locate the following sections for the hexagram in DocumentId B-YiTran\n " +
			$"{seq}.X, Overall Image - hexagram image\n " +
			$"{seq}.0, {m_hvsCurrent.Label} - hexagram judgment\n " +
			$"{seq}.M, Keywords - hexagram keywords\n " +
			$"{seq}.G, Glossary - hexagram glossary\n " +
			$"{seq}.T, Tuan Zhuan - hexagram judgment commentary\n " +
			$"{seq}.1, 1st - first moving line\n " +
			$"{seq}.2, 2nd - second moving line\n " +
			$"{seq}.3, 3rd - third moving line\n " +
			$"{seq}.4, 4th - fourth moving line\n " +
			$"{seq}.5, 5th - fith moving line\n " +
			$"{seq}.6, Top - sixth moving line\n\n ";

		s += $"Please summarise all section for hexagram {m_hvsCurrent.DescribeCast()}";


		var description = new StringBuilder();
		var process = Process.GetCurrentProcess();
		foreach (ProcessModule module in process.Modules)
		{
			description.AppendLine(module.FileName);
		}

		Window[] w = Application.Current.Windows.ToArray();
		WindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(w[0]);

#if WINDOWS
		nint a = User32.GetActiveWindow();
		string b = Kernel32Extensions.GetMessage(Win32ErrorCode.ERROR_CREATE_FAILED);
		bool b1 = System.Runtime.GCSettings.IsServerGC;
		//AsyncInfo.Run(null);
		//interface iii = System.Runtime.InteropServices.ComTypes.IDataObject;
#endif

		DisplayAlert("Error", "Failed to locate document folder", "OK");
		string location = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "Download", "abc.jpg");
		m_strDocumentHome = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
		image.Source = ImageSource.FromFile(location);

		Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync("");

		Application.Current.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().SetImageDirectory("Assets");
		var y = Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific.Application.GetImageDirectory(btnEeg);

		File.Delete("");
		Directory.Delete("", true);
		DriveInfo.GetDrives();

public static async void ShareCast()
{
	Appointment appointment = new Appointment();
	appointment.Subject = "☯️ " + m_hvsCurrent.DescribeCast();
	appointment.DetailsKind = AppointmentDetailsKind.Html;
	appointment.Details = "Type: " + QuestionPage.Type + " Question: " + QuestionPage.Text +
		" https://www.microsoft.com/store/apps/9n5q9qxxh7wj https://play.google.com/store/apps/details?id=org.yijing";
	await AppointmentManager.ShowAddAppointmentAsync(appointment, default(Windows.Foundation.Rect));

	//appointment.Uri = new Uri("ms-windows-store://pdp/?productid=9wzdncrfj6qs");
	//appointment.Location = "";
	//Calendar calendar = new Calendar(new string[] { "en-US" }, CalendarIdentifiers.Gregorian, ClockIdentifiers.TwentyFourHour, "America/Los_Angeles");
	//calendar.GetCalendarSystem();
}
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Windows.ApplicationModel.Appointments;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

//using Neurosky;
//using Mindwave.Common;
//using MindSetUWA;

using ValueSequencer;
//using YijingDb;

namespace Yijing
{
	public sealed partial class DiagramPage : Page
	{

		public static DiagramPage m_dp;

		public static CHexagramValueSequencer m_hvsCurrent;
		public static CHexagramValueSequencer m_hvsCast;
		public static CHexagramValueSequencer m_hvsPrimary = null;
		public static CValueSequencer m_vsCurrent;

		public static int[] m_nSpeeds = { 2000, 600, 5 };

		public static int m_nDiagramColor = 2;
		public static int m_nDiagramSpeed = 1;
		/*
		public static int m_nDiagramLsb = 0;

		public static int m_nLineSequence = 1;
		public static int m_nLineRatio = 1;
		public static int m_nLineLabel = 3;
		public static int m_nLineText = 0;

		public static int m_nTrigramSequence = 1;
		public static int m_nTrigramRatio = 0;
		public static int m_nTrigramLabel = 2;
		public static int m_nTrigramText = 0;

		public static int m_nHexagramSequence = 2;
		public static int m_nHexagramRatio = 0;
		public static int m_nHexagramLabel = 9;
		public static int m_nHexagramText = 6;
		*/
		public static bool m_bTimerOn = false;
		public static bool m_bForward = true;
		public static ComboBoxItem m_cbiDiagramMode = null;

		private int m_nCurrentLine = 0;
		private int m_nCurrentTrigram = 0;

		private Rectangle[,] m_recLines = new Rectangle[6, 2];
		private Ellipse[] m_recLinesO = new Ellipse[6];
		private UIElement m_uieLine = null;

		private static Timer m_timDiagram = new Timer(DiagramTimer, null, Timeout.Infinite, 0);
		private Task m_tskAutoCast = null;

		//private static IMindwaveSensor m_imsDevice = Neurosky.Mindwave.Device;
		//private static MindSetConnection m_mscDevice = new MindSetConnection();
		private Task m_tskThinkGear = new Task(ThinkGearTask);

		private SolidColorBrush m_brBlue = new SolidColorBrush(Colors.Blue);
		private SolidColorBrush m_brBlack = new SolidColorBrush(Colors.Black);
		private SolidColorBrush m_brDarkGray = new SolidColorBrush(Color.FromArgb(0xFF, 0x40, 0x40, 0x40));
		private SolidColorBrush m_brLightGray = new SolidColorBrush(Colors.LightGray);
		private SolidColorBrush m_brWhite = new SolidColorBrush(Colors.White);
		private SolidColorBrush m_brRed = new SolidColorBrush(Colors.Red);
		private SolidColorBrush m_brMonoColor;
		private SolidColorBrush m_brMovingYang;
		private SolidColorBrush m_brSelectStoke;
		private SolidColorBrush m_brBackStoke;

		public DiagramPage()
		{
			m_dp = this;
			InitializeComponent();
			NavigationCacheMode = NavigationCacheMode.Enabled;

			m_brMonoColor = App.Current.RequestedTheme == ApplicationTheme.Dark ? m_brLightGray : m_brDarkGray;
			m_brMovingYang = App.Current.RequestedTheme == ApplicationTheme.Dark ? m_brBlack : m_brWhite;
			m_brSelectStoke = App.Current.RequestedTheme == ApplicationTheme.Dark ? m_brBlue : m_brRed;
			m_brBackStoke = App.Current.RequestedTheme == ApplicationTheme.Dark ? m_brWhite : m_brBlack;

			m_recLines[0, 0] = recLine0L;
			m_recLines[1, 0] = recLine1L;
			m_recLines[2, 0] = recLine2L;
			m_recLines[3, 0] = recLine3L;
			m_recLines[4, 0] = recLine4L;
			m_recLines[5, 0] = recLine5L;

			m_recLines[0, 1] = recLine0R;
			m_recLines[1, 1] = recLine1R;
			m_recLines[2, 1] = recLine2R;
			m_recLines[3, 1] = recLine3R;
			m_recLines[4, 1] = recLine4R;
			m_recLines[5, 1] = recLine5R;

			m_recLinesO[0] = recLine0O;
			m_recLinesO[1] = recLine1O;
			m_recLinesO[2] = recLine2O;
			m_recLinesO[3] = recLine3O;
			m_recLinesO[4] = recLine4O;
			m_recLinesO[5] = recLine5O;

			Sequences.Initialise();

			//YijingDbTest ydb = new YijingDbTest();
			//ydb.Main();
			//QueryDB();

			m_hvsCurrent = new CHexagramValueSequencer(0);

			//CLineValueSequencer.SetCurrentSequence(m_nLineSequence);
			//CLineValueSequencer.SetCurrentRatio(m_nLineRatio);
			//CLineValueSequencer.SetCurrentLabel(m_nLineLabel);

			//CTrigramValueSequencer.SetCurrentSequence(m_nTrigramSequence);
			//CTrigramValueSequencer.SetCurrentRatio(m_nTrigramRatio);
			//CTrigramValueSequencer.SetCurrentLabel(m_nTrigramLabel);

			//CHexagramValueSequencer.SetCurrentSequence(m_nHexagramSequence);
			//CHexagramValueSequencer.SetCurrentRatio(m_nHexagramRatio);
			//CHexagramValueSequencer.SetCurrentLabel(m_nHexagramLabel);

			SetDiagramLsb(0);

			m_hvsCurrent.First();
			m_vsCurrent = m_hvsCurrent;
			cbiHexagram.IsSelected = true;
			cbxDiagramType.SelectedItem = cbiHexagram;
			UpdateDiagrams(true);

			//m_tskThinkGear.Start();
		}

		/// <summary>
		/// ////////////////////////////////////////
		/// ////////////////////////////////////////
		/// ////////////////////////////////////////
		/// </summary>
		/*
		public void QueryDB()
		{
			using (YijingEntities ye = new YijingEntities())
			{
				YijingDb.Type t = ye.Types.Find(3);
				LabelSery ls2 = ye.LabelSeries.Where(ls1 => ls1.Name == "Vivash" &&
					ls1.TypeId == (int)Sequences.ValueType.Hexagram).First(); // .OrderBy(ls1 => ls1.Name)
				IEnumerable<LabelSery> iels = from ls3 in ye.LabelSeries
					where ls3.TypeId == (int)Sequences.ValueType.Line orderby ls3.Name select ls3;
				List<LabelSery> lls = iels.ToList();
				LabelSery ls4 = ye.LabelSeries.Single(ls5 => ls5.Name == "Vivash"); // Vivash Numeric
				List<Label> ll1 = ls2.Labels.ToList();
				t = ls2.Type;
				int x = 0;
			}
		}
		*/

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
		}

		private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (ActualHeight < 500)
			{
				cbxDiagramSpeed.Visibility = Visibility.Collapsed;
				cbxDiagramColor.Visibility = Visibility.Collapsed;
			}
			else
			{
				cbxDiagramSpeed.Visibility = Visibility.Visible;
				cbxDiagramColor.Visibility = Visibility.Visible;
			}
			if (ActualWidth <= 380)
			{
				btnFirst.Width = 34;
				btnLast.Width = 34;
				btnInverse.Width = 34;
				btnOpposite.Width = 34;
				btnTransverse.Width = 34;
				btnNuclear.Width = 34;
				btnMove.Width = 34;
				btnHome.Width = 34;
			}
			else
			{
				btnFirst.Width = 48; // 50
				btnLast.Width = 48;
				btnInverse.Width = 48;
				btnOpposite.Width = 48;
				btnTransverse.Width = 48;
				btnNuclear.Width = 48;
				btnMove.Width = 48;
				btnHome.Width = 48;
			}
		}

		private void recDiagram_PointerPressed(object sender, PointerRoutedEventArgs e)
		{
			if (cbxDiagramMode.SelectedItem != cbiAutoCast)
			{
				m_bForward = e.GetCurrentPoint(grdDiagram).Position.X > 140;
				SetCurrentLine(Int32.Parse(((Rectangle)sender).Name.Substring(7, 1)),false);
				cbxDiagramType_SelectionChanged(null, null);
				if (!m_bTimerOn)
				{
					m_bTimerOn = true;
					if (sender != null)
					{
						m_uieLine = (UIElement)sender;
						m_uieLine.CapturePointer(e.Pointer);
					}
					m_timDiagram.Change(0, m_nSpeeds[cbxDiagramSpeed.SelectedIndex]); // m_nSpeed
				}
			}
		}

		private void recDiagram_PointerReleased(object sender, PointerRoutedEventArgs e)
		{
			if (cbxDiagramMode.SelectedItem == cbiExplore)
			{
				m_hvsPrimary = null;
				//SetTextUrl(m_hvsCurrent, m_nHexagramLabel);
			}
			if (m_bTimerOn && (cbxDiagramMode.SelectedItem != cbiAnimate) && (cbxDiagramMode.SelectedItem != cbiAutoCast))
			{
				m_bTimerOn = false;
				if (m_uieLine != null)
					m_uieLine.ReleasePointerCapture(e.Pointer);
				m_uieLine = null;
				m_timDiagram.Change(Timeout.Infinite, 0);
				if ((cbxDiagramMode.SelectedItem == cbiTouchCast) && (m_nCurrentLine == 5))
					EndCast();
			}
		}

		private void cbxDiagramMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cbxDiagramType != null)
			{
				m_cbiDiagramMode = (ComboBoxItem)cbxDiagramMode.SelectedItem;
				EnableControls(true);
				if (cbxDiagramMode.SelectedItem == cbiAnimate)
				{
					m_timDiagram.Change(0, m_nSpeeds[cbxDiagramSpeed.SelectedIndex]);
					m_bTimerOn = true;
				}
				else
				{
					if (m_bTimerOn)
						m_timDiagram.Change(Timeout.Infinite, 0);
					m_bTimerOn = false;
					if ((cbxDiagramMode.SelectedItem == cbiTouchCast) || (cbxDiagramMode.SelectedItem == cbiAutoCast))
					{
						SetCurrentLine(0, true);
						cbxDiagramType.SelectedItem = cbiLine;
						cbxDiagramSpeed.SelectedItem = cbiFast;
						EnableControls(false);
					}
					if (cbxDiagramMode.SelectedItem == cbiTouchCast)
						cbxDiagramMode.IsEnabled = true;
					if (cbxDiagramMode.SelectedItem == cbiAutoCast)
					{
						//bool b = m_imsDevice.IsConnected;
						m_tskAutoCast = new Task(AutoCast);
						m_tskAutoCast.Start();
					}
				}
			}
		}

		private void cbxDiagramType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if ((cbxDiagramType != null) && (m_hvsCurrent != null))
			{
				if (cbxDiagramType.SelectedItem == cbiLine)
					m_vsCurrent = m_hvsCurrent.Trigram(m_nCurrentTrigram).Line(m_nCurrentLine % 3);
				else
					if (cbxDiagramType.SelectedItem == cbiTrigram)
						m_vsCurrent = m_hvsCurrent.Trigram(m_nCurrentTrigram);
					else
						m_vsCurrent = m_hvsCurrent;
				UpdateDiagrams(true);
			}
		}

		private void cbxDiagramColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cbxDiagramColor != null)
				m_nDiagramColor = cbxDiagramColor.SelectedIndex;
			if (cbxDiagramSpeed != null)
				UpdateDiagrams(true);
		}

		private void cbxDiagramSpeed_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cbxDiagramSpeed != null)
				m_nDiagramSpeed = cbxDiagramSpeed.SelectedIndex;
			if (m_bTimerOn)
				m_timDiagram.Change(0, m_nSpeeds[cbxDiagramSpeed.SelectedIndex]);
		}

		private void btnFirst_Click(object sender, RoutedEventArgs e)
		{
			SetFirst();
		}

		private void btnLast_Click(object sender, RoutedEventArgs e)
		{
			SetLast();
		}

		private void btnInverse_Click(object sender, RoutedEventArgs e)
		{
			SetInverse();
		}

		private void btnOpposite_Click(object sender, RoutedEventArgs e)
		{
			SetOpposite();
		}

		private void btnTransverse_Click(object sender, RoutedEventArgs e)
		{
			SetTransverse();
		}

		private void btnNuclear_Click(object sender, RoutedEventArgs e)
		{
			SetNuclear();
		}

		private void btnMove_Click(object sender, RoutedEventArgs e)
		{
			SetMove();
		}

		private void btnHome_Click(object sender, RoutedEventArgs e)
		{
			SetHome();
		}

		public static void SetDiagramColor(int nColor)
		{
			m_dp.cbxDiagramColor.SelectedIndex = nColor;
		}

		public static void SetDiagramSpeed(int nSpeed)
		{
			m_dp.cbxDiagramSpeed.SelectedIndex = nSpeed;
		}

		public static async void SetDiagramLsb(int nLsb)
		{
			Sequences.DiagramLsb = nLsb;
			Sequences.SetLSB(Sequences.DiagramLsb == 0);
			m_hvsCurrent.Update();
			await m_dp.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, UpdateDiagrams);
		}

		public static async void SetLineSequence(int nSequence)
		{
			Sequences.LineSequence = nSequence;
			CLineValueSequencer.SetCurrentSequence(nSequence);
			m_hvsCurrent.Update();
			await m_dp.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, UpdateDiagrams);
		}

		public static async void SetLineRatio(int nRatio)
		{
			Sequences.LineRatio = nRatio;
			CLineValueSequencer.SetCurrentRatio(nRatio);
			m_hvsCurrent.Update();
			await m_dp.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, UpdateDiagrams);
		}

		public static async void SetLineLabel(int nLabel)
		{
			Sequences.LineLabel = nLabel;
			CLineValueSequencer.SetCurrentLabel(nLabel);
			await m_dp.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, UpdateDiagrams);
		}

		public static void SetLineText(int nText)
		{
			Sequences.LineText = nText;
		}

		public static async void SetTrigramSequence(int nSequence)
		{
			Sequences.TrigramSequence = nSequence;
			CTrigramValueSequencer.SetCurrentSequence(nSequence);
			m_hvsCurrent.Update();
			await m_dp.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, UpdateDiagrams);
		}

		public static async void SetTrigramRatio(int nRatio)
		{
			Sequences.TrigramRatio = nRatio;
			CTrigramValueSequencer.SetCurrentRatio(nRatio);
			m_hvsCurrent.Update();
			await m_dp.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, UpdateDiagrams);
		}

		public static async void SetTrigramLabel(int nLabel)
		{
			Sequences.TrigramLabel = nLabel;
			CTrigramValueSequencer.SetCurrentLabel(nLabel);
			await m_dp.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, UpdateDiagrams);
		}

		public static void SetTrigramText(int nText)
		{
			Sequences.TrigramText = nText;
		}

		public static async void SetHexagramSequence(int nSequence)
		{
			Sequences.HexagramSequence = nSequence;
			CHexagramValueSequencer.SetCurrentSequence(nSequence);
			m_hvsCurrent.Update();
			await m_dp.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, UpdateDiagrams);
		}

		public static async void SetHexagramRatio(int nRatio)
		{
			Sequences.HexagramRatio = nRatio;
			CHexagramValueSequencer.SetCurrentRatio(nRatio);
			m_hvsCurrent.Update();
			await m_dp.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, UpdateDiagrams);
		}

		public static async void SetHexagramLabel(int nLabel)
		{
			Sequences.HexagramLabel = nLabel;
			CHexagramValueSequencer.SetCurrentLabel(nLabel);
			await m_dp.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, UpdateDiagrams);
		}

		public static void SetHexagramText(int nText)
		{
			Sequences.HexagramText = nText;
		}

		public static async void SetHexagramValue(int nValue)
		{
			m_hvsCurrent.Value = nValue;
			m_hvsCurrent.Update();
			await m_dp.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, UpdateDiagrams);
		}

		private static void Transition()
		{
			m_hvsPrimary = null;
			UpdateDiagrams();
		}

		public static void SetFirst()
		{
			m_vsCurrent.First();
			Transition();
		}

		public static void SetPrevious()
		{
			m_vsCurrent.Previous();
			Transition();
		}

		public static void SetNext()
		{
			m_vsCurrent.Next();
			Transition();
		}

		public static void SetLast()
		{
			m_vsCurrent.Last();
			Transition();
		}

		public static void SetInverse()
		{
			m_vsCurrent.Inverse();
			Transition();
		}

		public static void SetOpposite()
		{
			m_vsCurrent.Opposite();
			Transition();
		}

		public static void SetTransverse()
		{
			m_vsCurrent.Transverse();
			Transition();
		}

		public static void SetNuclear()
		{
			m_vsCurrent.Nuclear();
			Transition();
		}

		private static void SetMove()
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
			UpdateDiagrams();
		}

		public static void SetHome()
		{
			if (m_hvsCast != null)
			{
				m_hvsCurrent = new CHexagramValueSequencer(ref m_hvsCast);
				m_vsCurrent = m_hvsCurrent;
				UpdateDiagrams();
			}
		}

		private void SetCurrentLine(int nLine, bool bDiagram)
		{
			m_nCurrentLine = nLine;
			m_nCurrentTrigram = m_nCurrentLine / 3;
			if (bDiagram)
				m_vsCurrent = m_hvsCurrent.Trigram(nLine / 3).Line(nLine % 3);
		}

		private void EnableControls(bool bEnable)
		{
			MainPage.EnableControls(bEnable);
			SettingsPage.EnableControls(bEnable);
			cbxDiagramMode.IsEnabled = bEnable;
			cbxDiagramType.IsEnabled = bEnable;
			cbxDiagramSpeed.IsEnabled = bEnable;
			btnFirst.IsEnabled = bEnable;
			btnLast.IsEnabled = bEnable;
			btnInverse.IsEnabled = bEnable;
			btnOpposite.IsEnabled = bEnable;
			btnTransverse.IsEnabled = bEnable;
			btnNuclear.IsEnabled = bEnable;
			btnMove.IsEnabled = bEnable;
			btnHome.IsEnabled = bEnable;
		}

		private SolidColorBrush TrigramColor(int nValue)
		{
			switch (nValue)
			{
				case 0: // earth
					return new SolidColorBrush(Color.FromArgb(0xFF, 0x40, 0x40, 0x40));
				case 1: // thunder
					return new SolidColorBrush(Colors.Green);
				case 2: // water
					return new SolidColorBrush(Colors.Blue);
				case 3: // lake
					return new SolidColorBrush(Colors.Cyan);
				case 4: // mountain
					return new SolidColorBrush(Colors.Magenta);
				case 5: // fire
					return new SolidColorBrush(Colors.Red);
				case 6: // wind
					return new SolidColorBrush(Colors.Yellow);
				case 7: // heaven
					break;
			}
			return new SolidColorBrush(Colors.LightGray);
		}

		private SolidColorBrush RgbColor()
		{
			bool bLower, bUpper;
			byte[] nRgb = { 0, 0, 0 };
			for (int i = 0; i < 3; ++i)
			{
				bUpper = m_hvsCurrent.Trigram(1).Line(i).Value % 2 == 1 ? true : false;
				bLower = m_hvsCurrent.Trigram(0).Line(i).Value % 2 == 1 ? true : false;
				nRgb[i] = (byte) (bLower ? bUpper ? 0xFF : 0x55 : bUpper ? 0xAA : 0x00);
			}
			if ((nRgb[0] == 0) && (nRgb[1] == 0) && (nRgb[2] == 0))
				return new SolidColorBrush(Color.FromArgb(0xFF, 0x40, 0x40, 0x40));
			if ((nRgb[0] == 255) && (nRgb[1] == 255) && (nRgb[2] == 255))
				return new SolidColorBrush(Colors.LightGray);
			return new SolidColorBrush(Color.FromArgb(0xFF, nRgb[0], nRgb[1], nRgb[2]));
		}

		private void UpdateDiagrams(bool bAll)
		{
			SolidColorBrush m_brDiagram = RgbColor();
			for (int i = 0; i < 6; ++i)
			{
				bool bSpecific = (cbiLine.IsSelected && (m_nCurrentLine == i)) ||
					(cbiTrigram.IsSelected && (m_nCurrentTrigram == (i / 3))) ||
					cbiHexagram.IsSelected;
				if (bAll || bSpecific)
				{
					int nValue = m_hvsCurrent.Trigram(i / 3).Line(i % 3).Value;
					bool bYang = nValue % 2 == 1;
					bool bOld = (nValue == 0) || (nValue == 3);

					if (cbxDiagramColor.SelectedItem == cbiTriColor)
						m_brDiagram = TrigramColor(m_hvsCurrent.Trigram(i / 3).Value);
					else
						m_brDiagram = (cbxDiagramColor.SelectedItem == cbiMono) ? m_brMonoColor : 
							(cbxDiagramColor.SelectedItem == cbiDual) ?	bYang ? m_brLightGray : m_brDarkGray : m_brDiagram;

					m_recLines[i, 0].Fill = m_brDiagram;
					m_recLines[i, 0].Width = bYang ? 290 : 118;
					m_recLines[i, 0].Opacity = 1;
					m_recLines[i, 0].Stroke = bSpecific ? m_brSelectStoke : m_brBackStoke;

					m_recLinesO[i].Fill = bYang ? m_brMovingYang : (cbxDiagramColor.SelectedItem == cbiMono) ? m_brMonoColor :
						(cbxDiagramColor.SelectedItem == cbiDual) ? m_brDarkGray : m_brDiagram;
					m_recLinesO[i].Width = bYang & !bOld ? 0 : 50;
					m_recLinesO[i].Opacity = bOld ? /*0.9*/1 : 0;
					m_recLinesO[i].Stroke = bSpecific ? m_brSelectStoke : m_brBackStoke;

					m_recLines[i, 1].Fill = m_brDiagram;
					m_recLines[i, 1].Width = bYang ? 0 : 118;
					m_recLines[i, 1].Opacity = 1;
					m_recLines[i, 1].Stroke = bSpecific ? m_brSelectStoke : m_brBackStoke;
				}
				txtHexagram.Text = m_hvsCurrent.SequenceStr + ". " + m_hvsCurrent.Label + " (" + m_hvsCurrent.ValueStr + ")";
			}
		}

		private static void UpdateDiagrams()
		{
			m_dp.UpdateDiagrams(true);
		}

		public static String DescribeCast()
		{
			String strTemp = m_hvsCurrent.SequenceStr;
			for (int i = 0; i < 6; ++i)
				if (m_hvsCurrent.Trigram(i / 3).Line(i % 3).IsMoving)
					strTemp += "." + (i + 1).ToString();
			strTemp += " " + m_hvsCurrent.Label;
			if (m_hvsCurrent.IsMoving)
			{
				CHexagramValueSequencer hvsSecondary = new CHexagramValueSequencer(ref m_hvsCurrent);
				hvsSecondary.Move();
				strTemp += " > " + hvsSecondary.SequenceStr + " " + hvsSecondary.Label;
			}
			return strTemp;
		} 

		private async static void EndCast()
		{
			String strTemp = "#Yijing ☯️ " + DescribeCast() + "\n\nW10 v5.2.9  ";
			strTemp += m_dp.cbxDiagramMode.SelectionBoxItem + " " + Sequences.DiagramSetting(13, Sequences.HexagramSequence) + " " +
				Sequences.DiagramSetting(4, 00000000000) + " " + Sequences.DiagramSetting(6, Sequences.LineRatio) + " " +
				Sequences.DiagramSetting(15, Sequences.HexagramLabel) + "\n\nhttps://www.microsoft.com/store/apps/9n5q9qxxh7wj\n\n";

			m_hvsCast = new CHexagramValueSequencer(ref m_hvsCurrent);
			m_vsCurrent = m_hvsCurrent;

			m_dp.cbxDiagramMode.SelectedItem = m_dp.cbiExplore;
			m_dp.cbxDiagramType.SelectedItem = m_dp.cbiHexagram;
			m_dp.cbxDiagramSpeed.SelectedItem = m_dp.cbiMedium;
			m_dp.EnableControls(true);

			await m_dp.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, UpdateDiagrams);
			await Task.Delay(1000);
			MainPage.NavigateTo(typeof(TextPage));
		}

		public static async void ShareCast()
		{
			Appointment appointment = new Appointment();
			appointment.Subject = "☯️ " + DescribeCast();
			appointment.DetailsKind = AppointmentDetailsKind.Html;
			appointment.Details = "Type: " + QuestionPage.Type + " Question: " + QuestionPage.Text +
				" https://www.microsoft.com/store/apps/9n5q9qxxh7wj https://play.google.com/store/apps/details?id=org.yijing";
			await AppointmentManager.ShowAddAppointmentAsync(appointment, default(Windows.Foundation.Rect));

			//appointment.Uri = new Uri("ms-windows-store://pdp/?productid=9wzdncrfj6qs");
			//appointment.Location = "-35.711027, 150.181490";
			//Calendar calendar = new Calendar(new string[] { "en-US" }, CalendarIdentifiers.Gregorian, ClockIdentifiers.TwentyFourHour, "America/Los_Angeles");
			//calendar.GetCalendarSystem();
		}

		private static async void DiagramTimer(object state)
		{
			bool bRatio = (m_cbiDiagramMode == (ComboBoxItem)m_dp.cbiTouchCast) || (m_cbiDiagramMode == (ComboBoxItem)m_dp.cbiAutoCast);
			if (m_bForward)
				m_vsCurrent.Next(bRatio);
			else
				m_vsCurrent.Previous(bRatio);
			await m_dp.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, UpdateDiagrams);
		}

		private static async void MindCast()
		{
#if THINKGEAR
			Random r = new Random(DateTime.Now.Millisecond);
			for (int i = 0; i < 6; ++i)
			{
				m_dp.SetCurrentLine(i, true);
				while (m_tg.Meditation() <= 60)
				{
					await Task.Delay(100 + r.Next(500));
					await m_dp.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, UpdateDiagrams);
				}
				m_timDiagram.Change(0, m_nSpeeds[m_nSpeed]);
				while (m_tg.Meditation() >= 50) 
					await Task.Delay(100 + r.Next(500));
				m_timDiagram.Change(Timeout.Infinite, 0);
				await m_dp.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, UpdateDiagrams);
				await Task.Delay(500);
			}
			await m_dp.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, EndCast);
#else
			await m_dp.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, UpdateDiagrams);
#endif
		}

		private static async void AutoCast()
		{
			Random r = new Random(DateTime.Now.Millisecond);
			for (int i = 0; i < 6; ++i)
			{
				m_dp.SetCurrentLine(i, true);
				m_timDiagram.Change(0, m_nSpeeds[2]); // m_dp.cbxDiagramSpeed.SelectedIndex 
				await Task.Delay((r.Next(5) + 1) * 1000 + r.Next(1000));
				m_timDiagram.Change(Timeout.Infinite, 0);
			}
			await m_dp.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, EndCast);
		}

		public async static void ThinkGearTask()
		{
			//m_mscDevice.ConnectBluetooth(""); // MindWave Mobile
											  //m_imsDevice.Start(MindwaveProtocol.ThinkGear);
			while (true)
			{
				await Task.Delay(1000);
				//EMindSetStatus emss = m_mscDevice.ConnectionStatus();
				//bool b = m_imsDevice.IsConnected;
				//MindwaveReading mr = m_imsDevice.CurrentValue;
			}
		}

		private static void Tweet(String strDescription)
		{
		}
	}
/*
	class SafeNativeMethods
	{
		public int CP_ACP = 0;
		public int CP_OEMCP = 1;
		public int CP_MACCP = 2;
		public int CP_THREAD_ACP = 3;
		public int CP_SYMBOL = 42;
		public int CP_UTF7 = 65000;
		public int CP_UTF8 = 65001;

		[DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		public static extern int MultiByteToWideChar(Int32 CodePage, Int32 dwFlags, Byte[] lpMultiByte, Int32 cchMultiByte,
			String lpWideCharStr, Int32 cchWideChar);

		[DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		public static extern int WideCharToMultiByte(Int32 CodePage, Int32 dwFlags, String lpWideCharStr, Int32 cchWideChar,
			Byte[] lpMultiByte, Int32 cchMultiByte, String lpDefaultChar, Int32 lpUsedDefaultChar);

		[DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		public static extern int GetPrivateProfileStringW(String lpAppName, String lpKeyName, String lpDefault,
			StringBuilder lpReturnedString, Int32 nSize, String lpFileName);
	}


		UIElement x1 = Window.Current.Content;
		DependencyObject x2 = Frame.Parent;
		DependencyProperty dp = SettingsPage.FrameProperty;
		ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
		if (titleBar != null)
		{
			//Color titleBarColor = (Color)App.Current.Resources["SystemChromeMediumColor"];
			//titleBar.BackgroundColor = titleBarColor;
		}

		DependencyObject doParent = VisualTreeHelper.GetParent(this);
		DiagramPage hp;
		hp = VisualTreeHelper.GetChild(doParent, 0) as DiagramPage;

		DependencyObject parent = VisualTreeHelper.GetParent(this);
		while (parent != null)
		{
			parent = VisualTreeHelper.GetParent(parent);
		}
		AppShell shell = Window.Current.Content as AppShell;
		Window.Current.Content = shell;
		if (shell.AppFrame.Content == null)
			shell.AppFrame.Navigate(typeof(LandingPage), e.Arguments, new Windows.UI.Xaml.Media.Animation.SuppressNavigationTransitionInfo());

		Application.Current.Exit();
		Type t = Parent.GetType();
		double d1 = BottomAppBar.ActualHeight;
		Visibility v = Window.Current.Content.Visibility;
		SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
		TopAppBar.Visibility = Visibility.Collapsed;
		BottomAppBar.Visibility = Visibility.Collapsed;
		double d1 = Frame.ActualWidth;
		int x = Frame.TabNavigation;
		DependencyProperty x1 = HideablePivotItemBehavior.VisibleProperty;
		App.Current.DebugSettings.IsTextPerformanceVisualizationEnabled = true;
		AppBar.VisibilityProperty
*/
	}

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using ValueSequencer;

namespace Yijing
{
	public sealed partial class SettingsPage : Page
	{
		public static SettingsPage m_sp;

		public SettingsPage()
		{
			m_sp = this;
			InitializeComponent();
			NavigationCacheMode = NavigationCacheMode.Enabled;

			int nLength = Sequences.strDiagramSettings.Length / 17;
			for (int i = 1; i < nLength; ++i)
				if (Sequences.strDiagramSettings[5, i].Length > 0)
					cbxLineSequence.Items.Add(Sequences.strDiagramSettings[5, i]);
			cbxLineSequence.SelectedIndex = Sequences.LineSequence;

			for (int i = 1; i < nLength; ++i)
				if (Sequences.strDiagramSettings[6, i].Length > 0)
					cbxLineRatio.Items.Add(Sequences.strDiagramSettings[6, i]);
			cbxLineRatio.SelectedIndex = Sequences.LineRatio;

			for (int i = 1; i < nLength; ++i)
				if (Sequences.strDiagramSettings[7, i].Length > 0)
					cbxLineLabel.Items.Add(Sequences.strDiagramSettings[7, i]);
			cbxLineLabel.SelectedIndex = Sequences.LineLabel;

			for (int i = 1; i < nLength; ++i)
				if (Sequences.strDiagramSettings[8, i].Length > 0)
					cbxLineText.Items.Add(Sequences.strDiagramSettings[8, i]);
			cbxLineText.SelectedIndex = Sequences.LineText;

			for (int i = 1; i < nLength; ++i)
				if (Sequences.strDiagramSettings[9, i].Length > 0)
					cbxTrigramSequence.Items.Add(Sequences.strDiagramSettings[9, i]);
			cbxTrigramSequence.SelectedIndex = Sequences.TrigramSequence;

			for (int i = 1; i < nLength; ++i)
				if (Sequences.strDiagramSettings[10, i].Length > 0)
					cbxTrigramRatio.Items.Add(Sequences.strDiagramSettings[10, i]);
			cbxTrigramRatio.SelectedIndex = Sequences.TrigramRatio;

			for (int i = 1; i < nLength; ++i)
				if (Sequences.strDiagramSettings[11, i].Length > 0)
					cbxTrigramLabel.Items.Add(Sequences.strDiagramSettings[11, i]);
			cbxTrigramLabel.SelectedIndex = Sequences.TrigramLabel;

			for (int i = 1; i < nLength; ++i)
				if (Sequences.strDiagramSettings[12, i].Length > 0)
					cbxTrigramText.Items.Add(Sequences.strDiagramSettings[12, i]);
			cbxTrigramText.SelectedIndex = Sequences.TrigramText;

			for (int i = 1; i < nLength; ++i)
				if (Sequences.strDiagramSettings[13, i].Length > 0)
					cbxHexagramSequence.Items.Add(Sequences.strDiagramSettings[13, i]);
			cbxHexagramSequence.SelectedIndex = Sequences.HexagramSequence;

			for (int i = 1; i < nLength; ++i)
				if (Sequences.strDiagramSettings[14, i].Length > 0)
					cbxHexagramRatio.Items.Add(Sequences.strDiagramSettings[14, i]);
			cbxHexagramRatio.SelectedIndex = Sequences.HexagramRatio;

			for (int i = 1; i < nLength; ++i)
				if (Sequences.strDiagramSettings[15, i].Length > 0)
					cbxHexagramLabel.Items.Add(Sequences.strDiagramSettings[15, i]);
			cbxHexagramLabel.SelectedIndex = Sequences.HexagramLabel;

			for (int i = 1; i < nLength; ++i)
				if (Sequences.strDiagramSettings[16, i].Length > 0)
					cbxHexagramText.Items.Add(Sequences.strDiagramSettings[16, i]);
			cbxHexagramText.SelectedIndex = Sequences.HexagramText;
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			cbxDiagramColor.SelectedIndex = DiagramPage.m_nDiagramColor;
			cbxDiagramSpeed.SelectedIndex = DiagramPage.m_nDiagramSpeed;
			cbxDiagramLsb.SelectedIndex = Sequences.DiagramLsb;
		}

		private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (ActualHeight >= 500)
			{
				//lbxSettings.Items.RemoveAt(0);
				//((StackPanel)lbxSettings.Items[0]).Visibility = Visibility.Collapsed;
			}
		}

		private void cbxDiagramColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cbxDiagramColor != null)
				DiagramPage.SetDiagramColor(cbxDiagramColor.SelectedIndex);
		}

		private void cbxDiagramSpeed_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cbxDiagramSpeed != null)
				DiagramPage.SetDiagramSpeed(cbxDiagramSpeed.SelectedIndex);
		}

		private void cbxDiagramLsb_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cbxDiagramLsb != null)
				DiagramPage.SetDiagramLsb(cbxDiagramLsb.SelectedIndex);
		}

		private void cbxLineSequence_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cbxLineSequence != null)
				DiagramPage.SetLineSequence(cbxLineSequence.SelectedIndex);
		}

		private void cbxLineRatio_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cbxLineRatio != null)
				DiagramPage.SetLineRatio(cbxLineRatio.SelectedIndex);
		}

		private void cbxLineLabel_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cbxLineLabel != null)
				DiagramPage.SetLineLabel(cbxLineLabel.SelectedIndex);
		}

		private void cbxLineText_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cbxLineText != null)
				DiagramPage.SetLineText(cbxLineText.SelectedIndex);
		}

		private void cbxTrigramSequence_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cbxTrigramSequence != null)
				DiagramPage.SetTrigramSequence(cbxTrigramSequence.SelectedIndex);
		}

		private void cbxTrigramRatio_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cbxTrigramRatio != null)
				DiagramPage.SetTrigramRatio(cbxTrigramRatio.SelectedIndex);
		}

		private void cbxTrigramLabel_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cbxTrigramLabel != null)
				DiagramPage.SetTrigramLabel(cbxTrigramLabel.SelectedIndex);
		}

		private void cbxTrigramText_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cbxTrigramText != null)
				DiagramPage.SetTrigramText(cbxTrigramText.SelectedIndex);
		}

		private void cbxHexagramSequence_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cbxHexagramSequence != null)
				DiagramPage.SetHexagramSequence(cbxHexagramSequence.SelectedIndex);
		}

		private void cbxHexagramRatio_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cbxHexagramRatio != null)
				DiagramPage.SetHexagramRatio(cbxHexagramRatio.SelectedIndex);
		}

		private void cbxHexagramLabel_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cbxHexagramLabel != null)
				DiagramPage.SetHexagramLabel(cbxHexagramLabel.SelectedIndex);
		}

		private void cbxHexagramText_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cbxHexagramText != null)
				DiagramPage.SetHexagramText(cbxHexagramText.SelectedIndex);
		}

		public static void EnableControls(bool bEnable)
		{
			m_sp.cbxDiagramSpeed.IsEnabled = bEnable;
		}

	}
}

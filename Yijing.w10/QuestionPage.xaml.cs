using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Yijing
{
	public sealed partial class QuestionPage : Page
	{
		public static QuestionPage m_qp;
		private static int m_nType = 0;
		private static String m_strText = "";

		public QuestionPage()
		{
			m_qp = this;
			this.InitializeComponent();
		}

		public static String Type
		{
			get { return ((ComboBoxItem) m_qp.cbxType.SelectedItem).Content.ToString();	}
		}

		public static String Text
		{
			get { return m_strText;	}
			set { m_strText = value; }
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			cbxType.SelectedIndex = m_nType;
			txtQuestion.Text = m_strText;
		}

		private void Page_Unloaded(object sender, RoutedEventArgs e)
		{
			m_nType = cbxType.SelectedIndex;
			m_strText = txtQuestion.Text;
		}

		private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
		{
		}

		private void cbxType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
		}

		private void btnAccept_Click(object sender, RoutedEventArgs e)
		{
			MainPage.NavigateTo(typeof(DiagramPage));
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			cbxType.SelectedItem = cbiPersonal;
			txtQuestion.Text = "";
		}
	}
}

#include "StdAfx.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

CTextEditDlg::CTextEditDlg(CWnd* pParent) : CDialog(CTextEditDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CTextEditDlg)
	//}}AFX_DATA_INIT
}

void CTextEditDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CTextEditDlg)
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CTextEditDlg, CDialog)
	//{{AFX_MSG_MAP(CTextEditDlg)
	ON_CBN_SELCHANGE(IDC_TE_OPTIONS,OnTextTypeChanged)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

BOOL CTextEditDlg::OnInitDialog()
{
	CDialog::OnInitDialog();
	CComboBox* pedcOptions = (CComboBox*) GetDlgItem(IDC_TE_OPTIONS);
	for(int nTextType = 0; !g_strTextTypes[nTextType].IsEmpty(); ++nTextType)
		pedcOptions->AddString(g_strTextTypes[nTextType]);
	pedcOptions->SetCurSel(0);
	m_strTextType = g_strTextTypes[0];
	CRichEditCtrl* precText = (CRichEditCtrl*) GetDlgItem(IDC_TE_TEXT);
	precText->SetWordWrapMode(WBF_WORDBREAK);
	precText->SetFont(XTPPaintManager()->GetRegularFont());
	OnTextTypeChanged();
	return TRUE;
}

void CTextEditDlg::OnTextTypeChanged()
{
	CTextColumnSet TCS;
	CString strTemp;
	CComboBox* pedcOptions = (CComboBox*) GetDlgItem(IDC_TE_OPTIONS);
	CRichEditCtrl* precText = (CRichEditCtrl*) GetDlgItem(IDC_TE_TEXT);
	if(precText->GetModify())
		SaveText();
	pedcOptions->GetWindowText(m_strTextType);
	strTemp.Format(_T("select V%d from Text6 where Type = '%s' and Sequence = 100"),
		::GetApp()->GetCurrentHexagram()->GetValue(),m_strTextType);
	if(TCS.OpenRowset(::GetApp()->GetBeliefBase(),strTemp))
		precText->SetWindowText(TCS.m_szText);
	precText->SetModify(FALSE);
}

void CTextEditDlg::OnOK()
{
	CRichEditCtrl* precText = (CRichEditCtrl*) GetDlgItem(IDC_TE_TEXT);
	if(precText->GetModify())
		SaveText();
	::GetApp()->UpdateAllViews();
	CDialog::OnOK();
}

bool CTextEditDlg::SaveText()
{
	CTextColumnSet TCS;
	CString strTemp1,strTemp2;
	CRichEditCtrl* precText = (CRichEditCtrl*) GetDlgItem(IDC_TE_TEXT);
	precText->GetWindowText(strTemp1);
	strTemp1 = ReplaceString(strTemp1,_T("''"),_T("'"));
	strTemp2.Format(_T("update Text6 set V%d = '%s' where Type = '%s' and Sequence = 100"),
		::GetApp()->GetCurrentHexagram()->GetValue(),strTemp1,m_strTextType);
	if(TCS.Execute(::GetApp()->GetBeliefBase(),strTemp2))
		return true;
	return false;
}

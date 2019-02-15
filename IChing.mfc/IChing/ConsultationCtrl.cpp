#include "StdAfx.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

BEGIN_MESSAGE_MAP(CConsultationCtrl, CFormView)
	ON_WM_CREATE()
	ON_BN_CLICKED(IDC_CF_CAST,OnClickCast)
	ON_BN_CLICKED(IDC_CF_PRIMARY,OnClickPrimary)
	ON_BN_CLICKED(IDC_CF_SECONDARY,OnClickSecondary)
END_MESSAGE_MAP()

IMPLEMENT_DYNCREATE(CConsultationCtrl, CFormView)

CConsultationCtrl::CConsultationCtrl() : CFormView(IDD_CONSULTATION_FORM)
{
}

CConsultationCtrl::~CConsultationCtrl()
{
}

BOOL CConsultationCtrl::PreCreateWindow(CREATESTRUCT& cs)
{
	return CFormView::PreCreateWindow(cs);
}

int CConsultationCtrl::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if(CFormView::OnCreate(lpCreateStruct) != -1)
		return 0;
	return -1;
}

void CConsultationCtrl::OnInitialUpdate()
{
	CFormView::OnInitialUpdate();
//	SetFont(XTPPaintManager()->GetRegularFont());

	GetDlgItem(IDC_CF_TITLE)->SetFont(XTPPaintManager()->GetRegularFont());
	GetDlgItem(IDC_CF_TYPE)->SetFont(XTPPaintManager()->GetRegularFont());
	GetDlgItem(IDC_CF_LABEL)->SetFont(XTPPaintManager()->GetRegularFont());
	GetDlgItem(IDC_CF_SOURCE)->SetFont(XTPPaintManager()->GetRegularFont());
	GetDlgItem(IDC_CF_QUESTION)->SetFont(XTPPaintManager()->GetRegularFont());
	GetDlgItem(IDC_CF_NOTES)->SetFont(XTPPaintManager()->GetRegularFont());
	GetDlgItem(IDC_CF_CAST)->SetFont(XTPPaintManager()->GetRegularFont());
//	GetDlgItem(IDC_CF_PRIMARY)->SetFont(XTPPaintManager()->GetRegularFont());
//	GetDlgItem(IDC_CF_SECONDARY)->SetFont(XTPPaintManager()->GetRegularFont());
}

void CConsultationCtrl::Initialise()
{
	CLabelBlockArray* prgLB;

	((CComboBox*) GetDlgItem(IDC_CF_TYPE))->AddString(_T("Relationship"));
	((CComboBox*) GetDlgItem(IDC_CF_TYPE))->AddString(_T("Business"));
//	((CComboBox*) GetDlgItem(IDC_CF_TYPE))->AddString(_T("Ethics"));

	prgLB = ::GetLabelBlock6();
	for(int i = 0; i <= prgLB->GetUpperBound(); ++i)
		((CComboBox*) GetDlgItem(IDC_CF_LABEL))->AddString(prgLB->GetAt(i)->GetName());

	((CComboBox*) GetDlgItem(IDC_CF_SOURCE))->AddString(_T("Processed Wilhelm"));
	((CComboBox*) GetDlgItem(IDC_CF_SOURCE))->AddString(_T("Wilhelm"));
	((CComboBox*) GetDlgItem(IDC_CF_SOURCE))->AddString(_T("Legge"));
	((CComboBox*) GetDlgItem(IDC_CF_SOURCE))->AddString(_T("Andrade"));
	((CComboBox*) GetDlgItem(IDC_CF_SOURCE))->AddString(_T("Heyboer"));
	((CComboBox*) GetDlgItem(IDC_CF_SOURCE))->AddString(_T("Harvard Yenching"));
	((CComboBox*) GetDlgItem(IDC_CF_SOURCE))->AddString(_T("YellowBridge"));
	((CComboBox*) GetDlgItem(IDC_CF_SOURCE))->AddString(_T("Stackhouse"));
}

void CConsultationCtrl::Update(CConsultation* pC)
{
	m_pC = pC;

	((CEdit*) GetDlgItem(IDC_CF_TITLE))->SetWindowText(pC->m_strTitle);
	((CComboBox*) GetDlgItem(IDC_CF_TYPE))->SelectString(-1,pC->m_strType);
	((CComboBox*) GetDlgItem(IDC_CF_LABEL))->SelectString(-1,pC->m_strLabel);
	((CComboBox*) GetDlgItem(IDC_CF_SOURCE))->SelectString(-1,pC->m_strSource);
	((CRichEditCtrl*) GetDlgItem(IDC_CF_QUESTION))->SetWindowText(pC->m_strQuestion);
	((CRichEditCtrl*) GetDlgItem(IDC_CF_NOTES))->SetWindowText(pC->m_strNotes);

//	static CBitmap bmPrimary,bmSecondary;
//	bmPrimary.LoadBitmap(_T("IDB_HEXAGRAM00"));
//	bmSecondary.LoadBitmap(_T("IDB_HEXAGRAM01"));

//	((CButton*) GetDlgItem(IDC_CF_PRIMARY))->SetBitmap(bmPrimary);
//	((CButton*) GetDlgItem(IDC_CF_SECONDARY))->SetBitmap(bmSecondary);

	((CButton*) GetDlgItem(IDC_CF_PRIMARY))->ShowWindow(SW_HIDE);
	((CButton*) GetDlgItem(IDC_CF_SECONDARY))->ShowWindow(SW_HIDE);
}

void CConsultationCtrl::OnClickCast()
{
	::GetPropertyCtrl()->SetAnimation(_T("False"));
	m_dlgC.DoModal();
	CHexagramValueSequencer* phvsCurrent = ::GetApp()->GetCurrentHexagram();
	m_pC->m_strResult.Format(_T("%d,%d,%d,%d,%d,%d,%d"),phvsCurrent->GetValue(),
		(*phvsCurrent)[1][2].GetValue() % 3 == 0 ? 1 : 0,(*phvsCurrent)[1][1].GetValue() % 3 == 0 ? 1 : 0,(*phvsCurrent)[1][0].GetValue() % 3 == 0 ? 1 : 0,
		(*phvsCurrent)[0][2].GetValue() % 3 == 0 ? 1 : 0,(*phvsCurrent)[0][1].GetValue() % 3 == 0 ? 1 : 0,(*phvsCurrent)[0][0].GetValue() % 3 == 0 ? 1 : 0);
//	OnClickPrimary();
}

void CConsultationCtrl::OnClickPrimary()
{
	TCHAR szTemp[1024];
	int nOffset = 0,nIndex = -1;
	int nResult[7] = {-1,0,0,0,0,0,0};
	while(SubString(szTemp,m_pC->m_strResult,&nOffset,_T(','),false))
		nResult[++nIndex] = atoi(szTemp);
	CHexagramValueSequencer* phvsPrimary = ::GetApp()->GetCurrentHexagram();
	(*phvsPrimary) = nResult[0];
	if(nResult[6] == 1)
		++(*phvsPrimary)[1][2];
	if(nResult[5] == 1)
		++(*phvsPrimary)[1][1];
	if(nResult[4] == 1)
		++(*phvsPrimary)[1][0];
	if(nResult[3] == 1)
		++(*phvsPrimary)[0][2];
	if(nResult[2] == 1)
		++(*phvsPrimary)[0][1];
	if(nResult[1] == 1)
		++(*phvsPrimary)[0][0];
	::GetApp()->UpdateAllViews();
}

void CConsultationCtrl::OnClickSecondary()
{
}


/*
	Deferred consulation
	Group boxes
	Consultation / Result buttons
	Consultation search control (left dock)
*/

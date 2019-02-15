#include "StdAfx.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

BEGIN_MESSAGE_MAP(CTextView, CHtmlView)
	//{{AFX_MSG_MAP(CTextView)
	//}}AFX_MSG_MAP
	ON_COMMAND(ID_FILE_PRINT, CHtmlView::OnFilePrint)
END_MESSAGE_MAP()

IMPLEMENT_DYNCREATE(CTextView, CHtmlView)

CTextView::CTextView()
{
}

CTextView::~CTextView()
{
}

BOOL CTextView::PreCreateWindow(CREATESTRUCT& cs)
{
	return CHtmlView::PreCreateWindow(cs);
}

void CTextView::OnDraw(CDC*)
{
	CTextDoc* pDoc = GetDocument();
	ASSERT_VALID(pDoc);
}

void CTextView::OnInitialUpdate()
{
	CHtmlView::OnInitialUpdate();
//	SetSilent(TRUE);
	Update();
}

void CTextView::Update()
{
	CString strTemp1,strTemp2 = ::GetPropertyCtrl()->GetTextSource();
	CTextColumnSet TCS;
	if((strTemp2 == _T("Heyboer")) || (strTemp2 == _T("Harvard Yenching")) || (strTemp2 == _T("YellowBridge")) || (strTemp2 == _T("Stackhouse")))
	{
		strTemp1.Format(_T("select V%d from Text6 where Type = 'Text' and Name = '%s'"),
			::GetApp()->GetCurrentHexagram()->GetValue(),strTemp2);
		if(TCS.OpenRowset(::GetApp()->GetBeliefBase(),strTemp1))
			Navigate2(TCS.m_szText,NULL,NULL);
	}
	else
		Navigate2(_T("file:///") + ::ReplaceString(::ConcatPaths(::GetProgramFolder(),_T("Html"),_T("Hexagram.html"),NULL),
			_T("%20"),_T(" ")) + _T("?Name=") + ::GetApp()->GetCurrentHexagram()->GetLabel(),NULL,NULL);
}

void CTextView::Navigate(LPCTSTR lpszUrl)
{
	Navigate2(_T("file://") + ::ConcatPaths(::GetProgramFolder(),_T("Html"),lpszUrl,NULL),NULL,NULL);
}

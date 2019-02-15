#include "StdAfx.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

BEGIN_MESSAGE_MAP(CConversationCtrl, CTreeCtrl)
	//{{AFX_MSG_MAP(CConversationCtrl)
	ON_WM_CREATE()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

IMPLEMENT_DYNCREATE(CConversationCtrl, CTreeCtrl)

/////////////////////////////////////////////////////////////////////////////
// CConversationCtrl

CConversationCtrl::CConversationCtrl()
{
}

CConversationCtrl::~CConversationCtrl()
{
}

BOOL CConversationCtrl::PreCreateWindow(CREATESTRUCT& cs)
{
	return CTreeCtrl::PreCreateWindow(cs);
}

int CConversationCtrl::OnCreate(LPCREATESTRUCT lpCreateStruct) 
{
//	HTREEITEM hti,hti1 = TVI_ROOT;
	HTREEITEM hti1,hti2,hti3,hti4,hti5;
	CString strTemp;
	if(CTreeCtrl::OnCreate(lpCreateStruct) == -1)
		return -1;
	hti1 = InsertItem(_T("Stephen Vivash"),TVI_ROOT,TVI_LAST);
	
	CConsultationList* plsoC = ::GetConsultationList();
	CConsultation* pC;
	POSITION Pos = plsoC->GetHeadPosition();
	while(Pos)
		pC = plsoC->GetNext(Pos);

	hti2 = InsertItem("Relationship: Does she love me",hti1,TVI_LAST);
	hti3 = InsertItem("Relationship: Do I love her",hti2,TVI_LAST);
	InsertItem("Relationship: Should we get married",hti3,TVI_LAST);
	InsertItem("Relationship: Do I still love her",hti3,TVI_LAST);

	hti4 = InsertItem("Business: Will I open a shop",hti1,TVI_LAST);
	hti5 = InsertItem("Business: Where are all the customers",hti4,TVI_LAST);
	InsertItem("Business: Should I file for bankruptcy",hti5,TVI_LAST);
	InsertItem("Business: Why did I open that shop",hti4,TVI_LAST);

/*
	for(int i = 0; i < 10; ++i)
	{
		hti = hti1;
		for(int j = 0; j < 10; ++j)
		{
			strTemp.Format(_T("Question %d%d"),i,j);
			hti = InsertItem(strTemp,hti,TVI_LAST);
		}
	}
*/
	Expand(hti1,TVE_EXPAND);
	Expand(hti2,TVE_EXPAND);
	Expand(hti3,TVE_EXPAND);
	Expand(hti4,TVE_EXPAND);
	Expand(hti5,TVE_EXPAND);
//	return InsertItem(TVIF_TEXT | TVIF_IMAGE | TVIF_SELECTEDIMAGE | TVIF_STATE | TVIF_PARAM,lpszItem,nImage,nImage,nState,TVIS_BOLD | TVIS_EXPANDED | TVIS_SELECTED,NULL,htiParent,htiInsertAfter);
	return 0;
}

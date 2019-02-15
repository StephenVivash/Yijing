#include "StdAfx.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

BEGIN_MESSAGE_MAP(CConsultationPane, CWnd)
	ON_WM_CREATE()
	ON_WM_SIZE()
END_MESSAGE_MAP()

CConsultationPane::CConsultationPane()
{
}

CConsultationPane::~CConsultationPane()
{
}

int CConsultationPane::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if(CWnd::OnCreate(lpCreateStruct) != -1)
	{
		m_pfrmConsultation = CreateFrameDocView(RUNTIME_CLASS(CFrameWnd),RUNTIME_CLASS(CConsultationCtrl),NULL,true);
		m_pfrmConsultation->ModifyStyleEx(0, WS_EX_STATICEDGE);
		return 0;
	}
	return -1;
}

CFrameWnd* CConsultationPane::CreateFrameDocView(CRuntimeClass* pFrameClass, CRuntimeClass* pViewClass, CDocument* pDocument, bool bNoBorder)
{
	CView* pView = NULL;
	CCreateContext context;
	CObject* pObject = pFrameClass->CreateObject();
	ASSERT_KINDOF(CFrameWnd, pObject);

	CFrameWnd* pFrameWnd = DYNAMIC_DOWNCAST(CFrameWnd,pObject);
	ASSERT_VALID(pFrameWnd);

	::ZeroMemory(&context,sizeof(context));
	context.m_pCurrentDoc = pDocument;
	context.m_pCurrentFrame = GetParentFrame();
	context.m_pNewViewClass = pViewClass;
	
	if(!pFrameWnd->Create(NULL,NULL,WS_CHILD | WS_CLIPCHILDREN | WS_VISIBLE,CRect(0,0,0,0),this,NULL,0,&context))
	{
		TRACE0("Unable to create docking window frame.\n");
		return NULL;
	}
	if(pFrameWnd->GetActiveView() == NULL)
	{
		CWnd* pWnd = pFrameWnd->GetDescendantWindow(AFX_IDW_PANE_FIRST,TRUE);
		if((pWnd != NULL) && pWnd->IsKindOf(RUNTIME_CLASS(CView)))
		{
			pView = DYNAMIC_DOWNCAST(CView,pWnd);
			pFrameWnd->SetActiveView(pView,FALSE);
		}
	}
	if(bNoBorder && pFrameWnd)
		pFrameWnd->ModifyStyleEx(WS_EX_CLIENTEDGE,0,SWP_FRAMECHANGED);
	return pFrameWnd;
}

void CConsultationPane::OnSize(UINT nType, int cx, int cy)
{
	CWnd::OnSize(nType, cx, cy);
	if(m_pfrmConsultation->GetSafeHwnd())
		m_pfrmConsultation->MoveWindow(0, 0, cx, cy);
}

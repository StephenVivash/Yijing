#include "StdAfx.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

BEGIN_MESSAGE_MAP(CConversationPane,CWnd)
	//{{AFX_MSG_MAP(CConversationPane)
	ON_WM_CREATE()
	ON_WM_SIZE()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CConversationPane

CConversationPane::CConversationPane()
{
}

CConversationPane::~CConversationPane()
{
}

/////////////////////////////////////////////////////////////////////////////
// CConversationPane message handlers

int CConversationPane::OnCreate(LPCREATESTRUCT lpCreateStruct) 
{
	if((CWnd::OnCreate(lpCreateStruct) != -1) && m_wndToolBar.CreateToolBar(WS_TABSTOP | WS_VISIBLE | WS_CHILD | 
		CBRS_TOOLTIPS,this) && m_wndToolBar.LoadToolBar(IDR_PANE_PROPERTIES))
		if(m_trcConversation.GetSafeHwnd() == 0)
			if(m_trcConversation.Create(WS_CHILD | WS_VISIBLE | TVS_HASLINES | TVS_LINESATROOT | 
				TVS_HASBUTTONS | TVS_SHOWSELALWAYS | TVS_EDITLABELS,CRect(0,0,0,0),this,IDC_CONVERSATION))
			{
				m_trcConversation.ModifyStyleEx(0,WS_EX_STATICEDGE);
				return 0;
			}
	return -1;
}

void CConversationPane::OnSize(UINT nType, int cx, int cy)
{
	CWnd::OnSize(nType, cx, cy);
	int nTop = 0;
	if(m_wndToolBar.GetSafeHwnd())
	{
		CSize sz = m_wndToolBar.CalcDockingLayout(cx,/*LM_HIDEWRAP |*/ LM_HORZDOCK | LM_HORZ | LM_COMMIT);
		m_wndToolBar.MoveWindow(0, nTop, cx, sz.cy);
		m_wndToolBar.Invalidate(FALSE);
		nTop += sz.cy;
	}
	if(m_trcConversation.GetSafeHwnd())
	{
		m_trcConversation.MoveWindow(0, nTop, cx, cy - nTop);
		m_trcConversation.Invalidate(FALSE);
	}
}

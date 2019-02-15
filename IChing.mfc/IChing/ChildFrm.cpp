#include "StdAfx.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

IMPLEMENT_DYNCREATE(CChildFrame, CMDIChildWnd)

BEGIN_MESSAGE_MAP(CChildFrame, CMDIChildWnd)
	ON_WM_CLOSE()
END_MESSAGE_MAP()

CChildFrame::CChildFrame()
{
}

CChildFrame::~CChildFrame()
{
}

BOOL CChildFrame::PreCreateWindow(CREATESTRUCT& cs)
{
	cs.x = cs.y = 0;
	cs.cx = cs.cy = 32767;
	if(!CMDIChildWnd::PreCreateWindow(cs))
		return FALSE;
	return TRUE;
}

void CChildFrame::ActivateFrame(int nCmdShow)
{
	if(GetParent()->GetWindow(GW_CHILD) == this)
		nCmdShow = SW_SHOWMAXIMIZED;
	CMDIChildWnd::ActivateFrame(nCmdShow);
}

void CChildFrame::OnClose()
{
}


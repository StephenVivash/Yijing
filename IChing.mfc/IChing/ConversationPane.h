#if !defined(AFX_CONVERSATIONPANE_H__DC6478BF_D62A_4546_A544_4592857EF3A8__INCLUDED_)
#define AFX_CONVERSATIONPANE_H__DC6478BF_D62A_4546_A544_4592857EF3A8__INCLUDED_

#pragma once

class CConversationPane : public CWnd
{
public:
	CConversationPane();
	virtual ~CConversationPane();
	CConversationCtrl* GetConversationCtrl() {return &m_trcConversation;}

	//{{AFX_VIRTUAL(CConversationPane)
	//}}AFX_VIRTUAL

protected:
	CXTPToolBar m_wndToolBar;

	//{{AFX_MSG(CConversationPane)
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	void OnSize(UINT nType, int cx, int cy);
	//}}AFX_MSG

	CConversationCtrl m_trcConversation;
	
	DECLARE_MESSAGE_MAP()
};

#endif

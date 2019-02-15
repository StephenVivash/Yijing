#if !defined(AFX_CONVERSATIONCTRL_H__5C5B3F6E_ACC2_4171_8658_E36EB338A038__INCLUDED_)
#define AFX_CONVERSATIONCTRL_H__5C5B3F6E_ACC2_4171_8658_E36EB338A038__INCLUDED_

#pragma once

class CConversationCtrl : public CTreeCtrl
{
public:
	CConversationCtrl();
	virtual ~CConversationCtrl();

	//{{AFX_VIRTUAL(CConversationCtrl)
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	//}}AFX_VIRTUAL

protected:
	//{{AFX_MSG(CConversationCtrl)
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	//}}AFX_MSG

	DECLARE_MESSAGE_MAP()
	DECLARE_DYNCREATE(CConversationCtrl)
};

#endif

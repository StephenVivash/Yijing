#if !defined(AFX_LOGICPANE_H__C81C6B0E_433E_46A5_9598_EFBD9CD6A93E__INCLUDED_)
#define AFX_LOGICPANE_H__C81C6B0E_433E_46A5_9598_EFBD9CD6A93E__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif

class CLogicPane : public CWnd
{
public:
	CLogicPane();
	virtual ~CLogicPane();

	CLogicCtrl* GetLogicCtrl() {return (CLogicCtrl*) m_pfrmLogic->GetActiveView();}

protected:
	CFrameWnd* m_pfrmLogic;

	CFrameWnd* CreateFrameDocView(CRuntimeClass* pFrameClass, CRuntimeClass* pViewClass, CDocument* pDocument, bool bNoBorder);

	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnSize(UINT nType, int cx, int cy);

	DECLARE_MESSAGE_MAP()
};

#endif

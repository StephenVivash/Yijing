#if !defined(AFX_EXPLORATIONPANE_H__C81C6B0E_433E_46A5_9598_EFBD9CD6A93E__INCLUDED_)
#define AFX_EXPLORATIONPANE_H__C81C6B0E_433E_46A5_9598_EFBD9CD6A93E__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif

class CExplorationPane : public CWnd
{
public:
	CExplorationPane();
	virtual ~CExplorationPane();

	CExplorationCtrl* GetExplorationCtrl() {return (CExplorationCtrl*) m_pfrmExploration->GetActiveView();}

protected:
	CFrameWnd* m_pfrmExploration;

	CFrameWnd* CreateFrameDocView(CRuntimeClass* pFrameClass, CRuntimeClass* pViewClass, CDocument* pDocument, bool bNoBorder);

	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnSize(UINT nType, int cx, int cy);

	DECLARE_MESSAGE_MAP()
};

#endif

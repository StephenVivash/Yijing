#if !defined(AFX_CONSULTATIONPANE_H__C81C6B0E_433E_46A5_9598_EFBD9CD6A93E__INCLUDED_)
#define AFX_CONSULTATIONPANE_H__C81C6B0E_433E_46A5_9598_EFBD9CD6A93E__INCLUDED_

#pragma once

class CConsultationPane : public CWnd
{
public:
	CConsultationPane();
	virtual ~CConsultationPane();

	CConsultationCtrl* GetConsultationCtrl() {return (CConsultationCtrl*) m_pfrmConsultation->GetActiveView();}

protected:
	CFrameWnd* m_pfrmConsultation;

	CFrameWnd* CreateFrameDocView(CRuntimeClass* pFrameClass, CRuntimeClass* pViewClass, CDocument* pDocument, bool bNoBorder);

	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnSize(UINT nType, int cx, int cy);

	DECLARE_MESSAGE_MAP()
};

#endif

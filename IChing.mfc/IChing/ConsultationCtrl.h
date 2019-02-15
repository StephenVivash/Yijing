#if !defined(AFX_CONSULTATIONCTRL_H__C9B8108E_0E89_11D6_894C_FE7D3B55052B__INCLUDED_)
#define AFX_CONSULTATIONCTRL_H__C9B8108E_0E89_11D6_894C_FE7D3B55052B__INCLUDED_

#pragma once

class CConsultationCtrl : public CFormView
{
public:
	CConsultationCtrl();
	virtual ~CConsultationCtrl();

	void Initialise();
	virtual void Update(CConsultation* pC);
	CConsultationDlg* GetConsultationDlg() {return &m_dlgC;}

	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	virtual void OnInitialUpdate();

protected:
	CConsultation* m_pC;
	CConsultationDlg m_dlgC;

	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnClickCast();
	afx_msg void OnClickPrimary();
	afx_msg void OnClickSecondary();

	DECLARE_MESSAGE_MAP()
	DECLARE_DYNCREATE(CConsultationCtrl)
};

#endif

#if !defined(AFX_BELIEFBASEDLG_H__6CD2DCAD_3A97_4E10_8C6D_B29A17504B47__INCLUDED_)
#define AFX_BELIEFBASEDLG_H__6CD2DCAD_3A97_4E10_8C6D_B29A17504B47__INCLUDED_

#pragma once

class CBeliefBaseDlg : public CDialog
{
public:
	CBeliefBaseDlg(CWnd* pParent = NULL);
	CString GetName() {return m_strName;}

	//{{AFX_DATA(CBeliefBaseDlg)
	enum { IDD = IDD_BELIEFBASE };
	//}}AFX_DATA

	//{{AFX_VIRTUAL(CBeliefBaseDlg)
	virtual BOOL OnInitDialog();
	virtual void DoDataExchange(CDataExchange* pDX);
	virtual void OnOK();
	//}}AFX_VIRTUAL

protected:
	CString m_strName;
//	HICON m_hIcon;

	//{{AFX_MSG(CBeliefBaseDlg)
//	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
//	afx_msg void OnPaint();
//	afx_msg HCURSOR OnQueryDragIcon();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}

#endif

#if !defined(AFX_TEXTEDITDLG_H__6CD2DCAD_3A97_4E10_8C6D_B29A17504B47__INCLUDED_)
#define AFX_TEXTEDITDLG_H__6CD2DCAD_3A97_4E10_8C6D_B29A17504B47__INCLUDED_

#pragma once

class CTextEditDlg : public CDialog
{
public:
	CTextEditDlg(CWnd* pParent = NULL);

	bool SaveText();

	//{{AFX_DATA(CTextEditDlg)
	enum { IDD = IDD_TEXTEDIT };
	//}}AFX_DATA

	//{{AFX_VIRTUAL(CTextEditDlg)
	virtual BOOL OnInitDialog();
	virtual void DoDataExchange(CDataExchange* pDX);
	virtual void OnOK();
	//}}AFX_VIRTUAL

protected:
	CString m_strTextType;
//	HICON m_hIcon;

	//{{AFX_MSG(CTextEditDlg)
	afx_msg void OnTextTypeChanged();
//	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
//	afx_msg void OnPaint();
//	afx_msg HCURSOR OnQueryDragIcon();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}

#endif

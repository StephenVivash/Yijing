
#pragma once

class CConsultationDlg : public CDialog, public CValueSequencerPane
{
public:
	CConsultationDlg(CWnd* pParent = NULL);
	BOOL KillTimer(UINT_PTR nIDEvent);

	virtual void Update();
	virtual void SetCurrentSequencerCtrl(CValueSequencerCtrl* pvscCurrentSequencer) {m_pvscCurrentSequencer = pvscCurrentSequencer;}

	static int m_nLineDelay;

protected:
	HICON m_hIcon;
	int m_nCurrentSpeed;
	int m_nCurrentTrigram;
	int m_nCurrentLine;

	CHexagramValueSequencer m_vsHexagram;
	CValueSequencerCtrl m_vscLines[2][3];
	CValueSequencerCtrl* m_pvscCurrentSequencer;

	virtual BOOL OnInitDialog();
	virtual void DoDataExchange(CDataExchange* pDX);

	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	afx_msg void OnTimer(UINT nIDEvent);
	LRESULT OnUpdateControl(WPARAM wParam, LPARAM lParam);
	afx_msg void OnClose();

	DECLARE_MESSAGE_MAP()
};

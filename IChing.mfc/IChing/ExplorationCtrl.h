#if !defined(AFX_EXPLORATIONCTRL_H__C9B8108E_0E89_11D6_894C_FE7D3B55052B__INCLUDED_)
#define AFX_EXPLORATIONCTRL_H__C9B8108E_0E89_11D6_894C_FE7D3B55052B__INCLUDED_

#pragma once

class CExplorationCtrl : public CFormView, public CValueSequencerPane
{
public:
	CExplorationCtrl();
	virtual ~CExplorationCtrl();

	void CreateAndLoadControls();
	virtual void Update();
	void UpdateBits(bool bImages);
	void UpdateLines(bool bImages);
	void UpdateTrigrams(bool bImages);
	void UpdateHexagrams(bool bImages);

	virtual void SetCurrentSequencerCtrl(CValueSequencerCtrl* pvscCurrentSequencer) {m_pvscCurrentSequencer = pvscCurrentSequencer;}
	CValueSequencerCtrl* GetCurrentSequencerCtrl() {return m_pvscCurrentSequencer;}
	CValueSequencerCtrl* GetHexagramSequencerCtrl() {return &m_vscHexagram;}

	CHexagramValueSequencer* GetCurrentHexagram();

	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	virtual void OnInitialUpdate();

	static int m_nLineDelay;

protected:
	CHexagramValueSequencer m_vsHexagram;
//	CHexagramValueSequencer m_vsLastHexagram;

	CValueSequencerCtrl m_vscBits    [2][3][3];
	CValueSequencerCtrl m_vscLines   [2][3];
	CValueSequencerCtrl m_vscTrigrams[2];
	CValueSequencerCtrl m_vscHexagram;
	CValueSequencerCtrl* m_pvscCurrentSequencer;

	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnTimer(UINT nIDEvent);

	DECLARE_MESSAGE_MAP()
	DECLARE_DYNCREATE(CExplorationCtrl)
};

#endif

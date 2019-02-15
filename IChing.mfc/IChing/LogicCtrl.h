#if !defined(AFX_LOGICCTRL_H__C9B8108E_0E89_11D6_894C_FE7D3B55052B__INCLUDED_)
#define AFX_LOGICCTRL_H__C9B8108E_0E89_11D6_894C_FE7D3B55052B__INCLUDED_

#pragma once

enum eOperator {eAnd,eOr,eXor};
enum eAssign {eEqual,eNotEqual};

class CLogicCtrl : public CFormView, public CValueSequencerPane
{
public:
	CLogicCtrl();
	virtual ~CLogicCtrl();

	void CreateAndLoadControls();
	void Initialise();
	virtual void Update();
	void UpdateHexagrams(bool bImages);

	virtual void SetCurrentSequencerCtrl(CValueSequencerCtrl* pvscCurrentSequencer) {m_pvscCurrentSequencer = pvscCurrentSequencer;}
	CValueSequencerCtrl* GetCurrentSequencerCtrl() {return m_pvscCurrentSequencer;}
	CValueSequencerCtrl* GetHexagramSequencerCtrl() {return &m_vscHexagram[0];}

	CHexagramValueSequencer* GetCurrentHexagram();

	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	virtual void OnInitialUpdate();

protected:
	CHexagramValueSequencer m_vsHexagram[3];

	CValueSequencerCtrl m_vscHexagram[3];
	CValueSequencerCtrl* m_pvscCurrentSequencer;

	CButton m_btnOperator;
	CButton m_btnAssign;

	eOperator m_eOperator;
	eAssign m_eAssign;

	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnTimer(UINT nIDEvent);
	afx_msg void OnOperator();
	afx_msg void OnAssign();

	DECLARE_MESSAGE_MAP()
	DECLARE_DYNCREATE(CLogicCtrl)
};

#endif

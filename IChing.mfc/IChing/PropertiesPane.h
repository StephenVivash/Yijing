#if !defined(AFX_PROPERTIESPANE_H__9A60EAC5_EB5B_4C96_BE70_C9A972B10CDC__INCLUDED_)
#define AFX_PROPERTIESPANE_H__9A60EAC5_EB5B_4C96_BE70_C9A972B10CDC__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif

class CPropertiesPane : public CWnd
{
public:
	CPropertiesPane();
	virtual ~CPropertiesPane();

	void CreateAndLoadControls();

	CString GetBeliefBase() {return m_pgiBeliefBase->GetValue();}

	CString GetUserName() {return m_pgiUserName->GetValue();}
	CString GetUserGender() {return m_pgiUserGender->GetValue();}
	
	bool GetAnimation() {return m_pgiAnimation->GetValue() == _T("True");}
	void SetAnimation(LPCTSTR lpszAnimation);
	int GetSpeed() {return m_nSpeed;}
	void SetSpeed(int nSpeed) {m_nSpeed = nSpeed;}

	bool IsBitImages() {return m_pgiBitDisplay->GetValue() == _T("Tarot Images");}
	bool IsLineImages() {return m_pgiLineDisplay->GetValue() == _T("Tarot Images");}
	bool IsTrigramImages() {return m_pgiTrigramDisplay->GetValue() == _T("Tarot Images");}
	bool IsHexagramImages() {return m_pgiHexagramDisplay->GetValue() == _T("Tarot Images");}
	
	CString GetBitLabel() {return m_pgiBitLabels->GetValue();}
	int GetBitLabelIndex() {return m_pgiBitLabels->GetConstraints()->FindConstraint(m_pgiBitLabels->GetValue());}
	CString GetBitSequence() {return m_pgiBitSequence->GetValue();}
	int GetBitSequenceIndex() {return m_pgiBitSequence->GetConstraints()->FindConstraint(m_pgiBitSequence->GetValue());}
	CString GetBitDisplay() {return m_pgiBitDisplay->GetValue();}

	CString GetLineLabel() {return m_pgiLineLabels->GetValue();}
	int GetLineLabelIndex() {return m_pgiLineLabels->GetConstraints()->FindConstraint(m_pgiLineLabels->GetValue());}
	CString GetLineSequence() {return m_pgiLineSequence->GetValue();}
	int GetLineSequenceIndex() {return m_pgiLineSequence->GetConstraints()->FindConstraint(m_pgiLineSequence->GetValue());}
	CString GetLineDisplay() {return m_pgiLineDisplay->GetValue();}

	CString GetTrigramLabel() {return m_pgiTrigramLabels->GetValue();}
	int GetTrigramLabelIndex() {return m_pgiTrigramLabels->GetConstraints()->FindConstraint(m_pgiTrigramLabels->GetValue());}
	CString GetTrigramSequence() {return m_pgiTrigramSequence->GetValue();}
	int GetTrigramSequenceIndex() {return m_pgiTrigramSequence->GetConstraints()->FindConstraint(m_pgiTrigramSequence->GetValue());}
	CString GetTrigramDisplay() {return m_pgiTrigramDisplay->GetValue();}

	CString GetHexagramLabel() {return m_pgiHexagramLabels->GetValue();}
	int GetHexagramLabelIndex() {return m_pgiHexagramLabels->GetConstraints()->FindConstraint(m_pgiHexagramLabels->GetValue());}
	CString GetHexagramSequence() {return m_pgiHexagramSequence->GetValue();}
	int GetHexagramSequenceIndex() {return m_pgiHexagramSequence->GetConstraints()->FindConstraint(m_pgiHexagramSequence->GetValue());}
	CString GetHexagramDisplay() {return m_pgiHexagramDisplay->GetValue();}

	CString GetTextSource() {return m_pgiTextSource->GetValue();}
	bool GetDynamicHyperlink() {return m_pgiDynamicHyperlink->GetValue() == _T("True");}
	CString GetDesignation() {return m_pgiDesignation->GetValue();}

protected:
	bool m_bInitialised;
	int m_nSpeed;

	CXTPPropertyGrid m_wndPropertyGrid;
	CXTPToolBar m_wndToolBar;

	CXTPPropertyGridItem* m_pgiApplication;
	CXTPPropertyGridItem* m_pgiBeliefBase;

	CXTPPropertyGridItem* m_pgiUser;
	CXTPPropertyGridItem* m_pgiUserName;
	CXTPPropertyGridItem* m_pgiUserGender;

	CXTPPropertyGridItem* m_pgiLogicgrams;
	CXTPPropertyGridItem* m_pgiAnimation;
	CXTPPropertyGridItem* m_pgiSpeed;

	CXTPPropertyGridItem* m_pgiBits;
	CXTPPropertyGridItem* m_pgiBitLabels;
	CXTPPropertyGridItem* m_pgiBitSequence;
	CXTPPropertyGridItem* m_pgiBitDisplay;

	CXTPPropertyGridItem* m_pgiLines;
	CXTPPropertyGridItem* m_pgiLineLabels;
	CXTPPropertyGridItem* m_pgiLineSequence;
	CXTPPropertyGridItem* m_pgiLineDisplay;
	CXTPPropertyGridItem* m_pgiLineFastRatio;

	CXTPPropertyGridItem* m_pgiTrigrams;
	CXTPPropertyGridItem* m_pgiTrigramLabels;
	CXTPPropertyGridItem* m_pgiTrigramSequence;
	CXTPPropertyGridItem* m_pgiTrigramDisplay;

	CXTPPropertyGridItem* m_pgiHexagrams;
	CXTPPropertyGridItem* m_pgiHexagramLabels;
	CXTPPropertyGridItem* m_pgiHexagramSequence;
	CXTPPropertyGridItem* m_pgiHexagramDisplay;

	CXTPPropertyGridItem* m_pgiText;
	CXTPPropertyGridItem* m_pgiTextSource;
	CXTPPropertyGridItem* m_pgiDynamicHyperlink;
	CXTPPropertyGridItem* m_pgiDesignation;

	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnPanePropertiesCategorized();
	afx_msg void OnUpdatePanePropertiesCategorized(CCmdUI* pCmdUI);
	afx_msg void OnPanePropertiesAlphabetic();
	afx_msg void OnUpdatePanePropertiesAlphabetic(CCmdUI* pCmdUI);
	afx_msg void OnSetFocus(CWnd* pOldWnd);
	afx_msg LRESULT OnGridNotify(WPARAM wParam, LPARAM lParam);

	DECLARE_MESSAGE_MAP()
};

#endif

#if !defined(AFX_SEQUENCEVIEW_H__CA985D70_ECE2_489F_8F2E_C79568FE91FC__INCLUDED_)
#define AFX_SEQUENCEVIEW_H__CA985D70_ECE2_489F_8F2E_C79568FE91FC__INCLUDED_

#pragma once

class CSequenceView : public CView
{
public:
	CSequenceView();
	virtual ~CSequenceView();

	CSequenceDoc* GetDocument() {return reinterpret_cast<CSequenceDoc*>(m_pDocument);}
	void Update();

	int GetColumns() {return m_nColumns;}
	CSequenceBlockArray* GetSequenceBlockArray() {return m_prgSequenceBlock;}
	CListCtrlEx* GetSequenceListCtrl() {return &m_lscSequences;}

protected:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	void CreateAndLoadControls();

	virtual BOOL OnPreparePrinting(CPrintInfo* pInfo);
	virtual void OnBeginPrinting(CDC* pDC, CPrintInfo* pInfo);
	virtual void OnEndPrinting(CDC* pDC, CPrintInfo* pInfo);

	int OnCreate(LPCREATESTRUCT lpCreateStruct); 
	void OnWindowPosChanged(WINDOWPOS* pwndPos); 
	virtual void OnDraw(CDC* pDC);
	void OnItemChanging(NMHDR* pNMHDR, LRESULT* pResult);
	void OnItemChanged(NMHDR* pNMHDR, LRESULT* pResult);

	CListCtrlEx m_lscSequences;
	CSequenceBlockArray* m_prgSequenceBlock;

	int m_nRows;
	int m_nColumns;
	bool m_bUserInput;
	bool m_bInitialised;

	DECLARE_DYNCREATE(CSequenceView)
	DECLARE_MESSAGE_MAP()
};

#endif

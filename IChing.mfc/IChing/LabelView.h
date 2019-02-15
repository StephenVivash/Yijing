#if !defined(AFX_LABELVIEW_H__CA985D70_ECE2_489F_8F2E_C79568FE91FC__INCLUDED_)
#define AFX_LABELVIEW_H__CA985D70_ECE2_489F_8F2E_C79568FE91FC__INCLUDED_

#pragma once

class CLabelView : public CView
{
public:
	CLabelView();
	virtual ~CLabelView();

	CLabelDoc* GetDocument() {return reinterpret_cast<CLabelDoc*>(m_pDocument);}
	void Update();

	int GetColumns() {return m_nColumns;}
	CLabelBlockArray* GetLabelBlockArray() {return m_prgLabelBlock;}
	CListCtrlEx* GetLabelListCtrl() {return &m_lscLabels;}

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

	CListCtrlEx m_lscLabels;
	CLabelBlockArray* m_prgLabelBlock;

	int m_nRows;
	int m_nColumns;
	bool m_bUserInput;
	bool m_bInitialised;

	DECLARE_DYNCREATE(CLabelView)
	DECLARE_MESSAGE_MAP()
};

#endif

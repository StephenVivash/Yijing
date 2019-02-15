#if !defined(AFX_LISTCTRLEX_H__B391CB4A_1487_4EC0_B6D3_516D7738C01C__INCLUDED_)
#define AFX_LISTCTRLEX_H__B391CB4A_1487_4EC0_B6D3_516D7738C01C__INCLUDED_

#pragma once

typedef enum SORT_DATA_TYPE
{
	SORT_INT = 1,
	SORT_STRING,
	SORT_DATETIME,
	SORT_DEC
};

struct SORT_LIST_ITEM
{
	SORT_LIST_ITEM(const DWORD dwData, const CString &strItemText) 
	{
		m_dwData = dwData;
		m_strItemText = strItemText;
	}
	DWORD	m_dwData;
	CString m_strItemText;
};

class CSortList
{
public:
	CSortList(CListCtrl* pListCtrl,const int nCol)
	{
		ASSERT(pListCtrl);
		m_pListCtrl = pListCtrl;
		int nCount = m_pListCtrl->GetItemCount();
		for(int i = 0; i < nCount; i++)
		{
			DWORD dwData = (DWORD) m_pListCtrl->GetItemData(i);
			CString strItemText = m_pListCtrl->GetItemText(i, nCol); 
			m_pListCtrl->SetItemData(i, (DWORD_PTR) new SORT_LIST_ITEM(dwData, strItemText));
		}
	}

	virtual ~CSortList()
	{
		ASSERT(m_pListCtrl);
		int nCount = m_pListCtrl->GetItemCount();
		for(int i = 0; i < nCount; i++)
		{
			SORT_LIST_ITEM* pItem = (SORT_LIST_ITEM*)m_pListCtrl->GetItemData(i);
			ASSERT(pItem);
			m_pListCtrl->SetItemData(i, pItem->m_dwData);
			delete pItem;
		}
	}

	virtual void Sort(bool bAsc, SORT_DATA_TYPE eType)
	{
		long lParamSort = eType;
		if(!bAsc) 
			lParamSort *= -1; 
		m_pListCtrl->SortItems(Compare, lParamSort);
	}

protected:
	CListCtrl* m_pListCtrl;
	static int CALLBACK Compare(LPARAM lParam1,LPARAM lParam2,LPARAM lParamSort);
};

//////////////////////////////////////////////////////////////////////
// CListCtrlEx Class

class CListCtrlEx : public CListCtrl
{
public:
	CListCtrlEx();
	virtual ~CListCtrlEx();

	BOOL Create(CView* pwndParent, DWORD dwStyle = 0);

	void Reset();
	void RemoveAllColumns();
	void AddColumns(const CStringArray& rlssColumns);
	void AddColumn(LPCTSTR lpszColumn, int nSize = -1);

protected:
	CEdit m_ctrlEdit;
	CComboBox m_ctrlCombo;
//	CImageList m_lsiSortArrows;

	int m_nSelectedItem;
	int m_nSelectedSubItem;
	int m_nSortColumn;
	bool m_bSortAscending;
	int m_nLastSelectedColumn;
	int m_nLastEditedColumn;
	bool m_bEditing;
	bool m_bScrolling;

	void ShowEditBox(int nItem, int nSubItem);
	void HideEditBox();
	void ShowComboBox(CStringList &lssValues, int nItem, int nSubItem);
	void HideComboBox();
	int GetComboDroppedWidth();
	void SetColumnSort();
	void LoadColumnsWidth(CStringList& rlssSizes);
	void SaveColumnsWidth(CStringList& rlssSizes);

	bool OpenEditorCtrl(int nRow, int nColumn);
	void CloseEditCtrl();
	void CloseComboCtrl();
	void MouseOverKeyword(POINT point);

	bool IsEditable(int nRow, int nColumn);
	bool GetFieldValues(CStringList& rlssValues, int nRow, int nColumn);

	//{{AFX_VIRTUAL(CListCtrlEx)
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	//}}AFX_VIRTUAL

	//{{AFX_MSG(CListCtrlEx)
	afx_msg void OnDestroy();
	afx_msg void OnRButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnLButtonDblClk(UINT nFlags, CPoint point);
	afx_msg void OnLButtonUp(UINT nFlags, CPoint point);
	afx_msg void OnNcLButtonDown(UINT nHitTest, CPoint point);
	afx_msg void OnColumnclick(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg void OnMouseMove(UINT nFlags, CPoint point);
	afx_msg void OnVScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar);
	afx_msg void OnHScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar);
	afx_msg void OnLvnBeginScroll(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnLvnEndScroll(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnComboBoxSelChange();
	afx_msg void OnComboBoxKillFocus();
	afx_msg void OnEditBoxKillFocus();
	//}}AFX_MSG

	DECLARE_MESSAGE_MAP()
};

#endif

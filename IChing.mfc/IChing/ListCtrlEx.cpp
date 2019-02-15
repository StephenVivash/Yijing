#include "StdAfx.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

int CALLBACK CSortList::Compare(LPARAM lParam1, LPARAM lParam2, LPARAM lParamSort)
{
//	COleDateTime t1,t2;
	SORT_LIST_ITEM* item1 = (SORT_LIST_ITEM*) lParam1;
	SORT_LIST_ITEM* item2 = (SORT_LIST_ITEM*) lParam2;
	if(!item1 || !item2)
		return 0;
	int nOrder = lParamSort < 0 ? -1 : 1; 
	SORT_DATA_TYPE eType  = (SORT_DATA_TYPE) (lParamSort * nOrder);
	switch(eType)
	{
		case SORT_INT:
			return (_ttol(item1->m_strItemText) - _ttol(item2->m_strItemText)) * nOrder;
		case SORT_DEC:
			return (atof(item1->m_strItemText) < atof(item2->m_strItemText) ? -1 : 1) * nOrder;
//		case SORT_DATETIME:
//			if(t1.ParseDateTime(item1->m_strItemText) && t2.ParseDateTime(item2->m_strItemText)) 
//				return (t1 < t2 ? -1 : 1) * nOrder;
//			return 0;
		case SORT_STRING:
			if(!item1->m_strItemText.IsEmpty())
				if(item2->m_strItemText.IsEmpty() && (nOrder > 0))
					return -1;
			if(!item2->m_strItemText.IsEmpty())
				if(item1->m_strItemText.IsEmpty() && (nOrder > 0))
					return 1;
			return item1->m_strItemText.CompareNoCase(item2->m_strItemText) * nOrder;
		default:
			return 0;
	}
}

/////////////////////////////////////////////////////////////////////////////
// CListCtrlEx

BEGIN_MESSAGE_MAP(CListCtrlEx, CListCtrl)
	//{{AFX_MSG_MAP(CListCtrlEx)
	ON_WM_DESTROY()
	ON_WM_RBUTTONDOWN()
	ON_WM_LBUTTONDOWN()
	ON_WM_LBUTTONUP()	
	ON_WM_LBUTTONDBLCLK()
	ON_WM_NCLBUTTONDOWN()
	ON_WM_MOUSEMOVE()

	ON_NOTIFY_REFLECT(LVN_BEGINSCROLL, OnLvnBeginScroll)
	ON_NOTIFY_REFLECT(LVN_ENDSCROLL, OnLvnEndScroll)
	
	ON_EN_KILLFOCUS(IDC_EDIT_CTRL, OnEditBoxKillFocus)
	ON_CBN_KILLFOCUS(IDC_COMBO_CTRL, OnComboBoxKillFocus)
	ON_CBN_SELENDOK(IDC_COMBO_CTRL, OnComboBoxSelChange)
	ON_CBN_CLOSEUP(IDC_COMBO_CTRL, OnComboBoxKillFocus)

//	ON_NOTIFY_REFLECT(LVN_COLUMNCLICK, OnColumnclick)

//	ON_COMMAND(ID_LISTVIEW_GOTO_MODULE, OnGotoModule)
//	ON_COMMAND(ID_LISTVIEW_CUT, OnCut)
//	ON_COMMAND(ID_LISTVIEW_COPY, OnCopy)
//	ON_COMMAND(ID_LISTVIEW_PASTE, OnPaste)
//	ON_COMMAND(ID_LISTVIEW_PROPERTIES, OnProperties)
	
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

CListCtrlEx::CListCtrlEx()
{
	Reset();
	m_bScrolling = FALSE;
}

CListCtrlEx::~CListCtrlEx()
{
}

BOOL CListCtrlEx::Create(CView* pwndParent, DWORD dwStyle)
{
	if(CListCtrl::Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | LVS_REPORT | LVS_SINGLESEL | dwStyle,CRect(0,0,0,0),pwndParent,IDC_LIST_CTRL))
	{
		SetExtendedStyle(GetExtendedStyle() | LVS_EX_GRIDLINES | LVS_EX_FULLROWSELECT | LVS_EX_HEADERDRAGDROP/* | LVS_EX_CHECKBOXES*/);
		if(m_ctrlEdit.Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | ES_AUTOHSCROLL,CRect(0,0,0,0),this,IDC_EDIT_CTRL))
		{
			m_ctrlEdit.ShowWindow(SW_HIDE);
			m_ctrlEdit.SetFont(XTPPaintManager()->GetRegularFont());
		}
		if(m_ctrlCombo.Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | WS_VSCROLL | CBS_DROPDOWNLIST,CRect(0,0,0,200),this,IDC_COMBO_CTRL))
		{
			m_ctrlCombo.ShowWindow(SW_HIDE);
			m_ctrlCombo.SetFont(XTPPaintManager()->GetRegularFont());
		}
//		m_lsiSortArrows.Create(IDB_LISTVIEW,8,0,0xc0c0c0);
//		GetHeaderCtrl()->SetImageList(&m_lsiSortArrows);
//		if(m_bTip)
//		{
//			m_tipWindow.SetMargins(CSize(1,1));
//			m_tipWindow.SetLineSpace(1);
//			m_tipWindow.Create(this);
//			m_tipWindow.ShowWindow(SW_HIDE);
//		}
		return true;
	}
	return false;
}

//void CListCtrlEx::Bind(CDocument* pDocument)
//{
//	Reset();
//	DeleteAllItems();
//	m_pDocument = pDocument;
//	RemoveAllColumns();
//	AddColumns(pXmlReport->GetKeyColumnNames());
//	LoadListState();
//}

void CListCtrlEx::Reset()
{
	m_nSelectedItem = -1;
	m_nSelectedSubItem = -1;
	m_nSortColumn = -1;
	m_bSortAscending = false;
	m_nLastSelectedColumn = -1;
	m_nLastEditedColumn = 1;
	m_bEditing = false;
}

void CListCtrlEx::RemoveAllColumns()
{
	int nCount = GetHeaderCtrl()->GetItemCount();
	for(int i = 0; i < nCount; i++)
		DeleteColumn(0);
}

void CListCtrlEx::AddColumns(const CStringArray& rlssColumns)
{
	int nColumns = (int) rlssColumns.GetSize();
	for(int i = 0; i < nColumns; i++)
		AddColumn(rlssColumns[i]);
}

void CListCtrlEx::AddColumn(LPCTSTR lpszColumn, int nSize)
{
	int nIndex = GetHeaderCtrl()->GetItemCount();
	LVCOLUMN lvc;
	memset(&lvc,0,sizeof LVCOLUMN);
	lvc.mask = LVCF_WIDTH | LVCF_TEXT | LVCF_FMT;
	if(nSize == -1)
		lvc.cx = GetStringWidth(lpszColumn) + 40;
	else
		lvc.cx = nSize;
	lvc.pszText = (LPTSTR) lpszColumn;
//	if(m_pXmlReport->GetColumnJustification(nIndex) == CXmlReport::eLeft)
//		lvc.fmt = LVCFMT_LEFT;
//	else
//		if(m_pXmlReport->GetColumnJustification(nIndex) == CXmlReport::eCenter)
//			lvc.fmt = LVCFMT_CENTER;
//		else
//			if(m_pXmlReport->GetColumnJustification(nIndex) == CXmlReport::eRight)
//				lvc.fmt = LVCFMT_RIGHT;
	InsertColumn(nIndex,&lvc);
}

//bool CListCtrlEx::IsPasteEnabled()
//{
//	CXmlElementList lsoElements;
//	MSXML::IXMLDOMElementPtr pIElement;
//	CString strTemp;
//	if(m_pxmlDocument->GetDocumentType() == _T("FabricModule"))
//		if(m_pxmlDocument->CopyFromClipboard(&lsoElements,GetSafeHwnd()))
//			if((pIElement = lsoElements.GetHead()) != NULL)
//			{
//				strTemp = (LPCTSTR) pIElement->GetnodeName();
//				if(strTemp == _T("ModuleInstance"))
//					return CXmlDocument::GetClipboardDocument() == m_pxmlDocument->GetDocumentType();
//				else
//					if(strTemp == _T("FabricModule"))
//						return CXmlDocument::GetClipboardDocument() == _T("Modules");
//			}
//	return false;
//}

//void CListCtrlEx::GotoModule(int nRow)
//{
//	SaveListState();
//}

//void CListCtrlEx::LoadListState()
//{
//	if(m_pxmlDocument != NULL)
//	{
//		CStringList lssSizes;
//		::GetXmlWorkspace()->LoadListState(lssSizes,m_pxmlDocument->GetPathName(),
//			m_pXmlReport->GetName() + _T(" ") + m_pXmlReport->GetType());
//		LoadColumnsWidth(lssSizes);
//	}
//}

//void CListCtrlEx::SaveListState()
//{
//	if(m_pxmlDocument != NULL)
//	{
//		CStringList lssSizes;
//		SaveColumnsWidth(lssSizes);
//		::GetXmlWorkspace()->SaveListState(lssSizes,m_pxmlDocument->GetPathName(),
//			m_pXmlReport->GetName() + _T(" ") + m_pXmlReport->GetType());
//	}
//}

/////////////////////////////////////////////////////////////////////////////
// CListCtrlEx message handlers

void CListCtrlEx::OnDestroy()
{
//	SaveListState();
//	if(m_bTip && ::IsWindow(m_tipWindow.m_hWnd))
//		m_tipWindow.DestroyWindow();
	CListCtrl::OnDestroy();
}

void CListCtrlEx::OnLvnBeginScroll(NMHDR *pNMHDR, LRESULT *pResult)
{
	LPNMLVSCROLL pStateChanged = reinterpret_cast<LPNMLVSCROLL>(pNMHDR);
	if(pStateChanged->dx < -50 || pStateChanged->dx > 50)
		m_bScrolling = TRUE;
	*pResult = 0;
}

void CListCtrlEx::OnLvnEndScroll(NMHDR *pNMHDR, LRESULT *pResult)
{
	LPNMLVSCROLL pStateChanged = reinterpret_cast<LPNMLVSCROLL>(pNMHDR);
	m_bScrolling = FALSE;
	Invalidate();
	*pResult = 0;
}

BOOL CListCtrlEx::PreTranslateMessage(MSG* pMsg)
{
	int nRow;
	switch(pMsg->message)
	{
		case WM_KEYDOWN:
			switch(pMsg->wParam)
			{
				case VK_F2:
					if((nRow = GetNextItem(-1,LVNI_SELECTED)) != -1)
						OpenEditorCtrl(nRow,m_nLastEditedColumn);
					break;
				case VK_DELETE:
					if(!m_bEditing)
					{
//						OnDeleteModule();
						return TRUE;
					}
					break;
				case 'P':
					if(!m_bEditing)
						if((GetKeyState(VK_CONTROL) & 128) == 128)
						{
//							OnProperties();
							return TRUE;
						}
					break;
				case 'X':
					if(!m_bEditing)
						if((GetKeyState(VK_CONTROL) & 128) == 128)
						{
//							OnCut();
							return TRUE;
						}
					break;
				case 'C':
					if(!m_bEditing)
						if((GetKeyState(VK_CONTROL) & 128) == 128)
						{
//							OnCopy();
							return TRUE;
						}
					break;
				case 'V':
					if(!m_bEditing)
						if((GetKeyState(VK_CONTROL) & 128) == 128)
						{
//							OnPaste();
							return TRUE;
						}
					break;
				case VK_RETURN:
					CloseEditCtrl();
					CloseComboCtrl();
					break;
				case VK_ESCAPE:
					HideEditBox();
					HideComboBox();
					break;
				default:
					break;
			}
//		case WM_MOUSEMOVE:
//			if(m_bTip && !pMsg->wParam)
//			{
//				CPoint pointMouse;
//				GetCursorPos(&pointMouse);
//				if(m_tipWindow.HideTipWindow(pointMouse))
//					break;
//				MouseOverKeyword(pointMouse);
//				break;
//			}
//			else 
//				if(m_bTip && pMsg->wParam == MK_LBUTTON) 
//					m_tipWindow.HideTipWindow();
//			if(m_bTip && m_tipWindow.IsWindowVisible())
//				m_tipWindow.HideTipWindow();
//			break;
		case WM_MOUSELEAVE:
//			if(m_bTip)
//				m_tipWindow.HideTipWindow();
			break;

		default:
			break;
	}
	return CListCtrl::PreTranslateMessage(pMsg);
}

void CListCtrlEx::OnRButtonDown(UINT nFlags, CPoint point)
{
/*
	CXmlReportList* plsoXR = ::GetXmlReportList();
	POSITION Pos = plsoXR->GetHeadPosition();
	CMenu menu;
	LVHITTESTINFO lvhti;
	CString strTemp;
	UINT nHitFlags;
	int nItem = HitTest(point,&nHitFlags);
	int nIndex = -1;

	m_nLastSelectedColumn = -1;
	lvhti.pt = point;
	SubItemHitTest(&lvhti);
	if(lvhti.flags & LVHT_ONITEMLABEL)
		m_nLastSelectedColumn = lvhti.iSubItem;
	
	bool bFabricModule = m_pxmlDocument->GetDocumentType() == _T("FabricModule");
	UINT nEnabled = (nItem != LB_ERR) ? MF_ENABLED : MF_GRAYED;
	UINT nAddEnabled = bFabricModule && !::GetMainFrame()->IsBuilding() ? MF_ENABLED : MF_GRAYED;
	UINT nEditColumnEnabled = (nItem != LB_ERR) && (m_nLastSelectedColumn > 1) && !::GetMainFrame()->IsBuilding() ? MF_ENABLED : MF_GRAYED;
	UINT nEditModuleEnabled = bFabricModule && (nItem != LB_ERR) && !::GetMainFrame()->IsBuilding() ? MF_ENABLED : MF_GRAYED;
	UINT nDeleteEnabled = bFabricModule && (nItem != LB_ERR) && !::GetMainFrame()->IsBuilding() ? MF_ENABLED : MF_GRAYED;
	UINT nPasteEnabled = bFabricModule && !::GetMainFrame()->IsBuilding() && IsPasteEnabled() ? MF_ENABLED : MF_GRAYED;

	CListCtrl::OnRButtonDown(nFlags,point);
	ClientToScreen(&point); 
	if((nFlags & MK_RBUTTON) == MK_RBUTTON && !m_bEditing)
		if(menu.CreatePopupMenu())
		{
			menu.AppendMenu(MF_STRING | nEnabled,ID_LISTVIEW_GOTO_MODULE,_T("Go To Module Instance\tCtrl+G"));
			menu.AppendMenu(MF_SEPARATOR);

			if(bFabricModule)
			{
				menu.AppendMenu(MF_STRING | nAddEnabled,ID_LISTVIEW_ADD_MODULE,_T("Add Module Instance"));
				if(m_pXmlReport->GetType() == _T("Connection"))
				{
					menu.AppendMenu(MF_STRING | nAddEnabled,ID_LISTVIEW_ADD_NET,_T("Add Net"));
					menu.AppendMenu(MF_STRING | nAddEnabled,ID_LISTVIEW_ADD_PORT,_T("Add Port"));
				}
			}

			menu.AppendMenu(MF_STRING | nEditColumnEnabled,ID_LISTVIEW_EDIT_COLUMN,_T("Edit Column\tF2"));
			menu.AppendMenu(MF_STRING | nEditModuleEnabled,ID_LISTVIEW_EDIT_MODULE,_T("Edit Module Instance"));

			menu.AppendMenu(MF_STRING | nDeleteEnabled,ID_LISTVIEW_DELETE_MODULE,_T("Delete Module Instance\tDel"));
			menu.AppendMenu(MF_SEPARATOR);

			menu.AppendMenu(MF_STRING | nDeleteEnabled,ID_LISTVIEW_CUT,_T("Cut Module Instance\tCtrl+X"));
			menu.AppendMenu(MF_STRING | nEnabled,ID_LISTVIEW_COPY,_T("Copy Module Instance\tCtrl+C"));
			menu.AppendMenu(MF_STRING | nPasteEnabled,ID_LISTVIEW_PASTE,_T("Paste Module Instance\tCtrl+V"));
			menu.AppendMenu(MF_SEPARATOR);

			if(m_pxmlDocument->GetDocumentType() == _T("FabricModule"))
			{
				menu.AppendMenu(MF_STRING,ID_LISTVIEW_FABRIC_TREE,_T("Xml Tree View"));
				menu.AppendMenu(MF_STRING,ID_LISTVIEW_FABRIC_XML,_T("Xml Text View"));
				while(Pos)
				{
					CXmlReport* pXR = plsoXR->GetNext(Pos);
					++nIndex;
					UINT nChecked = 0;
					if((pXR->GetName() == m_pXmlReport->GetName()) && 
						(pXR->GetType() == m_pXmlReport->GetType()))
						nChecked = MF_CHECKED;
					strTemp.Format(_T("%s %s View"),pXR->GetName(),pXR->GetType());
					if(pXR->GetType() == _T("Parameter"))
						menu.AppendMenu(MF_STRING | nChecked,ID_LISTVIEW_FABRIC_LIST + nIndex,strTemp);
				}
				menu.AppendMenu(MF_STRING,ID_LISTVIEW_FABRIC_CONNECT,_T("2d Connection View"));
			}
			else
				if(m_pxmlDocument->GetDocumentType() == _T("PlatformConfig")) 
				{
					menu.AppendMenu(MF_STRING,ID_LISTVIEW_PLATFORM_TREE,_T("Xml Tree View"));
					menu.AppendMenu(MF_STRING,ID_LISTVIEW_PLATFORM_XML,_T("Xml Text View"));
					while(Pos)
					{
						CXmlReport* pXR = plsoXR->GetNext(Pos);
						++nIndex;
						if(pXR->GetType() == _T("Parameter"))
						{
							UINT nChecked = 0;
							if(pXR->GetName() == m_pXmlReport->GetName())
								nChecked = MF_CHECKED;
							strTemp.Format(_T("%s %s View"),pXR->GetName(),pXR->GetType());
							menu.AppendMenu(MF_STRING | nChecked,ID_LISTVIEW_PLATFORM_LIST + nIndex,strTemp);
						}
					}
				}
			menu.AppendMenu(MF_SEPARATOR);
			menu.AppendMenu(MF_STRING | MF_GRAYED,ID_LISTVIEW_PROPERTIES,_T("XML Table Properties\tCtrl+P"));
			menu.TrackPopupMenu(TPM_LEFTALIGN | TPM_LEFTBUTTON,point.x,point.y,this); 
			menu.DestroyMenu();
		}
*/
}

void CListCtrlEx::OnLButtonDown(UINT nFlags ,CPoint point)
{
	LVHITTESTINFO lvhti;
	lvhti.pt = point;
	SubItemHitTest(&lvhti);
	if(lvhti.flags & LVHT_ONITEMLABEL)
		if(GetItemState(lvhti.iItem,LVIS_SELECTED) & LVIS_SELECTED)
			if(OpenEditorCtrl(lvhti.iItem,lvhti.iSubItem))
				return;
	CloseEditCtrl();
	CloseComboCtrl();
	CListCtrl::OnLButtonDown(nFlags,point);
}

void CListCtrlEx::OnLButtonUp(UINT nFlags, CPoint point)
{
	CListCtrl::OnLButtonUp(nFlags,point);
}

void CListCtrlEx::OnLButtonDblClk(UINT nFlags, CPoint point)
{
	CString strTemp;
	LVHITTESTINFO lvhti;
	lvhti.pt = point;
	SubItemHitTest(&lvhti);
	CListCtrl::OnLButtonDblClk(nFlags,point);
//	if(lvhti.flags & LVHT_ONITEMLABEL) 
//		if(lvhti.iSubItem == 0)
//			GotoModule(lvhti.iItem);
}

void CListCtrlEx::OnNcLButtonDown(UINT nHitTest, CPoint point)
{
	CListCtrl::OnNcLButtonDown(nHitTest,point);
	HideEditBox();
	HideComboBox();
}

void CListCtrlEx::OnMouseMove(UINT nFlags, CPoint point)
{
	CListCtrl::OnMouseMove(nFlags,point);
}

void CListCtrlEx::OnColumnclick(NMHDR* pNMHDR, LRESULT* pResult)
{
	NM_LISTVIEW* pNMListView = (NM_LISTVIEW*) pNMHDR;
	CSortList sort(this,pNMListView->iSubItem);
	if(m_nSortColumn == pNMListView->iSubItem)
		m_bSortAscending = !m_bSortAscending;
	else
	{
		m_nSortColumn = pNMListView->iSubItem;	
		m_bSortAscending = true;
	}
	sort.Sort(m_bSortAscending,SORT_STRING);
//	SetColumnSort();
	*pResult = 0;
}

void CListCtrlEx::OnComboBoxSelChange()
{
	CloseComboCtrl();
}

void CListCtrlEx::OnComboBoxKillFocus()
{
	HideComboBox();
}

void CListCtrlEx::OnEditBoxKillFocus()
{
	HideEditBox();
}

void CListCtrlEx::ShowEditBox(int nItem, int nSubItem)
{
	CRect rc;
	CString strTemp = GetItemText(nItem,nSubItem);
	m_ctrlEdit.Clear();
	GetSubItemRect(nItem,nSubItem,LVIR_LABEL,rc);
	rc.left += 5; rc.bottom -= 1;
	m_ctrlEdit.SetWindowText(strTemp);
	m_ctrlEdit.MoveWindow(&rc,TRUE);
	m_ctrlEdit.ShowWindow(SW_SHOW);
	m_bEditing = true;
	m_ctrlEdit.SetFocus();
	m_ctrlEdit.SetSel(0,strTemp.GetLength());
}

void CListCtrlEx::HideEditBox()
{
	if(m_ctrlEdit.IsWindowVisible()) 
	{
		m_ctrlEdit.Clear();
		m_ctrlEdit.ShowWindow(SW_HIDE);
		m_bEditing = false;
		m_nSelectedItem = -1;
		m_nSelectedSubItem = -1;
		SetFocus();
	}
}

void CListCtrlEx::ShowComboBox(CStringList& lssValues, int nItem, int nSubItem)
{
	CRect rc;
	GetSubItemRect(nItem,nSubItem,LVIR_LABEL,rc);
	rc.top -= 3;
	rc.right += 1;
	m_ctrlCombo.MoveWindow(&rc,TRUE);
	m_bEditing = true;
	m_ctrlCombo.ResetContent();
	POSITION Pos = lssValues.GetHeadPosition();
	while(Pos)
		m_ctrlCombo.AddString(lssValues.GetNext(Pos));
	m_ctrlCombo.SetCurSel(m_ctrlCombo.FindStringExact(0,GetItemText(nItem,nSubItem)));
	m_ctrlCombo.ShowWindow(SW_SHOW);
	m_ctrlCombo.SetFocus();
	m_ctrlCombo.ShowDropDown(TRUE);
}

void CListCtrlEx::HideComboBox()
{
	CRect rc;
	if(m_ctrlCombo.IsWindowVisible()) 
	{
		m_ctrlCombo.ShowWindow(SW_HIDE);
		m_bEditing = false;
		m_nSelectedItem = -1;
		m_nSelectedSubItem = -1;
		SetFocus();
	}
}

void CListCtrlEx::SetColumnSort()
{
	CHeaderCtrl* pHdCtrl = GetHeaderCtrl();
	int nCount = pHdCtrl->GetItemCount();
	for(int i = 0; i < nCount; i++)
	{
		HDITEM hdi = {0};
		hdi.mask = HDI_IMAGE | HDI_FORMAT;
		pHdCtrl->GetItem(i,&hdi);
		hdi.iImage = -1;
		hdi.fmt &= ~HDF_IMAGE;
		if(i == m_nSortColumn)
		{
			hdi.fmt |= HDF_IMAGE;
			hdi.iImage = m_bSortAscending ? 1 : 2;
		}
		pHdCtrl->SetItem(i,&hdi);
	}
}

void CListCtrlEx::LoadColumnsWidth(CStringList& rlssSizes)
{
	CHeaderCtrl* pHdCtrl = GetHeaderCtrl();
	if(pHdCtrl != NULL)
	{
		int nColumnCount = pHdCtrl->GetItemCount();
		for(int i = 0; i < nColumnCount; i++)
			if(!rlssSizes.IsEmpty())
			{
				LVCOLUMN lvc = {0};
				lvc.mask = LVCF_WIDTH;
				lvc.cx = atoi(rlssSizes.RemoveHead());
				SetColumn(i,&lvc);
			}
			else
				break;
	}
}

void CListCtrlEx::SaveColumnsWidth(CStringList& rlssSizes)
{
	CHeaderCtrl* pHdCtrl = GetHeaderCtrl();
	CString strTemp;
	rlssSizes.RemoveAll();
	if(pHdCtrl != NULL)
	{
		int nColumnCount = pHdCtrl->GetItemCount();
		for(int i = 0; i < nColumnCount; i++)
		{
			LVCOLUMN lvc = {0};
			lvc.mask = LVCF_WIDTH;
			GetColumn(i,&lvc);
			strTemp.Format(_T("%d"),lvc.cx);
			rlssSizes.AddTail(strTemp);
		}
	}
}

bool CListCtrlEx::OpenEditorCtrl(int nRow, int nColumn)
{
	CStringList lssValues;
	if(!m_ctrlEdit.IsWindowVisible() && !m_ctrlCombo.IsWindowVisible())
		if(IsEditable(nRow,nColumn))
			if(GetFieldValues(lssValues,nRow,nColumn))
			{
				ShowComboBox(lssValues,nRow,nColumn);
				m_nSelectedItem = nRow;
				m_nSelectedSubItem = nColumn;
				m_nLastEditedColumn = m_nSelectedSubItem;
				return true;
			}
			else 
			{
				ShowEditBox(nRow,nColumn);
				m_nSelectedItem = nRow;
				m_nSelectedSubItem = nColumn;
				m_nLastEditedColumn = m_nSelectedSubItem;
				return true;
			}
	return false;
}

void CListCtrlEx::CloseEditCtrl()
{
	CString strTemp;
	m_ctrlEdit.GetWindowText(strTemp);
	if(m_ctrlEdit.IsWindowVisible()) 
		if(strTemp != GetItemText(m_nSelectedItem,m_nSelectedSubItem))
		{
//			SetItemText(m_nSelectedItem,m_nSelectedSubItem,strTemp);
		}
	HideEditBox();
}

void CListCtrlEx::CloseComboCtrl()
{
/* ??????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????
	CString strTemp1,strTemp2,strTemp3;
	CLabelBlockArray* prgLB;
	CLabelBlock* pLB;
	CLabelEntryArray* prgLE; 
	CLabelEntry* pLE;
	m_ctrlCombo.GetWindowText(strTemp1);
	strTemp2 = GetItemText(m_nSelectedItem,m_nSelectedSubItem);
	if(m_ctrlCombo.IsWindowVisible())
		if(strTemp1 != strTemp2)
		{
			prgLB = ((CLabelView*) GetParent())->GetLabelBlockArray();
			pLB = prgLB->GetAt(m_nSelectedItem);
			prgLE = pLB->GetEntryArray();
			int nNoColumns = ((CLabelView*) GetParent())->GetColumns();
			int nOffset = m_nSelectedSubItem % 2 == 0 ? 2 : 1;
			for(int c = 0; c < nNoColumns; ++c)
			{
				strTemp3 = GetItemText(m_nSelectedItem,(c * 2) + nOffset);
				if(strTemp1 == strTemp3)
				{
					SetItemText(m_nSelectedItem,(c * 2) + nOffset,strTemp2);
					SetItemText(m_nSelectedItem,m_nSelectedSubItem,strTemp1);

					pLE = prgLE->GetAt(c);
					if(nOffset == 2)
						pLE->SetLabel(strTemp2);
					else
						pLE->SetSequence(atoi(strTemp2));
					pLE = prgLE->GetAt((m_nSelectedSubItem - nOffset) / 2);
					if(nOffset == 2)
						pLE->SetLabel(strTemp1);
					else
						pLE->SetSequence(atoi(strTemp1));
					prgLB->SetModified();
					prgLE->SetModified();

					::GetApp()->UpdateAllViews();
					break;
				}
			}
		}
*/
	HideComboBox();
}
/*
void CListCtrlEx::MouseOverKeyword(POINT point)
{
	CPoint pt = point;
	CPoint ptText = point;
	CHeaderCtrl *pHdCtrl = GetHeaderCtrl();
	CWnd *pWnd = WindowFromPoint(point);
	if(m_bTip && (pHdCtrl == pWnd))
	{
		CRect rcItem;
		int offset = 8;
		for(int i = 0; i < pHdCtrl->GetItemCount(); i++)
		{
			if(!pHdCtrl->GetItemRect(i,&rcItem))
				continue;
			pHdCtrl->ClientToScreen(&rcItem);
			if(point.x > rcItem.left + offset && point.x < rcItem.right - offset)
			{
				HDITEM hd = {0};
				hd.mask = HDI_TEXT;
				CString strBuffer;
				hd.pszText = strBuffer.GetBuffer(100);
				hd.cchTextMax = 100;
				pHdCtrl->GetItem(i,&hd);
				strBuffer.ReleaseBuffer();
//				m_tipWindow.m_ptStart = point;
//				m_tipWindow.SetTipText(NULL,strBuffer);
//				m_tipWindow.SetMargins(CSize(1,1));
//				m_tipWindow.SetLineSpace(1);
//				CRect rc;
//				m_tipWindow.GetWindowRect(&rc);
//				m_tipWindow.ShowTipWindow(CPoint(point.x,point.y - rc.Height()),TWS_XT_ALPHASHADOW,1000,5000);

				// Register for WM_MOUSELEAVE
				// in case when mouse moves out of the client area
//				TRACKMOUSEEVENT tme;
//				tme.cbSize = sizeof(TRACKMOUSEEVENT);
//				tme.dwFlags = TME_LEAVE;
//				tme.hwndTrack = pHdCtrl->GetSafeHwnd();
//				_TrackMouseEvent(&tme);
				break;
			}
		}
	}
}
*/
bool CListCtrlEx::IsEditable(int nRow, int nColumn)
{
	return true;
//	return (!::GetMainFrame()->IsBuilding() && (nColumn >= m_pXmlReport->GetKeyColumnNameSize())) &&
//		((m_pXmlReport->GetType() == _T("Connection")) || !GetItemText(nRow,nColumn).IsEmpty());
}

bool CListCtrlEx::GetFieldValues(CStringList& rlssValues, int nRow, int nColumn)
{
	CString strTemp;
	int nNoColumns = ((CLabelView*) GetParent())->GetColumns();
//	int nOffset = nColumn % 2 == 0 ? 2 : 1;
	rlssValues.RemoveAll();
	if(nColumn > 0)
		for(int c = 0; c < nNoColumns; ++c)
		{
			strTemp = GetItemText(nRow,c + 1/*(c * 2) + nOffset*/);
			if(!strTemp.IsEmpty() && (strTemp != _T("-1")))
				rlssValues.AddTail(strTemp);
		}
	return !rlssValues.IsEmpty();
}

///////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////
/*
void CListCtrlEx::OnGotoModule()
{
	GotoModule(GetNextItem(-1,LVNI_ALL | LVNI_SELECTED));
}

void CListCtrlEx::OnAddModule()
{
	MSXML::IXMLDOMElementPtr pIElement,pIParent;
	int nListItem;
	CString strTemp;
	if((pIParent = m_pxmlDocument->GetDocumentRoot()->selectSingleNode(
		_T("ModuleInstances"))) != NULL)
		if((pIElement = m_pxmlDocument->AddElementWithChildren(pIParent,
			_T("ModuleInstance"),_T("New ModuleInstance"))) != NULL)
			if((nListItem = InsertItem(GetItemCount(),_T("New ModuleInstance"))) != LB_ERR)
			{
				CXmlPropertySheet dlgPS(m_pxmlDocument,pIElement,this,nListItem,true);
				if(dlgPS.DoModal() == IDOK)
					CXmlFabricModule::AddFabricModuleInstance(m_pxmlDocument,pIElement);
				else
				{
					DeleteItem(nListItem);
					m_pxmlDocument->DeleteElement(pIParent,pIElement);
				}
				SetFocus();
			}
}

void CListCtrlEx::OnAddNet()
{
	MSXML::IXMLDOMElementPtr pIElement,pIParent;
	CString strTemp;
	if((pIParent = m_pxmlDocument->GetDocumentRoot()->selectSingleNode(_T("Nets"))) != NULL)
		if((pIElement = m_pxmlDocument->AddElementWithChildren(pIParent,
			_T("Net"),_T("New Net"))) != NULL)
		{
			CXmlPropertySheet dlgPS(m_pxmlDocument,pIElement,this,-1,true);
			if(dlgPS.DoModal() == IDOK)
			{
				strTemp = pIElement->getAttribute(_T("Name")).bstrVal;
				strTemp += _T(".Net");
				m_pXmlReport->AddColumn(strTemp);
				AddColumn(strTemp);
			}
			SetFocus();
		}
}

void CListCtrlEx::OnAddPort()
{
	MSXML::IXMLDOMElementPtr pIElement,pIParent;
	CString strTemp;
	if((pIParent = m_pxmlDocument->GetDocumentRoot()->selectSingleNode(_T("Ports"))) != NULL)
		if((pIElement = m_pxmlDocument->AddElementWithChildren(pIParent,
			_T("Port"),_T("New Port"))) != NULL)
		{
			CXmlPropertySheet dlgPS(m_pxmlDocument,pIElement,this,-1,true);
			if(dlgPS.DoModal() == IDOK)
			{
				strTemp = pIElement->getAttribute(_T("Name")).bstrVal;
				strTemp += _T(".Port");
				m_pXmlReport->AddColumn(strTemp);
				AddColumn(strTemp);
			}
			SetFocus();
		}
}

void CListCtrlEx::OnEditColumn()
{
	int nRow;
	if((nRow = GetNextItem(-1,LVNI_SELECTED)) != -1)
		if(m_nLastSelectedColumn >= m_pXmlReport->GetKeyColumnNameSize())
			OpenEditorCtrl(nRow,m_nLastSelectedColumn);
	m_nLastSelectedColumn = -1;
}

void CListCtrlEx::OnEditModule()
{
	MSXML::IXMLDOMElementPtr pIElement,pIParent;
	int nListItem;
	CString strTemp1,strTemp2;
	if((nListItem = GetNextItem(-1,LVNI_ALL | LVNI_SELECTED)) != LB_ERR)
	{
		strTemp1 = GetItemText(nListItem,0);
		strTemp2.Format(_T("./ModuleInstances/ModuleInstance[@InstanceName=\"%s\"]"),strTemp1);
		if((pIElement = m_pxmlDocument->GetDocumentRoot()->selectSingleNode((LPCTSTR) strTemp2)) != NULL)
		{
			CXmlPropertySheet dlgPS(m_pxmlDocument,pIElement,this,nListItem,false);
			if(dlgPS.DoModal() == IDOK)
			{
				strTemp1 = pIElement->getAttribute(_T("InstanceName")).bstrVal;
				SetItemText(nListItem,0,strTemp1);
				strTemp1 = pIElement->getAttribute(_T("Type")).bstrVal;
				SetItemText(nListItem,1,strTemp1);
			}
			SetFocus();
		}
	}
}

void CListCtrlEx::OnDeleteModule()
{
	MSXML::IXMLDOMElementPtr pIElement,pIParent;
	CString strTemp1,strTemp2;
	int nItem;
	if((nItem = GetNextItem(-1,LVNI_ALL | LVNI_SELECTED)) != LB_ERR)
	{
		strTemp1 = GetItemText(nItem,0);
		strTemp2.Format(_T("Are you sure you want to delete %s ?"),strTemp1);
		if(::AppMessage(strTemp2,MB_YESNO | MB_ICONQUESTION,::GetMainFrame()) == IDYES)
		{
			strTemp2.Format(_T("./ModuleInstances/ModuleInstance[@InstanceName=\"%s\"]"),strTemp1);
			if((pIElement = m_pxmlDocument->GetDocumentRoot()->selectSingleNode((LPCTSTR) strTemp2)) != NULL)
			{
				pIParent = pIElement->GetparentNode();
				if(m_pxmlDocument->DeleteElement(pIParent,pIElement))
					DeleteItem(nItem);
			}
		}
		SetFocus();
	}
}

void CListCtrlEx::OnCut()
{
	MSXML::IXMLDOMElementPtr pIElement,pIParent;
	CString strTemp1,strTemp2;
	int nItem;
	if((nItem = GetNextItem(-1,LVNI_ALL | LVNI_SELECTED)) != LB_ERR)
	{
		strTemp1 = GetItemText(nItem,0);
		strTemp2.Format(_T("Are you sure you want to cut %s ?"),strTemp1);
		if(::AppMessage(strTemp2,MB_YESNO | MB_ICONQUESTION,::GetMainFrame()) == IDYES)
		{
			strTemp2.Format(_T("./ModuleInstances/ModuleInstance[@InstanceName=\"%s\"]"),strTemp1);
			if((pIElement = m_pxmlDocument->GetDocumentRoot()->selectSingleNode((LPCTSTR) strTemp2)) != NULL)
			{
				OnCopy();
				pIParent = pIElement->GetparentNode();
				if(m_pxmlDocument->DeleteElement(pIParent,pIElement))
					DeleteItem(nItem);
			}
		}
		SetFocus();
	}
}

void CListCtrlEx::OnCopy()
{
	CXmlElementList lsoElements;
	MSXML::IXMLDOMElementPtr pIElement,pIParent;
	CString strTemp1,strTemp2;
	int nItem;
	if((nItem = GetNextItem(-1,LVNI_ALL | LVNI_SELECTED)) != LB_ERR)
	{
		strTemp1 = GetItemText(nItem,0);
		strTemp2.Format(_T("./ModuleInstances/ModuleInstance[@InstanceName=\"%s\"]"),strTemp1);
		if((pIElement = m_pxmlDocument->GetDocumentRoot()->selectSingleNode((LPCTSTR) strTemp2)) != NULL)
		{
			lsoElements.AddTail(new MSXML::IXMLDOMElementPtr(pIElement));
			m_pxmlDocument->CopyToClipboard(&lsoElements,GetSafeHwnd());
		}
	}
}

void CListCtrlEx::OnPaste()
{
	CXmlElementList lsoElements;
	MSXML::IXMLDOMElementPtr pIParent,pIElement,pIElement1;
	CString strTemp,strInstanceName,strModuleType;
	bool bPaste = false;
	if(m_pxmlDocument->CopyFromClipboard(&lsoElements,GetSafeHwnd()))
	{
		POSITION Pos = lsoElements.GetHeadPosition();
		if((pIParent = m_pxmlDocument->GetDocumentRoot()->selectSingleNode(
			_T("ModuleInstances"))) != NULL)
		{
			while(Pos) 
			{
				pIElement = lsoElements.GetNext(Pos);
				strTemp = (LPCTSTR) pIElement->GetnodeName();
				if(strTemp == _T("ModuleInstance"))
					if(pIParent->appendChild(pIElement) != NULL)
						bPaste = true;
					else ;
				else
					if(strTemp == _T("FabricModule"))
					{
						strModuleType = pIElement->getAttribute(_T("Type")).bstrVal;
						m_pxmlDocument->GetNextModuleInstanceName(strInstanceName,pIParent,pIElement);
						if((pIElement1 = m_pxmlDocument->AddElementWithChildren(pIParent,
							_T("ModuleInstance"),strInstanceName)) != NULL)
						{
							pIElement1->setAttribute(_T("Type"),(LPCTSTR) strModuleType);
							CXmlFabricModule::AddFabricModuleInstance(m_pxmlDocument,pIElement1);
							if(pIParent->appendChild(pIElement1) != NULL)
								bPaste = true;
						}
					}
			}
			if(bPaste)
				dynamic_cast<CXmlFabricModule*>(m_pxmlDocument)->LoadFabricModule(this,
					m_pXmlReport,m_pxmlDocument->GetDocumentRoot());
		}
	}
}

void CListCtrlEx::OnProperties()
{
}

void CListCtrlEx::OnSwitchFabricXmlView()
{
	SaveListState();
	GetParent()->SendMessage(WM_COMMAND,MAKEWPARAM(ID_LISTVIEW_FABRIC_XML,0),0);
}

void CListCtrlEx::OnSwitchFabricTreeView()
{
	SaveListState();
	GetParent()->SendMessage(WM_COMMAND,MAKEWPARAM(ID_LISTVIEW_FABRIC_TREE,0),0);
}

void CListCtrlEx::OnSwitchFabricListView(UINT nID)
{
	SaveListState();
	GetParent()->SendMessage(WM_COMMAND,MAKEWPARAM(nID,0),0);
}

void CListCtrlEx::OnSwitchConnectView()
{
	SaveListState();
	GetParent()->SendMessage(WM_COMMAND,MAKEWPARAM(ID_LISTVIEW_FABRIC_CONNECT,0),0);
}

void CListCtrlEx::OnSwitchPlatformXmlView()
{
	SaveListState();
	GetParent()->SendMessage(WM_COMMAND,MAKEWPARAM(ID_LISTVIEW_PLATFORM_XML,0),0);
}

void CListCtrlEx::OnSwitchPlatformTreeView()
{
	SaveListState();
	GetParent()->SendMessage(WM_COMMAND,MAKEWPARAM(ID_LISTVIEW_PLATFORM_TREE,0),0);
}

void CListCtrlEx::OnSwitchPlatformListView(UINT nID)
{
	SaveListState();
	GetParent()->SendMessage(WM_COMMAND,MAKEWPARAM(nID,0),0);
}

MSXML::IXMLDOMElementPtr CListCtrlEx::GetModuleInstance(int nRow)
{
	CXmlFabricModule* pFmxDoc;
	CXmlPlatformConfig* pPcxDoc;
	if((pFmxDoc = dynamic_cast<CXmlFabricModule*>(m_pxmlDocument)) != NULL)
		return pFmxDoc->GetModuleInstance(GetItemText(nRow,0));
	if((pPcxDoc = dynamic_cast<CXmlPlatformConfig*>(m_pxmlDocument)) != NULL)
		return pPcxDoc->GetModuleInstance(GetItemText(nRow,0));
	return NULL;
}

MSXML::IXMLDOMElementPtr CListCtrlEx::GetParameter(MSXML::IXMLDOMElement* pIElement, 
	LPCTSTR lpszParameterName, bool bOverride)
{
	CString strTemp1,strTemp2,strTemp3,strTemp5,strTemp4 = lpszParameterName;
	strTemp3 = ::GetLastField(strTemp4);
	if(bOverride)
		strTemp1.Format(_T(".//ParameterOverride[@Name=\"%s\"]"),strTemp3);
	else
		strTemp1.Format(_T(".//Parameter[@Name=\"%s\"]"),strTemp3);
	if(strTemp4.IsEmpty())
		return pIElement->selectSingleNode((LPCTSTR) strTemp1);
	else
	{
		MSXML::IXMLDOMNodeListPtr lsoNodes;
		MSXML::IXMLDOMElementPtr pIElement1,pIElement2;
		strTemp5 = strTemp4;
		if((lsoNodes = pIElement->selectNodes((LPCTSTR) strTemp1)) != NULL)
			while((pIElement1 = lsoNodes->nextNode()) != NULL)
			{
				bool bFound = true;
				pIElement2 = pIElement1;
				strTemp4 = strTemp5;
				while(!strTemp4.IsEmpty())
				{
					strTemp3 = ::GetLastField(strTemp4);
					pIElement2 = pIElement2->GetparentNode();
					strTemp1 = (LPCTSTR) pIElement2->GetnodeName();
					strTemp2 = pIElement2->getAttribute(_T("Name")).bstrVal;
					if((strTemp1 != _T("SubParameterOverrides")) || (strTemp2 != strTemp3))
					{
						bFound = false;
						break;
					}
				}
				if(bFound)
					return pIElement1;
			}
	}
	return NULL;
}

bool CListCtrlEx::UpdateParameter(LPCTSTR lpszValue)
{
	if(m_pICurrentElement)
		return m_pxmlDocument->ModifyElement(m_pICurrentElement,_T("Value"),lpszValue,true);
	return false;
}

bool CListCtrlEx::GetInstancePorts(CStringList& rlssInstancePorts, int nRow, int nColumn)
{
	MSXML::IXMLDOMNodeListPtr plsoINodes;
	MSXML::IXMLDOMElementPtr pIElement;
	CString strTemp1,strTemp2;
	rlssInstancePorts.RemoveAll();
	rlssInstancePorts.AddTail(_T(""));
	if(!(strTemp1 = GetItemText(nRow,nColumn)).IsEmpty())
		rlssInstancePorts.AddTail(strTemp1);
	if((m_pICurrentElement = GetModuleInstance(nRow)) != NULL)
	{
		strTemp1 = m_pICurrentElement->getAttribute(_T("InstanceName")).bstrVal;
		strTemp2.Format(_T("./ModuleInstances/ModuleInstance[@InstanceName=\"%s\"]"
			"/InstancePorts/InstancePort"),strTemp1);
		if((plsoINodes = m_pxmlDocument->GetDocumentRoot()->selectNodes((LPCTSTR) strTemp2)) != NULL)
			while((pIElement = plsoINodes->nextNode()) != NULL)
			{
				strTemp2 = pIElement->getAttribute(_T("Name")).bstrVal;
				if(pIElement->GetfirstChild() == NULL)
					rlssInstancePorts.AddTail(strTemp2);
			}
		}
	return rlssInstancePorts.GetCount() > 1;
}

bool CListCtrlEx::AddConnection(LPCTSTR lpszInstancePort, int nColumn)
{
	MSXML::IXMLDOMElementPtr pIElement,pIParent;
	CString strConnectionElement,strTemp;
	strConnectionElement = m_pXmlReport->GetColumnName(nColumn - 
		m_pXmlReport->GetKeyColumnNameSize());
	strConnectionElement = strConnectionElement.Left(strConnectionElement.Find(_T('.')));
	strTemp.Format(_T("./InstancePorts/InstancePort[@Name=\"%s\"]"),lpszInstancePort);
	if((pIParent = m_pICurrentElement->selectSingleNode((LPCTSTR) strTemp)) != NULL)
		if((pIElement = m_pxmlDocument->GetDocument()->createElement(
			_T("PortConnection"))) != NULL)
			if(SUCCEEDED(pIElement->setAttribute(_T("PortOrNetName"),
				(LPCTSTR) strConnectionElement)))
				if(SUCCEEDED(pIElement->setAttribute(_T("Description"),_T(""))))
					if(m_pxmlDocument->AddElement(pIParent,pIElement))
						return true;
	return false;
}

bool CListCtrlEx::ModifyConnection(LPCTSTR lpszOldInstancePort, LPCTSTR lpszNewInstancePort, 
	int nColumn)
{
	if(DeleteConnection(lpszOldInstancePort,nColumn))
		return AddConnection(lpszNewInstancePort,nColumn);
	return false;
}

bool CListCtrlEx::DeleteConnection(LPCTSTR lpszInstancePort, int nColumn)
{
	MSXML::IXMLDOMElementPtr pIElement,pIParent;
	CString strConnectionElement,strTemp;
	strConnectionElement = m_pXmlReport->GetColumnName(nColumn - 
		m_pXmlReport->GetKeyColumnNameSize());
	strConnectionElement = strConnectionElement.Left(strConnectionElement.Find(_T('.')));
	strTemp.Format(_T("./InstancePorts/InstancePort[@Name=\"%s\"]/"
		"PortConnection[@PortOrNetName=\"%s\"]"),lpszInstancePort,strConnectionElement);
	if((pIElement = m_pICurrentElement->selectSingleNode((LPCTSTR) strTemp)) != NULL)
	{
		pIParent = pIElement->GetparentNode();
		if(m_pxmlDocument->DeleteElement(pIParent,pIElement))
			return true;
	}
	return false;
}
*/

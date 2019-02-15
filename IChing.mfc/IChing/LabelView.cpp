#include "StdAfx.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

BEGIN_MESSAGE_MAP(CLabelView, CView)
	ON_WM_CREATE()
	ON_WM_WINDOWPOSCHANGED()

	ON_NOTIFY(LVN_ITEMCHANGING, IDC_LIST_CTRL, OnItemChanging)
	ON_NOTIFY(LVN_ITEMCHANGED, IDC_LIST_CTRL, OnItemChanged)

	ON_COMMAND(ID_FILE_PRINT, CView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_DIRECT, CView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_PREVIEW, CView::OnFilePrintPreview)
END_MESSAGE_MAP()

IMPLEMENT_DYNCREATE(CLabelView, CView)

CLabelView::CLabelView()
{
	m_bUserInput = false;
	m_bInitialised = false;
}

CLabelView::~CLabelView()
{
}

BOOL CLabelView::PreCreateWindow(CREATESTRUCT& cs) 
{
	if(!CView::PreCreateWindow(cs))
		return FALSE;
    cs.style |= WS_CLIPCHILDREN | WS_CLIPSIBLINGS;
	return TRUE;
}

int CLabelView::OnCreate(LPCREATESTRUCT lpCreateStruct) 
{
    if(CView::OnCreate(lpCreateStruct) == -1)
        return -1;
    if(!m_lscLabels.Create(this, 0))
		return -1;
	CreateAndLoadControls();
	m_bUserInput = true;
	m_bInitialised = true;
	return 0;
}

void CLabelView::OnWindowPosChanged(WINDOWPOS* pwndPos) 
{
    CView::OnWindowPosChanged(pwndPos);
	if(::IsWindow(m_lscLabels.GetSafeHwnd()))
		::SetWindowPos(m_lscLabels.GetSafeHwnd(),NULL,0,0,pwndPos->cx - 4,
			pwndPos->cy - 4,SWP_NOZORDER | SWP_NOACTIVATE);
}

void CLabelView::OnDraw(CDC*)
{
}

BOOL CLabelView::OnPreparePrinting(CPrintInfo* pInfo)
{
	return DoPreparePrinting(pInfo);
}

void CLabelView::OnBeginPrinting(CDC*, CPrintInfo*)
{
}

void CLabelView::OnEndPrinting(CDC*, CPrintInfo*)
{
}

void CLabelView::OnItemChanging(NMHDR* pNMHDR, LRESULT* pResult) 
{
	NM_LISTVIEW* pNMListView = (NM_LISTVIEW*) pNMHDR;
	if(m_bUserInput && (pNMListView->uNewState == 4096) && (pNMListView->uOldState == 8192))
		*pResult = 1;
	else
		*pResult = 0;
}

void CLabelView::OnItemChanged(NMHDR* pNMHDR, LRESULT* pResult) 
{
	NM_LISTVIEW* pNMListView = (NM_LISTVIEW*) pNMHDR;
	if((pNMListView->uNewState == 8192) && (pNMListView->uOldState == 4096))
	{
		m_bUserInput = false;
//		if(m_bInitialised)
//			m_prgLabelBlock->SetModified();
		::GetApp()->UpdateAllViews();
		m_bUserInput = true;
	}
	*pResult = 0;
}

void CLabelView::CreateAndLoadControls() 
{
	CString strTemp;
	int nLabel = ::GetApp()->GetLabel();
	m_prgLabelBlock = ::GetApp()->GetLabelBlockArray(nLabel);
	CLabelBlock* pLB;
	CLabelEntryArray* prgLE;
	CLabelEntry* pLE;
	m_nColumns = 1 << nLabel;
	m_lscLabels.AddColumn(_T("Name"),150);
	for(int c = 0; c < m_nColumns; ++c)
	{
		strTemp.Format(_T("Label%d"),c);
		m_lscLabels.AddColumn(strTemp,150);
	}	
	m_nRows = m_prgLabelBlock->GetUpperBound() + 1;
	for(int r = 0; r < m_nRows; ++r)
	{
		pLB = m_prgLabelBlock->GetAt(r);
		prgLE = pLB->GetEntryArray();
		m_lscLabels.InsertItem(r,pLB->GetName());
		for(int c = 0; c < m_nColumns; ++c)
		{
			pLE = prgLE->GetAt(c);
			m_lscLabels.SetItemText(r,c/* * 2 + 2*/ + 1,pLE->GetLabel());
		}
	}
	m_lscLabels.SetItemState(0,LVIS_SELECTED | LVIS_FOCUSED,LVIS_SELECTED | LVIS_FOCUSED);
	m_lscLabels.SetFocus();
}

void CLabelView::Update()
{
}


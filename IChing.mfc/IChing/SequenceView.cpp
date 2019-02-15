#include "StdAfx.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

BEGIN_MESSAGE_MAP(CSequenceView, CView)
	ON_WM_CREATE()
	ON_WM_WINDOWPOSCHANGED()

	ON_NOTIFY(LVN_ITEMCHANGING, IDC_LIST_CTRL, OnItemChanging)
	ON_NOTIFY(LVN_ITEMCHANGED, IDC_LIST_CTRL, OnItemChanged)

	ON_COMMAND(ID_FILE_PRINT, CView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_DIRECT, CView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_PREVIEW, CView::OnFilePrintPreview)
END_MESSAGE_MAP()

IMPLEMENT_DYNCREATE(CSequenceView, CView)

CSequenceView::CSequenceView()
{
	m_bUserInput = false;
	m_bInitialised = false;
}

CSequenceView::~CSequenceView()
{
}

BOOL CSequenceView::PreCreateWindow(CREATESTRUCT& cs) 
{
	if(!CView::PreCreateWindow(cs))
		return FALSE;
    cs.style |= WS_CLIPCHILDREN | WS_CLIPSIBLINGS;
	return TRUE;
}

int CSequenceView::OnCreate(LPCREATESTRUCT lpCreateStruct) 
{
    if(CView::OnCreate(lpCreateStruct) == -1)
        return -1;
    if(!m_lscSequences.Create(this, 0))
		return -1;
	CreateAndLoadControls();
	m_bUserInput = true;
	m_bInitialised = true;
	return 0;
}

void CSequenceView::OnWindowPosChanged(WINDOWPOS* pwndPos) 
{
    CView::OnWindowPosChanged(pwndPos);
	if(::IsWindow(m_lscSequences.GetSafeHwnd()))
		::SetWindowPos(m_lscSequences.GetSafeHwnd(),NULL,0,0,pwndPos->cx - 4,
			pwndPos->cy - 4,SWP_NOZORDER | SWP_NOACTIVATE);
}

void CSequenceView::OnDraw(CDC*)
{
}

BOOL CSequenceView::OnPreparePrinting(CPrintInfo* pInfo)
{
	return DoPreparePrinting(pInfo);
}

void CSequenceView::OnBeginPrinting(CDC*, CPrintInfo*)
{
}

void CSequenceView::OnEndPrinting(CDC*, CPrintInfo*)
{
}

void CSequenceView::OnItemChanging(NMHDR* pNMHDR, LRESULT* pResult) 
{
	NM_LISTVIEW* pNMListView = (NM_LISTVIEW*) pNMHDR;
	if(m_bUserInput && (pNMListView->uNewState == 4096) && (pNMListView->uOldState == 8192))
		*pResult = 1;
	else
		*pResult = 0;
}

void CSequenceView::OnItemChanged(NMHDR* pNMHDR, LRESULT* pResult) 
{
	NM_LISTVIEW* pNMListView = (NM_LISTVIEW*) pNMHDR;
	if((pNMListView->uNewState == 8192) && (pNMListView->uOldState == 4096))
	{
		m_bUserInput = false;
//		if(m_bInitialised)
//			m_prgSequenceBlock->SetModified();
		::GetApp()->UpdateAllViews();
		m_bUserInput = true;
	}
	*pResult = 0;
}

void CSequenceView::CreateAndLoadControls() 
{
	TCHAR szTemp[1024];
	CString strTemp;
	int nSequence = ::GetApp()->GetSequence();
	m_prgSequenceBlock = ::GetApp()->GetSequenceBlockArray(nSequence);
	CSequenceBlock* pSB;
	CSequenceEntryArray* prgSE;
	CSequenceEntry* pSE;
	m_nColumns = 1 << nSequence;
	m_lscSequences.AddColumn(_T("Name"),150);
	for(int c = 0; c < m_nColumns; ++c)
	{
		strTemp.Format(_T("S%d"),c);
		m_lscSequences.AddColumn(strTemp,40);
	}	
	m_nRows = m_prgSequenceBlock->GetUpperBound() + 1;
	for(int r = 0; r < m_nRows; ++r)
	{
		pSB = m_prgSequenceBlock->GetAt(r);
		prgSE = pSB->GetEntryArray();
		m_lscSequences.InsertItem(r,pSB->GetName());
		for(int c = 0; c < m_nColumns; ++c)
		{
			pSE = prgSE->GetAt(c);
			if((nSequence = pSE->GetSequence()) != -1)
				m_lscSequences.SetItemText(r,c + 1,_itoa(nSequence,szTemp,10));
		}
	}
	m_lscSequences.SetItemState(0,LVIS_SELECTED | LVIS_FOCUSED,LVIS_SELECTED | LVIS_FOCUSED);
	m_lscSequences.SetFocus();
}

void CSequenceView::Update()
{
}


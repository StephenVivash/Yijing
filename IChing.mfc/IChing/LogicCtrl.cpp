#include "StdAfx.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

BEGIN_MESSAGE_MAP(CLogicCtrl, CFormView)
	ON_WM_CREATE()
	ON_WM_TIMER()
	ON_BN_CLICKED(IDC_OPERATOR_CTRL,OnOperator)
	ON_BN_CLICKED(IDC_ASSIGN_CTRL,OnAssign)
END_MESSAGE_MAP()

IMPLEMENT_DYNCREATE(CLogicCtrl, CFormView)

CLogicCtrl::CLogicCtrl() : CFormView(IDD_LOGIC_FORM)
{
	m_eOperator = eXor;
	m_eAssign = eEqual;
}

CLogicCtrl::~CLogicCtrl()
{
}

BOOL CLogicCtrl::PreCreateWindow(CREATESTRUCT& cs)
{
	return CFormView::PreCreateWindow(cs);
}

int CLogicCtrl::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if(CFormView::OnCreate(lpCreateStruct) != -1)
	{
		return 0;
	}
	return -1;
}

void CLogicCtrl::OnInitialUpdate()
{
	CFormView::OnInitialUpdate();
}

void CLogicCtrl::OnOperator()
{
	if(m_eOperator == eAnd)
	{
		m_btnOperator.SetWindowText(_T("OR"));
		m_eOperator = eOr;
	}
	else
		if(m_eOperator == eOr)
		{
			m_btnOperator.SetWindowText(_T("XOR"));
			m_eOperator = eXor;
		}
		else
		{
			m_btnOperator.SetWindowText(_T("AND"));
			m_eOperator = eAnd;
		}
	Update();
	::GetApp()->UpdateAllTextViews();
}

void CLogicCtrl::OnAssign()
{
	m_eAssign = eEqual;
	Update();
}

void CLogicCtrl::OnTimer(UINT /*nIDEvent*/)
{
	m_pvscCurrentSequencer->Move();
}

void CLogicCtrl::CreateAndLoadControls()
{
	int nSeperator   = 16; // 13,16,17
	int nBitCx		 = 88;
	int nLthCx		 = nBitCx * 3;
	int nBitX		 = nSeperator;
	int nLineX		 = nBitX + nSeperator + nLthCx;
//	int nTrigramX	 = nBitX + (nSeperator * 2) + (nLthCx * 2);
	int nHexagramX	 = nBitX + (nSeperator * 3) + (nLthCx * 3);
	int nBlthY		 = nBitX;
	int nBlCy		 = 53;
	int nTCy		 = nBlCy * 3;
	int nHCy		 = nTCy * 2;

	UINT nHexagramStyle = ::GetPropertyCtrl()->IsHexagramImages() ? BS_BITMAP :  BS_OWNERDRAW;

	int nX			 = nBitX;
	int nY			 = nBlthY;
	int nCX			 = nLthCx;
	int nCY			 = nHCy;
	m_vscHexagram[0].Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nHexagramStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_HEXAGRAM0_CTRL,&(m_vsHexagram[0]),::GetPropertyCtrl()->IsHexagramImages());

	m_btnOperator.Create(_T("XOR"),WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON,CRect(CPoint(335,160),CSize(50,30)),this,IDC_OPERATOR_CTRL);

	nX = nLineX + 140;
	m_vscHexagram[1].Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nHexagramStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_HEXAGRAM0_CTRL,&(m_vsHexagram[1]),::GetPropertyCtrl()->IsHexagramImages());

	m_btnAssign.Create(_T("="),WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON,CRect(CPoint(750,160),CSize(50,30)),this,IDC_ASSIGN_CTRL);

	nX = nHexagramX;
	m_vscHexagram[2].Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nHexagramStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_HEXAGRAM0_CTRL,&(m_vsHexagram[2]),::GetPropertyCtrl()->IsHexagramImages());

	m_vscHexagram[0].SetFont(XTPPaintManager()->GetRegularBoldFont());
	m_vscHexagram[1].SetFont(XTPPaintManager()->GetRegularBoldFont());
	m_vscHexagram[2].SetFont(XTPPaintManager()->GetRegularBoldFont());

	m_vscHexagram[2].SetMoveable(false);

	m_btnOperator.SetFont(XTPPaintManager()->GetRegularFont());
	m_btnAssign.SetFont(XTPPaintManager()->GetRegularFont());

	SetCurrentSequencerCtrl((CValueSequencerCtrl*) &m_vsHexagram);
}

void CLogicCtrl::Initialise()
{
	m_vsHexagram[0].First();
	m_vsHexagram[1].First();
	++m_vsHexagram[1];
	m_vsHexagram[2].First();
	Update();
}

void CLogicCtrl::UpdateHexagrams(bool bImages)
{
	m_vscHexagram[0].SetMode(bImages);
	m_vscHexagram[1].SetMode(bImages);
	m_vscHexagram[2].SetMode(bImages);
}

void CLogicCtrl::Update()
{
	for(int j = 0; j < 2; ++j)
		for(int k = 0; k < 3; ++k)
		{
			if(m_eOperator == eAnd)
				m_vsHexagram[2][j][k].SetValue((m_vsHexagram[0][j][k].GetValue() % 2) & (m_vsHexagram[1][j][k].GetValue() % 2) ? 1 : 2);
			else
				if(m_eOperator == eOr)
					m_vsHexagram[2][j][k].SetValue((m_vsHexagram[0][j][k].GetValue() % 2) | (m_vsHexagram[1][j][k].GetValue() % 2) ? 1 : 2);
				else
					m_vsHexagram[2][j][k].SetValue((m_vsHexagram[0][j][k].GetValue() % 2) ^ (m_vsHexagram[1][j][k].GetValue() % 2) ? 1 : 2);
			m_vsHexagram[2][j][k].UpdateInnerValues();
			m_vsHexagram[2][j][k].UpdateOuterValues();
		}
	m_vscHexagram[0].Update();
	m_vscHexagram[1].Update();
	m_vscHexagram[2].Update();

	CHexagramValueSequencer* phvsCurrent = ::GetApp()->GetCurrentHexagram();
	phvsCurrent->operator=(m_vsHexagram[2].GetValue());
	::GetApp()->UpdateExplorationCtrl();
}

CHexagramValueSequencer* CLogicCtrl::GetCurrentHexagram()
{
	return &m_vsHexagram[0];
}

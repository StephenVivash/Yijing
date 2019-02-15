#include "StdAfx.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

BEGIN_MESSAGE_MAP(CExplorationCtrl, CFormView)
	ON_WM_CREATE()
	ON_WM_TIMER()
END_MESSAGE_MAP()

IMPLEMENT_DYNCREATE(CExplorationCtrl, CFormView)

int CExplorationCtrl::m_nLineDelay = 0;

CExplorationCtrl::CExplorationCtrl() : CFormView(IDD_EXPLORATION_FORM)
{
}

CExplorationCtrl::~CExplorationCtrl()
{
}

BOOL CExplorationCtrl::PreCreateWindow(CREATESTRUCT& cs)
{
	return CFormView::PreCreateWindow(cs);
}

int CExplorationCtrl::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if(CFormView::OnCreate(lpCreateStruct) != -1)
	{
		return 0;
	}
	return -1;
}

void CExplorationCtrl::OnInitialUpdate()
{
	CFormView::OnInitialUpdate();
}

void CExplorationCtrl::OnTimer(UINT /*nIDEvent*/)
{
	if(!::GetPropertyCtrl()->GetAnimation() && (::GetPropertyCtrl()->GetSpeed() == 10) && 
		(m_pvscCurrentSequencer->GetValueSequencer()->GetNoValues() == 4) && 
		((m_pvscCurrentSequencer->GetValueSequencer()->GetValue() == 1) || 
		(m_pvscCurrentSequencer->GetValueSequencer()->GetValue() == 2)) && (++m_nLineDelay < 3))
		return;
	m_nLineDelay = 0;
	m_pvscCurrentSequencer->Move();
}

void CExplorationCtrl::CreateAndLoadControls()
{
	int nSeperator   = 16; // 13,16,17
	int nBitCx		 = 88;
	int nLthCx		 = nBitCx * 3;

	int nBitX		 = nSeperator;
	int nLineX		 = nBitX + nSeperator + nLthCx;
	int nTrigramX	 = nBitX + (nSeperator * 2) + (nLthCx * 2);
	int nHexagramX	 = nBitX + (nSeperator * 3) + (nLthCx * 3);

	int nBlthY		 = nBitX;
	int nBlCy		 = 53;
	int nTCy		 = nBlCy * 3;
	int nHCy		 = nTCy * 2;

	int nX			 = nBitX;
	int nY			 = nBlthY;
	int nCX			 = nBitCx;
	int nCY			 = nBlCy;

	UINT nBitStyle = ::GetPropertyCtrl()->IsBitImages() ? BS_BITMAP : BS_OWNERDRAW;
	UINT nLineStyle = ::GetPropertyCtrl()->IsLineImages() ? BS_BITMAP : BS_OWNERDRAW;
	UINT nTrigramStyle = ::GetPropertyCtrl()->IsTrigramImages() ? BS_BITMAP : BS_OWNERDRAW;
	UINT nHexagramStyle = ::GetPropertyCtrl()->IsHexagramImages() ? BS_BITMAP : BS_OWNERDRAW;

	m_vscBits[1][2][2].Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nBitStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_BIT52_CTRL,&(m_vsHexagram[1][2][2]),::GetPropertyCtrl()->IsBitImages());
	nX += nCX;
	m_vscBits[1][2][1].Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nBitStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_BIT51_CTRL,&(m_vsHexagram[1][2][1]),::GetPropertyCtrl()->IsBitImages());
	nX += nCX;
	m_vscBits[1][2][0].Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nBitStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_BIT50_CTRL,&(m_vsHexagram[1][2][0]),::GetPropertyCtrl()->IsBitImages());
	nX = nBitX;
	nY += nCY;
	m_vscBits[1][1][2].Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nBitStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_BIT42_CTRL,&(m_vsHexagram[1][1][2]),::GetPropertyCtrl()->IsBitImages());
	nX += nCX;
	m_vscBits[1][1][1].Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nBitStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_BIT41_CTRL,&(m_vsHexagram[1][1][1]),::GetPropertyCtrl()->IsBitImages());
	nX += nCX;
	m_vscBits[1][1][0].Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nBitStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_BIT40_CTRL,&(m_vsHexagram[1][1][0]),::GetPropertyCtrl()->IsBitImages());
	nX = nBitX;
	nY += nCY;
	m_vscBits[1][0][2].Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nBitStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_BIT32_CTRL,&(m_vsHexagram[1][0][2]),::GetPropertyCtrl()->IsBitImages());
	nX += nCX;
	m_vscBits[1][0][1].Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nBitStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_BIT31_CTRL,&(m_vsHexagram[1][0][1]),::GetPropertyCtrl()->IsBitImages());
	nX += nCX;
	m_vscBits[1][0][0].Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nBitStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_BIT30_CTRL,&(m_vsHexagram[1][0][0]),::GetPropertyCtrl()->IsBitImages());
	nX = nBitX;
	nY += nCY;
	m_vscBits[0][2][2].Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nBitStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_BIT22_CTRL,&(m_vsHexagram[0][2][2]),::GetPropertyCtrl()->IsBitImages());
	nX += nCX;
	m_vscBits[0][2][1].Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nBitStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_BIT21_CTRL,&(m_vsHexagram[0][2][1]),::GetPropertyCtrl()->IsBitImages());
	nX += nCX;
	m_vscBits[0][2][0].Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nBitStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_BIT20_CTRL,&(m_vsHexagram[0][2][0]),::GetPropertyCtrl()->IsBitImages());
	nX = nBitX;
	nY += nCY;
	m_vscBits[0][1][2].Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nBitStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_BIT12_CTRL,&(m_vsHexagram[0][1][2]),::GetPropertyCtrl()->IsBitImages());
	nX += nCX;
	m_vscBits[0][1][1].Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nBitStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_BIT11_CTRL,&(m_vsHexagram[0][1][1]),::GetPropertyCtrl()->IsBitImages());
	nX += nCX;
	m_vscBits[0][1][0].Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nBitStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_BIT10_CTRL,&(m_vsHexagram[0][1][0]),::GetPropertyCtrl()->IsBitImages());
	nX = nBitX;
	nY += nCY;
	m_vscBits[0][0][2].Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nBitStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_BIT02_CTRL,&(m_vsHexagram[0][0][2]),::GetPropertyCtrl()->IsBitImages());
	nX += nCX;
	m_vscBits[0][0][1].Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nBitStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_BIT01_CTRL,&(m_vsHexagram[0][0][1]),::GetPropertyCtrl()->IsBitImages());
	nX += nCX;
	m_vscBits[0][0][0].Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nBitStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_BIT00_CTRL,&(m_vsHexagram[0][0][0]),::GetPropertyCtrl()->IsBitImages());

	nX = nLineX;
	nY = nBlthY;
	nCX = nLthCx;
	nCY = nBlCy;
	m_vscLines[1][2].Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nLineStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_LINE12_CTRL,&(m_vsHexagram[1][2]),::GetPropertyCtrl()->IsLineImages());
	nY += nCY;
	m_vscLines[1][1].Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nLineStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_LINE11_CTRL,&(m_vsHexagram[1][1]),::GetPropertyCtrl()->IsLineImages());
	nY += nCY;
	m_vscLines[1][0].Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nLineStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_LINE10_CTRL,&(m_vsHexagram[1][0]),::GetPropertyCtrl()->IsLineImages());
	nY += nCY;
	m_vscLines[0][2].Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nLineStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_LINE02_CTRL,&(m_vsHexagram[0][2]),::GetPropertyCtrl()->IsLineImages());
	nY += nCY;
	m_vscLines[0][1].Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nLineStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_LINE01_CTRL,&(m_vsHexagram[0][1]),::GetPropertyCtrl()->IsLineImages());
	nY += nCY;
	m_vscLines[0][0].Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nLineStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_LINE00_CTRL,&(m_vsHexagram[0][0]),::GetPropertyCtrl()->IsLineImages());

	nX = nTrigramX;
	nY = nBlthY;
	nCX = nLthCx;
	nCY = nTCy;
	m_vscTrigrams[1].Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nTrigramStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_TRIGRAM1_CTRL,&(m_vsHexagram[1]),::GetPropertyCtrl()->IsTrigramImages());
	nY += nCY;
	m_vscTrigrams[0].Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nTrigramStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_TRIGRAM0_CTRL,&(m_vsHexagram[0]),::GetPropertyCtrl()->IsTrigramImages());

	nX = nHexagramX;
	nY = nBlthY;
	nCX = nLthCx;
	nCY = nHCy;
	m_vscHexagram.Create(WS_CHILD | WS_VISIBLE | WS_TABSTOP | BS_PUSHBUTTON | nHexagramStyle, 
		CRect(nX,nY,nX + nCX,nY + nCY),this,IDC_HEXAGRAM_CTRL,&(m_vsHexagram),::GetPropertyCtrl()->IsHexagramImages());

	for(int t = 0; t < 2; ++t)
	{
		for(int l = 0; l < 3; ++l)
		{
			for(int b = 0; b < 3; ++b)
				m_vscBits[t][l][b].SetFont(XTPPaintManager()->GetRegularBoldFont());
			m_vscLines[t][l].SetFont(XTPPaintManager()->GetRegularBoldFont());
		}
		m_vscTrigrams[t].SetFont(XTPPaintManager()->GetRegularBoldFont());
	}
	m_vscHexagram.SetFont(XTPPaintManager()->GetRegularBoldFont());

	SetCurrentSequencerCtrl((CValueSequencerCtrl*) &m_vsHexagram);
//	m_vsHexagram.First();
	Update();
}

void CExplorationCtrl::UpdateBits(bool bImages)
{
	for(int t = 0; t < 2; ++t)
		for(int l = 0; l < 3; ++l)
			for(int b = 0; b < 3; ++b)
				m_vscBits[t][l][b].SetMode(bImages);
}

void CExplorationCtrl::UpdateLines(bool bImages)
{
	for(int t = 0; t < 2; ++t)
		for(int l = 0; l < 3; ++l)
			m_vscLines[t][l].SetMode(bImages);
}

void CExplorationCtrl::UpdateTrigrams(bool bImages)
{
	for(int t = 0; t < 2; ++t)
		m_vscTrigrams[t].SetMode(bImages);
}

void CExplorationCtrl::UpdateHexagrams(bool bImages)
{
	m_vscHexagram.SetMode(bImages);
}

void CExplorationCtrl::Update()
{
	for(int t = 0; t < 2; ++t)
	{
		for(int l = 0; l < 3; ++l)
		{
			for(int b = 0; b < 3; ++b)
				m_vscBits[t][l][b].Update();
			m_vscLines[t][l].Update();
		}
		m_vscTrigrams[t].Update();
	}
	m_vscHexagram.Update();
/*
	if(m_vsLastHexagram[1][2].GetValue() != m_vsHexagram[1][2].GetValue())
	{
		m_vscLines[1][2].Update();
		m_vscBits[1][2][2].Update();
		m_vscBits[1][2][1].Update();
		m_vscBits[1][2][0].Update();
	}
	m_vsLastHexagram = m_vsHexagram;
*/
}

CHexagramValueSequencer* CExplorationCtrl::GetCurrentHexagram()
{
	return &m_vsHexagram;
}

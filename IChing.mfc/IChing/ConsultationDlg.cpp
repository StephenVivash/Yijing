
#include "StdAfx.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

BEGIN_MESSAGE_MAP(CConsultationDlg, CDialog)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_WM_TIMER()
	ON_WM_CLOSE()

	ON_MESSAGE(WM_UPDATE_CTRL,OnUpdateControl)
END_MESSAGE_MAP()

int CConsultationDlg::m_nLineDelay = 0;

CConsultationDlg::CConsultationDlg(CWnd* pParent)	: CDialog(IDD_CONSULTATION_DLG, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CConsultationDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BOOL CConsultationDlg::OnInitDialog()
{
	CDialog::OnInitDialog();
	SetIcon(m_hIcon, TRUE);
	SetIcon(m_hIcon, FALSE);

	int nSeperator   = 16;
	int nBitCx		 = 88;
	int nLthCx		 = nBitCx * 3;

	int nBitX		 = nSeperator;
	int nLineX		 = nSeperator;

	int nBlthY		 = nBitX;
	int nBlCy		 = 53;
	int nTCy		 = nBlCy * 3;
	int nHCy		 = nTCy * 2;
	
	int nX			 = nBitX;
	int nY			 = nBlthY;
	int nCX			 = nBitCx;
	int nCY			 = nBlCy;

	UINT nLineStyle = ::GetPropertyCtrl()->IsLineImages() ? BS_BITMAP : BS_OWNERDRAW;

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

	for(int t = 0; t < 2; ++t)
		for(int l = 0; l < 3; ++l)
			m_vscLines[t][l].SetFont(XTPPaintManager()->GetRegularBoldFont());

	SetCurrentSequencerCtrl((CValueSequencerCtrl*) &m_vsHexagram);
//	m_vsHexagram.First();

	CHexagramValueSequencer* phvsCurrent = ::GetApp()->GetCurrentHexagram();
	m_vsHexagram[1][2] = (*phvsCurrent)[1][2];
	m_vsHexagram[1][1] = (*phvsCurrent)[1][1];
	m_vsHexagram[1][0] = (*phvsCurrent)[1][0];
	m_vsHexagram[0][2] = (*phvsCurrent)[0][2];
	m_vsHexagram[0][1] = (*phvsCurrent)[0][1];
	m_vsHexagram[0][0] = (*phvsCurrent)[0][0];

	Update();

	m_vscLines[1][2].SetMoveable(false);
	m_vscLines[1][1].SetMoveable(false);
	m_vscLines[1][0].SetMoveable(false);
	m_vscLines[0][2].SetMoveable(false);
	m_vscLines[0][1].SetMoveable(false);
	m_vscLines[0][0].SetMoveable(true);

	m_nCurrentTrigram = 0;
	m_nCurrentLine = 0;

	ShowConversationPane();
	m_nCurrentSpeed = ::GetPropertyCtrl()->GetSpeed();
	::GetPropertyCtrl()->SetSpeed(10);

	return TRUE;
}

void CConsultationDlg::OnClose()
{
	::GetPropertyCtrl()->SetSpeed(m_nCurrentSpeed);
	CDialog::OnClose();
}

void CConsultationDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	CDialog::OnSysCommand(nID, lParam);
}

void CConsultationDlg::OnPaint()
{
	if(IsIconic())
	{
		CPaintDC dc(this);
		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
		CDialog::OnPaint();
}

HCURSOR CConsultationDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

void CConsultationDlg::Update()
{
	for(int t = 0; t < 2; ++t)
		for(int l = 0; l < 3; ++l)
			m_vscLines[t][l].Update();
}

void CConsultationDlg::OnTimer(UINT /*nIDEvent*/)
{
	if(!::GetPropertyCtrl()->GetAnimation() && (::GetPropertyCtrl()->GetSpeed() == 10) && 
		(m_pvscCurrentSequencer->GetValueSequencer()->GetNoValues() == 4) && 
		((m_pvscCurrentSequencer->GetValueSequencer()->GetValue() == 1) || 
		(m_pvscCurrentSequencer->GetValueSequencer()->GetValue() == 2)) && (++m_nLineDelay < 3))
		return;
	m_nLineDelay = 0;
	m_pvscCurrentSequencer->Move();
}

LRESULT CConsultationDlg::OnUpdateControl(WPARAM wParam, LPARAM lParam)
{
	m_vscLines[m_nCurrentTrigram][m_nCurrentLine].SetMoveable(false);
	if(++m_nCurrentLine == 3)
	{
		if(++m_nCurrentTrigram == 2)
		{
			::GetPropertyCtrl()->SetSpeed(m_nCurrentSpeed);
			CHexagramValueSequencer* phvsCurrent = ::GetApp()->GetCurrentHexagram();
			(*phvsCurrent)[1][2] = m_vsHexagram[1][2];
			(*phvsCurrent)[1][1] = m_vsHexagram[1][1];
			(*phvsCurrent)[1][0] = m_vsHexagram[1][0];
			(*phvsCurrent)[0][2] = m_vsHexagram[0][2];
			(*phvsCurrent)[0][1] = m_vsHexagram[0][1];
			(*phvsCurrent)[0][0] = m_vsHexagram[0][0];
//			::ProcessMessages();
//			::Sleep(1000);
			::GetApp()->UpdateAllViews();
			ShowExplorationPane();
			ShowPropertiesPane();
//			EndDialog(IDOK);
			return 0;
		}
		m_nCurrentLine = 0;
	}
	m_vscLines[m_nCurrentTrigram][m_nCurrentLine].SetMoveable(true);
	return 0;
}

BOOL CConsultationDlg::KillTimer(UINT_PTR nIDEvent)
{
	if(GetSafeHwnd() != NULL)
		return CWnd::KillTimer(nIDEvent);
	return FALSE;
}

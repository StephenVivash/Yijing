#include "StdAfx.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

class CLoadBeliefBases : public CProcessPath
{                      
public:             
	CLoadBeliefBases(CStringList* plssFiles) {m_plssFiles = plssFiles;}
	virtual ~CLoadBeliefBases() {}
	virtual bool ProcessFile(LPCTSTR lpszPath, LPCTSTR lpszFile, int nPathNo, int nFileNo);
protected:
	CStringList* m_plssFiles;
};

bool CLoadBeliefBases::ProcessFile(LPCTSTR lpszPath, LPCTSTR lpszFile, int nPathNo, int nFileNo)
{
	CString strTemp = ::ConcatPath(lpszPath,lpszFile);
	if(_tcsicmp(lpszFile,_T("Default.mdb")) != 0)
		if(m_plssFiles->Find(strTemp) == NULL)
			m_plssFiles->AddTail(strTemp);
	return true;
}

CBeliefBaseDlg::CBeliefBaseDlg(CWnd* pParent) : CDialog(CBeliefBaseDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CBeliefBaseDlg)
	//}}AFX_DATA_INIT
//	m_hIcon = ::GetApp()->LoadIcon(IDR_MAINFRAME);
}

void CBeliefBaseDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CBeliefBaseDlg)
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CBeliefBaseDlg, CDialog)
	//{{AFX_MSG_MAP(CBeliefBaseDlg)
//	ON_WM_SYSCOMMAND()
//	ON_WM_PAINT()
//	ON_WM_QUERYDRAGICON()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

BOOL CBeliefBaseDlg::OnInitDialog()
{
	int i,nIndex = 0;
	CString strTemp;
	CStringList lssBB;
	CLoadBeliefBases lsoBB(&lssBB);
	CDialog::OnInitDialog();
	if(lsoBB.Go(::ConcatPath(::GetProgramFolder(),_T("Access")),_T("*.mdb"),true))
	{
		POSITION Pos = lssBB.GetHeadPosition();
		while(Pos)
		{
			strTemp = ::GetFileName(lssBB.GetNext(Pos));
			i = ((CComboBox*) GetDlgItem(IDC_BB_OPTIONS))->AddString(strTemp);
			if(strTemp == _T("Femanist"))
				nIndex = i;
		}
	}
	((CComboBox*) GetDlgItem(IDC_BB_OPTIONS))->SetCurSel(nIndex);
	return TRUE;

//	CString strAboutMenu;
//	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
//	ASSERT(IDM_ABOUTBOX < 0xF000);
//	CMenu* pSysMenu = GetSystemMenu(FALSE);
//	if(pSysMenu != NULL)
//	{
//		strAboutMenu.LoadString(IDS_ABOUTBOX);
//		if (!strAboutMenu.IsEmpty())
//		{
//			pSysMenu->AppendMenu(MF_SEPARATOR);
//			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
//		}
//	}
//	SetIcon(m_hIcon, TRUE);
//	SetIcon(m_hIcon, FALSE);
}

void CBeliefBaseDlg::OnOK()
{
	GetDlgItem(IDC_BB_OPTIONS)->GetWindowText(m_strName);
	CDialog::OnOK();
}

/*
void CBeliefBaseDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialog::OnSysCommand(nID, lParam);
	}
}

void CBeliefBaseDlg::OnPaint() 
{
	if (IsIconic())
	{
		CPaintDC dc(this);
		SendMessage(WM_ICONERASEBKGND, (WPARAM) dc.GetSafeHdc(), 0);
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

HCURSOR CBeliefBaseDlg::OnQueryDragIcon()
{
	return (HCURSOR) m_hIcon;
}
*/

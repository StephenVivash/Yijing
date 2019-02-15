#include "StdAfx.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

BEGIN_MESSAGE_MAP(CMainFrame, CMDIFrameWnd)
	ON_WM_CREATE()
	ON_WM_CLOSE()
	ON_WM_DESTROY()

	ON_XTP_CREATECONTROL()

//	ON_COMMAND(ID_HELP_FINDER, &CMDIFrameWnd::OnHelpFinder)
//	ON_COMMAND(ID_HELP, &CMDIFrameWnd::OnHelp)
//	ON_COMMAND(ID_CONTEXT_HELP, &CMDIFrameWnd::OnContextHelp)
//	ON_COMMAND(ID_DEFAULT_HELP, &CMDIFrameWnd::OnHelpFinder)
//	ON_COMMAND(XTP_ID_CUSTOMIZE, OnCustomize)

	ON_COMMAND(IDR_PANE_CONVERSATION, OnShowConversationPane)
	ON_COMMAND(IDR_PANE_PROPERTIES, OnShowPropertiesPane)
	ON_COMMAND(IDR_PANE_CONSULTATION, OnShowConsultationPane)
	ON_COMMAND(IDR_PANE_EXPLORATION, OnShowExplorationPane)
	ON_COMMAND(IDR_PANE_LOGIC, OnShowLogicPane)

	ON_COMMAND(ID_HEXAGRAM_FIRST, OnHexagramFirst)
	ON_COMMAND(ID_HEXAGRAM_PREVIOUS, OnHexagramPrevious)
	ON_COMMAND(ID_HEXAGRAM_NEXT, OnHexagramNext)
	ON_COMMAND(ID_HEXAGRAM_LAST, OnHexagramLast)

	ON_COMMAND(ID_TEXT_BACK, OnTextBack)
	ON_COMMAND(ID_TEXT_FORWARD, OnTextForward)

	ON_MESSAGE(XTPWM_DOCKINGPANE_NOTIFY, OnDockingPaneNotify)
	ON_MESSAGE(WM_UPDATE_TEXT,OnUpdateText)

END_MESSAGE_MAP()

IMPLEMENT_DYNAMIC(CMainFrame, CMDIFrameWnd)

static UINT indicators[] =
{
	ID_SEPARATOR,
	ID_INDICATOR_CAPS,
	ID_INDICATOR_NUM,
	ID_INDICATOR_SCRL,
};

static UINT uHideCmds[] =
{
	ID_WINDOW_CASCADE,
	ID_FILE_PRINT,
	ID_FILE_PRINT_PREVIEW,
};

int nIDIcons[] = 
{
	IDR_PANE_CONVERSATION,
	IDR_PANE_PROPERTIES, 
	IDR_PANE_CONSULTATION,
	IDR_PANE_EXPLORATION,
	IDR_PANE_LOGIC,
};

static UINT commands[] = 
{
	IDR_PANE_CONVERSATION,
	IDR_PANE_PROPERTIES, 
	IDR_PANE_CONSULTATION,
	IDR_PANE_EXPLORATION,
	IDR_PANE_LOGIC,
};

CString g_strTextTypes[] = {
	_T("Text"),
	_T("Image"),
	_T("Judgment"),
	_T("Line0"),
	_T("Line1"),
	_T("Line2"),
	_T("Line3"),
	_T("Line4"),
	_T("Line5"),
	_T("")};

CMainFrame::CMainFrame()
{
}

CMainFrame::~CMainFrame()
{
}

int CMainFrame::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if(CMDIFrameWnd::OnCreate(lpCreateStruct) == -1)
		return -1;
	if(!m_wndStatusBar.Create(this) || !m_wndStatusBar.SetIndicators(indicators,sizeof(indicators)/sizeof(UINT)))
		return -1;
	if(!InitCommandBars())
		return -1;
	CXTPCommandBars* pCommandBars = GetCommandBars();
	if(pCommandBars == NULL)
		return -1;
	CXTPCommandBar* pMenuBar = pCommandBars->SetMenu(_T("Menu Bar"), IDR_MAINFRAME);
	if(pMenuBar == NULL)
		return -1;
	pMenuBar->SetFlags(xtpFlagHideMinimizeBox | xtpFlagHideMaximizeBox);

	CXTPPopupBar::m_dMaxWidthDivisor = 1.0;
	CXTPPaintManager::SetTheme(xtpThemeWhidbey);
	XTPImageManager()->SetMaskColor(RGB(192,192,192));
	CXTPToolBar* pToolBar = (CXTPToolBar*) pCommandBars->Add(_T("Standard"), xtpBarTop);
	if(!pToolBar || !pToolBar->LoadToolBar(IDR_MAINFRAME))
		return -1;
	pToolBar->SetButtonSize(CSize(26,26));
	pCommandBars->HideCommands(uHideCmds,_countof(uHideCmds));
	pCommandBars->GetCommandBarsOptions()->bAlwaysShowFullMenus = FALSE;
	pCommandBars->GetShortcutManager()->SetAccelerators(IDR_MAINFRAME);

//	LoadCommandBars(_T("CommandBars"));
	m_wndMTIClient.Attach(this);
//	m_wndMTIClient.EnableToolTips();
//	m_wndMTIClient.GetPaintManager()->m_bShowIcons = FALSE;

	m_paneManager.InstallDockingPanes(this);
	m_paneManager.SetTheme(xtpPaneThemeWhidbey);

	CXTPDockingPane* pwndPane1 = m_paneManager.CreatePane(IDR_PANE_CONSULTATION,CRect(0,0,300,395),xtpPaneDockTop);
	CXTPDockingPane* pwndPane2 = m_paneManager.CreatePane(IDR_PANE_EXPLORATION,CRect(0,0,300,395),xtpPaneDockTop);
	CXTPDockingPane* pwndPane3 = m_paneManager.CreatePane(IDR_PANE_LOGIC,CRect(0,0,300,395),xtpPaneDockTop);
	
	CXTPDockingPane* pwndPane4 = m_paneManager.CreatePane(IDR_PANE_CONVERSATION,CRect(0,0,300,395),xtpPaneDockLeft);
	CXTPDockingPane* pwndPane5 = m_paneManager.CreatePane(IDR_PANE_PROPERTIES,CRect(0,0,300,395),xtpPaneDockLeft);
	
	m_paneManager.AttachPane(pwndPane2,pwndPane1);
	m_paneManager.AttachPane(pwndPane3,pwndPane2);
	pwndPane1->Select();

	m_paneManager.AttachPane(pwndPane5,pwndPane4);
	pwndPane4->Select();

	if(XTPImageManager()->IsAlphaIconsSupported())
		XTPPaintManager()->GetIconsInfo()->bUseDisabledIcons = TRUE;
	XTPImageManager()->SetIcons(IDB_PANE_COMMANDS,commands,sizeof(commands) / sizeof(UINT),CSize(20,16),xtpImageNormal);

	m_wndConsultation.Create(_T("STATIC"),NULL,WS_CHILD | WS_VISIBLE | WS_CLIPCHILDREN | WS_CLIPSIBLINGS,CXTPEmptyRect(),this,0);
	m_wndExploration.Create(_T("STATIC"),NULL,WS_CHILD | WS_VISIBLE | WS_CLIPCHILDREN | WS_CLIPSIBLINGS,CXTPEmptyRect(),this,0);
	m_wndLogic.Create(_T("STATIC"),NULL,WS_CHILD | WS_VISIBLE | WS_CLIPCHILDREN | WS_CLIPSIBLINGS,CXTPEmptyRect(),this,0);
	
	m_wndConversation.Create(_T("STATIC"),NULL,WS_CHILD | WS_VISIBLE | WS_CLIPCHILDREN | WS_CLIPSIBLINGS,CXTPEmptyRect(),this,0);
	m_wndProperties.Create(_T("STATIC"),NULL,WS_CHILD | WS_VISIBLE | WS_CLIPCHILDREN | WS_CLIPSIBLINGS,CXTPEmptyRect(),this,0);

	CXTPImageManager* pImageManager = pCommandBars->GetImageManager();
	pImageManager->InternalAddRef();
	m_paneManager.SetImageManager(pImageManager);
	m_paneManager.SetAlphaDockingContext(TRUE);
	m_paneManager.SetShowDockingContextStickers(TRUE);

	HKEY hKey;
	if(RegOpenKey(HKEY_CURRENT_USER,_T("Software\\Mystical Logic\\IChing\\DockingPaneLayouts\\Layout2"),&hKey) == ERROR_SUCCESS)
		RegCloseKey(hKey);
	else
		SHDeleteValue(HKEY_CURRENT_USER,_T("Software\\Mystical Logic\\IChing\\Settings"),_T("Window Position"));
	SHDeleteKey(HKEY_CURRENT_USER,_T("Software\\Mystical Logic\\IChing\\DockingPaneLayouts\\Layout1"));

	CXTPDockingPaneLayout layoutNormal(&m_paneManager);
	if(layoutNormal.Load(_T("Layout2")))
		m_paneManager.SetLayout(&layoutNormal);
	return 0;
}

BOOL CMainFrame::PreCreateWindow(CREATESTRUCT& cs)
{
	if(!CMDIFrameWnd::PreCreateWindow(cs))
		return FALSE;
	return TRUE;
}

int CMainFrame::OnCreateControl(LPCREATECONTROLSTRUCT lpCreateControl)
{
	CXTPMenuBar* pMenuBar = DYNAMIC_DOWNCAST(CXTPMenuBar, lpCreateControl->pCommandBar);
	if(pMenuBar)
	{
		ASSERT(lpCreateControl->pMenu);
		CMenu* pPopupMenu = lpCreateControl->pMenu->GetSubMenu(lpCreateControl->nIndex);
		if(lpCreateControl->strCaption == _T("&Window"))
		{
			CXTPControlPopup* pControlPopup = CXTPControlPopup::CreateControlPopup(xtpControlPopup);
			pControlPopup->ExcludeDocTemplate(IDR_MAINFRAME);
			pControlPopup->SetCommandBar(pPopupMenu);
			lpCreateControl->pControl = pControlPopup;
			return TRUE;
		}
//		if(lpCreateControl->strCaption == _T("&Edit"))
//		{
//			CXTPControlPopup* pControlPopup = CXTPControlPopup::CreateControlPopup(xtpControlPopup);
//			pControlPopup->AssignDocTemplate(IDR_IChingTYPE);
//			pControlPopup->SetCommandBar(pPopupMenu);
//			lpCreateControl->pControl = pControlPopup;
//			return TRUE;
//		}
	}
	return FALSE;
}

LRESULT CMainFrame::OnDockingPaneNotify(WPARAM wParam, LPARAM lParam)
{
	if(wParam == XTP_DPN_SHOWWINDOW)
	{
		CXTPDockingPane* pPane = (CXTPDockingPane*) lParam;
		if(!pPane->IsValid())
		{
			switch(pPane->GetID())
			{
			case IDR_PANE_CONVERSATION:
				pPane->Attach(&m_wndConversation);
				break;
			case IDR_PANE_PROPERTIES:
				pPane->Attach(&m_wndProperties);
				break;
			case IDR_PANE_CONSULTATION:
				pPane->Attach(&m_wndConsultation);
				break;
			case IDR_PANE_EXPLORATION:
				pPane->Attach(&m_wndExploration);
				break;
			case IDR_PANE_LOGIC:
				pPane->Attach(&m_wndLogic);
				break;
			}
		}
		return TRUE;
	}
	return FALSE;
}

void CMainFrame::OnCustomize()
{
	CXTPCommandBars* pCommandBars = GetCommandBars();
	if(pCommandBars != NULL)
	{
		CXTPCustomizeSheet dlg(pCommandBars);
		CXTPCustomizeKeyboardPage pageKeyboard(&dlg);
		dlg.AddPage(&pageKeyboard);
		pageKeyboard.AddCategories(IDR_MAINFRAME);
		CXTPCustomizeOptionsPage pageOptions(&dlg);
		dlg.AddPage(&pageOptions);
		CXTPCustomizeCommandsPage* pCommands = dlg.GetCommandsPage();
		pCommands->AddCategories(IDR_MAINFRAME);
		pCommands->InsertAllCommandsCategory();
		pCommands->InsertBuiltInMenus(IDR_MAINFRAME);
		pCommands->InsertNewMenuCategory();
		dlg.DoModal();
	}
}

void CMainFrame::OnShowConversationPane()
{
	m_paneManager.ShowPane(IDR_PANE_CONVERSATION);
}

void CMainFrame::OnShowPropertiesPane()
{
	m_paneManager.ShowPane(IDR_PANE_PROPERTIES);
}

void CMainFrame::OnShowConsultationPane()
{
	m_paneManager.ShowPane(IDR_PANE_CONSULTATION);
}

void CMainFrame::OnShowExplorationPane()
{
	m_paneManager.ShowPane(IDR_PANE_EXPLORATION);
}

void CMainFrame::OnShowLogicPane()
{
	m_paneManager.ShowPane(IDR_PANE_LOGIC);
}

void CMainFrame::OnClose()
{
	CXTWindowPos wndPosition;
//	SaveCommandBars(_T("CommandBars"));

	CXTPDockingPaneLayout layoutNormal(&m_paneManager);
	m_paneManager.GetLayout(&layoutNormal);
	layoutNormal.Save(_T("Layout2"));
	wndPosition.SaveWindowPos(this);

	CMDIFrameWnd::OnClose();
}

void CMainFrame::OnDestroy()
{
	CMDIFrameWnd::OnDestroy();
	m_wndMTIClient.Detach();
}

BOOL CMainFrame::ShowWindowEx(int nCmdShow)
{
	CRect rect;
	CXTWindowPos wndPosition;

	int xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx;
//	save full screen size and compare

	if(wndPosition.LoadWindowPos(this))
		nCmdShow = wndPosition.showCmd;
	else
	{
		GetDesktopWindow()->GetWindowRect(&rect);
		if(rect.Width() < 1155)
		{
			rect.right -= 100;
			rect.bottom -= 100;
			nCmdShow = SW_SHOWMAXIMIZED;
			m_paneManager.ClosePane(IDR_PANE_CONVERSATION);
			m_paneManager.ClosePane(IDR_PANE_PROPERTIES);
		}
		else
		if(rect.Width() < 1460)
		{
			rect.right = 1155;
			rect.bottom = 900;
			m_paneManager.ClosePane(IDR_PANE_CONVERSATION);
			m_paneManager.ClosePane(IDR_PANE_PROPERTIES);
		}
		else
		{
			rect.right = 1460;
			rect.bottom = 1000;
		}
		SetWindowPos(NULL,0,0,rect.right,rect.bottom,SWP_NOMOVE);
		CenterWindow();
	}
	return ShowWindow(nCmdShow);
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void CMainFrame::OnHexagramFirst()
{
	::GetExplorationCtrl()->GetHexagramSequencerCtrl()->OnContextFirst();
	::GetApp()->UpdateAllViews();
}

void CMainFrame::OnHexagramPrevious()
{
	::GetExplorationCtrl()->GetHexagramSequencerCtrl()->OnContextPrevious();
	::GetApp()->UpdateAllViews();
}

void CMainFrame::OnHexagramNext()
{
	::GetExplorationCtrl()->GetHexagramSequencerCtrl()->OnContextNext();
	::GetApp()->UpdateAllViews();
}

void CMainFrame::OnHexagramLast()
{
	::GetExplorationCtrl()->GetHexagramSequencerCtrl()->OnContextLast();
	::GetApp()->UpdateAllViews();
}

void CMainFrame::OnTextBack()
{
	::GetTextDoc()->GetView()->GoBack();
}

void CMainFrame::OnTextForward()
{
	::GetTextDoc()->GetView()->GoForward();
}

LRESULT CMainFrame::OnUpdateText(WPARAM wParam, LPARAM lParam)
{
	::GetApp()->UpdateAllTextViews();
	return 0;
}

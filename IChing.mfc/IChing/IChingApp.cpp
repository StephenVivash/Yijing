#include "StdAfx.h"
#include "MainFrm.h"

#include <initguid.h>
#include "IChing_i.c"
#include "Application.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////
//	1. Bio-feedback (Lie Detector) Calibration
//	2. Bio-feedback Consultation
//	3. Virtual Reality Consultation (Priming)
//	4. Enable users to write their own text
//	5. Coin & Yarrow fast line odds
//	6. Multiple users ?
//	7. Conversation tree, starting with the result of the previous consultation
//	8. Time / space divination selector
//	9. Opposite, vertical, horizontal & calculated sequences
//	10. Alphabetic, ying / yang distibution sequences
//	11. Shiva's hands pointing out trigrams
//	12. Search journal by question / answer etc.
//	13. Google question / answer search ?
//	14. Selectable lines, trigrams or hexagrams in Logic view
//	15. Derive CExlorationCtrl and ClogicCtrl from CFormView 
//	16. Find by name diagram context menu item
//
//	Minimum screen size of 1024x768, 1152x864, 1280x960 - 1280x1024
//
/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////

BEGIN_OBJECT_MAP(ObjectMap)
	OBJECT_ENTRY(CLSID_Application, CApplication)
	OBJECT_ENTRY(CLSID_ValueSequencer, CComValueSequencer)
END_OBJECT_MAP()

class CAboutDlg : public CDialog
{
public:
	CAboutDlg();
	enum {IDD = IDD_ABOUTBOX};

protected:
	virtual void DoDataExchange(CDataExchange* pDX);
	DECLARE_MESSAGE_MAP()
};

BEGIN_MESSAGE_MAP(CAboutDlg, CDialog)
END_MESSAGE_MAP()

BEGIN_MESSAGE_MAP(CIChingApp, CWinApp)
//	ON_COMMAND(ID_FILE_NEW, &CWinApp::OnFileNew)
//	ON_COMMAND(ID_FILE_OPEN, &CWinApp::OnFileOpen)
//	ON_COMMAND(ID_FILE_PRINT_SETUP, &CWinApp::OnFilePrintSetup)
	ON_COMMAND(ID_APP_ABOUT, &CIChingApp::OnAppAbout)
END_MESSAGE_MAP()

CIChingApp g_appIChing;
CComModule _Module;

LPCTSTR FindOneOf(LPCTSTR p1, LPCTSTR p2);

CIChingApp::CIChingApp()
{
	m_nLabel = 0;
	m_nSequence = 0;

	m_ptemLabel = NULL;
	m_ptemText = NULL;

	m_pLabelDoc1 = NULL;
	m_pLabelDoc2 = NULL;
	m_pLabelDoc3 = NULL;
	m_pLabelDoc6 = NULL;
	m_pTextDoc = NULL;

	EnableHtmlHelp();
}

CIChingApp::~CIChingApp()
{
}

BOOL CIChingApp::InitInstance()
{
	INITCOMMONCONTROLSEX InitCtrls;
	InitCtrls.dwSize = sizeof(InitCtrls);
	InitCtrls.dwICC = ICC_WIN95_CLASSES;
	InitCommonControlsEx(&InitCtrls);

	CWinApp::InitInstance();
//	if(!AfxSocketInit())
//	{
//		AfxMessageBox(IDP_SOCKETS_INIT_FAILED);
//		return FALSE;
//	}
	if(!AfxOleInit())
	{
		AfxMessageBox(IDP_OLE_INIT_FAILED);
		return FALSE;
	}
	AfxEnableControlContainer();

	if(!InitATL())
		return FALSE;

	AfxInitRichEdit2();
	AfxEnableControlContainer();
	SetRegistryKey(_T("Mystical Logic"));
	LoadStdProfileSettings();

	if(!InitIChing())
		return FALSE;

	m_ptemLabel = new CMultiDocTemplate(IDR_LABEL_TYPE,
		RUNTIME_CLASS(CLabelDoc),
		RUNTIME_CLASS(CChildFrame),
		RUNTIME_CLASS(CLabelView));
	AddDocTemplate(m_ptemLabel);

	m_ptemSequence = new CMultiDocTemplate(IDR_SEQUENCE_TYPE,
		RUNTIME_CLASS(CSequenceDoc),
		RUNTIME_CLASS(CChildFrame),
		RUNTIME_CLASS(CSequenceView));
	AddDocTemplate(m_ptemSequence);

	m_ptemText = new CMultiDocTemplate(IDR_TEXT_TYPE,
		RUNTIME_CLASS(CTextDoc),
		RUNTIME_CLASS(CChildFrame),
		RUNTIME_CLASS(CTextView));
	AddDocTemplate(m_ptemText);

	CMainFrame* pMainFrame = new CMainFrame;
	if(!pMainFrame || !pMainFrame->LoadFrame(IDR_MAINFRAME))
		return FALSE;
	m_pMainWnd = pMainFrame;

	m_pMainWnd->DragAcceptFiles();
	EnableShellOpen();
	RegisterShellFileTypes(TRUE);

//	CCommandLineInfo cmdInfo;
//	ParseCommandLine(cmdInfo);

	OpenIChing();
	pMainFrame->ShowWindowEx(m_nCmdShow);
	pMainFrame->UpdateWindow();

	return TRUE;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

BOOL CIChingApp::InitATL()
{
	LPTSTR lpCmdLine = GetCommandLine();
	TCHAR szTokens[] = _T("-/");
	BOOL bRun = TRUE;
//	LPCTSTR lpszToken = _T("RegServer");
	LPCTSTR lpszToken = FindOneOf(lpCmdLine, szTokens);
	m_bATLInited = TRUE;
//	HRESULT hRes = CoInitializeEx(NULL,COINIT_MULTITHREADED);
	HRESULT hRes = CoInitialize(NULL);
	if(FAILED(hRes))
	{
		m_bATLInited = FALSE;
		return FALSE;
	}
	_Module.Init(ObjectMap, AfxGetInstanceHandle());
	while (lpszToken != NULL)
	{
		if(lstrcmpi(lpszToken, _T("UnregServer"))==0)
		{
			_Module.UpdateRegistryFromResource(IDR_ICHING, FALSE);
			_Module.UnregisterServer(TRUE);
			UnRegisterControl();
			bRun = FALSE;
			break;
		}
		if(lstrcmpi(lpszToken, _T("RegServer"))==0)
		{
			_Module.UpdateRegistryFromResource(IDR_ICHING, TRUE);
			_Module.RegisterServer(TRUE);
			RegisterControl();
			bRun = FALSE;
			break;
		}
		lpszToken = NULL; // _Module.FindOneOf(lpszToken, szTokens);
	}
	if(!bRun)
	{
		m_bATLInited = FALSE;
		_Module.Term();
		CoUninitialize();
		return FALSE;
	}
	hRes = _Module.RegisterClassObjects(CLSCTX_LOCAL_SERVER,REGCLS_MULTIPLEUSE);
	if(FAILED(hRes))
	{
		m_bATLInited = FALSE;
		CoUninitialize();
		return FALSE;
	}	
	return TRUE;
}

bool CIChingApp::InitIChing()
{
#ifdef _DEBUG
	m_strProgramFolder = _T("C:\\Program Files (x86)\\IChing");
#else
	TCHAR szTemp[1024];
	::GetCurrentDirectory(1024,szTemp);
	m_strProgramFolder = szTemp;
#endif
	if(SelectBB() && m_oleBB.Open(_T("IChing")))
		return LoadBelief();
	return false;
}

bool CIChingApp::OpenIChing()
{
	::GetPropertyCtrl()->CreateAndLoadControls();
	::GetExplorationCtrl()->CreateAndLoadControls();
	::GetLogicCtrl()->CreateAndLoadControls();

	m_pLabelDoc1 = (CLabelDoc*) m_ptemLabel->OpenDocumentFile(NULL);
	m_pLabelDoc1->SetTitle(_T("Bit Labels"));

	m_pSequenceDoc1 = (CSequenceDoc*) m_ptemSequence->OpenDocumentFile(NULL);
	m_pSequenceDoc1->SetTitle(_T("Bit Sequences"));

	m_pLabelDoc2 = (CLabelDoc*) m_ptemLabel->OpenDocumentFile(NULL);
	m_pLabelDoc2->SetTitle(_T("Line Labels"));

	m_pSequenceDoc2 = (CSequenceDoc*) m_ptemSequence->OpenDocumentFile(NULL);
	m_pSequenceDoc2->SetTitle(_T("Line Sequences"));

	m_pLabelDoc3 = (CLabelDoc*) m_ptemLabel->OpenDocumentFile(NULL);
	m_pLabelDoc3->SetTitle(_T("Trigram Labels"));

	m_pSequenceDoc3 = (CSequenceDoc*) m_ptemSequence->OpenDocumentFile(NULL);
	m_pSequenceDoc3->SetTitle(_T("Trigram Sequences"));

	m_pLabelDoc6 = (CLabelDoc*) m_ptemLabel->OpenDocumentFile(NULL);
	m_pLabelDoc6->SetTitle(_T("Hexagram Labels"));

	m_pSequenceDoc6 = (CSequenceDoc*) m_ptemSequence->OpenDocumentFile(NULL);
	m_pSequenceDoc6->SetTitle(_T("Hexagram Sequences"));

	m_pTextDoc = (CTextDoc*) m_ptemText->OpenDocumentFile(NULL); 
	m_pTextDoc->SetTitle(GetCurrentHexagram()->GetLabel());

	::GetExplorationCtrl()->Update();

	::GetLogicCtrl()->Update();
	::GetLogicCtrl()->Initialise();

	::GetConsultationCtrl()->Initialise();
	::GetConsultationCtrl()->Update(::GetConsultationList()->GetHead());

	return true;
}

int CIChingApp::ExitInstance()
{
	if(m_oleBB.IsOpen())
	{
		SaveBelief();
		m_oleBB.Close();
	}
	if(m_bATLInited)
	{
		_Module.RevokeClassObjects();
		_Module.Term();
		CoUninitialize();
	}
	return CWinApp::ExitInstance();
}

/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////

void CIChingApp::OnAppAbout()
{
//	CTextEditDlg dlgTE;
//	dlgTE.DoModal();

	CLoadData LD;
	LD.LoadAll();

//	CAboutDlg aboutDlg;
//	aboutDlg.DoModal();

	::GetTextDoc()->SetTitle(_T("About I Ching"));
	::GetTextDoc()->GetView()->Navigate(_T("About.html"));
	((CChildFrame*) m_pTextDoc->GetView()->GetParent())->ActivateFrame(SW_SHOW);
}

bool CIChingApp::SelectBB()
{
	CString strTemp;
//	CBeliefBaseDlg dlgBB(::GetMainFrame());
//	if(dlgBB.DoModal() == IDOK)
//	{
		strTemp.Format(_T("DSN=IChing%cDBQ=%s.mdb%c%c"),0,::ConcatPaths(m_strProgramFolder,
			_T("Access"),_T("Atheist")/*dlgBB.GetName()*/,NULL),0,0);
#ifdef _DEBUG		
		return true;
#else
		return SQLConfigDataSource(NULL,ODBC_ADD_SYS_DSN,_T("Microsoft Access Driver (*.mdb)"),strTemp);
#endif
//	}
//	return false;
}

bool CIChingApp::LoadBelief()
{
	m_rgLabelBlock1.LoadBelief(&m_oleBB);
	m_rgLabelBlock2.LoadBelief(&m_oleBB);
	m_rgLabelBlock3.LoadBelief(&m_oleBB);
	m_rgLabelBlock6.LoadBelief(&m_oleBB);

	m_rgSequenceBlock1.LoadBelief(&m_oleBB);
	m_rgSequenceBlock2.LoadBelief(&m_oleBB);
	m_rgSequenceBlock3.LoadBelief(&m_oleBB);
	m_rgSequenceBlock6.LoadBelief(&m_oleBB);

	m_rgImageBlock1.LoadBelief(&m_oleBB);
	m_rgImageBlock2.LoadBelief(&m_oleBB);
	m_rgImageBlock3.LoadBelief(&m_oleBB);
	m_rgImageBlock6.LoadBelief(&m_oleBB);

	m_lsoCConsultation.LoadBelief(&m_oleBB);
	return true;
}

bool CIChingApp::SaveBelief()
{
/*
	if(m_rgLabelBlock1.GetModified())
		if(::AppMessage(_T("Save Bit Labels ?"),MB_YESNO | MB_ICONQUESTION,
			::GetMainFrame()) == IDYES)
			m_rgLabelBlock1.SaveBelief(&m_oleBB);
	if(m_rgLabelBlock2.GetModified())
		if(::AppMessage(_T("Save Line Labels ?"),MB_YESNO | MB_ICONQUESTION,
			::GetMainFrame()) == IDYES)
			m_rgLabelBlock2.SaveBelief(&m_oleBB);
	if(m_rgLabelBlock3.GetModified())
		if(::AppMessage(_T("Save Trigram Labels ?"),MB_YESNO | MB_ICONQUESTION,
			::GetMainFrame()) == IDYES)
			m_rgLabelBlock3.SaveBelief(&m_oleBB);
	if(m_rgLabelBlock6.GetModified())
		if(::AppMessage(_T("Save Hexagram Labels ?"),MB_YESNO | MB_ICONQUESTION,
			::GetMainFrame()) == IDYES)
			m_rgLabelBlock6.SaveBelief(&m_oleBB);
*/
//	m_rgImageBlock1.SaveBelief(&m_oleBB);
//	m_rgImageBlock2.SaveBelief(&m_oleBB);
//	m_rgImageBlock3.SaveBelief(&m_oleBB);
//	m_rgImageBlock6.SaveBelief(&m_oleBB);
	return true;
}

int CIChingApp::GetLabel()
{
	if(m_nLabel == 3)
		m_nLabel = 5;
	else
		if(m_nLabel == 6)
			m_nLabel = 0;
	return ++m_nLabel;
}

int CIChingApp::GetSequence()
{
	if(m_nSequence == 3)
		m_nSequence = 5;
	else
		if(m_nSequence == 6)
			m_nSequence = 0;
	return ++m_nSequence;
}

CLabelBlockArray* CIChingApp::GetLabelBlockArray(int nLabel)
{
	switch(nLabel)
	{
		case 1:
			return GetLabelBlock1();
		case 2:
			return GetLabelBlock2();
		case 3:
			return GetLabelBlock3();
		case 6:
			return GetLabelBlock6();
	}
	return NULL;
}

CSequenceBlockArray* CIChingApp::GetSequenceBlockArray(int nSequence)
{
	switch(nSequence)
	{
		case 1:
			return GetSequenceBlock1();
		case 2:
			return GetSequenceBlock2();
		case 3:
			return GetSequenceBlock3();
		case 6:
			return GetSequenceBlock6();
	}
	return NULL;
}

bool CIChingApp::UpdateExplorationCtrl()
{
	::GetExplorationCtrl()->Update();
	return true;
}

bool CIChingApp::UpdateLogicCtrl()
{
	::GetLogicCtrl()->Update();
	return true;
}

bool CIChingApp::UpdateAllLabelViews()
{
	if(m_pLabelDoc1)
		m_pLabelDoc1->GetView()->Update();
	if(m_pLabelDoc2)
		m_pLabelDoc2->GetView()->Update();
	if(m_pLabelDoc3)
		m_pLabelDoc3->GetView()->Update();
	if(m_pLabelDoc6)
		m_pLabelDoc6->GetView()->Update();
	return true;
}

bool CIChingApp::UpdateAllSequenceViews()
{
	if(m_pSequenceDoc1)
		m_pSequenceDoc1->GetView()->Update();
	if(m_pSequenceDoc2)
		m_pSequenceDoc2->GetView()->Update();
	if(m_pSequenceDoc3)
		m_pSequenceDoc3->GetView()->Update();
	if(m_pSequenceDoc6)
		m_pSequenceDoc6->GetView()->Update();
	return true;
}

bool CIChingApp::UpdateAllTextViews()
{
	if(m_pTextDoc)
	{
		m_pTextDoc->SetTitle(GetCurrentHexagram()->GetLabel());
//		::GetTextDoc()->SetTitle(::GetTextDoc()->GetView()->GetLocationURL());
		m_pTextDoc->GetView()->Update();
	}
	return true;
}

bool CIChingApp::UpdateAllViews()
{
	return UpdateExplorationCtrl() && /*UpdateAllLabelViews() &&*/ UpdateAllTextViews();
}

CHexagramValueSequencer* CIChingApp::GetCurrentHexagram()
{
	return ::GetExplorationCtrl()->GetCurrentHexagram();
}

/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////

COleBeliefBase::COleBeliefBase() 
{
	m_bOpen = false;
}

COleBeliefBase::~COleBeliefBase() 
{
}

bool COleBeliefBase::Open(LPCTSTR lpszBeliefSource)
{
	HRESULT hr;
	CDBPropSet dbinit(DBPROPSET_DBINIT);
	dbinit.AddProperty(DBPROP_AUTH_PERSIST_SENSITIVE_AUTHINFO, false);
	dbinit.AddProperty(DBPROP_INIT_DATASOURCE, lpszBeliefSource);
	dbinit.AddProperty(DBPROP_INIT_PROMPT, (short) 4);
	dbinit.AddProperty(DBPROP_INIT_LCID, (long) 3081);
	if(SUCCEEDED(hr = m_DB.OpenWithServiceComponents("MSDASQL.1", &dbinit)))
		if(SUCCEEDED(hr = m_Session.Open(m_DB)))
		{
			m_bOpen = true;
			return true;
		}
	return false;
}

void COleBeliefBase::Close()
{
	m_Session.Close();
	m_DB.Close();
	m_bOpen = false;
}

CAboutDlg::CAboutDlg() : CDialog(CAboutDlg::IDD)
{
	//{{AFX_DATA_INIT(CAboutDlg)
	//}}AFX_DATA_INIT
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CAboutDlg)
	//}}AFX_DATA_MAP
}

LPCTSTR FindOneOf(LPCTSTR p1, LPCTSTR p2)
{
	while (*p1 != NULL)
	{
		LPCTSTR p = p2;
		while (*p != NULL)
		{
			if(*p1 == *p)
				return CharNext(p1);
			p = CharNext(p);
		}
		p1++;
	}
	return NULL;
}

/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////

/*
LONG CIChingModule::Unlock()
{
	AfxOleUnlockApp();
	return 0;
}

LONG CIChingModule::Lock()
{
	AfxOleLockApp();
	return 1;
}

LPCTSTR CIChingModule::FindOneOf(LPCTSTR p1, LPCTSTR p2)
{
	while (*p1 != NULL)
	{
		LPCTSTR p = p2;
		while (*p != NULL)
		{
			if(*p1 == *p)
				return CharNext(p1);
			p = CharNext(p);
		}
		p1++;
	}
	return NULL;
}
*/

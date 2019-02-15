#pragma once

class COleBeliefBase;
class CPropertiesPane;
class CExplorationCtrl;
class CExplorationPane;
class CLogicCtrl;
class CLogicPane;
class CLabelView;
class CSequenceView;
class CTextView;

#include "Resource.h"
#include "IChing_i.h"

#include "Misc.h"
#include "BeliefBaseDlg.h"
#include "TextEditDlg.h"

#include "LabelSet.h"
#include "SequenceSet.h"
#include "ImageSet.h"
#include "TextSet.h"
#include "ConsultationSet.h"

#include "ValueSequencer.h"

#include "ConversationCtrl.h"
#include "ConversationPane.h"

#include "PropertiesPane.h"

#include "ConsultationDlg.h"
#include "ConsultationCtrl.h"
#include "ConsultationPane.h"

#include "ExplorationCtrl.h"
#include "ExplorationPane.h"

#include "LogicCtrl.h"
#include "LogicPane.h"

#include "ListCtrlEx.h"

#include "LabelDoc.h"
#include "LabelView.h"

#include "SequenceDoc.h"
#include "SequenceView.h"

#include "TextDoc.h"
#include "TextView.h"

#include "LoadData.h"
#include "ChildFrm.h"
#include "IChingApp.h"

class CMainFrame : public CXTPMDIFrameWnd
{
public:
	CMainFrame();
	virtual ~CMainFrame();

	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	BOOL ShowWindowEx(int nCmdShow);

	CConversationCtrl* GetConversationCtrl() {return m_wndConversation.GetConversationCtrl();}
	CPropertiesPane* GetPropertyCtrl() {return &m_wndProperties;}
	CConsultationCtrl* GetConsultationCtrl() {return m_wndConsultation.GetConsultationCtrl();}
	CExplorationCtrl* GetExplorationCtrl() {return m_wndExploration.GetExplorationCtrl();}
	CLogicCtrl* GetLogicCtrl() {return m_wndLogic.GetLogicCtrl();}

	afx_msg void OnShowConversationPane();
	afx_msg void OnShowPropertiesPane();
	afx_msg void OnShowConsultationPane();
	afx_msg void OnShowExplorationPane();
	afx_msg void OnShowLogicPane();

protected:
	CXTPTabClientWnd m_wndMTIClient;
	CXTPStatusBar m_wndStatusBar;
	CXTPDockingPaneManager m_paneManager;

	CXTPOfficeBorder<CConversationPane> m_wndConversation;
	CXTPOfficeBorder<CPropertiesPane> m_wndProperties;
	CXTPOfficeBorder<CConsultationPane> m_wndConsultation;
	CXTPOfficeBorder<CExplorationPane> m_wndExploration;
	CXTPOfficeBorder<CLogicPane> m_wndLogic;

	afx_msg void OnHexagramFirst();
	afx_msg void OnHexagramPrevious();
	afx_msg void OnHexagramNext();
	afx_msg void OnHexagramLast();

	afx_msg void OnTextBack();
	afx_msg void OnTextForward();

	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnClose();
	afx_msg void OnDestroy();
	afx_msg void OnCustomize();
	afx_msg int OnCreateControl(LPCREATECONTROLSTRUCT lpCreateControl);
	afx_msg LRESULT OnDockingPaneNotify(WPARAM wParam, LPARAM lParam);
	afx_msg LRESULT OnUpdateText(WPARAM wParam, LPARAM lParam);

	DECLARE_MESSAGE_MAP()
	DECLARE_DYNAMIC(CMainFrame)
};

extern CIChingApp g_appIChing;

extern CString g_strTextTypes[];

inline CIChingApp* GetApp() {return &g_appIChing;}
inline CString GetAppName() {return _T("IChing");}
inline CMainFrame* GetMainFrame() {return (CMainFrame*) ::GetApp()->m_pMainWnd;}

inline CString& GetProgramFolder() {return ::GetApp()->GetProgramFolder();}

inline void ShowConversationPane() {::GetMainFrame()->OnShowConversationPane();}
inline void ShowPropertiesPane() {::GetMainFrame()->OnShowPropertiesPane();}
inline void ShowConsultationPane() {::GetMainFrame()->OnShowConsultationPane();}
inline void ShowExplorationPane() {::GetMainFrame()->OnShowExplorationPane();}
inline void ShowLogicPane() {::GetMainFrame()->OnShowLogicPane();}

inline CConversationCtrl* GetConversationCtrl() {return ::GetMainFrame()->GetConversationCtrl();}
inline CPropertiesPane* GetPropertyCtrl() {return ::GetMainFrame()->GetPropertyCtrl();}
inline CConsultationCtrl* GetConsultationCtrl() {return ::GetMainFrame()->GetConsultationCtrl();}
inline CExplorationCtrl* GetExplorationCtrl() {return ::GetMainFrame()->GetExplorationCtrl();}
inline CLogicCtrl* GetLogicCtrl() {return ::GetMainFrame()->GetLogicCtrl();}
inline CConsultationDlg* GetConsultationDlg() {return ::GetConsultationCtrl()->GetConsultationDlg();}

inline CMultiDocTemplate* GetLabelTemplate() {return ::GetApp()->GetLabelTemplate();}
inline CMultiDocTemplate* GetSequenceTemplate() {return ::GetApp()->GetSequenceTemplate();}
inline CMultiDocTemplate* GetTextTemplate() {return ::GetApp()->GetTextTemplate();}

inline CLabelBlockArray* GetLabelBlock1() {return ::GetApp()->GetLabelBlock1();}
inline CLabelBlockArray* GetLabelBlock2() {return ::GetApp()->GetLabelBlock2();}
inline CLabelBlockArray* GetLabelBlock3() {return ::GetApp()->GetLabelBlock3();}
inline CLabelBlockArray* GetLabelBlock6() {return ::GetApp()->GetLabelBlock6();}
inline CLabelBlockArray* GetLabelBlockArray(int nLabel) {return ::GetApp()->GetLabelBlockArray(nLabel);}

inline CSequenceBlockArray* GetSequenceBlock1() {return ::GetApp()->GetSequenceBlock1();}
inline CSequenceBlockArray* GetSequenceBlock2() {return ::GetApp()->GetSequenceBlock2();}
inline CSequenceBlockArray* GetSequenceBlock3() {return ::GetApp()->GetSequenceBlock3();}
inline CSequenceBlockArray* GetSequenceBlock6() {return ::GetApp()->GetSequenceBlock6();}
inline CSequenceBlockArray* GetSequenceBlockArray(int nSequence) {return ::GetApp()->GetSequenceBlockArray(nSequence);}

inline CImageBlockArray* GetImageBlock1() {return ::GetApp()->GetImageBlock1();}
inline CImageBlockArray* GetImageBlock2() {return ::GetApp()->GetImageBlock2();}
inline CImageBlockArray* GetImageBlock3() {return ::GetApp()->GetImageBlock3();}
inline CImageBlockArray* GetImageBlock6() {return ::GetApp()->GetImageBlock6();}

inline CConsultationList* GetConsultationList() {return ::GetApp()->GetConsultationList();}

inline int GetAtiveBitLabelIndex() {return ::GetPropertyCtrl()->GetBitLabelIndex();}
inline int GetAtiveLineLabelIndex() {return ::GetPropertyCtrl()->GetLineLabelIndex();}
inline int GetAtiveTrigramLabelIndex() {return ::GetPropertyCtrl()->GetTrigramLabelIndex();}
inline int GetAtiveHexagramLabelIndex() {return ::GetPropertyCtrl()->GetHexagramLabelIndex();}

inline int GetAtiveBitSequenceIndex() {return ::GetPropertyCtrl()->GetBitSequenceIndex();}
inline int GetAtiveLineSequenceIndex() {return ::GetPropertyCtrl()->GetLineSequenceIndex();}
inline int GetAtiveTrigramSequenceIndex() {return ::GetPropertyCtrl()->GetTrigramSequenceIndex();}
inline int GetAtiveHexagramSequenceIndex() {return ::GetPropertyCtrl()->GetHexagramSequenceIndex();}

inline CLabelDoc* GetLabelDoc1() {return ::GetApp()->GetLabelDoc1();}
inline CLabelDoc* GetLabelDoc2() {return ::GetApp()->GetLabelDoc2();}
inline CLabelDoc* GetLabelDoc3() {return ::GetApp()->GetLabelDoc3();}
inline CLabelDoc* GetLabelDoc6() {return ::GetApp()->GetLabelDoc6();}

inline CSequenceDoc* GetSequenceDoc1() {return ::GetApp()->GetSequenceDoc1();}
inline CSequenceDoc* GetSequenceDoc2() {return ::GetApp()->GetSequenceDoc2();}
inline CSequenceDoc* GetSequenceDoc3() {return ::GetApp()->GetSequenceDoc3();}
inline CSequenceDoc* GetSequenceDoc6() {return ::GetApp()->GetSequenceDoc6();}

inline CTextDoc* GetTextDoc() {return ::GetApp()->GetTextDoc();}

//inline CHexagramValueSequencer* GetCurrentHexagram() (return ::GetApp()->GetCurrentHexagram();}

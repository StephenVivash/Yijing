#pragma once

#ifndef __AFXWIN_H__
	#error "include 'stdafx.h' before including this file for PCH"
#endif

#include "Resource.h"

class COleBeliefBase
{
public:
	COleBeliefBase();
	virtual ~COleBeliefBase();
	bool Open(LPCTSTR lpszBeliefSource);
	void Close();
	bool IsOpen() {return m_bOpen;}
	CDataSource& GetDataSource() {return m_DB;}
	CSession& GetSession() {return m_Session;}

protected:
	bool m_bOpen;
	CDataSource m_DB;
	CSession m_Session;
};

class CIChingApp : public CWinApp
{
public:
	CIChingApp();
	virtual ~CIChingApp();

	bool InitIChing();
	bool OpenIChing();

	bool SelectBB();
	COleBeliefBase* GetBeliefBase() {return &m_oleBB;}
	CString& GetProgramFolder() {return m_strProgramFolder;}
	
	int GetLabel();
    int GetSequence();

	CLabelBlockArray* GetLabelBlock1() {return &m_rgLabelBlock1;}
	CLabelBlockArray* GetLabelBlock2() {return &m_rgLabelBlock2;}
	CLabelBlockArray* GetLabelBlock3() {return &m_rgLabelBlock3;}
	CLabelBlockArray* GetLabelBlock6() {return &m_rgLabelBlock6;}
	CLabelBlockArray* GetLabelBlockArray(int nLabel);

	CSequenceBlockArray* GetSequenceBlock1() {return &m_rgSequenceBlock1;}
	CSequenceBlockArray* GetSequenceBlock2() {return &m_rgSequenceBlock2;}
	CSequenceBlockArray* GetSequenceBlock3() {return &m_rgSequenceBlock3;}
	CSequenceBlockArray* GetSequenceBlock6() {return &m_rgSequenceBlock6;}
	CSequenceBlockArray* GetSequenceBlockArray(int nSequence);

	CImageBlockArray* GetImageBlock1() {return &m_rgImageBlock1;}
	CImageBlockArray* GetImageBlock2() {return &m_rgImageBlock2;}
	CImageBlockArray* GetImageBlock3() {return &m_rgImageBlock3;}
	CImageBlockArray* GetImageBlock6() {return &m_rgImageBlock6;}

	CConsultationList* GetConsultationList() {return &m_lsoCConsultation;}

	CMultiDocTemplate* GetLabelTemplate() {return m_ptemLabel;}
	CMultiDocTemplate* GetSequenceTemplate() {return m_ptemSequence;}
	CMultiDocTemplate* GetTextTemplate() {return m_ptemText;}

	CLabelDoc* GetLabelDoc1() {return m_pLabelDoc1;}
	CLabelDoc* GetLabelDoc2() {return m_pLabelDoc2;}
	CLabelDoc* GetLabelDoc3() {return m_pLabelDoc3;}
	CLabelDoc* GetLabelDoc6() {return m_pLabelDoc6;}

	CSequenceDoc* GetSequenceDoc1() {return m_pSequenceDoc1;}
	CSequenceDoc* GetSequenceDoc2() {return m_pSequenceDoc2;}
	CSequenceDoc* GetSequenceDoc3() {return m_pSequenceDoc3;}
	CSequenceDoc* GetSequenceDoc6() {return m_pSequenceDoc6;}

	CTextDoc* GetTextDoc() {return m_pTextDoc;}

	CHexagramValueSequencer* GetCurrentHexagram();

	bool UpdateExplorationCtrl();
	bool UpdateLogicCtrl();

	bool UpdateAllViews();
	bool UpdateAllLabelViews();
	bool UpdateAllSequenceViews();
	bool UpdateAllTextViews();

	//{{AFX_VIRTUAL(CIChingApp)
	virtual BOOL InitInstance();
	virtual int ExitInstance();
	//}}AFX_VIRTUAL

	//{{AFX_MSG(CIChingApp)
	afx_msg void OnAppAbout();
	//}}AFX_MSG

protected:
	BOOL m_bATLInited;
	CString m_strProgramFolder;
	COleBeliefBase m_oleBB;
	int m_nLabel;
	int m_nSequence;

	CLabelBlockArray1 m_rgLabelBlock1;
	CLabelBlockArray2 m_rgLabelBlock2;
	CLabelBlockArray3 m_rgLabelBlock3;
	CLabelBlockArray6 m_rgLabelBlock6;

	CSequenceBlockArray1 m_rgSequenceBlock1;
	CSequenceBlockArray2 m_rgSequenceBlock2;
	CSequenceBlockArray3 m_rgSequenceBlock3;
	CSequenceBlockArray6 m_rgSequenceBlock6;

	CImageBlockArray1 m_rgImageBlock1;
	CImageBlockArray2 m_rgImageBlock2;
	CImageBlockArray3 m_rgImageBlock3;
	CImageBlockArray6 m_rgImageBlock6;

	CConsultationList m_lsoCConsultation;

	CMultiDocTemplate* m_ptemLabel;
	CMultiDocTemplate* m_ptemSequence;
	CMultiDocTemplate* m_ptemText;

	CLabelDoc* m_pLabelDoc1;
	CLabelDoc* m_pLabelDoc2;
	CLabelDoc* m_pLabelDoc3;
	CLabelDoc* m_pLabelDoc6;

	CSequenceDoc* m_pSequenceDoc1;
	CSequenceDoc* m_pSequenceDoc2;
	CSequenceDoc* m_pSequenceDoc3;
	CSequenceDoc* m_pSequenceDoc6;

	CTextDoc* m_pTextDoc;

	BOOL InitATL();
	bool LoadBelief();
	bool SaveBelief();

	DECLARE_MESSAGE_MAP()
};

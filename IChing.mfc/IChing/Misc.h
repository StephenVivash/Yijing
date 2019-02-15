#if !defined(AFX_MISC_H__140E2487_0C2D_4E82_88A5_17F6AFEC2CB9__INCLUDED_)
#define AFX_MISC_H__140E2487_0C2D_4E82_88A5_17F6AFEC2CB9__INCLUDED_

#pragma once

#define WM_UPDATE_TEXT				WM_USER + 1
#define WM_UPDATE_CTRL				WM_USER + 2

/////////////////////////////////////////////////////////////////////////////
// COutputCtrl AppentText struct

typedef struct tagAPPENDTEXT
{
	COLORREF crTextColor;
	bool bForce;
	bool bPrompt;
} APPENDTEXT, *LPAPPENDTEXT;

/////////////////////////////////////////////////////////////////////////////
// CStringBuffer class

class CStringBuffer
{
public:
	CStringBuffer(int nSize) {m_nSize = nSize; m_lpszString = new TCHAR[nSize]; *m_lpszString = 0;}
	virtual ~CStringBuffer() {delete [] m_lpszString;}
	operator LPTSTR() {return m_lpszString;}
protected:
	int m_nSize;
	LPTSTR m_lpszString;
};

/////////////////////////////////////////////////////////////////////////////
// VastDocTemplate class

class VastDocTemplate : public CMultiDocTemplate
{
public:
	VastDocTemplate(UINT nIDResource, CRuntimeClass *pDocClass, CRuntimeClass *pFrameClass, 
		CRuntimeClass *pViewClass) : CMultiDocTemplate(nIDResource, pDocClass, 
		pFrameClass, pViewClass) {};
	virtual BOOL GetDocString(CString &rString, enum DocStringIndex index) const;
	virtual Confidence MatchDocType(LPCTSTR lpszPathName, CDocument *&rpDocMatch);
};

/////////////////////////////////////////////////////////////////////////////
// CVaSTTool class

class CVaSTTool : public CObject
{
public:
	CVaSTTool(CString strTool);
	virtual ~CVaSTTool() {}
	CString m_strName;
	CString m_strCommand;
	CString m_strArguments;
	CString m_strDirectory;
	BOOL m_bOutputWindow;
	BOOL m_bPromptForArguments;
	BOOL m_bCloseWindow;
};

/////////////////////////////////////////////////////////////////////////////
// CFileType class

class CFileType
{
public:
	int nId;
	TCHAR szExtension[256];
	TCHAR szTitle[256];
	TCHAR szFilter[1024];
	TCHAR szLastLocation[1024];
};

/////////////////////////////////////////////////////////////////////////////
// CProcessPath classes

class CProcessPath
{                      
public:             
	CProcessPath() {Reset();}
	virtual ~CProcessPath() {}
	void Reset() {m_bScanFlag = true; m_nPathNo = 0;}
	void SetScanFlag(bool bScanFlag) {m_bScanFlag = bScanFlag;}
	bool Go(LPCTSTR lpszPath, LPCTSTR lpszFileSpec, bool bSubDirectories);
	bool Go2(LPCTSTR lpszPath, LPCTSTR lpszFileSpec);
	virtual bool ProcessFile(LPCTSTR lpszPath, LPCTSTR lpszFile, int nPathNo, int nFileNo) = 0;
protected:
	bool m_bScanFlag;
	int m_nPathNo;
};

class CLoadRvpms : public CProcessPath
{                      
public:             
	CLoadRvpms(CStringList* plssRvpms) {m_plssRvpms = plssRvpms;}
	virtual ~CLoadRvpms() {}
	virtual bool ProcessFile(LPCTSTR lpszPath, LPCTSTR lpszFile, int nPathNo, int nFileNo);
protected:
	CStringList* m_plssRvpms;
};

class CLoadRvpmVersions : public CProcessPath
{                      
public:             
	CLoadRvpmVersions(CStringList* plssRvpmVersions) {m_plssRvpmVersions = plssRvpmVersions;}
	virtual ~CLoadRvpmVersions() {}
	virtual bool ProcessFile(LPCTSTR lpszPath, LPCTSTR lpszFile, int nPathNo, int nFileNo);
protected:
	CStringList* m_plssRvpmVersions;
};

class CLoadFileList : public CProcessPath
{                      
public:             
	CLoadFileList(CStringList* plssFiles) {m_plssFiles = plssFiles;}
	virtual ~CLoadFileList() {}
	virtual bool ProcessFile(LPCTSTR lpszPath, LPCTSTR lpszFile, int nPathNo, int nFileNo);
protected:
	CStringList* m_plssFiles;
};

class CLoadHelpFiles : public CLoadFileList
{
public:
	CLoadHelpFiles(CStringList* plssFiles) : CLoadFileList(plssFiles) {}
	virtual ~CLoadHelpFiles() {}
	virtual bool ProcessFile(LPCTSTR lpszPath, LPCTSTR lpszFile, int nPathNo, int nFileNo);
};

/////////////////////////////////////////////////////////////////////////////
// CPtrListEx template

template <class T>
class CPtrListEx : public CPtrList
{
public:
	CPtrListEx() {}
	virtual ~CPtrListEx() {RemoveAll();}
	POSITION AddTail(T* pT) {return CPtrList::AddTail(pT);}
	void AddTail(CPtrListEx<T>* plsoT) {CPtrList::AddTail(plsoT);}
	T* GetHead() const {return (T*) (CPtrList::GetHead());}
	T* GetNext(POSITION& rPosition) const {return (T*) (CPtrList::GetNext(rPosition));}
	void RemoveAll() {while(!IsEmpty()) delete RemoveHead(); CPtrList::RemoveAll();}
	T* RemoveHead() {return (T*) CPtrList::RemoveHead();}
};

/////////////////////////////////////////////////////////////////////////////
// CMessageLog class

#define MSG_ERROR			EVENTLOG_ERROR_TYPE
#define MSG_WARNING			EVENTLOG_WARNING_TYPE
#define MSG_INFORMATION		EVENTLOG_INFORMATION_TYPE
#define MSG_QUESTION		0x0020	

class CMessageLog
{
public:
	CMessageLog();
	virtual ~CMessageLog();

	bool Open(LPCTSTR lpszName, int nLogLevel, int nInteractiveLevel);
	void Close();
	bool Message(LPCTSTR lpszText, int nLogLevel, ...);
	bool Event(LPCTSTR lpszText, int nLogLevel);
	bool MessageBox(LPCTSTR lpszText, int nLogLevel);

protected:
	HANDLE m_hEventLog;
	int m_nLogLevel;
	int m_nInteractiveLevel;
};

/////////////////////////////////////////////////////////////////////////////
// Global functions

//void SetIntialFileTypeLocation(LPCTSTR lpszPath);
//CFileType* GetFileType(int nFileType);
//CFileType* GetDefaultFileType(int nFileType);
//void SetFileType(CFileType* pFT);
//void AddFileType(CFileType* pFT);
//DWORD GetFileTypesLength();
void SetFileTypesLength(DWORD dwLength);
void SetDefaultFileTypes();
void FreeFileTypes();
bool IsFileType(LPCTSTR lpszFileTypes, LPCTSTR lpszExtension);
//bool IsFileType(LPCTSTR lpszPath, int nExtension);
CString FindFile(LPCTSTR lpszPath, LPCTSTR lpszFileSpec, bool bSubFolders);

CString GetPath(LPCTSTR lpszPath);
CString GetShortPath(LPCTSTR lpszLongPath);
CString GetFileName(LPCTSTR lpszPath);
CString GetExtension(LPCTSTR lpszPath);
CString SetDrive(LPCTSTR lpszPath);
bool GetUnc(CString& rstrPath, CString& rstrUnc);
CString CleanupFileName(LPCTSTR lpszFileName);

CString ConcatPaths(LPCTSTR lpszPath1, LPCTSTR lpszPath2, ...);
CString ConcatPath(LPCTSTR lpszPath1, LPCTSTR lpszPath2);
CString GetVastPath(LPCTSTR lpszBasePath, LPCTSTR lpszPath, bool bFile);
CString GetRelativePath(LPCTSTR lpszBasePath, LPCTSTR lpszPath, bool bFile);
CString GetFullPath(LPCTSTR lpszBasePath, LPCTSTR lpszPath);

CString GetOldGccPath(LPCTSTR lpszPath, bool bSpaceEscape = false);
CString GetGccPath(LPCTSTR lpszPath, bool bSpaceEscape = true, bool bQuote = false);

bool CreateDirectory(LPCTSTR lpszPath);
bool CopyFiles(LPCTSTR lpszTo, ...);
bool CopyFolders(LPCTSTR lpszTo, ...);
bool DeleteFiles(LPCTSTR lpszFrom, ...);
bool DeleteFolders(LPCTSTR lpszFrom, ...);
bool DeleteFolder(LPCTSTR lpszFrom);

CString GetVersionString(LPCTSTR lpszVersion);
CString GetBinVersion(LPCTSTR lpszPath);
CString GetNameVersion(LPCTSTR lpszPath);
CString GetFileVersion(LPCTSTR lpszPath);
CString GetWinVersion();

CString GetWinErrorMsg(DWORD dwError);
void ProcessWindowMessages(const HWND hWnd);
//CString i642s(tInt64 i64);
LPTSTR StripString(LPTSTR lpszString);
CString ProperCase(LPCTSTR lpszString);
CString SplitLabel(LPCTSTR lpszLabel);
CString QuoteString(LPCTSTR lpszString);
CString ReplaceWholeWord(LPCTSTR lpszString, LPCTSTR lpszSource, 
	LPCTSTR lpszTarget, LPCTSTR lpszHRef = NULL);
CString ReplaceString(LPCTSTR lpszString, LPCTSTR lpszSource, LPCTSTR lpszTarget);
CString ReplaceStringCase(LPCTSTR lpszString, LPCTSTR lpszSource, LPCTSTR lpszTarget);
bool LoadCommonStrings();
CString ReplaceCommonStrings(LPCTSTR lpszString);
bool IsFabricModule(CStringList* plssProjectSettings, LPCTSTR lpszName, LPCTSTR lpszValue);
CString GetSimulator(CString& rstrBuildType);
int GetTimeUnits(LPCTSTR lpszUnits);

TCHAR* SubString(TCHAR *szTarget, const TCHAR *szSource, int *nOffset, TCHAR cToken, bool bSkipTokens);
TCHAR* SubString(TCHAR *szTarget, const TCHAR *szSource, int *nOffset, TCHAR* szTokens, bool bSkipTokens);
CString GetLastField(CString& rstrPath, TCHAR chDelimiter = _T('.'));
int FindOneOf(LPCTSTR lpszText, TCHAR *szCharSet, int nStartFrom, bool bForward);
int AppMessage(LPCTSTR lpszText, UINT nType, CWnd* pwndParent = NULL);
//void WriteBatchLog();
void ProcessMessages(const HWND hWnd = NULL);
CString YouCantDoThat(LPCTSTR lpszWhat, bool bRunning);

bool CompareFileSpec(LPCTSTR lpszFileSpec, LPCTSTR lpszPath, LPCTSTR lpszFile);

bool EnumurateNetDevices(CStringList& lssDrvs);
bool VersionMatch(LPCTSTR lpszActualVersion, LPCTSTR lpszRequiredVersion, 
	LPTSTR lpszBestVersion);

CString ExtractArrayDimension(LPCTSTR lpszName);
CString ExtractBaseName(LPCTSTR lpszName);

#endif

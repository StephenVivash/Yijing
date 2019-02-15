#include "StdAfx.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif
/*
LPTSTR g_pszTimeUnits[] = {_T("fs"),_T("ps"),_T("ns"),_T("us"),_T("ms"),_T("s"),NULL};

CFileType g_rgnDefaultFileTypes[] = 
{
	{ALL_FILES,				_T(".*"),	_T("Browse for All files"),				_T("All Files (*.*)\0*.*\0"),_T("")},
	{COMET4_FILES,			_T(".cmt"),	_T("Browse for CoMET4 files"),			_T("CoMET4 Files (*.cmt)\0*.cmt\0"
																					"All Files (*.*)\0*.*\0"),_T("")},
	{METEOR2_FILES,			_T(".mtr"),	_T("Browse for METeor2 files"),			_T("METeor2 Files (*.mtr)\0*.mtr\0"
																					"All Files (*.*)\0*.*\0"),_T("")},
	{VASTCONFIG_FILES,		_T(".cfx"),	_T("Browse for Configuration files"),	_T("Configuration Files (*.cfx)\0*.cfx\0"
																					"All Files (*.*)\0*.*\0"),_T("")},
#ifdef _COMET
	{WORKSPACE_FILES,		_T(".wkx"),	_T("Browse for Workspace files"),		_T("Workspace Files (*.wkx)\0*.wkx\0"
																					"CoMET4 Files (*.cmt)\0*.cmt\0"
																					"All Files (*.*)\0*.*\0"),_T("")},
#endif
#ifdef _METEOR
	{WORKSPACE_FILES,		_T(".mkx"),	_T("Browse for Workspace files"),		_T("Workspace Files (*.mkx)\0*.mkx\0"
																					"METeor2 Files (*.mtr)\0*.mtr\0"
																					"All Files (*.*)\0*.*\0"),_T("")},
#endif
	{PROJECT_FILES,			_T(".pjx"),	_T("Browse for Project files"),			_T("Project Files (*.pjx)\0*.pjx\0"
																					"All Files (*.*)\0*.*\0"),_T("")},
	{SOURCE_FILES,			_T(".cpp"),	_T("Browse for Source files"),			_T("Source Files (*.c*;*.h*;*.rc;*.s;*.v*)\0*.c*;*.h*;*.rc;*.s;*.v*\0"
																					"C Files (*.c;*.cc;*.cpp;*.cxx;*.h;*.hpp;*.hxx)\0*.c;*.cc;*.cpp;*.cxx;*.h;*.hpp;*.hxx\0"
																					"Resource Files (*.rc)\0*.rc\0"
																					"Assembler Files (*.asm;*.s)\0*.asm;*.s\0"
																					"Verilog & VHDL Files (*.v;*.vhd;*.vhdl;*.vrh)\0*.v;*.vhd;*.vhdl;*.vrh\0"
																					"Fabric Module Files (*.fmx)\0*.fmx\0"
																					"Peripheral Model Files (*.pmx)\0*.pmx\0"
																					"Platform Config Files (*.pcx)\0*.pcx\0"
																					"Metrix Config Files (*.mcx)\0*.mcx\0"
																					"Define Files (*.def)\0*.def\0"
																					"Dll Files (*.dll)\0*.dll\0"
																					"Text Files (*.txt)\0*.txt\0"
																					"All Files (*.*)\0*.*\0"),_T("")},
	
	{TARGET_FILES,			_T(".bin"),	_T("Browse for Target files"),			_T("Target Files (*.coff;*.elf;*.bin;*.axf;*.dxe;*.sre;*.srec;*.abs;*.eld;*.mot;*.lst)\0*.coff;*.elf;*.bin;*.axf;*.dxe;*.sre;*.srec;*.abs;*.eld;*.mot;*.lst\0"
																					"All Files (*.*)\0*.*\0"),_T("")},
	{EXECUTABLE_FILES,		_T(".exe"),	_T("Browse for Executable files"),		_T("Executable Files (*.exe;*.bin)\0*.exe;*.bin\0"
																					"All Files (*.*)\0*.*\0"),_T("")},
	{DLL_FILES,				_T(".dll"),	_T("Browse for DLL files"),				_T("DLL Files (*.dll)\0*.dll\0"
																					"All Files (*.*)\0*.*\0"),_T("")},
	{COBJECT_FILES,			_T(".obj"),	_T("Browse for C Object files"),		_T("C Object Files (*.o;*.obj)\0*.o;*.obj\0"
																					"All Files (*.*)\0*.*\0"),_T("")},
	{VERILOGOBJECT_FILES,	_T(".md"),	_T("Browse for Verilog Object files"),	_T("Verilog Object Files (*.md;*.edf;*.dat)\0*.md;*.edf;*.dat\0"
																					"All Files (*.*)\0*.*\0"),_T("")},
	{LIBRARY_FILES,			_T(".lib"),	_T("Browse for Library files"),			_T("Library Files (*.a;*.lib)\0*.a;*.lib\0"
																					"All Files (*.*)\0*.*\0"),_T("")},
	{CONFIG_FILES,			_T(".cfg"),	_T("Browse for Configuration files"),	_T("Configuration Files (*.cfg)\0*.cfg\0"
																					"All Files (*.*)\0*.*\0"),_T("")},
	{TAP_FILES,				_T(".tap"),	_T("Browse for TAP files"),				_T("TAP Files (*.tap)\0*.tap\0"
																					"All Files (*.*)\0*.*\0"),_T("")},
	{TAB_FILES,				_T(".tab"),	_T("Browse for TAB files"),				_T("TAB Files (*.tab)\0*.tab\0"
																					"All Files (*.*)\0*.*\0"),_T("")},
	{DEFINE_FILES,			_T(".def"),	_T("Browse for Define files"),			_T("Define Files (*.def)\0*.def\0"
																					"All Files (*.*)\0*.*\0"),_T("")},
	{FABRICMODULE_FILES,	_T(".fmx"),	_T("Browse for Fabric Module files"),	_T("Fabric Module Files (*.fmx)\0*.fmx\0"
																					"All Files (*.*)\0*.*\0"),_T("")},
	{PERIPHERALMODEL_FILES,_T(".pmx"),	_T("Browse for Peripheral Model files"),_T("Peripheral Model Files (*.pmx)\0*.pmx\0"
																					"All Files (*.*)\0*.*\0"),_T("")},
	{TARGETMODULE_FILES,	_T(".tmx"),	_T("Browse for Target Module files"),	_T("Target Module Files (*.tmx)\0*.tmx\0"
																					"All Files (*.*)\0*.*\0"),_T("")},
	{PLATFORMCONFIG_FILES,	_T(".pcx"),	_T("Browse for Platform Config files"),	_T("Platform Config Files (*.pcx)\0*.pcx\0"
																					"All Files (*.*)\0*.*\0"),_T("")},
	{METRIXCONFIG_FILES,	_T(".mcx"),	_T("Browse for Metrix Config files"),	_T("Metrix Config Files (*.mcx)\0*.mcx\0"
																					"All Files (*.*)\0*.*\0"),_T("")},
	{SYMBOL_FILES,			_T(".sym"),	_T("Browse for Symbol files"),			_T("Symbol Files (*.sym)\0*.sym\0"
																					"All Files (*.*)\0*.*\0"),_T("")},
	{VCD_FILES,				_T(".vcd"),	_T("Browse for Dump files"),			_T("Dump Files (*.vcd)\0*.vcd\0"
																					"All Files (*.*)\0*.*\0"),_T("")},
	{TEXT_FILES,			_T(".txt"),	_T("Browse for Text files"),			_T("Text Files (*.txt)\0*.txt\0"
																					"All Files (*.*)\0*.*\0"),_T("")},
	{DEPENDENCY_FILES,		_T(".dep"),	_T("Browse for Dependency files"),		_T("Dependency Files (*.dep)\0*.dep\0"
																					"All Files (*.*)\0*.*\0"),_T("")},
	{CABINET_FILES,			_T(".cab"),	_T("Browse for Cabinet files"),			_T("Cabinet Files (*.cab)\0*.cab\0"
																					"All Files (*.*)\0*.*\0"),_T("")}
};

CFileType* g_rgnFileTypes;
DWORD g_nFileTypesLength;

void SetIntialFileTypeLocation(LPCTSTR lpszPath)
{
	for(int i = 0; i <= LAST_FILE_TYPE; ++i)
		_tcscpy(g_rgnFileTypes[i].szLastLocation,lpszPath);
}

CFileType* GetDefaultFileType(int nFileType)
{
	return &g_rgnDefaultFileTypes[nFileType];
}

CFileType* GetFileType(int nFileType)
{
	return &g_rgnFileTypes[nFileType];
}

void SetFileType(CFileType* pFT)
{
	if(pFT != NULL) 
	{
		delete [] g_rgnFileTypes;
		g_rgnFileTypes = NULL;
		g_rgnFileTypes = pFT;
	}
}

void AddFileType(CFileType* pFT)
{
	if(pFT != NULL) 
	{
		CFileType* pNew = new CFileType[g_nFileTypesLength + 1];
		memcpy(pNew, g_rgnFileTypes, sizeof(CFileType)*g_nFileTypesLength);
		pFT->nId = g_nFileTypesLength;
		memcpy(&pNew[g_nFileTypesLength], pFT, sizeof(CFileType));
        delete [] g_rgnFileTypes;
		g_rgnFileTypes = pNew;
		g_nFileTypesLength ++;
	}
}

DWORD GetFileTypesLength()
{
	return g_nFileTypesLength;
}

void SetFileTypesLength(DWORD dwLength)
{
	g_nFileTypesLength = dwLength;
}

void SetDefaultFileTypes()
{
	if(g_rgnFileTypes)
		delete g_rgnFileTypes;
	g_rgnFileTypes  = new CFileType[LAST_FILE_TYPE + 1];
	for(int i = 0; i <= LAST_FILE_TYPE; ++i)
	{	
		g_rgnFileTypes[i].nId = GetDefaultFileType(i)->nId;
		memcpy(g_rgnFileTypes[i].szExtension, GetDefaultFileType(i)->szExtension, sizeof(GetDefaultFileType(i)->szExtension));
		memcpy(g_rgnFileTypes[i].szFilter, GetDefaultFileType(i)->szFilter, sizeof(GetDefaultFileType(i)->szFilter));
		memcpy(g_rgnFileTypes[i].szLastLocation, GetDefaultFileType(i)->szLastLocation, sizeof(GetDefaultFileType(i)->szLastLocation));
		memcpy(g_rgnFileTypes[i].szTitle, GetDefaultFileType(i)->szTitle, sizeof(GetDefaultFileType(i)->szTitle));
	}
	g_nFileTypesLength = LAST_FILE_TYPE + 1;
}

void FreeFileTypes()
{
	delete [] g_rgnFileTypes;
	g_rgnFileTypes = NULL;
}
*/
bool IsFileType(LPCTSTR lpszFileTypes, LPCTSTR lpszExtension)
{
	int nOffset = 0;
	TCHAR szTemp[1024];
	while(::SubString(szTemp,lpszFileTypes,&nOffset,_T('.'),true))
		if(_tcsicmp(szTemp,&lpszExtension[1]) == 0)
			return true;
	return false;
}

//bool IsFileType(LPCTSTR lpszPath, int nExtension)
//{
//	CString strTemp = ::GetExtension(lpszPath);
//	if(!strTemp.IsEmpty())
//		return IsFileType(::GetFileType(nExtension)->szExtension,strTemp);
//	return false;
//}

CString GetPath(LPCTSTR lpszPath)
{
	int nLength;
	TCHAR szPath[_MAX_DIR] = _T("");
	TCHAR szDrive[_MAX_DRIVE] = _T("");
	CString strPath;
	_tsplitpath(lpszPath,szDrive,szPath,NULL,NULL);
	if((nLength = _tcslen(szPath)) > 0)
		if(szPath[nLength - 1] == '\\')
			szPath[nLength - 1] = 0;
	strPath.Format(_T("%s%s"),szDrive,szPath);
	return strPath;
}

CString GetShortPath(LPCTSTR lpszLongPath)
{
	TCHAR szShortPath[1024];
	if(GetShortPathName(lpszLongPath,szShortPath,1024) > 0)
		return szShortPath;
	return lpszLongPath;
}

CString GetFileName(LPCTSTR lpszPath)
{
	TCHAR szFileName[_MAX_FNAME] = _T("");
	_tsplitpath(lpszPath,NULL,NULL,szFileName,NULL);
	return szFileName;
}

CString GetExtension(LPCTSTR lpszPath)
{
	TCHAR szExtension[_MAX_EXT] = _T("");
	_tsplitpath(lpszPath,NULL,NULL,NULL,szExtension);
	return szExtension;
}

bool GetUnc(CString& rstrPath, CString& rstrUnc)
{
	rstrUnc = _T("\\");
	int nIndex;
	if((rstrPath.GetLength() > 1) && (rstrPath[0] == _T('\\')) && (rstrPath[1] == _T('\\')))
	{
		rstrPath = rstrPath.Right(rstrPath.GetLength() - 2);
		for(int i = 0; i < 2; ++i)
			if((nIndex = rstrPath.Find(_T('\\'))) != -1)
			{
				rstrUnc += _T("\\") + rstrPath.Left(nIndex);
				rstrPath = rstrPath.Right(rstrPath.GetLength() - nIndex - 1);
				if(i == 1)
					return true;
			}
	}
	return false;
}

CString SetDrive(LPCTSTR lpszPath)
{
	CString strTemp = lpszPath;
	if((strTemp.GetLength() > 1) && (strTemp[1] == _T(':')))
		for(int i = 0; i < 5; ++i)
		{
			strTemp.SetAt(0,TCHAR(int(_T('C')) + i));
			if(_access(strTemp,0) == 0)
				return strTemp;
		}
	return lpszPath;
}

CString CleanupFileName(LPCTSTR lpszFileName)
{
	int nCount = 0,nLength = _tcslen(lpszFileName);
	TCHAR szTemp[1024];
	TCHAR szFileName[1024];
	CString strFileName;

	_tcscpy(szFileName,lpszFileName);

	for(int i = 0; i < nLength; ++i)
		if(szFileName[i] == '/')
			szFileName[i] = '\\';
	
	for(int i = 0; i < nLength; ++i)
		if((iswalnum(szFileName[i]) || ispunct(szFileName[i]) || (szFileName[i] == _T(' '))) && 
			(szFileName[i] != ',') && (szFileName[i] != '"') && (szFileName[i] != '\''))
			if((szFileName[i] != ':') || (szFileName[i + 1] == '\\'))
				szTemp[nCount++] = szFileName[i];

	szTemp[nCount] = 0;

	if((szTemp[0] == '\\') && (szTemp[1] == '\\'))
	{
		szFileName[0] = szTemp[2];
		szFileName[1] = ':';
		szFileName[2] = 0;
		_tcscat(szFileName,&szTemp[3]);
	}
	else
		_tcscpy(szFileName,szTemp);

	strFileName = szFileName;

	strFileName.TrimLeft();
	strFileName.TrimRight();

	if(strFileName[strFileName.GetLength() - 1] == _T('\\'))
		strFileName = strFileName.Left(strFileName.GetLength() - 1);

	return strFileName;
}

CString ConcatPaths(LPCTSTR lpszPath1, LPCTSTR lpszPath2, ...)
{
	va_list vaList;
//	int nCount = 0;
	CString strTemp = ConcatPath(lpszPath1,lpszPath2);
	LPCTSTR lpszArg;
	va_start(vaList,lpszPath2);
	while((lpszArg = va_arg(vaList,LPCTSTR)) != NULL)
	{
//		if(++nCount == 2)
//			ASSERT(FALSE);
		strTemp = ConcatPath(strTemp,lpszArg);
	}
	return strTemp;
}

CString ConcatPath(LPCTSTR lpszPath1, LPCTSTR lpszPath2)
{
	CString strPath1 = lpszPath1;
	CString strPath2 = lpszPath2;
	int nLength;

	if((nLength = strPath1.GetLength()) > 0)
		if(strPath1[nLength - 1] == _T('\\'))
			strPath1 = strPath1.Left(nLength - 1);
	
//	if((nLength = strPath1.GetLength()) > 0)
//		if(strPath1[0] == _T('\\'))
//			strPath1 = strPath1.Right(nLength - 1);

	if((strPath2.GetLength() > 1) && ((strPath2[1] == _T(':')) || 
		((strPath2[0] == _T('\\')) && (strPath2[1] == _T('\\')))))
		return strPath2;

	if((nLength = strPath2.GetLength()) > 0)
		if(strPath2[nLength - 1] == _T('\\'))
			strPath2 = strPath2.Left(nLength - 1);

	if((nLength = strPath2.GetLength()) > 0)
		if(strPath2[0] == _T('\\'))
			strPath2 = strPath2.Right(nLength - 1);

	if(strPath1.GetLength() > 0)
		if(strPath2.GetLength() > 0)
			return strPath1 + _T("\\") + strPath2;
		else
			return strPath1;
	else
		if(strPath2.GetLength() > 0)
			return strPath2;
	return _T("");
}

CString GetVastPath(LPCTSTR lpszBasePath, LPCTSTR lpszPath, bool bFile)
{
	CString strFullPath = ::GetFullPath(lpszBasePath,lpszPath);
	CString strVast5Home = ::GetProgramFolder();
	if(_tcsnicmp(lpszBasePath,strVast5Home,strVast5Home.GetLength()) != 0)
		if(_tcsnicmp(strFullPath,strVast5Home,strVast5Home.GetLength()) == 0)
			return strFullPath;
	return GetRelativePath(lpszBasePath,lpszPath,bFile);
}

CString GetRelativePath(LPCTSTR lpszBasePath, LPCTSTR lpszPath, bool bFile)
{
	CString strFileName,strTemp1,strTemp2,strTemp3,strTemp4;
	CString strRelativePath = lpszPath;
	CString strFullPath = ::GetFullPath(lpszBasePath,lpszPath);	
	int nIndex1,nIndex2;
	
//	if((_tcslen(lpszPath) > 1) && ((lpszPath[1] == _T(':')) || 
//		((lpszPath[0] == _T('\\')) && (lpszPath[0] == _T('\\')))))
//		return lpszPath;

	if(bFile)
		if((nIndex1 = strFullPath.ReverseFind('\\')) != -1)
		{
			strFileName = strFullPath.Right(strFullPath.GetLength() - nIndex1 - 1);
			strFullPath = strFullPath.Left(nIndex1 + 1);
		}
		else ;
	else
		if(strFullPath[strFullPath.GetLength() - 1] != _T('\\'))
			strFullPath += _T("\\");

	strTemp1 = strFullPath;
	strTemp3 = lpszBasePath;
	if(strTemp3[strTemp3.GetLength() - 1] != _T('\\'))
		strTemp3 += _T("\\");

	if(_tcsnicmp(strTemp1,strTemp3,2) != 0) 
		return strRelativePath; // Not the same drive

	if(strFullPath.CompareNoCase(strTemp3) == 0)
		if(strFileName.GetLength())
			strRelativePath = strFileName; // Equal File
		else
			strRelativePath = "."; // Equal Folder
	else
	{
		while(((nIndex1 = strTemp1.Find('\\')) != -1) && ((nIndex2 = strTemp3.Find('\\')) != -1))
		{
			strTemp2 = strTemp1.Left(nIndex1);
			strTemp1 = strTemp1.Right(strTemp1.GetLength() - nIndex1 - 1);
			strTemp4 = strTemp3.Left(nIndex2);
			strTemp3 = strTemp3.Right(strTemp3.GetLength() - nIndex2 - 1);
			if(strTemp2.CompareNoCase(strTemp4) != 0)
				break;
		}
	
		if(strTemp3.IsEmpty())
			if(strTemp2.CompareNoCase(strTemp4) != 0)
			{
				nIndex1 = strTemp4.GetLength(); // Different at same level
				strRelativePath = "..\\";
				for(nIndex2 = 0; nIndex2 < nIndex1; ++nIndex2)
					if(strTemp4[nIndex2] == '\\')
						strRelativePath += "..\\";
				strRelativePath += strTemp2 + "\\" + strTemp1 + strFileName;
			}
			else
				strRelativePath = strTemp1 + strFileName; // Longer
		else
		{
			nIndex1 = strTemp3.GetLength();
			strRelativePath.Empty();
			for(nIndex2 = 0; nIndex2 < nIndex1; ++nIndex2)
				if(strTemp3[nIndex2] == '\\')
					strRelativePath += "..\\";
			if(strTemp2.CompareNoCase(strTemp4) == 0)
				strRelativePath += strFileName; // Just shorter
			else
				strRelativePath += "..\\" + strTemp2 + "\\" + strTemp1 + strFileName;
		}
	}

	if(!bFile && (strRelativePath[strRelativePath.GetLength() - 1] == _T('\\')))
		return strRelativePath.Left(strRelativePath.GetLength() - 1);
	return strRelativePath;
}

CString GetFullPath(LPCTSTR lpszBasePath, LPCTSTR lpszPath)
{
	
//	if((_tcslen(lpszPath) > 1) && ((lpszPath[1] == _T(':')) ||
//		((lpszPath[0] == _T('\\')) && (lpszPath[1] == _T('\\')))))
//		return lpszPath;

	TCHAR szFullPath[MAX_PATH] = {0};
	TCHAR szPath[MAX_PATH] = {0};
	if(_tcslen(lpszBasePath) > 0)
		::SetCurrentDirectory(lpszBasePath);
	if(_tcslen(lpszPath) > 0)
		_tcscpy(szPath,lpszPath);
	else
		_tcscpy(szPath,lpszBasePath);
	::GetFullPathName(szPath,MAX_PATH,szFullPath,NULL);
	if(szFullPath[_tcslen(szFullPath) - 1] == _T('\\'))
		szFullPath[_tcslen(szFullPath) - 1] = 0;
	return szFullPath;
}

CString GetOldGccPath(LPCTSTR lpszPath, bool bSpaceEscape)
{
	CString strTemp1,strTemp2;
	if(lpszPath[1] == _T(':'))
	{
		strTemp1 = _T("//"); 
		strTemp1 += lpszPath[0];
		strTemp1 += &lpszPath[2]; 
	}
	else
		strTemp1 = lpszPath;
	int nLength = strTemp1.GetLength();
	for(int i = 0; i < nLength; ++i)
		if(strTemp1[i] == _T('\\'))
			strTemp2 += _T("/");
		else
			if(bSpaceEscape && (strTemp1[i] == _T(' ')))
				strTemp2 += _T("\\ ");
			else
				strTemp2 += strTemp1[i];
	return strTemp2;
}

CString GetGccPath(LPCTSTR lpszPath, bool bSpaceEscape, bool bQuote)
{
	if(bQuote)
		return ::QuoteString(::GetOldGccPath(lpszPath,bSpaceEscape));
	else
		return ::GetOldGccPath(lpszPath,bSpaceEscape);
}

bool CreateDirectory(LPCTSTR lpszPath)
{
	bool bContinue = true;
	int nIndex;
	CString strUnc,strTemp1,strTemp2 = lpszPath;
	if(GetUnc(strTemp2,strUnc))
		if(!SetCurrentDirectory(strUnc))
			return false;
	while(bContinue)
	{
		if((nIndex = strTemp2.Find(_T('\\'))) != -1)
		{
			strTemp1 = ::ConcatPath(strTemp1,strTemp2.Left(nIndex));
			strTemp2 = strTemp2.Right(strTemp2.GetLength() - nIndex - 1);
		}
		else
		{
			strTemp1 = ::ConcatPath(strTemp1,strTemp2);
			bContinue = false;
		}
		if(_access(strTemp1,0) != 0)		
			if(!::CreateDirectory(strTemp1,NULL))
				return false;
	}
	return true;
}

bool CopyFiles(LPCTSTR lpszTo, ...)
{
	va_list vaList;
	CString strTemp;
	int nIndex = 0;
	LPCTSTR lpszArg;
	va_start(vaList,lpszTo);
	while((lpszArg = va_arg(vaList,LPCTSTR)) != NULL)
		if((_tcslen(lpszArg) > 0) && (_access(lpszArg,0) == 0))
		{
			strTemp = lpszArg;
			if((nIndex = strTemp.ReverseFind(_T('\\'))) != -1)
				strTemp = strTemp.Right(strTemp.GetLength() - nIndex - 1);
			strTemp = ::ConcatPath(lpszTo,strTemp);
			if(!CopyFile(lpszArg,strTemp,FALSE))
				return false;
		}
	return true;
}

bool CopyFolders(LPCTSTR lpszTo, ...)
{
	va_list vaList;
	int nIndex = 0;
	TCHAR szFrom[10240];
	LPCTSTR lpszArg;
	SHFILEOPSTRUCT shFO;
	va_start(vaList,lpszTo);
	memset(&shFO,0,sizeof SHFILEOPSTRUCT);
	shFO.wFunc = FO_COPY;
	shFO.fFlags = FOF_NOCONFIRMATION | FOF_NOCONFIRMMKDIR;
	shFO.pFrom = szFrom;
	shFO.pTo = lpszTo;
	while((lpszArg = va_arg(vaList,LPCTSTR)) != NULL)
		if((_tcslen(lpszArg) > 0) && (_access(lpszArg,0) == 0))
		{
			_tcscpy(&szFrom[nIndex],lpszArg);
			nIndex += _tcslen(lpszArg) + 1;
		}
	szFrom[nIndex] = 0;
	va_end(vaList);
	if(!_tcslen(szFrom) || SHFileOperation(&shFO) == 0)
		return true;
	return false;
}

bool DeleteFiles(LPCTSTR lpszFrom, ...)
{
	va_list vaList;
	LPCTSTR lpszArg;
	va_start(vaList,lpszFrom);
	lpszArg = lpszFrom;
	do
	{
		if(lpszArg != NULL)
			::DeleteFile(lpszArg);

	} while((lpszArg = va_arg(vaList,LPCTSTR)) != NULL);
	va_end(vaList);
	return false;
}

bool DeleteFolder(LPCTSTR lpszFrom)
{
	return _rmdir(lpszFrom) == 0;
}

bool DeleteFolders(LPCTSTR lpszFrom, ...)
{
	va_list vaList;
	int nIndex;
	TCHAR szFrom[10240] = {0};
	LPCTSTR lpszArg;
	SHFILEOPSTRUCT shFO;
	if(_access(lpszFrom,0) == 0)
		strcpy(szFrom,lpszFrom);
	nIndex = _tcslen(szFrom) + 1;
	va_start(vaList,lpszFrom);
	memset(&shFO,0,sizeof SHFILEOPSTRUCT);
	shFO.wFunc = FO_DELETE;
	shFO.fFlags = FOF_NOCONFIRMATION/* | FOF_NOCONFIRMMKDIR*/;
	shFO.pFrom = szFrom;
	while((lpszArg = va_arg(vaList,LPCTSTR)) != NULL)
		if((_tcslen(lpszArg) > 0) && (_access(lpszArg,0) == 0))
		{
			_tcscpy(&szFrom[nIndex],lpszArg);
			nIndex += _tcslen(lpszArg) + 1;
		}
	szFrom[nIndex] = 0;
	va_end(vaList);
	if(!_tcslen(szFrom) || SHFileOperation(&shFO) == 0)
		return true;
	return false;
}

CString GetVersionString(LPCTSTR lpszVersion)
{
	int nLength = _tcslen(lpszVersion);
	CString strTemp;
	for(int i = 0; i < nLength; ++i)
		if(isdigit(lpszVersion[i]) || (lpszVersion[i] == _T('.')) || 
			(lpszVersion[i] == _T('a'))|| (lpszVersion[i] == _T('b')))
			strTemp += lpszVersion[i];
		else
			if(lpszVersion[i] == _T(','))
				strTemp += _T('.');
	return strTemp;
}
/*
CString GetVersionString(LPCTSTR lpszVersion)
{
	int nLength = _tcslen(lpszVersion);
	CString strTemp;
	for(int i = 0; i < nLength; ++i)
		if(isdigit(lpszVersion[i]) && ((i + 2) < nLength) &&
			(((lpszVersion[i + 1] == '.') && isdigit(lpszVersion[i + 2])) ||	// 1.2
			((lpszVersion[i + 2] == '.') && isdigit(lpszVersion[i + 3]))))		// 12.3
			return &lpszVersion[i];
		else
			if(isdigit(lpszVersion[i]) && ((i + 3) < nLength) && (lpszVersion[i + 1] == ',') && 	
				(isdigit(lpszVersion[i + 2]) ||	((lpszVersion[i + 2] == ' ') && 
				isdigit(lpszVersion[i + 3]))))
			{
				for(int j = 0; j < nLength; ++j)
					if((lpszVersion[j] == '.') || (lpszVersion[j] == ','))
						strTemp += ".";
					else
						if(lpszVersion[j] != ' ')
							strTemp += lpszVersion[j];
				return strTemp;
			}
	return _T("");
}
*/
CString GetBinVersion(LPCTSTR lpszPath)
{
	DWORD dwVISize,dwHandle;
	unsigned int dwBytes;
	TCHAR pchVersionInfo[10240];
	TCHAR* pszVersionValue;

	CString strKey;
	CString strLanguage[] = {TEXT("0C0904B0"),			// Lang: Aus English Unicode
							 TEXT("040904B0"),			// Lang: US English Unicode 
							 TEXT("040904E4")};			// Lang: US English Multilingual
	CString strVersion[]  = {TEXT("InternalFileVersion"),
							 TEXT("FileVersion"),
							 TEXT("ProductVersion"),
							 TEXT("PrivateBuild"),
							 TEXT("SpecialBuild")};
	
	if((dwVISize = GetFileVersionInfoSize((LPTSTR) lpszPath,&dwHandle)) > 0)
		if(GetFileVersionInfo((LPTSTR) lpszPath,0,10240,pchVersionInfo))
			for(int nLanguage = 0; nLanguage < 3; ++nLanguage)
				for(int nVersion = 0; nVersion < 4; ++nVersion)
				{
					strKey.Format("\\StringFileInfo\\%s\\%s",strLanguage[nLanguage],
						strVersion[nVersion]);
					if(VerQueryValue(pchVersionInfo,(LPTSTR) (LPCTSTR) strKey,
						(void**) &pszVersionValue,&dwBytes))
						return GetVersionString(pszVersionValue);
				}
	return _T("");
}

CString GetNameVersion(LPCTSTR lpszPath)
{
	int nIndex1,nIndex2;
	CString strVersion;
	CString strPath = lpszPath;
	if((nIndex1 = strPath.ReverseFind('v')) != -1)
		if((nIndex2 = strPath.ReverseFind('.')) != -1)
			if(!(strVersion = strPath.Mid(nIndex1 + 1,nIndex2 - nIndex1 - 1)).IsEmpty())
				if(isdigit(strVersion[0]))
					return GetVersionString(strVersion);
	return _T("");
}

CString GetFileVersion(LPCTSTR lpszPath)
{
	CString strTemp;
	if((strTemp = GetBinVersion(lpszPath)).IsEmpty())
		strTemp = GetNameVersion(lpszPath);
	return strTemp;
}

CString GetWinVersion()
{
	OSVERSIONINFOEX osvi;
	BOOL bOsVersionInfoEx;
	ZeroMemory(&osvi, sizeof(OSVERSIONINFOEX));
	osvi.dwOSVersionInfoSize = sizeof(OSVERSIONINFOEX);
	if((bOsVersionInfoEx = GetVersionEx((OSVERSIONINFO*) &osvi)) > 0)
	{
		osvi.dwOSVersionInfoSize = sizeof (OSVERSIONINFO);
		if(!GetVersionEx((OSVERSIONINFO*)&osvi)) 
			return _T("");
	}
	if(osvi.dwMajorVersion == 5 && osvi.dwMinorVersion == 2)
		return _T("5.2"); // Server 2003
	if(osvi.dwMajorVersion == 5 && osvi.dwMinorVersion == 1)
		return _T("5.1"); // Windows XP
	if(osvi.dwMajorVersion == 5 && osvi.dwMinorVersion == 0)
		return _T("5.0"); // Windows 2000
	return _T("");
}

CString GetWinErrorMsg(DWORD dwError)
{
	CString strTemp;
	LPTSTR lpMsgBuf;
	if(FormatMessage(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | 
		FORMAT_MESSAGE_IGNORE_INSERTS,NULL,dwError,MAKELANGID(LANG_NEUTRAL,
		SUBLANG_DEFAULT),(LPTSTR) &lpMsgBuf,0,NULL) > 0)
	{
		strTemp = StripString(lpMsgBuf);
		LocalFree(lpMsgBuf);
	}
	return strTemp;
}
/*
void ProcessWindowMessages(const HWND hWnd)
{       
	MSG Msg;
	while(PeekMessage(&Msg,hWnd,0,0,PM_REMOVE))
	{                            
		TranslateMessage(&Msg);
		DispatchMessage(&Msg);
	}
}

CString i642s(tInt64 i64)
{
	static TCHAR szTemp1[1024];
	TCHAR szTemp2[1024];
	int nTemp;
	if(i64 == 0)
		strcpy(szTemp1,"0");
	else
		*szTemp1 = 0;
	while(i64 != 0)
	{
		nTemp = (int)(i64 % 1000);
		strcpy(szTemp2,szTemp1);		
		if((i64 /= 1000) == 0)
			_stprintf(szTemp1,"%d%s%s",nTemp,_tcslen(szTemp2) ? "," : "",szTemp2);
		else
			_stprintf(szTemp1,"%03d%s%s",nTemp,_tcslen(szTemp2) ? "," : "",szTemp2);
	}
	return szTemp1;
}
*/
LPTSTR StripString(LPTSTR lpszString)
{
	LPTSTR lpsz = lpszString + _tcslen(lpszString);
	while((lpsz > lpszString) && --lpsz)
		if((*lpsz == '\n') || (*lpsz == '\r') || (*lpsz == '\t') || (*lpsz == ' '))
			*lpsz = 0;
		else
			break;
	return lpszString;
}

CString ProperCase(LPCTSTR lpszString)
{
	CString strTemp;
	bool bUpper = true;
	int nLength = _tcslen(lpszString);
	for(int i = 0; i < nLength; ++i)
		if(lpszString[i] != _T('_'))
			if(bUpper)
			{
				strTemp += (TCHAR)toupper(lpszString[i]);
				bUpper = false;
			}
			else
				strTemp += lpszString[i];
		else
			bUpper = true;
	return strTemp;
}

CString SplitLabel(LPCTSTR lpszLabel)
{
	TCHAR szTemp[1024];
	int nIndex = -1,nLength = _tcslen(lpszLabel);
	for(int i = 0; i < nLength; ++i)
	{
		if((nIndex != -1) && isupper(lpszLabel[i]) && 
			(i < (nLength - 1)) && islower(lpszLabel[i + 1]))
			szTemp[++nIndex] = ' ';
		szTemp[++nIndex] = lpszLabel[i];
	}
	szTemp[++nIndex] = 0;
	return szTemp;
}

CString QuoteString(LPCTSTR lpszString)
{
	CString strTemp = CString(_T("\"")) + lpszString + CString(_T("\""));
	return strTemp;
}

CString ReplaceWholeWord(LPCTSTR lpszString, LPCTSTR lpszSource, LPCTSTR lpszTarget, 
	LPCTSTR lpszHRef)
{
	int nIndex;
	CString strTemp = lpszString;
	CString strSource;
	LPTSTR lpszCurrent = (LPTSTR) (LPCTSTR) strTemp;
	while((lpszCurrent = StrStrI(lpszCurrent,lpszTarget)) != NULL)
	{
		nIndex = lpszCurrent - (LPCTSTR) strTemp;
		strSource = lpszSource;
		if(((lpszCurrent == (LPCTSTR) strTemp) || !isalpha(*(lpszCurrent - 1))) && 
			(!isalpha(*(lpszCurrent + _tcslen(lpszTarget))) || 

			(_tcsncmp(lpszCurrent + _tcslen(lpszTarget),_T("s "),2) == 0) ||
			(_tcsncmp(lpszCurrent + _tcslen(lpszTarget),_T("s,"),2) == 0) ||
			(_tcsncmp(lpszCurrent + _tcslen(lpszTarget),_T("s."),2) == 0) ||

			(_tcsncmp(lpszCurrent + _tcslen(lpszTarget),_T("d "),2) == 0) ||
			(_tcsncmp(lpszCurrent + _tcslen(lpszTarget),_T("d,"),2) == 0) ||
			(_tcsncmp(lpszCurrent + _tcslen(lpszTarget),_T("d."),2) == 0) ||

			(_tcsncmp(lpszCurrent + _tcslen(lpszTarget),_T("ed "),3) == 0) ||
			(_tcsncmp(lpszCurrent + _tcslen(lpszTarget),_T("ed,"),3) == 0) ||
			(_tcsncmp(lpszCurrent + _tcslen(lpszTarget),_T("ed."),3) == 0) ||

			(_tcsncmp(lpszCurrent + _tcslen(lpszTarget),_T("ful "),4) == 0) ||
			(_tcsncmp(lpszCurrent + _tcslen(lpszTarget),_T("ful,"),4) == 0) ||
			(_tcsncmp(lpszCurrent + _tcslen(lpszTarget),_T("ful."),4) == 0) ||

			(_tcsncmp(lpszCurrent + _tcslen(lpszTarget),_T("fully "),6) == 0) ||
			(_tcsncmp(lpszCurrent + _tcslen(lpszTarget),_T("fully,"),6) == 0) ||
			(_tcsncmp(lpszCurrent + _tcslen(lpszTarget),_T("fully."),6) == 0) ||

			(_tcsncmp(lpszCurrent + _tcslen(lpszTarget),_T("ous "),4) == 0) ||
			(_tcsncmp(lpszCurrent + _tcslen(lpszTarget),_T("ous,"),4) == 0) ||
			(_tcsncmp(lpszCurrent + _tcslen(lpszTarget),_T("ous."),4) == 0) ||

			(_tcsncmp(lpszCurrent + _tcslen(lpszTarget),_T("ive "),4) == 0) ||
			(_tcsncmp(lpszCurrent + _tcslen(lpszTarget),_T("ive,"),4) == 0) ||
			(_tcsncmp(lpszCurrent + _tcslen(lpszTarget),_T("ive."),4) == 0) ||

			(_tcsncmp(lpszCurrent + _tcslen(lpszTarget),_T("ing "),4) == 0) ||
			(_tcsncmp(lpszCurrent + _tcslen(lpszTarget),_T("ing,"),4) == 0) ||
			(_tcsncmp(lpszCurrent + _tcslen(lpszTarget),_T("ing."),4) == 0) ||

			(_tcsncmp(lpszCurrent + _tcslen(lpszTarget),_T("ness "),5) == 0) ||
			(_tcsncmp(lpszCurrent + _tcslen(lpszTarget),_T("ness,"),5) == 0) ||
			(_tcsncmp(lpszCurrent + _tcslen(lpszTarget),_T("ness."),5) == 0)) && 

			(_tcsncmp(lpszCurrent + _tcslen(lpszTarget),_T("</a>"),4) != 0) &&
			(_tcsncmp(lpszCurrent + _tcslen(lpszTarget),_T("\""),1) != 0))
		{
			if(isupper(lpszCurrent[0]))
				if(strSource.GetLength() > 0)
					strSource.SetAt(0,(TCHAR) toupper(strSource[0]));
				else ;
			else
				strSource.MakeLower();
			if(lpszHRef != NULL)
				strSource = CString(_T("<a href=\"")) + lpszHRef + _T("\">") + strSource + _T("</a>");
			strTemp = strTemp.Left(nIndex) + strSource + strTemp.Right(
				strTemp.GetLength() - nIndex - _tcslen(lpszTarget));
		}
		lpszCurrent = (LPTSTR) (((LPCTSTR) strTemp) + nIndex + strSource.GetLength());
	}
	return strTemp;
}

CString ReplaceString(LPCTSTR lpszString, LPCTSTR lpszSource, LPCTSTR lpszTarget)
{
	int nIndex;
	CString strSource,strTemp = lpszString;
	LPTSTR lpszCurrent = (LPTSTR) (LPCTSTR) strTemp;
	while((lpszCurrent = StrStrI(lpszCurrent,lpszTarget)) != NULL)
	{
		strSource = lpszSource;
		if(!strSource.IsEmpty() && islower(lpszCurrent[0]))
			strSource.SetAt(0,tolower(strSource[0]));
		nIndex = lpszCurrent - (LPCTSTR) strTemp;
		strTemp = strTemp.Left(nIndex) + strSource + strTemp.Right(
			strTemp.GetLength() - nIndex - _tcslen(lpszTarget));
		lpszCurrent = (LPTSTR) (((LPCTSTR) strTemp) + nIndex + _tcslen(lpszSource));
	}
	return strTemp;
}

CString ReplaceStringCase(LPCTSTR lpszString, LPCTSTR lpszSource, LPCTSTR lpszTarget)
{
	int nIndex;
	CString strTemp = lpszString;
	LPTSTR lpszCurrent = (LPTSTR) (LPCTSTR) strTemp;
	while((lpszCurrent = StrStr(lpszCurrent,lpszTarget)) != NULL)
	{
		nIndex = lpszCurrent - (LPCTSTR) strTemp;
		strTemp = strTemp.Left(nIndex) + lpszSource + strTemp.Right(
			strTemp.GetLength() - nIndex - _tcslen(lpszTarget));
		lpszCurrent = (LPTSTR) (((LPCTSTR) strTemp) + nIndex + _tcslen(lpszSource));
	}
	return strTemp;
}

CString f_strCommonStrings[20][2];

bool LoadCommonStrings()
{
//	f_strCommonStrings[ 0][0] = _T("$(Windows)");
//	f_strCommonStrings[ 0][1] = ::GetWindowsHome();

//	f_strCommonStrings[ 1][0] = _T("$(WinSystem)");
//	f_strCommonStrings[ 1][1] = ::GetWinSystemHome();
/*
	f_strCommonStrings[ 2][0] = _T("$(VastHome)");
	f_strCommonStrings[ 2][1] = ::GetVast5Home();
	f_strCommonStrings[ 3][0] = _T("%VAST5_HOME%");
	f_strCommonStrings[ 3][1] = ::GetVast5Home();
	
	f_strCommonStrings[ 4][0] = _T("$(MsVcHome)");
	f_strCommonStrings[ 4][1] = ::GetMsVcHome();
	f_strCommonStrings[ 5][0] = _T("$(MsDevHome)");
	f_strCommonStrings[ 5][1] = ::GetMsDevHome();

	f_strCommonStrings[ 6][0] = _T("$(GccHome)");
	f_strCommonStrings[ 6][1] = ::GetGccHome();
	
	f_strCommonStrings[ 7][0] = _T("$(FinSimHome)");
	f_strCommonStrings[ 7][1] = ::GetFinSimHome();
	f_strCommonStrings[ 8][0] = _T("$(SystemCHome)");
	f_strCommonStrings[ 8][1] = ::GetSystemCHome();
	
	f_strCommonStrings[ 9][0] = _T("$(ArmCcHome)");
	f_strCommonStrings[ 9][1] = ::GetArmCcHome();
	f_strCommonStrings[10][0] = _T("$(ArmElfGccHome)");
	f_strCommonStrings[10][1] = ::GetArmElfGccHome();
	f_strCommonStrings[11][0] = _T("$(MipsElfGccHome)");
	f_strCommonStrings[11][1] = ::GetMipsElfGccHome();
	f_strCommonStrings[12][0] = _T("$(v850ElfGccHome)");
	f_strCommonStrings[12][1] = ::Getv850ElfGccHome();

	f_strCommonStrings[13][0] = _T("");
	f_strCommonStrings[13][1] = _T("");
*/
	return true;
}

CString ReplaceCommonStrings(LPCTSTR lpszString)
{
	int nIndex = -1;
	CString strTemp = lpszString;
	while(!f_strCommonStrings[++nIndex][0].IsEmpty())
		if(strTemp.Find(f_strCommonStrings[nIndex][0]) != -1)
			strTemp = ReplaceString(strTemp,f_strCommonStrings[nIndex][1],
				f_strCommonStrings[nIndex][0]);
	return strTemp;
}

bool IsFabricModule(CStringList* plssProjectSettings, LPCTSTR lpszName, LPCTSTR lpszValue)
{
	CString strTemp;
	POSITION Pos = plssProjectSettings->GetHeadPosition();
	while(Pos)
	{
		strTemp = plssProjectSettings->GetNext(Pos);
		if(_tcsstr(strTemp,lpszName) != NULL)
			if(strTemp == lpszName + CString(_T("=")) + lpszValue)
				return true;
			else
				return false;
	}
	return false;
}

CString GetSimulator(CString& rstrBuildType)
{
	if(rstrBuildType.Find(_T("Nova2")) != -1)
		return _T("Nova2");
	if(rstrBuildType.Find(_T("Nova")) != -1)
		return _T("Nova");
	if(rstrBuildType.Find(_T("FinSim")) != -1)
		return _T("FinSim");
	if(rstrBuildType.Find(_T("SystemC")) != -1)
		return _T("SystemC");
	return _T("");
}
/*
int GetTimeUnits(LPCTSTR lpszUnits)
{
	LPTSTR lpszUnit;
	int nIndex = -1;
	while((lpszUnit = g_pszTimeUnits[++nIndex]) != NULL)
		if(StrStrI(lpszUnits,lpszUnit) != NULL)
			return nIndex;
	return 0;
}
*/
TCHAR* SubString(TCHAR *szTarget, const TCHAR *szSource, int *nOffset, TCHAR cToken, bool bSkipTokens)
{
	register int I;                           
	if(!szSource || !szTarget)  
		return(NULL);
	szTarget[0] = 0;
	if(!szSource[*nOffset])
		return(NULL);                                 
	if(bSkipTokens)
		while(szSource[*nOffset] && (szSource[*nOffset] == cToken))
			++*nOffset;
	for(I = 0; (szSource[I + *nOffset] != cToken) && 
		szSource[I + *nOffset]; ++I)
	szTarget[I] = szSource[I + *nOffset];
	if(!szSource[I + *nOffset])
		*nOffset += I;
	else
		*nOffset += (I + 1);
	szTarget[I] = 0;
	if(I == 0)
		return NULL;
	return szTarget;
}

TCHAR* SubString(TCHAR *szTarget, const TCHAR *szSource, int *nOffset, TCHAR* szTokens, bool bSkipTokens)
{
	register int I;                           
	if(!szSource || !szTarget)  
		return(NULL);
	szTarget[0] = 0;
	if(!szSource[*nOffset])
		return(NULL);                                 
	if(bSkipTokens)
	while(szSource[*nOffset] && (strchr(szTokens,szSource[*nOffset]) != NULL))
		++*nOffset;
	for(I = 0; (strchr(szTokens,szSource[I + *nOffset]) == NULL) && szSource[I + *nOffset]; ++I)
		szTarget[I] = szSource[I + *nOffset];
	if(!szSource[I + *nOffset])
		*nOffset += I;
	else
		*nOffset += (I + 1);
	szTarget[I] = 0;
	if(I == 0)
		return(NULL);
	return(szTarget);
}

CString GetLastField(CString& rstrPath, TCHAR chDelimiter)
{
	int nIndex;
	CString strTemp = rstrPath;
	if((nIndex = rstrPath.ReverseFind(chDelimiter)) != -1)
	{
		strTemp = rstrPath.Right(rstrPath.GetLength() - nIndex - 1);
		rstrPath = rstrPath.Left(nIndex);
	}
	else
		rstrPath.Empty();
	return strTemp;
}
/*
int FindOneOf(LPCTSTR lpszText, TCHAR *szCharSet, int nStartFrom, bool bForward)
{
	int pos = -1;
	if(lpszText == NULL || _tcslen(lpszText) <= 0 || _tcslen(lpszText) < nStartFrom)
		return -1;
	if(szCharSet == NULL || _tcslen(szCharSet) <= 0)
		return -1;
	TCHAR szTemp[1024];
	TCHAR szBuffer[1024];
	memset(szTemp, 0, 1024);
	memset(szBuffer, 0, 1024);
	_tcscpy(szTemp, lpszText);
	if(bForward) 
	{
		TCHAR* tch = NULL;
		_tcscpy(szBuffer, &szTemp[nStartFrom]);
		pos = nStartFrom;
		if((tch = _tcspbrk(szBuffer, szCharSet)) == NULL)
			return -1;
		pos += _tcslen(szBuffer) - _tcslen(tch);
	}
	else 
	{
		_tcsncpy(szBuffer, szTemp, nStartFrom);
		pos = 0;
		int i = 0;
		while(szCharSet[i] != 0) 
		{
			TCHAR* tch = NULL;
			if((tch = _tcsrchr(szBuffer, szCharSet[i])) != NULL) 
			{
				pos = _tcslen(szBuffer);
				pos-= _tcslen(tch);
				break;
			}
			i++;
		}
	}
	return pos;
}
*/
int AppMessage(LPCTSTR lpszText, UINT nType, CWnd* pwndParent)
{
	HWND hWnd = ::GetFocus();
	CWnd* pwnd;
//	if(::IsAutomated())
//	{
//		::GetLogCtrl()->AppendText(lpszText);
//		::GetLogCtrl()->AppendText(_T("\n"));
//		if(((nType & 0xF) == MB_OK) || ((nType & 0xF) == MB_OKCANCEL))
//			return IDOK;
//		else
//			if(((nType & 0xF) == MB_YESNO) || ((nType & 0xF) == MB_YESNOCANCEL))
//				return IDYES;
//			else
//				return 0;
//	}
	if(pwndParent == NULL)
		if((pwnd = ::GetMainFrame()) != NULL)
		{
			if(pwnd->IsIconic())
				pwnd->ShowWindow(SW_RESTORE);
			pwnd->BringWindowToTop();
			pwnd->SetForegroundWindow();
		}
		else
			pwnd = pwndParent;
	else
		pwnd = pwndParent;
	int nRC = ::MessageBox(pwnd->GetSafeHwnd(),lpszText,::GetAppName(),nType);
	::SetFocus(hWnd);
	return nRC;
}
/*
void WriteBatchLog()
{
	CString strTemp1,strTemp2;
	CTime t = CTime::GetCurrentTime();
	strTemp1 = ::ConcatPath(::GetVast5Home(),_T("Log"));
	::CreateDirectory(strTemp1);
	strTemp2.Format(_T("Log%s.txt"),t.Format("%Y%m%d%H%M%S"));
	::GetLogCtrl()->Save(::ConcatPath(strTemp1,strTemp2));
	strTemp2.Format(_T("Build%s.txt"),t.Format("%Y%m%d%H%M%S"));
	::GetBuildCtrl()->Save(::ConcatPath(strTemp1,strTemp2));
	strTemp2.Format(_T("Software%s.txt"),t.Format("%Y%m%d%H%M%S"));
	::GetSoftwareCtrl()->Save(::ConcatPath(strTemp1,strTemp2));
}
*/
void ProcessMessages(const HWND hWnd)
{       
	MSG Msg;
	while(PeekMessage(&Msg,hWnd,0,0,PM_REMOVE))
	{                            
		TranslateMessage(&Msg);
		DispatchMessage(&Msg);
	}
}

CString YouCantDoThat(LPCTSTR lpszWhat, bool bRunning)
{
	CString strTemp;
	if(bRunning)
		strTemp.Format(_T("You cannot %s while building, simulating or debugging."),lpszWhat);
	else
		strTemp.Format(_T("You cannot %s while building."),lpszWhat);
	return strTemp;
}

//////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////

bool CompareFileSpec(LPCTSTR lpszFileSpec, LPCTSTR lpszPath, LPCTSTR lpszFile)
{
	TCHAR szFileSpec1[1024],szFileSpec2[1024],szFileSpec3[1024],szTemp1[1024],szTemp2[1024];
	int I,nLength,nOffset1 = 0,nOffset2 = 0;
	bool bRC = false,bWildcard1 = false,bWildcard2 = false;
	*szFileSpec1 = 0;
	*szFileSpec2 = 0;
	*szFileSpec3 = 0;
	*szTemp1 = 0;
	*szTemp2 = 0;

	while(SubString(szFileSpec3,lpszFileSpec,&nOffset1,'\\',true))
	{
		if(*szFileSpec2)
			strcat(szFileSpec2,"\\");
		strcat(szFileSpec2,szFileSpec1);
		strcpy(szFileSpec1,szFileSpec3);
	}
	
	if(*szFileSpec2)
	{
		CString strTemp = lpszPath;
		strTemp = strTemp.Right(_tcslen(szFileSpec2));
		if(strTemp.Right(_tcslen(szFileSpec2)).CompareNoCase(szFileSpec2))
		return false;
	}

	nOffset1 = 0;
	nOffset2 = 0;

	//  SubString(szFileSpec2,szFileSpec1,&nOffset1,'.',false);
	//  SubString(szTemp1,lpszFile,&nOffset2,'.',false);

	CString strFileSpec1 = szFileSpec1;
	CString strFile = lpszFile;
	if((nOffset1 = strFileSpec1.ReverseFind('.')) != -1)
		strcpy(szFileSpec2,strFileSpec1.Left(nOffset1)); 
	if((nOffset2 = strFile.ReverseFind('.')) != -1)
		strcpy(szTemp1,strFile.Left(nOffset2)); 
	++nOffset1;
	++nOffset2;

	nLength = _tcslen(szFileSpec2);
	for(I = 0; I < nLength; ++I)
		if(szFileSpec2[I] == '*')
		{
			szFileSpec2[I] = 0;
			bWildcard1 = true;
			nLength = I;
			break;
		}
	if(nLength == 0)
		bRC = true;
	else
		if(bWildcard1)
			bRC = !_tcsnicmp(szFileSpec2,szTemp1,nLength);
		else
			bRC = !_tcsicmp(szFileSpec2,szTemp1);
	if(bRC)
	{
		SubString(szFileSpec3,szFileSpec1,&nOffset1,'\n',false);
		SubString(szTemp2,lpszFile,&nOffset2,'\n',false);
		nLength = _tcslen(szFileSpec3);
		for(I = 0; I < nLength; ++I)
			if(szFileSpec3[I] == '*')
			{
				szFileSpec3[I] = 0;
				bWildcard2 = true;
				nLength = I;
				break;
			}
		if(nLength == 0)
			bRC = true;
		else
			if(bWildcard2)
				bRC = !_tcsnicmp(szFileSpec3,szTemp2,nLength);
			else
			bRC = !_tcsicmp(szFileSpec3,szTemp2);
	}
	return bRC;
}

//////////////////////////////////////////////////////////////////////////////////////////////////
// VastDocTemplate class

BOOL VastDocTemplate::GetDocString(CString &rString, enum DocStringIndex i) const
{
	if(::AfxExtractSubString(rString,m_strDocStrings,static_cast<int>(i)))
	{
//		if(i == CDocTemplate::filterExt)
//			rString.Replace(_T(";"),_T(";*")); // Not with Vc7 ?
		return TRUE;
	}
	return FALSE;
}

CDocTemplate::Confidence VastDocTemplate::MatchDocType(LPCTSTR lpszPathName, CDocument *&rpDocMatch)
{
	CString strMultiFilterExt;
	POSITION pos = GetFirstDocPosition();
	rpDocMatch = NULL;
	while(pos != NULL)
	{
		CDocument *pDoc = GetNextDoc(pos);
		if(::AfxComparePath(pDoc->GetPathName(), lpszPathName))
		{
			rpDocMatch = pDoc;
			return yesAlreadyOpen;
		}
	}
	if(GetDocString(strMultiFilterExt, CDocTemplate::filterExt) && !strMultiFilterExt.IsEmpty())
	{
		strMultiFilterExt.Replace(_T(";*"), _T(";"));
		CString strFilterExt;
		for(int i = 0; ::AfxExtractSubString(strFilterExt,strMultiFilterExt,i,_T(';')); i++)
		{
			ASSERT(strFilterExt[0] == _T('.'));
			LPCTSTR lpszDot = ::_tcsrchr(lpszPathName, _T('.'));
			if(lpszDot != NULL && _tcsicmp(lpszDot, strFilterExt) == 0)
				return yesAttemptNative;
		}
	}
	return yesAttemptForeign;
}

//////////////////////////////////////////////////////////////////////////////////////////////////
// CVaSTTool class

CVaSTTool::CVaSTTool(CString strTool)
{
	int nStart = 0,nEnd = strTool.GetLength() + 1;
	m_strName = strTool.Mid(nStart,(nEnd = strTool.Find(',',nStart)) - nStart);
	nStart = nEnd + 1;
	m_strCommand = strTool.Mid(nStart,(nEnd = strTool.Find(',',nStart)) - nStart);
	nStart = nEnd + 1;
	m_strArguments = strTool.Mid(nStart,(nEnd = strTool.Find(',',nStart)) - nStart);
	nStart = nEnd + 1;
	m_strDirectory = strTool.Mid(nStart,(nEnd = strTool.Find(',',nStart)) - nStart);
	nStart = nEnd + 1;
	m_bOutputWindow = strTool.Mid(nStart,(nEnd = strTool.Find(',',nStart)) - nStart) == "1" ? TRUE : FALSE;
	nStart = nEnd + 1;
	m_bPromptForArguments = strTool.Mid(nStart,(nEnd = strTool.Find(',',nStart)) - nStart) == "1" ? TRUE : FALSE;
	nStart = nEnd + 1;
	m_bCloseWindow = strTool.Mid(nStart,(nEnd = strTool.Find(',',nStart)) - nStart) == "1" ? TRUE : FALSE;
}

//////////////////////////////////////////////////////////////////////////////////////////////////
// CProcessPath classes

bool CProcessPath::Go(LPCTSTR lpszPath, LPCTSTR lpszFileSpec, bool bSubDirectories)
{
	WIN32_FIND_DATA FindFileData;
	HANDLE hFindFile = NULL;
	int nFileNo = 0;
	CString sCurrentPath = lpszPath;                    
	++m_nPathNo;

	CStringList lstrFileSpec;
	CString strFileSpec;
	TCHAR szFileSpec[1024];
	int nOffset = 0;

	while(SubString(szFileSpec,lpszFileSpec,&nOffset,',',true))
		lstrFileSpec.AddTail(szFileSpec);

	if(m_bScanFlag && ((hFindFile = FindFirstFile(ConcatPath(sCurrentPath,"*"),&FindFileData)) != INVALID_HANDLE_VALUE))
	{
		do
		{
			if(_tcscmp(FindFileData.cFileName,".") && _tcscmp(FindFileData.cFileName,".."))
				if((FindFileData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) == FILE_ATTRIBUTE_DIRECTORY)
					if(bSubDirectories)
						m_bScanFlag = Go(ConcatPath(sCurrentPath,FindFileData.cFileName),lpszFileSpec,bSubDirectories);
					else ;
				else
				{
					POSITION Pos = lstrFileSpec.GetHeadPosition();
					while(Pos)
					{
						strFileSpec = lstrFileSpec.GetNext(Pos);
						if(CompareFileSpec(strFileSpec,lpszPath,FindFileData.cFileName))
						{
							if(!ProcessFile(sCurrentPath,FindFileData.cFileName,m_nPathNo,++nFileNo))
							{
								FindClose(hFindFile);
								return false;
							}
							break;
						}
					}
				}
		} while(m_bScanFlag && FindNextFile(hFindFile,&FindFileData));
		FindClose(hFindFile);
	}
	return m_bScanFlag;
}

bool CProcessPath::Go2(LPCTSTR lpszPath, LPCTSTR lpszFileSpec)
{
	WIN32_FIND_DATA FindFileData;
	HANDLE hFindFile = NULL;
	int nFileNo = 0;
	CString sCurrentPath = lpszPath;                    
	++m_nPathNo;
	if(m_bScanFlag && ((hFindFile = FindFirstFile(ConcatPath(sCurrentPath,"*"),&FindFileData)) != INVALID_HANDLE_VALUE))
	{
		do
		{
			if(_tcscmp(FindFileData.cFileName,".") && _tcscmp(FindFileData.cFileName,".."))
				if((FindFileData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) == FILE_ATTRIBUTE_DIRECTORY)
					m_bScanFlag = Go2(ConcatPath(sCurrentPath,FindFileData.cFileName),lpszFileSpec);
				else
					if(CompareFileSpec(lpszFileSpec,lpszPath,FindFileData.cFileName))
						if(!ProcessFile(sCurrentPath,FindFileData.cFileName,m_nPathNo,++nFileNo))
						{
							FindClose(hFindFile);
							return false;                         
						}
		} while(m_bScanFlag && FindNextFile(hFindFile,&FindFileData));
		FindClose(hFindFile);
	}
	return m_bScanFlag;
}

bool CLoadRvpms::ProcessFile(LPCTSTR lpszPath, LPCTSTR lpszFile, int nPathNo, int nFileNo)
{
	int nIndex;
	CString strTemp = lpszPath;
	if((nIndex = strTemp.ReverseFind('\\')) != -1)
		strTemp = strTemp.Left(nIndex);
	if((nIndex = strTemp.ReverseFind('\\')) != -1)
		strTemp = strTemp.Right(strTemp.GetLength() - nIndex - 1);
	if(m_plssRvpms->Find(strTemp) == NULL)
		m_plssRvpms->AddTail(strTemp);
	return true;
}

bool CLoadRvpmVersions::ProcessFile(LPCTSTR lpszPath, LPCTSTR lpszFile, int nPathNo, int nFileNo)
{
	CString strTemp;
	if(!(strTemp = GetFileVersion(::ConcatPath(lpszPath,lpszFile))).IsEmpty())
	{
		if(StrStrI(lpszFile,_T("_v")) == NULL)
			strTemp = _T("Default");
		if(m_plssRvpmVersions->Find(strTemp) == NULL)
			m_plssRvpmVersions->AddTail(strTemp);
	}
	return true;
}

bool CLoadFileList::ProcessFile(LPCTSTR lpszPath, LPCTSTR lpszFile, int nPathNo, int nFileNo)
{
	CString strTemp = ::ConcatPath(lpszPath,lpszFile);
	if(m_plssFiles->Find(strTemp) == NULL)
		m_plssFiles->AddTail(strTemp);
	return true;
}

bool CLoadHelpFiles::ProcessFile(LPCTSTR lpszPath, LPCTSTR lpszFile, int nPathNo, int nFileNo)
{
	if(_tcsicmp(lpszFile,_T("CoMET User Guide.chm")) != 0)
		if(_tcsicmp(lpszFile,_T("METeor User Guide.chm")) != 0)
			CLoadFileList::ProcessFile(lpszPath,lpszFile,nPathNo,nFileNo);
	return true;
}

CString FindFile(LPCTSTR lpszPath, LPCTSTR lpszFileSpec, bool bSubFolders)
{
	WIN32_FIND_DATA FindFileData;
	HANDLE hFindFile;
	CString strTemp;
	if((hFindFile = FindFirstFile(::ConcatPath(lpszPath,lpszFileSpec),&FindFileData)) != INVALID_HANDLE_VALUE)
	{
		strTemp = ::ConcatPath(lpszPath,FindFileData.cFileName);
		FindClose(hFindFile);
	}
	return strTemp;
}

/////////////////////////////////////////////////////////////////////////////
// CMessageLog class

CMessageLog::CMessageLog()
{
	m_hEventLog = NULL;
	m_nLogLevel = 0;
	m_nInteractiveLevel = 0;
}

CMessageLog::~CMessageLog()
{
	Close();
}

bool CMessageLog::Open(LPCTSTR lpszName, int nLogLevel, int nInteractiveLevel)
{
	if((m_hEventLog = RegisterEventSource(NULL,lpszName)) != NULL)
	{
		m_nLogLevel = nLogLevel;
		m_nInteractiveLevel = nInteractiveLevel;
		return true;
	}
	return false;
}

void CMessageLog::Close()
{
	if(m_hEventLog != NULL)
		DeregisterEventSource(m_hEventLog);
}

bool CMessageLog::Event(LPCTSTR lpszText, int nLogLevel)
{
	TCHAR szDomainName[1024];
	DWORD dwDomainName = sizeof szDomainName;
	TCHAR szUserName[1024];
	DWORD dwUserName = sizeof szUserName;
	BYTE pchSidBuffer[1024];
	DWORD dwSid = sizeof pchSidBuffer;
	SID_NAME_USE snu;
	PSID pSid = (PSID) pchSidBuffer;
	DWORD dwEventID = 1;
	LPCTSTR lpszMessage[] = {_T("Testing 123"),NULL};
	if(GetUserName(szUserName,&dwUserName))
		if(LookupAccountName(NULL,szUserName,pSid,&dwSid,szDomainName,&dwDomainName,&snu))
			if((dwSid = GetLengthSid(pSid)) > 0)
				if(ReportEvent(m_hEventLog,EVENTLOG_INFORMATION_TYPE,0,dwEventID,
					pSid,1,0,lpszMessage,NULL))
					return true;
	return true;
}

bool CMessageLog::MessageBox(LPCTSTR lpszText, int nLogLevel)
{
	UINT nButtons = 0,nIcon = 0;
	switch(nLogLevel)
	{
	case MSG_ERROR:
		nButtons = MB_OK;
		nIcon = MB_ICONERROR;
		break;
	case MSG_WARNING:
		nButtons = MB_OK;
		nIcon = MB_ICONWARNING;
		break;
	case MSG_INFORMATION:
		nButtons = MB_OK;
		nIcon = MB_ICONINFORMATION;
		break;
	case MSG_QUESTION:
		nButtons = MB_OK;
		nIcon = MB_ICONQUESTION;
		break;
	}
	::AppMessage(lpszText,nButtons | nIcon);
	return true;
}

bool CMessageLog::Message(LPCTSTR lpszText, int nLogLevel, ...)
{
	CString strText;
	va_list vaList;
	va_start(vaList,nLogLevel);
	strText.FormatV(lpszText,vaList);
	va_end(vaList);
	if((nLogLevel & m_nLogLevel) == nLogLevel)
		Event(strText,nLogLevel);
	if((nLogLevel & m_nInteractiveLevel) == m_nInteractiveLevel)
		MessageBox(strText,nLogLevel);
	return true;
}

void TestMessageLog()
{
	CMessageLog ML;
	if(ML.Open(_T("VaSTGui"),MSG_INFORMATION | MSG_WARNING | MSG_ERROR,MSG_QUESTION | MSG_ERROR))
	{
		ML.Message(_T("Testing %s %d %x."),MSG_ERROR,_T("VaSTGui Message Log"),1234,0xBabeFace);
		ML.Close();
	}
}
/*
bool EnumurateNetDevices(CStringList& lssDrvs)
{
	DWORD dwResult;
	HANDLE hEnum;
	DWORD cbBuffer = 16384;      // 16K
	DWORD cEntries = 0;
	LPNETRESOURCE lpnrLocal;
	DWORD i = 0;
	TCHAR tchDr = 'E';
	CString strTemp;
	while(tchDr <= _T('Z'))
	{
		strTemp = tchDr;
		strTemp += _T(':');
		lssDrvs.AddTail(strTemp);
		tchDr += 1;
	}
	dwResult = WNetOpenEnum(RESOURCE_CONNECTED,	RESOURCETYPE_DISK, 0, NULL,	&hEnum);
	if(dwResult != NO_ERROR)
		return FALSE;
	lpnrLocal = (LPNETRESOURCE) GlobalAlloc(GPTR, cbBuffer);
	if(lpnrLocal == NULL) 
		return FALSE;
	do
	{  
		ZeroMemory(lpnrLocal, cbBuffer);
		dwResult = WNetEnumResource(hEnum, &cEntries, lpnrLocal, &cbBuffer);
		if(dwResult == NO_ERROR)
		{
			for(i = 0; i < cEntries; i++)
				if((lpnrLocal[i].lpLocalName != NULL) && (_tcslen(lpnrLocal[i].lpLocalName) > 0))
				{
					POSITION pos = lssDrvs.Find(lpnrLocal[i].lpLocalName, 0);
					if(pos != NULL)
						lssDrvs.RemoveAt(pos);
				}
		}
		else 
			if(dwResult != ERROR_NO_MORE_ITEMS)
				break;
	} while(dwResult != ERROR_NO_MORE_ITEMS);
	GlobalFree((HGLOBAL)lpnrLocal);
	WNetCloseEnum(hEnum);
	return TRUE;
}
*/
///////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////

#define MAX_VERSIONS 4	// Major, Minor, Maintenace, alpha / beta / etc

bool VersionMatch(LPCTSTR lpszActualVersion, LPCTSTR lpszRequiredVersion, 
	LPTSTR lpszBestVersion)
{
	TCHAR szActualVersion[256],szRequiredVersion[256],szThisVersion[256] = {0};
	TCHAR szActualVersions[MAX_VERSIONS][256],szRequiredVersions[MAX_VERSIONS][256];
	TCHAR szSeperators[] = _T(".,;:-_/\\ abcdefghijklmnopqrstuvwxyz"); // Any reasonable seperator
	LPTSTR lpszToken;
	int nIndex;
	bool bRC = false,bExactMatch = (_tcslen(lpszRequiredVersion) > 0) && 
		(_tcschr(lpszRequiredVersion,_T('*')) == NULL);

	for(int i = 0; i < MAX_VERSIONS; ++i)
	{
		_tcscpy(szActualVersions[i],_T("0000"));
		if(bExactMatch || (i + 1 == MAX_VERSIONS))
			_tcscpy(szRequiredVersions[i],_T("0000")); // Beta versions don't beat non-beta versions
		else
			_tcscpy(szRequiredVersions[i],_T("*"));
	}

	_tcscpy(szActualVersion,lpszActualVersion);
	_tcscpy(szRequiredVersion,lpszRequiredVersion);
	_tcslwr(szActualVersion);
	_tcslwr(szRequiredVersion);

	if((lpszToken = _tcstok(szActualVersion,szSeperators)) != NULL)
		_stprintf(szActualVersions[0],_T("%04d"),atoi(lpszToken));
	nIndex = 0;
	while((lpszToken != NULL) && (nIndex < MAX_VERSIONS))
		if((lpszToken = _tcstok(NULL,szSeperators)) != NULL)
			_stprintf(szActualVersions[++nIndex],_T("%04d"),atoi(lpszToken));

	if((lpszToken = _tcstok(szRequiredVersion,szSeperators)) != NULL)
		if(_tcscmp(lpszToken,_T("*")) == 0)
			_tcscpy(szRequiredVersions[0],lpszToken);
		else
			_stprintf(szRequiredVersions[0],_T("%04d"),atoi(lpszToken));
	nIndex = 0;
	while((lpszToken != NULL) && (nIndex < MAX_VERSIONS))
		if((lpszToken = _tcstok(NULL,szSeperators)) != NULL)
			if(_tcscmp(lpszToken,_T("*")) == 0)
				_tcscpy(szRequiredVersions[++nIndex],lpszToken);
			else
				_stprintf(szRequiredVersions[++nIndex],_T("%04d"),atoi(lpszToken));

	*szRequiredVersion = 0;
	nIndex = -1;
	while(++nIndex < MAX_VERSIONS)
	{
		if(bExactMatch)
		{
			if(_tcslen(szRequiredVersion) > 0)
				_tcscat(szRequiredVersion,_T("."));
			_tcscat(szRequiredVersion,szRequiredVersions[nIndex]);
		}
		if(_tcslen(szThisVersion) > 0)
			_tcscat(szThisVersion,_T("."));
		if((_tcscmp(szRequiredVersions[nIndex],_T("*")) == 0) ||
			(_tcscmp(szRequiredVersions[nIndex],szActualVersions[nIndex]) == 0))
			_tcscat(szThisVersion,szActualVersions[nIndex]);
		else
			_tcscat(szThisVersion,_T("    ")); // Less than any number for wildcard match
	}

	if(bExactMatch)
		if(_tcscmp(szThisVersion,szRequiredVersion) == 0)
			bRC = _tcscpy(lpszBestVersion,szThisVersion) != NULL;
		else ;
	else
		if(_tcscmp(szThisVersion,lpszBestVersion) >= 0)
			bRC = _tcscpy(lpszBestVersion,szThisVersion) != NULL;

//	if(::GetApp()->IsVerboseMode())
//	{
//		CString strTemp;
//		strTemp.Format(_T("<%-10s> <%-10s> <%s.%s.%s.%s> <%s.%s.%s.%s> <%s> <%s>\n"),
//			lpszActualVersion,lpszRequiredVersion,szActualVersions[0],szActualVersions[1],
//			szActualVersions[2],szActualVersions[3],szRequiredVersions[0],szRequiredVersions[1],
//			szRequiredVersions[2],szRequiredVersions[3],szThisVersion,lpszBestVersion);
//		::GetLogCtrl()->AppendText(strTemp);
//	}

	return bRC;
}

CString ExtractArrayDimension(LPCTSTR lpszName)
{
	int nIndex;
	CString	strTemp = lpszName;
	if((nIndex = strTemp.Find('[',0)) != -1)
		return strTemp.Mid(nIndex + 1,strTemp.GetLength() - nIndex - 2);
	return _T("");
}

CString ExtractBaseName(LPCTSTR lpszName)
{
	int nIndex;
	CString	strTemp = lpszName;
	if((nIndex = strTemp.Find('[',0)) != -1)
	{
		strTemp.Delete(nIndex,strTemp.GetLength() - nIndex);
		return strTemp;
	}
	return lpszName;
}

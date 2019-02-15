#pragma once

#ifndef _SECURE_ATL
#define _SECURE_ATL 1
#endif

#pragma warning(disable:4267) 
#pragma warning(disable:4786) 
#pragma warning(disable:4800) 

#define WINVER 0x0501

#ifndef VC_EXTRALEAN
#define VC_EXTRALEAN
#endif

//#define _ATL_CSTRING_EXPLICIT_CONSTRUCTORS
//#define _AFX_ALL_WARNINGS

#include <AfxWin.h>                 // MFC core components
//#include <AfxExt.h>                 // MFC extensions
#include <AfxDb.h>                  // MFC ODBC database classes
//#include <AfxOle.h>                 // MFC OLE classes
//#include <AfxDisp.h>                // MFC OLE automation classes
//#include <AfxCview.h>               // MFC CView classes 
//#include <AfxCmn.h>			        // MFC Windows 95 common controls
//#include <AfxDtctl.h>				// MFC support for Internet Explorer 4 Common Controls
#include <AfxHtml.h>				// MFC HTML view support
//#include <AfxMt.h>                  // MFC Multi-Threading  

#include <..\Src\Mfc\AfxImpl.h>

#include <io.h>
#include <direct.h>
#include <shlwapi.h>				// Windows headers

#include <OdbcInst.h>
#include <AtlBase.h>

extern CComModule _Module;

#include <AtlCom.h>
#include <AtlCtl.h>
#include <AfxOledb.h>
//#include <OleDberr.h>

//#include <ComCat.h>

//#include <afxsock.h>

#ifdef _UNICODE
#if defined _M_IX86
#pragma comment(linker,"/manifestdependency:\"type='win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='x86' publicKeyToken='6595b64144ccf1df' language='*'\"")
#elif defined _M_IA64
#pragma comment(linker,"/manifestdependency:\"type='win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='ia64' publicKeyToken='6595b64144ccf1df' language='*'\"")
#elif defined _M_X64
#pragma comment(linker,"/manifestdependency:\"type='win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='amd64' publicKeyToken='6595b64144ccf1df' language='*'\"")
#else
#pragma comment(linker,"/manifestdependency:\"type='win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='*' publicKeyToken='6595b64144ccf1df' language='*'\"")
#endif
#endif

#include <XTToolkitPro.h>

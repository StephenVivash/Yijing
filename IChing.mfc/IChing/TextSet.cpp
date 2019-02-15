#include "StdAfx.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

CTextColumn::CTextColumn()
{
	memset( (void*)this, 0, sizeof(*this) );
};

CTextColumn::~CTextColumn()
{
}

CTextColumnSet::CTextColumnSet() 
{
}

CTextColumnSet::~CTextColumnSet() 
{
}

bool CTextColumnSet::OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL)
{
	HRESULT hr;
	m_poleBB = poleBB;
	CDBPropSet propset(DBPROPSET_ROWSET);
	propset.AddProperty(DBPROP_CANFETCHBACKWARDS, true);
	propset.AddProperty(DBPROP_IRowsetScroll, true);
	propset.AddProperty(DBPROP_IRowsetChange, true);
	propset.AddProperty(DBPROP_UPDATABILITY, DBPROPVAL_UP_CHANGE | 
		DBPROPVAL_UP_INSERT | DBPROPVAL_UP_DELETE );
	if(SUCCEEDED(hr = CCommand<CAccessor<CTextColumn> >::Open(m_poleBB->GetSession(),
		lpszSQL,&propset)))
		if(SUCCEEDED(hr = MoveNext()))
			return true;
	::AppMessage(_T("CTextColumnSet::OpenRowset failed\n\n") + GetWinErrorMsg(hr) + _T("\n\n") + lpszSQL,MB_OK | MB_ICONWARNING);
	return false;
}

bool CTextColumnSet::Execute(COleBeliefBase* poleBB, LPCTSTR lpszSQL, ...)
{
	HRESULT hr;
	va_list argList;
	CString strTemp;
	bool bRC = false;
	m_poleBB = poleBB;
	va_start(argList, lpszSQL);
	strTemp.FormatV(lpszSQL,argList);
	if(SUCCEEDED(hr = Open(m_poleBB->GetSession(),strTemp,NULL,NULL,DBGUID_DEFAULT,false)))
	{
		Close();
		ReleaseCommand();
		bRC = true;
	}
	else
		::AppMessage(_T("CTextColumnSet::Execute failed\n\n") + GetWinErrorMsg(hr) + _T("\n\n") + strTemp,MB_OK | MB_ICONWARNING);
	va_end(argList);
	return bRC;
}

////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////

CTextRow6::CTextRow6()
{
	memset( (void*)this, 0, sizeof(*this) );
};

CTextRow6::~CTextRow6()
{
}

CTextSet6::CTextSet6() 
{
}

CTextSet6::~CTextSet6() 
{
}

bool CTextSet6::OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL)
{
	HRESULT hr;
	m_poleBB = poleBB;
	CDBPropSet propset(DBPROPSET_ROWSET);
	propset.AddProperty(DBPROP_CANFETCHBACKWARDS, true);
	propset.AddProperty(DBPROP_IRowsetScroll, true);
	propset.AddProperty(DBPROP_IRowsetChange, true);
	propset.AddProperty(DBPROP_UPDATABILITY, DBPROPVAL_UP_CHANGE | 
		DBPROPVAL_UP_INSERT | DBPROPVAL_UP_DELETE );
	if(SUCCEEDED(hr = CCommand<CAccessor<CTextRow6> >::Open(m_poleBB->GetSession(),
		lpszSQL,&propset)))
		if(SUCCEEDED(hr = MoveNext()))
			return true;
	::AppMessage(_T("CTextSet6::OpenRowset failed\n\n") + GetWinErrorMsg(hr) + _T("\n\n") + lpszSQL,MB_OK | MB_ICONWARNING);
	return false;
}
/*
bool CTextSet6::Execute(COleBeliefBase* poleBB, LPCTSTR lpszSQL, ...)
{
	HRESULT hr;
	va_list argList;
	CString strTemp;
	bool bRC = false;
	m_poleBB = poleBB;
	va_start(argList, lpszSQL);
	strTemp.FormatV(lpszSQL,argList);
	if(SUCCEEDED(hr = Open(m_poleBB->GetSession(),strTemp,NULL,NULL,DBGUID_DEFAULT,false)))
	{
		Close();
		ReleaseCommand();
		bRC = true;
	}
	else
		::AppMessage(_T("CTextSet6::Execute failed\n\n") + GetWinErrorMsg(hr) + _T("\n\n") + strTemp,MB_OK | MB_ICONWARNING);
	va_end(argList);
	return bRC;
}
*/

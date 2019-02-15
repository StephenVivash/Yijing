#include "StdAfx.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

CLabelEntry::CLabelEntry(int nValue, LPCTSTR lpszLabel) 
{
	m_nValue = nValue;
	m_strLabel = lpszLabel;
}

CLabelBlock::CLabelBlock(int nSequence, int nSize, LPCTSTR lpszName, TCHAR lpszLabels[64][100])
{
	m_nSequence = nSequence;
	m_nSize = nSize;
	m_strName = lpszName;
	m_rgLabelEntry.SetSize(nSize);
	for(int i = 0; i < nSize; ++i)
		m_rgLabelEntry.SetAt(i,new CLabelEntry(i,lpszLabels[i]));
}

bool CLabelBlockArray::SaveBelief(COleBeliefBase* poleDB, LPCTSTR lpszTable)
{
	int i,j;
	CLabelSet6 LabelSet6;
	CLabelBlock* pLB;
	CLabelEntryArray* prgLE = NULL;
	CLabelEntry* pLE;
	CString strTemp1,strTemp2;
	for(i = 0; i <= GetUpperBound(); ++i)
	{
		pLB = GetAt(i);
		prgLE = pLB->GetEntryArray();
		if(prgLE->GetModified())
		{
			strTemp2.Format(_T("update %s set "),lpszTable);
			for(j = 0; j <= prgLE->GetUpperBound(); ++j)
			{
				pLE = prgLE->GetAt(j);
				strTemp1.Format(_T("V%d = '%s'"),j,j,pLE->GetLabel());
				strTemp2 += (j > 0 ? _T(", ") : _T("")) + strTemp1;
			}
			strTemp1.Format(_T("%s where Sequence = %d"),strTemp2,pLB->GetSequence());
			LabelSet6.Execute(poleDB,strTemp1);
		}
	}
	LabelSet6.Execute(poleDB,_T("update ") + CString(lpszTable) + _T(" set Active = 0"));
//	LabelSet6.Execute(poleDB,_T("update ") + CString(lpszTable) + _T(" set Active = 1 where Sequence = %d"),nActive);
	return true;
}

//////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////

bool CLabelBlockArray1::LoadBelief(COleBeliefBase* poleDB)
{
	int nRow = 0;
	CLabelSet1 LabelSet1;
	RemoveAll();
	SetSize(100,2);
	if(LabelSet1.OpenRowset(poleDB,_T("select * from Label1 order by Sequence")))
	{
		do
		{
			SetAt(nRow++,new CLabelBlock(LabelSet1.m_nSequence,2,LabelSet1.m_szName,LabelSet1.m_szLabels));
		} while(LabelSet1.MoveNext() == S_OK);
		return true;
	}
	return false;
}

bool CLabelBlockArray1::SaveBelief(COleBeliefBase* poleDB)
{
	return CLabelBlockArray::SaveBelief(poleDB,_T("Label1"));
}

//////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////

bool CLabelBlockArray2::LoadBelief(COleBeliefBase* poleDB)
{
	int nRow = 0;
	CLabelSet2 LabelSet2;
	RemoveAll();
	SetSize(50,2);
	if(LabelSet2.OpenRowset(poleDB,_T("select * from Label2 order by Sequence")))
	{
		do
		{
			SetAt(nRow++,new CLabelBlock(LabelSet2.m_nSequence,4,LabelSet2.m_szName,LabelSet2.m_szLabels));
		} while(LabelSet2.MoveNext() == S_OK);
		return true;
	}
	return false;
}

bool CLabelBlockArray2::SaveBelief(COleBeliefBase* poleDB)
{
	return CLabelBlockArray::SaveBelief(poleDB,_T("Label2"));
}

bool CLabelBlockArray3::LoadBelief(COleBeliefBase* poleDB)
{
	int nRow = 0;
	CLabelSet3 LabelSet3;
	RemoveAll();
	SetSize(20,2);
	if(LabelSet3.OpenRowset(poleDB,_T("select * from Label3 order by Sequence")))
	{
		do
		{
			SetAt(nRow++,new CLabelBlock(LabelSet3.m_nSequence,8,LabelSet3.m_szName,LabelSet3.m_szLabels));
		} while(LabelSet3.MoveNext() == S_OK);
		return true;
	}
	return false;
}

bool CLabelBlockArray3::SaveBelief(COleBeliefBase* poleDB)
{
	return CLabelBlockArray::SaveBelief(poleDB,_T("Label3"));
}

bool CLabelBlockArray6::LoadBelief(COleBeliefBase* poleDB)
{
	int nRow = 0;
	CLabelSet6 LabelSet6;
	RemoveAll();
	SetSize(30,2);
	if(LabelSet6.OpenRowset(poleDB,_T("select * from Label6 order by Sequence")))
	{
		do
		{
			SetAt(nRow++,new CLabelBlock(LabelSet6.m_nSequence,64,LabelSet6.m_szName,LabelSet6.m_szLabels));
		} while(LabelSet6.MoveNext() == S_OK);
		return true;
	}
	return false;
}

bool CLabelBlockArray6::SaveBelief(COleBeliefBase* poleDB)
{
	return CLabelBlockArray::SaveBelief(poleDB,_T("Label6"));
}

CLabelRow1::CLabelRow1()
{
	memset((void*) this,0,sizeof(*this));
};

CLabelRow1::~CLabelRow1()
{
}

CLabelSet::CLabelSet() 
{
}

CLabelSet::~CLabelSet() 
{
}

CLabelSet1::CLabelSet1() 
{
}

CLabelSet1::~CLabelSet1() 
{
}

bool CLabelSet1::OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL)
{
	HRESULT hr;
	m_poleBB = poleBB;
	CDBPropSet propset(DBPROPSET_ROWSET);
	propset.AddProperty(DBPROP_CANFETCHBACKWARDS, true);
	propset.AddProperty(DBPROP_IRowsetScroll, true);
	propset.AddProperty(DBPROP_IRowsetChange, true);
	propset.AddProperty(DBPROP_UPDATABILITY, DBPROPVAL_UP_CHANGE | DBPROPVAL_UP_INSERT | DBPROPVAL_UP_DELETE);
	if(SUCCEEDED(hr = CCommand<CAccessor<CLabelRow1> >::Open(m_poleBB->GetSession(),lpszSQL,&propset)))
		if(SUCCEEDED(hr = MoveNext()))
			return true;
	::AppMessage(_T("CLabelSet1::OpenRowset failed\n\n") + GetWinErrorMsg(hr) + _T("\n\n") + lpszSQL,MB_OK | MB_ICONWARNING);
	return false;
}
/*
bool CLabelSet1::Execute(COleBeliefBase* poleBB, LPCTSTR lpszSQL, ...)
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
		::AppMessage(_T("CLabelSet1::Execute failed\n\n") + GetWinErrorMsg(hr) + _T("\n\n") + strTemp,MB_OK | MB_ICONWARNING);
	va_end(argList);
	return bRC;
}
*/
CLabelRow2::CLabelRow2()
{
	memset((void*) this,0,sizeof(*this));
};

CLabelRow2::~CLabelRow2()
{
}

CLabelSet2::CLabelSet2() 
{
}

CLabelSet2::~CLabelSet2() 
{
}

bool CLabelSet2::OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL)
{
	HRESULT hr;
	m_poleBB = poleBB;
	CDBPropSet propset(DBPROPSET_ROWSET);
	propset.AddProperty(DBPROP_CANFETCHBACKWARDS, true);
	propset.AddProperty(DBPROP_IRowsetScroll, true);
	propset.AddProperty(DBPROP_IRowsetChange, true);
	propset.AddProperty(DBPROP_UPDATABILITY, DBPROPVAL_UP_CHANGE | DBPROPVAL_UP_INSERT | DBPROPVAL_UP_DELETE);
	if(SUCCEEDED(hr = CCommand<CAccessor<CLabelRow2> >::Open(m_poleBB->GetSession(),lpszSQL,&propset)))
		if(SUCCEEDED(hr = MoveNext()))
			return true;
	::AppMessage(_T("CLabelSet2::OpenRowset failed\n\n") + GetWinErrorMsg(hr) + _T("\n\n") + lpszSQL,MB_OK | MB_ICONWARNING);
	return false;
}
/*
bool CLabelSet2::Execute(COleBeliefBase* poleBB, LPCTSTR lpszSQL, ...)
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
		::AppMessage(_T("CLabelSet2::Execute failed\n\n") + GetWinErrorMsg(hr) + _T("\n\n") + strTemp,MB_OK | MB_ICONWARNING);
	va_end(argList);
	return bRC;
}
*/
CLabelRow3::CLabelRow3()
{
	memset((void*) this,0,sizeof(*this));
};

CLabelRow3::~CLabelRow3()
{
}

CLabelSet3::CLabelSet3() 
{
}

CLabelSet3::~CLabelSet3() 
{
}

bool CLabelSet3::OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL)
{
	HRESULT hr;
	m_poleBB = poleBB;
	CDBPropSet propset(DBPROPSET_ROWSET);
	propset.AddProperty(DBPROP_CANFETCHBACKWARDS, true);
	propset.AddProperty(DBPROP_IRowsetScroll, true);
	propset.AddProperty(DBPROP_IRowsetChange, true);
	propset.AddProperty(DBPROP_UPDATABILITY, DBPROPVAL_UP_CHANGE | DBPROPVAL_UP_INSERT | DBPROPVAL_UP_DELETE);
	if(SUCCEEDED(hr = CCommand<CAccessor<CLabelRow3> >::Open(m_poleBB->GetSession(),lpszSQL,&propset)))
		if(SUCCEEDED(hr = MoveNext()))
			return true;
	::AppMessage(_T("CLabelSet3::OpenRowset failed\n\n") + GetWinErrorMsg(hr) + _T("\n\n") + lpszSQL,MB_OK | MB_ICONWARNING);
	return false;
}
/*
bool CLabelSet3::Execute(COleBeliefBase* poleBB, LPCTSTR lpszSQL, ...)
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
		::AppMessage(_T("CLabelSet3::Execute failed\n\n") + GetWinErrorMsg(hr) + _T("\n\n") + strTemp,MB_OK | MB_ICONWARNING);
	va_end(argList);
	return bRC;
}
*/
CLabelRow6::CLabelRow6()
{
	memset((void*) this,0,sizeof(*this));
};

CLabelRow6::~CLabelRow6()
{
}

CLabelSet6::CLabelSet6() 
{
}

CLabelSet6::~CLabelSet6() 
{
}

bool CLabelSet6::OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL)
{
	HRESULT hr;
	m_poleBB = poleBB;
	CDBPropSet propset(DBPROPSET_ROWSET);
	propset.AddProperty(DBPROP_CANFETCHBACKWARDS, true);
	propset.AddProperty(DBPROP_IRowsetScroll, true);
	propset.AddProperty(DBPROP_IRowsetChange, true);
	propset.AddProperty(DBPROP_UPDATABILITY, DBPROPVAL_UP_CHANGE | DBPROPVAL_UP_INSERT | DBPROPVAL_UP_DELETE);
	if(SUCCEEDED(hr = CCommand<CAccessor<CLabelRow6> >::Open(m_poleBB->GetSession(),lpszSQL,&propset)))
		if(SUCCEEDED(hr = MoveNext()))
			return true;
	::AppMessage(_T("CLabelSet6::OpenRowset failed\n\n") + GetWinErrorMsg(hr) + _T("\n\n") + lpszSQL,MB_OK | MB_ICONWARNING);
	return false;
}

bool CLabelSet6::Execute(COleBeliefBase* poleBB, LPCTSTR lpszSQL, ...)
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
		::AppMessage(_T("CLabelSet6::Execute failed\n\n") + GetWinErrorMsg(hr) + _T("\n\n") + strTemp,MB_OK | MB_ICONWARNING);
	va_end(argList);
	return bRC;
}


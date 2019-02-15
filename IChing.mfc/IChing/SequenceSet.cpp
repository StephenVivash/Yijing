#include "StdAfx.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

CSequenceEntry::CSequenceEntry(int nValue, int nSequence) 
{
	m_nValue = nValue;
	m_nSequence = nSequence;
}

int CSequenceEntryArray::FindValue(int nSequence)
{
	for(int i = 0; i <= m_nUpperBound; ++i)
		if(GetAt(i)->GetSequence() == nSequence)
			return i;
	return -1;
}

int CSequenceEntryArray::GetFirstSequence()
{
	int nLowerBound = 0;
	while(true)
	{
		for(int i = nLowerBound; i <= m_nUpperBound; ++i)
			if(GetAt(i)->GetSequence() == nLowerBound)
				return i;
		++nLowerBound;
	}
	return -1;
}

int CSequenceEntryArray::GetPreviousSequence(int nSequence)
{
	while(true)
	{
		if(nSequence > 0)
			--nSequence;
		else
			nSequence = m_nUpperBound;
		for(int i = 0; i <= m_nUpperBound; ++i)
			if(GetAt(i)->GetSequence() == nSequence)
				return i;
	}
	return -1;
}

int CSequenceEntryArray::GetNextSequence(int nSequence)
{
	while(true)
	{
		if(nSequence < m_nUpperBound)
			++nSequence;
		else
			nSequence = 0;
		for(int i = 0; i <= m_nUpperBound; ++i)
			if(GetAt(i)->GetSequence() == nSequence)
				return i;
	}
	return -1;
}

int CSequenceEntryArray::GetLastSequence()
{
	int nUpperBound = m_nUpperBound;
	while(true)
	{
		for(int i = 0; i <= nUpperBound; ++i)
			if(GetAt(i)->GetSequence() == nUpperBound)
				return i;
		--nUpperBound;
	}
	return -1;
}

CSequenceBlock::CSequenceBlock(int nSequence, int nSize, LPCTSTR lpszName, int nSequences[64])
{
	m_nSequence = nSequence;
	m_nSize = nSize;
	m_strName = lpszName;
	m_rgSequenceEntry.SetSize(nSize);
	for(int i = 0; i < nSize; ++i)
		m_rgSequenceEntry.SetAt(i,new CSequenceEntry(i,nSequences[i]));
}

bool CSequenceBlockArray::SaveBelief(COleBeliefBase* poleDB, LPCTSTR lpszTable)
{
	int i,j;
	CSequenceSet6 SequenceSet6;
	CSequenceBlock* pLS;
	CSequenceEntryArray* prgLE = NULL;
	CSequenceEntry* pLE;
	CString strTemp1,strTemp2;
	for(i = 0; i <= GetUpperBound(); ++i)
	{
		pLS = GetAt(i);
		prgLE = pLS->GetEntryArray();
		if(prgLE->GetModified())
		{
			strTemp2.Format(_T("update %s set "),lpszTable);
			for(j = 0; j <= prgLE->GetUpperBound(); ++j)
			{
				pLE = prgLE->GetAt(j);
				strTemp1.Format(_T("S%d = %d"),j,pLE->GetSequence(),j);
				strTemp2 += (j > 0 ? _T(", ") : _T("")) + strTemp1;
			}
			strTemp1.Format(_T("%s where Sequence = %d"),strTemp2,pLS->GetSequence());
			SequenceSet6.Execute(poleDB,strTemp1);
		}
	}
	SequenceSet6.Execute(poleDB,_T("update ") + CString(lpszTable) + _T(" set Active = 0"));
//	SequenceSet6.Execute(poleDB,_T("update ") + CString(lpszTable) + _T(" set Active = 1 where Sequence = %d"),nActive);
	return true;
}

//////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////

bool CSequenceBlockArray1::LoadBelief(COleBeliefBase* poleDB)
{
	int nRow = 0;
	CSequenceSet1 SequenceSet1;
	RemoveAll();
	SetSize(100,2);
	if(SequenceSet1.OpenRowset(poleDB,_T("select * from Sequence1 order by Sequence")))
	{
		do
		{
			SetAt(nRow++,new CSequenceBlock(SequenceSet1.m_nSequence,2,SequenceSet1.m_szName,SequenceSet1.m_nSequences));
		} while(SequenceSet1.MoveNext() == S_OK);
		return true;
	}
	return false;
}

bool CSequenceBlockArray1::SaveBelief(COleBeliefBase* poleDB)
{
	return CSequenceBlockArray::SaveBelief(poleDB,_T("Sequence1"));
}

//////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////

bool CSequenceBlockArray2::LoadBelief(COleBeliefBase* poleDB)
{
	int nRow = 0;
	CSequenceSet2 SequenceSet2;
	RemoveAll();
	SetSize(50,2);
	if(SequenceSet2.OpenRowset(poleDB,_T("select * from Sequence2 order by Sequence")))
	{
		do
		{
			SetAt(nRow++,new CSequenceBlock(SequenceSet2.m_nSequence,4,SequenceSet2.m_szName,SequenceSet2.m_nSequences));
		} while(SequenceSet2.MoveNext() == S_OK);
		return true;
	}
	return false;
}

bool CSequenceBlockArray2::SaveBelief(COleBeliefBase* poleDB)
{
	return CSequenceBlockArray::SaveBelief(poleDB,_T("Sequence2"));
}

bool CSequenceBlockArray3::LoadBelief(COleBeliefBase* poleDB)
{
	int nRow = 0;
	CSequenceSet3 SequenceSet3;
	RemoveAll();
	SetSize(20,2);
	if(SequenceSet3.OpenRowset(poleDB,_T("select * from Sequence3 order by Sequence")))
	{
		do
		{
			SetAt(nRow++,new CSequenceBlock(SequenceSet3.m_nSequence,8,SequenceSet3.m_szName,SequenceSet3.m_nSequences));
		} while(SequenceSet3.MoveNext() == S_OK);
		return true;
	}
	return false;
}

bool CSequenceBlockArray3::SaveBelief(COleBeliefBase* poleDB)
{
	return CSequenceBlockArray::SaveBelief(poleDB,_T("Sequence3"));
}

bool CSequenceBlockArray6::LoadBelief(COleBeliefBase* poleDB)
{
	int nRow = 0;
	CSequenceSet6 SequenceSet6;
	RemoveAll();
	SetSize(30,2);
	if(SequenceSet6.OpenRowset(poleDB,_T("select * from Sequence6 order by Sequence")))
	{
		do
		{
			SetAt(nRow++,new CSequenceBlock(SequenceSet6.m_nSequence,64,SequenceSet6.m_szName,SequenceSet6.m_nSequences));
		} while(SequenceSet6.MoveNext() == S_OK);
		return true;
	}
	return false;
}

bool CSequenceBlockArray6::SaveBelief(COleBeliefBase* poleDB)
{
	return CSequenceBlockArray::SaveBelief(poleDB,_T("Sequence6"));
}

CSequenceRow1::CSequenceRow1()
{
	memset((void*) this,0,sizeof(*this));
};

CSequenceRow1::~CSequenceRow1()
{
}

CSequenceSet::CSequenceSet() 
{
}

CSequenceSet::~CSequenceSet() 
{
}

CSequenceSet1::CSequenceSet1() 
{
}

CSequenceSet1::~CSequenceSet1() 
{
}

bool CSequenceSet1::OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL)
{
	HRESULT hr;
	m_poleBB = poleBB;
	CDBPropSet propset(DBPROPSET_ROWSET);
	propset.AddProperty(DBPROP_CANFETCHBACKWARDS, true);
	propset.AddProperty(DBPROP_IRowsetScroll, true);
	propset.AddProperty(DBPROP_IRowsetChange, true);
	propset.AddProperty(DBPROP_UPDATABILITY, DBPROPVAL_UP_CHANGE | DBPROPVAL_UP_INSERT | DBPROPVAL_UP_DELETE);
	if(SUCCEEDED(hr = CCommand<CAccessor<CSequenceRow1> >::Open(m_poleBB->GetSession(),lpszSQL,&propset)))
		if(SUCCEEDED(hr = MoveNext()))
			return true;
	::AppMessage(_T("CSequenceSet1::OpenRowset failed\n\n") + GetWinErrorMsg(hr) + _T("\n\n") + lpszSQL,MB_OK | MB_ICONWARNING);
	return false;
}
/*
bool CSequenceSet1::Execute(COleBeliefBase* poleBB, LPCTSTR lpszSQL, ...)
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
		::AppMessage(_T("CSequenceSet1::Execute failed\n\n") + GetWinErrorMsg(hr) + _T("\n\n") + strTemp,MB_OK | MB_ICONWARNING);
	va_end(argList);
	return bRC;
}
*/
CSequenceRow2::CSequenceRow2()
{
	memset((void*) this,0,sizeof(*this));
};

CSequenceRow2::~CSequenceRow2()
{
}

CSequenceSet2::CSequenceSet2() 
{
}

CSequenceSet2::~CSequenceSet2() 
{
}

bool CSequenceSet2::OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL)
{
	HRESULT hr;
	m_poleBB = poleBB;
	CDBPropSet propset(DBPROPSET_ROWSET);
	propset.AddProperty(DBPROP_CANFETCHBACKWARDS, true);
	propset.AddProperty(DBPROP_IRowsetScroll, true);
	propset.AddProperty(DBPROP_IRowsetChange, true);
	propset.AddProperty(DBPROP_UPDATABILITY, DBPROPVAL_UP_CHANGE | DBPROPVAL_UP_INSERT | DBPROPVAL_UP_DELETE);
	if(SUCCEEDED(hr = CCommand<CAccessor<CSequenceRow2> >::Open(m_poleBB->GetSession(),lpszSQL,&propset)))
		if(SUCCEEDED(hr = MoveNext()))
			return true;
	::AppMessage(_T("CSequenceSet2::OpenRowset failed\n\n") + GetWinErrorMsg(hr) + _T("\n\n") + lpszSQL,MB_OK | MB_ICONWARNING);
	return false;
}
/*
bool CSequenceSet2::Execute(COleBeliefBase* poleBB, LPCTSTR lpszSQL, ...)
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
		::AppMessage(_T("CSequenceSet2::Execute failed\n\n") + GetWinErrorMsg(hr) + _T("\n\n") + strTemp,MB_OK | MB_ICONWARNING);
	va_end(argList);
	return bRC;
}
*/
CSequenceRow3::CSequenceRow3()
{
	memset((void*) this,0,sizeof(*this));
};

CSequenceRow3::~CSequenceRow3()
{
}

CSequenceSet3::CSequenceSet3() 
{
}

CSequenceSet3::~CSequenceSet3() 
{
}

bool CSequenceSet3::OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL)
{
	HRESULT hr;
	m_poleBB = poleBB;
	CDBPropSet propset(DBPROPSET_ROWSET);
	propset.AddProperty(DBPROP_CANFETCHBACKWARDS, true);
	propset.AddProperty(DBPROP_IRowsetScroll, true);
	propset.AddProperty(DBPROP_IRowsetChange, true);
	propset.AddProperty(DBPROP_UPDATABILITY, DBPROPVAL_UP_CHANGE | DBPROPVAL_UP_INSERT | DBPROPVAL_UP_DELETE);
	if(SUCCEEDED(hr = CCommand<CAccessor<CSequenceRow3> >::Open(m_poleBB->GetSession(),lpszSQL,&propset)))
		if(SUCCEEDED(hr = MoveNext()))
			return true;
	::AppMessage(_T("CSequenceSet3::OpenRowset failed\n\n") + GetWinErrorMsg(hr) + _T("\n\n") + lpszSQL,MB_OK | MB_ICONWARNING);
	return false;
}
/*
bool CSequenceSet3::Execute(COleBeliefBase* poleBB, LPCTSTR lpszSQL, ...)
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
		::AppMessage(_T("CSequenceSet3::Execute failed\n\n") + GetWinErrorMsg(hr) + _T("\n\n") + strTemp,MB_OK | MB_ICONWARNING);
	va_end(argList);
	return bRC;
}
*/
CSequenceRow6::CSequenceRow6()
{
	memset((void*) this,0,sizeof(*this));
};

CSequenceRow6::~CSequenceRow6()
{
}

CSequenceSet6::CSequenceSet6() 
{
}

CSequenceSet6::~CSequenceSet6() 
{
}

bool CSequenceSet6::OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL)
{
	HRESULT hr;
	m_poleBB = poleBB;
	CDBPropSet propset(DBPROPSET_ROWSET);
	propset.AddProperty(DBPROP_CANFETCHBACKWARDS, true);
	propset.AddProperty(DBPROP_IRowsetScroll, true);
	propset.AddProperty(DBPROP_IRowsetChange, true);
	propset.AddProperty(DBPROP_UPDATABILITY, DBPROPVAL_UP_CHANGE | DBPROPVAL_UP_INSERT | DBPROPVAL_UP_DELETE);
	if(SUCCEEDED(hr = CCommand<CAccessor<CSequenceRow6> >::Open(m_poleBB->GetSession(),lpszSQL,&propset)))
		if(SUCCEEDED(hr = MoveNext()))
			return true;
	::AppMessage(_T("CSequenceSet6::OpenRowset failed\n\n") + GetWinErrorMsg(hr) + _T("\n\n") + lpszSQL,MB_OK | MB_ICONWARNING);
	return false;
}

bool CSequenceSet6::Execute(COleBeliefBase* poleBB, LPCTSTR lpszSQL, ...)
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
		::AppMessage(_T("CSequenceSet6::Execute failed\n\n") + GetWinErrorMsg(hr) + _T("\n\n") + strTemp,MB_OK | MB_ICONWARNING);
	va_end(argList);
	return bRC;
}


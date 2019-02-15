#include "StdAfx.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

CConsultationRow::CConsultationRow()
{
	memset((void*) this,0,sizeof(*this));
};

CConsultationRow::~CConsultationRow()
{
}

CConsultationSet::CConsultationSet() 
{
}

CConsultationSet::~CConsultationSet() 
{
}

bool CConsultationSet::OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL)
{
	HRESULT hr;
	m_poleBB = poleBB;
	CDBPropSet propset(DBPROPSET_ROWSET);
	propset.AddProperty(DBPROP_CANFETCHBACKWARDS, true);
	propset.AddProperty(DBPROP_IRowsetScroll, true);
	propset.AddProperty(DBPROP_IRowsetChange, true);
	propset.AddProperty(DBPROP_UPDATABILITY, DBPROPVAL_UP_CHANGE | DBPROPVAL_UP_INSERT | DBPROPVAL_UP_DELETE);
	if(SUCCEEDED(hr = CCommand<CAccessor<CConsultationRow> >::Open(m_poleBB->GetSession(),lpszSQL,&propset)))
		if(SUCCEEDED(hr = MoveNext()))
			return true;
	::AppMessage(_T("CConsultationSet::OpenRowset failed\n\n") + GetWinErrorMsg(hr) + _T("\n\n") + lpszSQL,MB_OK | MB_ICONWARNING);
	return false;
}

bool CConsultationList::LoadBelief(COleBeliefBase* poleDB)
{
	CConsultationSet cs;
	if(cs.OpenRowset(poleDB,_T("select * from Consultation order by Id")))
	{
		do
		{
			AddTail(new CConsultation(cs.m_nId,cs.m_nParentId,cs.m_szTitle,cs.m_szType,cs.m_szLabel,cs.m_szSource,cs.m_szQuestion,cs.m_szNotes,cs.m_szResult));
		} while(cs.MoveNext() == S_OK);
		return true;
	}
	return false;
}

CConsultation::CConsultation(int nId, int nParentId, LPCTSTR lpszTitle, LPCTSTR lpszType, LPCTSTR lpszLabel, LPCTSTR lpszSource, 
	LPCTSTR lpszQuestion, LPCTSTR lpszNotes, LPCTSTR lpszResult)
{
	m_nId = nId;
	m_nParentId = nParentId;
	m_strTitle = lpszTitle;
	m_strType = lpszType;
	m_strLabel = lpszLabel;
	m_strSource = lpszSource;
	m_strQuestion = lpszQuestion;
	m_strNotes = lpszNotes;
	m_strResult = lpszResult;
}

CConsultation::~CConsultation()
{
}

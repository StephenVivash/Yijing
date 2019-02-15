#if !defined(AFX_CONSULTATIONSET_H__CA985D70_ECE2_489F_8F2E_C79568FE91FC__INCLUDED_)
#define AFX_CONSULTATIONSET_H__CA985D70_ECE2_489F_8F2E_C79568FE91FC__INCLUDED_

#pragma once

class CConsultationRow
{
public:
	CConsultationRow();
	virtual ~CConsultationRow();

	int m_nId;
	int m_nParentId;
	TCHAR m_szTitle[100];
	TCHAR m_szType[100];
	TCHAR m_szLabel[100];
	TCHAR m_szSource[100];
	TCHAR m_szQuestion[10240];
	TCHAR m_szNotes[10240];
	TCHAR m_szResult[100];

	BEGIN_COLUMN_MAP(CConsultationRow)
		COLUMN_ENTRY_TYPE( 1, DBTYPE_I4,	m_nId)
		COLUMN_ENTRY_TYPE( 2, DBTYPE_I4,	m_nParentId)
		COLUMN_ENTRY_TYPE( 3, DBTYPE_STR,	m_szTitle)
		COLUMN_ENTRY_TYPE( 4, DBTYPE_STR,	m_szType)
		COLUMN_ENTRY_TYPE( 5, DBTYPE_STR,	m_szLabel)
		COLUMN_ENTRY_TYPE( 6, DBTYPE_STR,	m_szSource)
		COLUMN_ENTRY_TYPE( 7, DBTYPE_STR,	m_szQuestion)
		COLUMN_ENTRY_TYPE( 8, DBTYPE_STR,	m_szNotes)
		COLUMN_ENTRY_TYPE( 9, DBTYPE_STR,	m_szResult)
	END_COLUMN_MAP()
};

class CConsultationSet : public CCommand<CAccessor<CConsultationRow> >
{
public:
	CConsultationSet();
	virtual ~CConsultationSet();
	bool OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL);

protected:
	COleBeliefBase* m_poleBB;
};

class CConsultation : public CObject
{
public:
	CConsultation(int nId, int nParentId, LPCTSTR lpszTitle, LPCTSTR lpszType, LPCTSTR lpszLabel, LPCTSTR lpszSource, 
		LPCTSTR lpszQuestion, LPCTSTR lpszNotes, LPCTSTR lpszResult);
	virtual ~CConsultation();

	int m_nId;
	int m_nParentId;
	CString m_strTitle;
	CString m_strType;
	CString m_strLabel;
	CString m_strSource;
	CString m_strQuestion;
	CString m_strNotes;
	CString m_strResult;
};

class CConsultationList : public CPtrListEx<CConsultation>
{
public:
	virtual bool LoadBelief(COleBeliefBase* oleBB);
//	virtual bool SaveBelief(COleBeliefBase* oleBB);
};

#endif

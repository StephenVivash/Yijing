#if !defined(AFX_SEQUENCESET_H__CA985D70_ECE2_489F_8F2E_C79568FE91FC__INCLUDED_)
#define AFX_SEQUENCESET_H__CA985D70_ECE2_489F_8F2E_C79568FE91FC__INCLUDED_

#pragma once

class CSequenceEntry : public CObject
{
public:
	CSequenceEntry(int nValue, int nSequence);
	virtual ~CSequenceEntry() {}
	void SetValue(int nValue) {m_nValue = nValue;}
	int GetValue() {return m_nValue;}
	void SetSequence(int nSequence) {m_nSequence = nSequence;}
	int GetSequence() {return m_nSequence;}

protected:
	int m_nValue;
	int m_nSequence;
};

class CSequenceEntryArray : public CObArray
{
public:
	CSequenceEntryArray() {m_nUpperBound = -1; m_bModified = false;}
	virtual ~CSequenceEntryArray() {RemoveAll();}
	void RemoveAll() {if(m_nUpperBound == -1) return; for(int i = 0; i <= m_nUpperBound; ++i) delete GetAt(i); CObArray::RemoveAll(); m_nUpperBound = -1;}
	void SetAt(int nIndex, CSequenceEntry* newElement) {CObArray::SetAt(nIndex,newElement); m_nUpperBound = nIndex > m_nUpperBound ? nIndex : m_nUpperBound;}
	CSequenceEntry* GetAt(int nIndex) {return (CSequenceEntry*) CObArray::GetAt(nIndex);}
	int GetUpperBound() const {return m_nUpperBound;}
	void SetModified(bool bModified = true) {m_bModified = bModified;}
	bool GetModified() {return m_bModified;}

	int FindValue(int nSequence);
	int GetFirstSequence();
	int GetPreviousSequence(int nSequence);
	int GetNextSequence(int nSequence);
	int GetLastSequence();

protected:
	int m_nUpperBound;
	bool m_bModified;
};

class CSequenceBlock : public CObject
{
public:
	CSequenceBlock(int nSequence, int nSize, LPCTSTR lpszName, int nSequences[64]);
	virtual ~CSequenceBlock() {}
	CSequenceEntryArray* GetEntryArray() {return &m_rgSequenceEntry;}
	void SetSequence(int nSequence) {m_nSequence = nSequence;}
	int GetSequence() {return m_nSequence;}
	void SetName(LPCTSTR lpszName) {m_strName = lpszName;}
	LPCTSTR GetName(void) const {return(m_strName);}

protected:
	int m_nSequence;
	int m_nSize;
	CString m_strName;
	CSequenceEntryArray m_rgSequenceEntry;
};

class CSequenceBlockArray : public CObArray
{
public:
	CSequenceBlockArray() {m_nUpperBound = -1; m_bModified = false;}
	virtual ~CSequenceBlockArray() {RemoveAll();}

	bool SaveBelief(COleBeliefBase* poleDB, LPCTSTR lpszTable);

	virtual bool LoadBelief(COleBeliefBase*) {return false;}
	virtual bool SaveBelief(COleBeliefBase*) {return false;}

	void RemoveAll() {if(m_nUpperBound == -1) return; for(int i = 0; i <= m_nUpperBound; ++i) delete GetAt(i); CObArray::RemoveAll(); m_nUpperBound = -1;}
	void SetAt(int nIndex, CSequenceBlock* newElement) {CObArray::SetAt(nIndex,newElement); m_nUpperBound = nIndex > m_nUpperBound ? nIndex : m_nUpperBound;}
	CSequenceBlock* GetAt(int nIndex) {return (CSequenceBlock*) CObArray::GetAt(nIndex);}
	int GetUpperBound() const {return m_nUpperBound;}
	void SetModified(bool bModified = true) {m_bModified = bModified;}
	bool GetModified() {return m_bModified;}

protected:
	int m_nUpperBound;
	bool m_bModified;
};

class CSequenceBlockArray1 : public CSequenceBlockArray
{
public:
	virtual bool LoadBelief(COleBeliefBase* oleBB);
	virtual bool SaveBelief(COleBeliefBase* oleBB);
};

class CSequenceBlockArray2 : public CSequenceBlockArray
{
public:
	virtual bool LoadBelief(COleBeliefBase* oleBB);
	virtual bool SaveBelief(COleBeliefBase* oleBB);
};

class CSequenceBlockArray3 : public CSequenceBlockArray
{
public:
	virtual bool LoadBelief(COleBeliefBase* oleBB);
	virtual bool SaveBelief(COleBeliefBase* oleBB);
};

class CSequenceBlockArray6 : public CSequenceBlockArray
{
public:
	virtual bool LoadBelief(COleBeliefBase* oleBB);
	virtual bool SaveBelief(COleBeliefBase* oleBB);
};

//////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////

class CSequenceRow1
{
public:
	CSequenceRow1();
	virtual ~CSequenceRow1();

	int m_nSequence;
	TCHAR m_szName[100];
	int m_nSequences[2];

	BEGIN_COLUMN_MAP(CSequenceRow1)
		COLUMN_ENTRY_TYPE( 1, DBTYPE_I4,	m_nSequence)
		COLUMN_ENTRY_TYPE( 2, DBTYPE_STR,	m_szName)
		COLUMN_ENTRY_TYPE( 3, DBTYPE_I4,	m_nSequences[ 0])
		COLUMN_ENTRY_TYPE( 4, DBTYPE_I4,	m_nSequences[ 1])
	END_COLUMN_MAP()
};

class CSequenceSet
{
public:
	CSequenceSet();
	virtual ~CSequenceSet();

protected:
	COleBeliefBase* m_poleBB;
};

class CSequenceSet1 : public CSequenceSet, public CCommand<CAccessor<CSequenceRow1> >
{
public:
	CSequenceSet1();
	virtual ~CSequenceSet1();
	bool OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL);
//	bool Execute(COleBeliefBase* poleBB, LPCTSTR lpszSQL, ...);
};

class CSequenceRow2
{
public:
	CSequenceRow2();
	virtual ~CSequenceRow2();

	int m_nSequence;
	TCHAR m_szName[100];
	int m_nSequences[4];

	BEGIN_COLUMN_MAP(CSequenceRow2)
		COLUMN_ENTRY_TYPE( 1, DBTYPE_I4,	m_nSequence)
		COLUMN_ENTRY_TYPE( 2, DBTYPE_STR,	m_szName)
		COLUMN_ENTRY_TYPE( 3, DBTYPE_I4,	m_nSequences[ 0])
		COLUMN_ENTRY_TYPE( 4, DBTYPE_I4,	m_nSequences[ 1])
		COLUMN_ENTRY_TYPE( 5, DBTYPE_I4,	m_nSequences[ 2])
		COLUMN_ENTRY_TYPE( 6, DBTYPE_I4,	m_nSequences[ 3])
	END_COLUMN_MAP()
};

class CSequenceSet2 : public CSequenceSet, public CCommand<CAccessor<CSequenceRow2> >
{
public:
	CSequenceSet2();
	virtual ~CSequenceSet2();
	bool OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL);
//	bool Execute(COleBeliefBase* poleBB, LPCTSTR lpszSQL, ...);

//protected:
//	COleBeliefBase* m_poleBB;
};

class CSequenceRow3
{
public:
	CSequenceRow3();
	virtual ~CSequenceRow3();

	int m_nSequence;
	TCHAR m_szName[100];
	int m_nSequences[8];

	BEGIN_COLUMN_MAP(CSequenceRow3)
		COLUMN_ENTRY_TYPE( 1, DBTYPE_I4,	m_nSequence)
		COLUMN_ENTRY_TYPE( 2, DBTYPE_STR,	m_szName)
		COLUMN_ENTRY_TYPE( 3, DBTYPE_I4,	m_nSequences[ 0])
		COLUMN_ENTRY_TYPE( 4, DBTYPE_I4,	m_nSequences[ 1])
		COLUMN_ENTRY_TYPE( 5, DBTYPE_I4,	m_nSequences[ 2])
		COLUMN_ENTRY_TYPE( 6, DBTYPE_I4,	m_nSequences[ 3])
		COLUMN_ENTRY_TYPE( 7, DBTYPE_I4,	m_nSequences[ 4])
		COLUMN_ENTRY_TYPE( 8, DBTYPE_I4,	m_nSequences[ 5])
		COLUMN_ENTRY_TYPE( 9, DBTYPE_I4,	m_nSequences[ 6])
		COLUMN_ENTRY_TYPE(10, DBTYPE_I4,	m_nSequences[ 7])
	END_COLUMN_MAP()
};

class CSequenceSet3 : public CSequenceSet, public CCommand<CAccessor<CSequenceRow3> >
{
public:
	CSequenceSet3();
	virtual ~CSequenceSet3();
	bool OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL);
//	bool Execute(COleBeliefBase* poleBB, LPCTSTR lpszSQL, ...);

//protected:
//	COleBeliefBase* m_poleBB;
};

class CSequenceRow6
{
public:
	CSequenceRow6();
	virtual ~CSequenceRow6();

	int m_nSequence;
	TCHAR m_szName[100];
	int m_nSequences[64];

	BEGIN_COLUMN_MAP(CSequenceRow6)
		COLUMN_ENTRY_TYPE( 1, DBTYPE_I4,	m_nSequence)
		COLUMN_ENTRY_TYPE( 2, DBTYPE_STR,	m_szName)
		COLUMN_ENTRY_TYPE( 3, DBTYPE_I4,	m_nSequences[ 0])
		COLUMN_ENTRY_TYPE( 4, DBTYPE_I4,	m_nSequences[ 1])
		COLUMN_ENTRY_TYPE( 5, DBTYPE_I4,	m_nSequences[ 2])
		COLUMN_ENTRY_TYPE( 6, DBTYPE_I4,	m_nSequences[ 3])
		COLUMN_ENTRY_TYPE( 7, DBTYPE_I4,	m_nSequences[ 4])
		COLUMN_ENTRY_TYPE( 8, DBTYPE_I4,	m_nSequences[ 5])
		COLUMN_ENTRY_TYPE( 9, DBTYPE_I4,	m_nSequences[ 6])
		COLUMN_ENTRY_TYPE(10, DBTYPE_I4,	m_nSequences[ 7])
		COLUMN_ENTRY_TYPE(11, DBTYPE_I4,	m_nSequences[ 8])
		COLUMN_ENTRY_TYPE(12, DBTYPE_I4,	m_nSequences[ 9])
		COLUMN_ENTRY_TYPE(13, DBTYPE_I4,	m_nSequences[10])
		COLUMN_ENTRY_TYPE(14, DBTYPE_I4,	m_nSequences[11])
		COLUMN_ENTRY_TYPE(15, DBTYPE_I4,	m_nSequences[12])
		COLUMN_ENTRY_TYPE(16, DBTYPE_I4,	m_nSequences[13])
		COLUMN_ENTRY_TYPE(17, DBTYPE_I4,	m_nSequences[14])
		COLUMN_ENTRY_TYPE(18, DBTYPE_I4,	m_nSequences[15])
		COLUMN_ENTRY_TYPE(19, DBTYPE_I4,	m_nSequences[16])
		COLUMN_ENTRY_TYPE(20, DBTYPE_I4,	m_nSequences[17])
		COLUMN_ENTRY_TYPE(21, DBTYPE_I4,	m_nSequences[18])
		COLUMN_ENTRY_TYPE(22, DBTYPE_I4,	m_nSequences[19])
		COLUMN_ENTRY_TYPE(23, DBTYPE_I4,	m_nSequences[20])
		COLUMN_ENTRY_TYPE(24, DBTYPE_I4,	m_nSequences[21])
		COLUMN_ENTRY_TYPE(25, DBTYPE_I4,	m_nSequences[22])
		COLUMN_ENTRY_TYPE(26, DBTYPE_I4,	m_nSequences[23])
		COLUMN_ENTRY_TYPE(27, DBTYPE_I4,	m_nSequences[24])
		COLUMN_ENTRY_TYPE(28, DBTYPE_I4,	m_nSequences[25])
		COLUMN_ENTRY_TYPE(29, DBTYPE_I4,	m_nSequences[26])
		COLUMN_ENTRY_TYPE(30, DBTYPE_I4,	m_nSequences[27])
		COLUMN_ENTRY_TYPE(31, DBTYPE_I4,	m_nSequences[28])
		COLUMN_ENTRY_TYPE(32, DBTYPE_I4,	m_nSequences[29])
		COLUMN_ENTRY_TYPE(33, DBTYPE_I4,	m_nSequences[30])
		COLUMN_ENTRY_TYPE(34, DBTYPE_I4,	m_nSequences[31])
		COLUMN_ENTRY_TYPE(35, DBTYPE_I4,	m_nSequences[32])
		COLUMN_ENTRY_TYPE(36, DBTYPE_I4,	m_nSequences[33])
		COLUMN_ENTRY_TYPE(37, DBTYPE_I4,	m_nSequences[34])
		COLUMN_ENTRY_TYPE(38, DBTYPE_I4,	m_nSequences[35])
		COLUMN_ENTRY_TYPE(39, DBTYPE_I4,	m_nSequences[36])
		COLUMN_ENTRY_TYPE(40, DBTYPE_I4,	m_nSequences[37])
		COLUMN_ENTRY_TYPE(41, DBTYPE_I4,	m_nSequences[38])
		COLUMN_ENTRY_TYPE(42, DBTYPE_I4,	m_nSequences[39])
		COLUMN_ENTRY_TYPE(43, DBTYPE_I4,	m_nSequences[40])
		COLUMN_ENTRY_TYPE(44, DBTYPE_I4,	m_nSequences[41])
		COLUMN_ENTRY_TYPE(45, DBTYPE_I4,	m_nSequences[42])
		COLUMN_ENTRY_TYPE(46, DBTYPE_I4,	m_nSequences[43])
		COLUMN_ENTRY_TYPE(47, DBTYPE_I4,	m_nSequences[44])
		COLUMN_ENTRY_TYPE(48, DBTYPE_I4,	m_nSequences[45])
		COLUMN_ENTRY_TYPE(49, DBTYPE_I4,	m_nSequences[46])
		COLUMN_ENTRY_TYPE(50, DBTYPE_I4,	m_nSequences[47])
		COLUMN_ENTRY_TYPE(51, DBTYPE_I4,	m_nSequences[48])
		COLUMN_ENTRY_TYPE(52, DBTYPE_I4,	m_nSequences[49])
		COLUMN_ENTRY_TYPE(53, DBTYPE_I4,	m_nSequences[50])
		COLUMN_ENTRY_TYPE(54, DBTYPE_I4,	m_nSequences[51])
		COLUMN_ENTRY_TYPE(55, DBTYPE_I4,	m_nSequences[52])
		COLUMN_ENTRY_TYPE(56, DBTYPE_I4,	m_nSequences[53])
		COLUMN_ENTRY_TYPE(57, DBTYPE_I4,	m_nSequences[54])
		COLUMN_ENTRY_TYPE(58, DBTYPE_I4,	m_nSequences[55])
		COLUMN_ENTRY_TYPE(59, DBTYPE_I4,	m_nSequences[56])
		COLUMN_ENTRY_TYPE(60, DBTYPE_I4,	m_nSequences[57])
		COLUMN_ENTRY_TYPE(61, DBTYPE_I4,	m_nSequences[58])
		COLUMN_ENTRY_TYPE(62, DBTYPE_I4,	m_nSequences[59])
		COLUMN_ENTRY_TYPE(63, DBTYPE_I4,	m_nSequences[60])
		COLUMN_ENTRY_TYPE(64, DBTYPE_I4,	m_nSequences[61])
		COLUMN_ENTRY_TYPE(65, DBTYPE_I4,	m_nSequences[62])
		COLUMN_ENTRY_TYPE(66, DBTYPE_I4,	m_nSequences[63])
	END_COLUMN_MAP()
};

class CSequenceSet6 : public CSequenceSet, public CCommand<CAccessor<CSequenceRow6> >
{
public:
	CSequenceSet6();
	virtual ~CSequenceSet6();
	bool OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL);
	bool Execute(COleBeliefBase* poleBB, LPCTSTR lpszSQL, ...);

//protected:
//	COleBeliefBase* m_poleBB;
};

//{{AFX_INSERT_LOCATION}}

#endif

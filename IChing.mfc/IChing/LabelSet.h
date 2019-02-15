#if !defined(AFX_LABELSET_H__CA985D70_ECE2_489F_8F2E_C79568FE91FC__INCLUDED_)
#define AFX_LABELSET_H__CA985D70_ECE2_489F_8F2E_C79568FE91FC__INCLUDED_

#pragma once

class CLabelEntry : public CObject
{
public:
	CLabelEntry(int nValue, LPCTSTR lpszLabel);
	virtual ~CLabelEntry() {}
	void SetValue(int nValue) {m_nValue = nValue;}
	int GetValue() {return m_nValue;}
	void SetLabel(LPCTSTR lpszLabel) {m_strLabel = lpszLabel;}
	LPCTSTR GetLabel(void) const {return(m_strLabel);}

protected:
	int m_nValue;
	CString m_strLabel;
};

class CLabelEntryArray : public CObArray
{
public:
	CLabelEntryArray() {m_nUpperBound = -1; m_bModified = false;}
	virtual ~CLabelEntryArray() {RemoveAll();}
	void RemoveAll() {if(m_nUpperBound == -1) return; for(int i = 0; i <= m_nUpperBound; ++i) delete GetAt(i); CObArray::RemoveAll(); m_nUpperBound = -1;}
	void SetAt(int nIndex, CLabelEntry* newElement) {CObArray::SetAt(nIndex,newElement); m_nUpperBound = nIndex > m_nUpperBound ? nIndex : m_nUpperBound;}
	CLabelEntry* GetAt(int nIndex) {return (CLabelEntry*) CObArray::GetAt(nIndex);}
	int GetUpperBound() const {return m_nUpperBound;}
	void SetModified(bool bModified = true) {m_bModified = bModified;}
	bool GetModified() {return m_bModified;}

protected:
	int m_nUpperBound;
	bool m_bModified;
};

class CLabelBlock : public CObject
{
public:
	CLabelBlock(int nSequence, int nSize, LPCTSTR lpszName, TCHAR lpszLabels[64][100]);
	virtual ~CLabelBlock() {}
	CLabelEntryArray* GetEntryArray() {return &m_rgLabelEntry;}
	void SetSequence(int nSequence) {m_nSequence = nSequence;}
	int GetSequence() {return m_nSequence;}
	void SetName(LPCTSTR lpszName) {m_strName = lpszName;}
	LPCTSTR GetName(void) const {return(m_strName);}

protected:
	int m_nSequence;
	int m_nSize;
	CString m_strName;
	CLabelEntryArray m_rgLabelEntry;
};

class CLabelBlockArray : public CObArray
{
public:
	CLabelBlockArray() {m_nUpperBound = -1; m_bModified = false;}
	virtual ~CLabelBlockArray() {RemoveAll();}

	bool SaveBelief(COleBeliefBase* poleDB, LPCTSTR lpszTable);

	virtual bool LoadBelief(COleBeliefBase*) {return false;}
	virtual bool SaveBelief(COleBeliefBase*) {return false;}

	void RemoveAll() {if(m_nUpperBound == -1) return; for(int i = 0; i <= m_nUpperBound; ++i) delete GetAt(i); CObArray::RemoveAll(); m_nUpperBound = -1;}
	void SetAt(int nIndex, CLabelBlock* newElement) {CObArray::SetAt(nIndex,newElement); m_nUpperBound = nIndex > m_nUpperBound ? nIndex : m_nUpperBound;}
	CLabelBlock* GetAt(int nIndex) {return (CLabelBlock*) CObArray::GetAt(nIndex);}
	int GetUpperBound() const {return m_nUpperBound;}
	void SetModified(bool bModified = true) {m_bModified = bModified;}
	bool GetModified() {return m_bModified;}

protected:
	int m_nUpperBound;
	bool m_bModified;
};

class CLabelBlockArray1 : public CLabelBlockArray
{
public:
	virtual bool LoadBelief(COleBeliefBase* oleBB);
	virtual bool SaveBelief(COleBeliefBase* oleBB);
};

class CLabelBlockArray2 : public CLabelBlockArray
{
public:
	virtual bool LoadBelief(COleBeliefBase* oleBB);
	virtual bool SaveBelief(COleBeliefBase* oleBB);
};

class CLabelBlockArray3 : public CLabelBlockArray
{
public:
	virtual bool LoadBelief(COleBeliefBase* oleBB);
	virtual bool SaveBelief(COleBeliefBase* oleBB);
};

class CLabelBlockArray6 : public CLabelBlockArray
{
public:
	virtual bool LoadBelief(COleBeliefBase* oleBB);
	virtual bool SaveBelief(COleBeliefBase* oleBB);
};

//////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////

class CLabelRow1
{
public:
	CLabelRow1();
	virtual ~CLabelRow1();

	int m_nSequence;
	TCHAR m_szName[100];
	TCHAR m_szLabels[2][100];

	BEGIN_COLUMN_MAP(CLabelRow1)
		COLUMN_ENTRY_TYPE( 1, DBTYPE_I4,	m_nSequence)
		COLUMN_ENTRY_TYPE( 2, DBTYPE_STR,	m_szName)
		COLUMN_ENTRY_TYPE( 3, DBTYPE_STR,	m_szLabels  [ 0])
		COLUMN_ENTRY_TYPE( 4, DBTYPE_STR,	m_szLabels  [ 1])
	END_COLUMN_MAP()
};

class CLabelSet
{
public:
	CLabelSet();
	virtual ~CLabelSet();

protected:
	COleBeliefBase* m_poleBB;
};

class CLabelSet1 : public CLabelSet, public CCommand<CAccessor<CLabelRow1> >
{
public:
	CLabelSet1();
	virtual ~CLabelSet1();
	bool OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL);
//	bool Execute(COleBeliefBase* poleBB, LPCTSTR lpszSQL, ...);
};

class CLabelRow2
{
public:
	CLabelRow2();
	virtual ~CLabelRow2();

	int m_nSequence;
	TCHAR m_szName[100];
	TCHAR m_szLabels[4][100];

	BEGIN_COLUMN_MAP(CLabelRow2)
		COLUMN_ENTRY_TYPE( 1, DBTYPE_I4,	m_nSequence)
		COLUMN_ENTRY_TYPE( 2, DBTYPE_STR,	m_szName)
		COLUMN_ENTRY_TYPE( 3, DBTYPE_STR,	m_szLabels  [ 0])
		COLUMN_ENTRY_TYPE( 4, DBTYPE_STR,	m_szLabels  [ 1])
		COLUMN_ENTRY_TYPE( 5, DBTYPE_STR,	m_szLabels  [ 2])
		COLUMN_ENTRY_TYPE( 6, DBTYPE_STR,	m_szLabels  [ 3])
	END_COLUMN_MAP()
};

class CLabelSet2 : public CLabelSet, public CCommand<CAccessor<CLabelRow2> >
{
public:
	CLabelSet2();
	virtual ~CLabelSet2();
	bool OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL);
//	bool Execute(COleBeliefBase* poleBB, LPCTSTR lpszSQL, ...);

//protected:
//	COleBeliefBase* m_poleBB;
};

class CLabelRow3
{
public:
	CLabelRow3();
	virtual ~CLabelRow3();

	int m_nSequence;
	TCHAR m_szName[100];
	TCHAR m_szLabels[8][100];

	BEGIN_COLUMN_MAP(CLabelRow3)
		COLUMN_ENTRY_TYPE( 1, DBTYPE_I4,	m_nSequence)
		COLUMN_ENTRY_TYPE( 2, DBTYPE_STR,	m_szName)
		COLUMN_ENTRY_TYPE( 3, DBTYPE_STR,	m_szLabels  [ 0])
		COLUMN_ENTRY_TYPE( 4, DBTYPE_STR,	m_szLabels  [ 1])
		COLUMN_ENTRY_TYPE( 5, DBTYPE_STR,	m_szLabels  [ 2])
		COLUMN_ENTRY_TYPE( 6, DBTYPE_STR,	m_szLabels  [ 3])
		COLUMN_ENTRY_TYPE( 7, DBTYPE_STR,	m_szLabels  [ 4])
		COLUMN_ENTRY_TYPE( 8, DBTYPE_STR,	m_szLabels  [ 5])
		COLUMN_ENTRY_TYPE( 9, DBTYPE_STR,	m_szLabels  [ 6])
		COLUMN_ENTRY_TYPE(10, DBTYPE_STR,	m_szLabels  [ 7])
	END_COLUMN_MAP()
};

class CLabelSet3 : public CLabelSet, public CCommand<CAccessor<CLabelRow3> >
{
public:
	CLabelSet3();
	virtual ~CLabelSet3();
	bool OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL);
//	bool Execute(COleBeliefBase* poleBB, LPCTSTR lpszSQL, ...);

//protected:
//	COleBeliefBase* m_poleBB;
};

class CLabelRow6
{
public:
	CLabelRow6();
	virtual ~CLabelRow6();

	int m_nSequence;
	TCHAR m_szName[100];
	TCHAR m_szLabels[64][100];

	BEGIN_COLUMN_MAP(CLabelRow6)
		COLUMN_ENTRY_TYPE( 1, DBTYPE_I4,	m_nSequence)
		COLUMN_ENTRY_TYPE( 2, DBTYPE_STR,	m_szName)
		COLUMN_ENTRY_TYPE( 3, DBTYPE_STR,	m_szLabels  [ 0])
		COLUMN_ENTRY_TYPE( 4, DBTYPE_STR,	m_szLabels  [ 1])
		COLUMN_ENTRY_TYPE( 5, DBTYPE_STR,	m_szLabels  [ 2])
		COLUMN_ENTRY_TYPE( 6, DBTYPE_STR,	m_szLabels  [ 3])
		COLUMN_ENTRY_TYPE( 7, DBTYPE_STR,	m_szLabels  [ 4])
		COLUMN_ENTRY_TYPE( 8, DBTYPE_STR,	m_szLabels  [ 5])
		COLUMN_ENTRY_TYPE( 9, DBTYPE_STR,	m_szLabels  [ 6])
		COLUMN_ENTRY_TYPE(10, DBTYPE_STR,	m_szLabels  [ 7])
		COLUMN_ENTRY_TYPE(11, DBTYPE_STR,	m_szLabels  [ 8])
		COLUMN_ENTRY_TYPE(12, DBTYPE_STR,	m_szLabels  [ 9])
		COLUMN_ENTRY_TYPE(13, DBTYPE_STR,	m_szLabels  [10])
		COLUMN_ENTRY_TYPE(14, DBTYPE_STR,	m_szLabels  [11])
		COLUMN_ENTRY_TYPE(15, DBTYPE_STR,	m_szLabels  [12])
		COLUMN_ENTRY_TYPE(16, DBTYPE_STR,	m_szLabels  [13])
		COLUMN_ENTRY_TYPE(17, DBTYPE_STR,	m_szLabels  [14])
		COLUMN_ENTRY_TYPE(18, DBTYPE_STR,	m_szLabels  [15])
		COLUMN_ENTRY_TYPE(19, DBTYPE_STR,	m_szLabels  [16])
		COLUMN_ENTRY_TYPE(20, DBTYPE_STR,	m_szLabels  [17])
		COLUMN_ENTRY_TYPE(21, DBTYPE_STR,	m_szLabels  [18])
		COLUMN_ENTRY_TYPE(22, DBTYPE_STR,	m_szLabels  [19])
		COLUMN_ENTRY_TYPE(23, DBTYPE_STR,	m_szLabels  [20])
		COLUMN_ENTRY_TYPE(24, DBTYPE_STR,	m_szLabels  [21])
		COLUMN_ENTRY_TYPE(25, DBTYPE_STR,	m_szLabels  [22])
		COLUMN_ENTRY_TYPE(26, DBTYPE_STR,	m_szLabels  [23])
		COLUMN_ENTRY_TYPE(27, DBTYPE_STR,	m_szLabels  [24])
		COLUMN_ENTRY_TYPE(28, DBTYPE_STR,	m_szLabels  [25])
		COLUMN_ENTRY_TYPE(29, DBTYPE_STR,	m_szLabels  [26])
		COLUMN_ENTRY_TYPE(30, DBTYPE_STR,	m_szLabels  [27])
		COLUMN_ENTRY_TYPE(31, DBTYPE_STR,	m_szLabels  [28])
		COLUMN_ENTRY_TYPE(32, DBTYPE_STR,	m_szLabels  [29])
		COLUMN_ENTRY_TYPE(33, DBTYPE_STR,	m_szLabels  [30])
		COLUMN_ENTRY_TYPE(34, DBTYPE_STR,	m_szLabels  [31])
		COLUMN_ENTRY_TYPE(35, DBTYPE_STR,	m_szLabels  [32])
		COLUMN_ENTRY_TYPE(36, DBTYPE_STR,	m_szLabels  [33])
		COLUMN_ENTRY_TYPE(37, DBTYPE_STR,	m_szLabels  [34])
		COLUMN_ENTRY_TYPE(38, DBTYPE_STR,	m_szLabels  [35])
		COLUMN_ENTRY_TYPE(39, DBTYPE_STR,	m_szLabels  [36])
		COLUMN_ENTRY_TYPE(40, DBTYPE_STR,	m_szLabels  [37])
		COLUMN_ENTRY_TYPE(41, DBTYPE_STR,	m_szLabels  [38])
		COLUMN_ENTRY_TYPE(42, DBTYPE_STR,	m_szLabels  [39])
		COLUMN_ENTRY_TYPE(43, DBTYPE_STR,	m_szLabels  [40])
		COLUMN_ENTRY_TYPE(44, DBTYPE_STR,	m_szLabels  [41])
		COLUMN_ENTRY_TYPE(45, DBTYPE_STR,	m_szLabels  [42])
		COLUMN_ENTRY_TYPE(46, DBTYPE_STR,	m_szLabels  [43])
		COLUMN_ENTRY_TYPE(47, DBTYPE_STR,	m_szLabels  [44])
		COLUMN_ENTRY_TYPE(48, DBTYPE_STR,	m_szLabels  [45])
		COLUMN_ENTRY_TYPE(49, DBTYPE_STR,	m_szLabels  [46])
		COLUMN_ENTRY_TYPE(50, DBTYPE_STR,	m_szLabels  [47])
		COLUMN_ENTRY_TYPE(51, DBTYPE_STR,	m_szLabels  [48])
		COLUMN_ENTRY_TYPE(52, DBTYPE_STR,	m_szLabels  [49])
		COLUMN_ENTRY_TYPE(53, DBTYPE_STR,	m_szLabels  [50])
		COLUMN_ENTRY_TYPE(54, DBTYPE_STR,	m_szLabels  [51])
		COLUMN_ENTRY_TYPE(55, DBTYPE_STR,	m_szLabels  [52])
		COLUMN_ENTRY_TYPE(56, DBTYPE_STR,	m_szLabels  [53])
		COLUMN_ENTRY_TYPE(57, DBTYPE_STR,	m_szLabels  [54])
		COLUMN_ENTRY_TYPE(58, DBTYPE_STR,	m_szLabels  [55])
		COLUMN_ENTRY_TYPE(59, DBTYPE_STR,	m_szLabels  [56])
		COLUMN_ENTRY_TYPE(60, DBTYPE_STR,	m_szLabels  [57])
		COLUMN_ENTRY_TYPE(61, DBTYPE_STR,	m_szLabels  [58])
		COLUMN_ENTRY_TYPE(62, DBTYPE_STR,	m_szLabels  [59])
		COLUMN_ENTRY_TYPE(63, DBTYPE_STR,	m_szLabels  [60])
		COLUMN_ENTRY_TYPE(64, DBTYPE_STR,	m_szLabels  [61])
		COLUMN_ENTRY_TYPE(65, DBTYPE_STR,	m_szLabels  [62])
		COLUMN_ENTRY_TYPE(66, DBTYPE_STR,	m_szLabels  [63])

	END_COLUMN_MAP()
};

class CLabelSet6 : public CLabelSet, public CCommand<CAccessor<CLabelRow6> >
{
public:
	CLabelSet6();
	virtual ~CLabelSet6();
	bool OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL);
	bool Execute(COleBeliefBase* poleBB, LPCTSTR lpszSQL, ...);

//protected:
//	COleBeliefBase* m_poleBB;
};

//{{AFX_INSERT_LOCATION}}

#endif

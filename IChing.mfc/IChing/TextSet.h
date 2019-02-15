#if !defined(AFX_TEXTSET_H__F9AE70ED_DB7E_4839_BEB9_CE228BDC869E__INCLUDED_)
#define AFX_TEXTSET_H__F9AE70ED_DB7E_4839_BEB9_CE228BDC869E__INCLUDED_

#pragma once

class CTextColumn
{
public:
	CTextColumn();
	virtual ~CTextColumn();

	TCHAR m_szText[10240];

	BEGIN_COLUMN_MAP(CTextColumn)
		COLUMN_ENTRY_TYPE( 1, DBTYPE_STR, m_szText)
	END_COLUMN_MAP()
};

class CTextColumnSet : public CCommand<CAccessor<CTextColumn> >
{
public:
	CTextColumnSet();
	virtual ~CTextColumnSet();
	bool OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL);
	bool Execute(COleBeliefBase* poleBB, LPCTSTR lpszSQL, ...);

protected:
	COleBeliefBase* m_poleBB;
};

//////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////

class CTextRow6
{
public:
	CTextRow6();
	virtual ~CTextRow6();

	TCHAR m_szType[128];
	int m_nSequence;
	TCHAR m_szName[128];
	TCHAR m_szText[64][10240];

	BEGIN_COLUMN_MAP(CTextRow6)
		COLUMN_ENTRY_TYPE( 1, DBTYPE_STR, m_szType)
		COLUMN_ENTRY_TYPE( 2, DBTYPE_I4, m_nSequence)
		COLUMN_ENTRY_TYPE( 3, DBTYPE_STR, m_szName)

		COLUMN_ENTRY_TYPE( 4, DBTYPE_STR, m_szText[ 0])
		COLUMN_ENTRY_TYPE( 5, DBTYPE_STR, m_szText[ 1])
		COLUMN_ENTRY_TYPE( 6, DBTYPE_STR, m_szText[ 2])
		COLUMN_ENTRY_TYPE( 7, DBTYPE_STR, m_szText[ 3])
		COLUMN_ENTRY_TYPE( 8, DBTYPE_STR, m_szText[ 4])
		COLUMN_ENTRY_TYPE( 9, DBTYPE_STR, m_szText[ 5])
		COLUMN_ENTRY_TYPE(10, DBTYPE_STR, m_szText[ 6])
		COLUMN_ENTRY_TYPE(11, DBTYPE_STR, m_szText[ 7])
		COLUMN_ENTRY_TYPE(12, DBTYPE_STR, m_szText[ 8])
		COLUMN_ENTRY_TYPE(13, DBTYPE_STR, m_szText[ 9])
		COLUMN_ENTRY_TYPE(14, DBTYPE_STR, m_szText[10])
		COLUMN_ENTRY_TYPE(15, DBTYPE_STR, m_szText[11])
		COLUMN_ENTRY_TYPE(16, DBTYPE_STR, m_szText[12])
		COLUMN_ENTRY_TYPE(17, DBTYPE_STR, m_szText[13])
		COLUMN_ENTRY_TYPE(18, DBTYPE_STR, m_szText[14])
		COLUMN_ENTRY_TYPE(19, DBTYPE_STR, m_szText[15])
		COLUMN_ENTRY_TYPE(20, DBTYPE_STR, m_szText[16])
		COLUMN_ENTRY_TYPE(21, DBTYPE_STR, m_szText[17])
		COLUMN_ENTRY_TYPE(22, DBTYPE_STR, m_szText[18])
		COLUMN_ENTRY_TYPE(23, DBTYPE_STR, m_szText[19])
		COLUMN_ENTRY_TYPE(24, DBTYPE_STR, m_szText[20])
		COLUMN_ENTRY_TYPE(25, DBTYPE_STR, m_szText[21])
		COLUMN_ENTRY_TYPE(26, DBTYPE_STR, m_szText[22])
		COLUMN_ENTRY_TYPE(27, DBTYPE_STR, m_szText[23])
		COLUMN_ENTRY_TYPE(28, DBTYPE_STR, m_szText[24])
		COLUMN_ENTRY_TYPE(29, DBTYPE_STR, m_szText[25])
		COLUMN_ENTRY_TYPE(30, DBTYPE_STR, m_szText[26])
		COLUMN_ENTRY_TYPE(31, DBTYPE_STR, m_szText[27])
		COLUMN_ENTRY_TYPE(32, DBTYPE_STR, m_szText[28])
		COLUMN_ENTRY_TYPE(33, DBTYPE_STR, m_szText[29])
		COLUMN_ENTRY_TYPE(34, DBTYPE_STR, m_szText[30])
		COLUMN_ENTRY_TYPE(35, DBTYPE_STR, m_szText[31])
		COLUMN_ENTRY_TYPE(36, DBTYPE_STR, m_szText[32])
		COLUMN_ENTRY_TYPE(37, DBTYPE_STR, m_szText[33])
		COLUMN_ENTRY_TYPE(38, DBTYPE_STR, m_szText[34])
		COLUMN_ENTRY_TYPE(39, DBTYPE_STR, m_szText[35])
		COLUMN_ENTRY_TYPE(40, DBTYPE_STR, m_szText[36])
		COLUMN_ENTRY_TYPE(41, DBTYPE_STR, m_szText[37])
		COLUMN_ENTRY_TYPE(42, DBTYPE_STR, m_szText[38])
		COLUMN_ENTRY_TYPE(43, DBTYPE_STR, m_szText[39])
		COLUMN_ENTRY_TYPE(44, DBTYPE_STR, m_szText[40])
		COLUMN_ENTRY_TYPE(45, DBTYPE_STR, m_szText[41])
		COLUMN_ENTRY_TYPE(46, DBTYPE_STR, m_szText[42])
		COLUMN_ENTRY_TYPE(47, DBTYPE_STR, m_szText[43])
		COLUMN_ENTRY_TYPE(48, DBTYPE_STR, m_szText[44])
		COLUMN_ENTRY_TYPE(49, DBTYPE_STR, m_szText[45])
		COLUMN_ENTRY_TYPE(50, DBTYPE_STR, m_szText[46])
		COLUMN_ENTRY_TYPE(51, DBTYPE_STR, m_szText[47])
		COLUMN_ENTRY_TYPE(52, DBTYPE_STR, m_szText[48])
		COLUMN_ENTRY_TYPE(53, DBTYPE_STR, m_szText[49])
		COLUMN_ENTRY_TYPE(54, DBTYPE_STR, m_szText[50])
		COLUMN_ENTRY_TYPE(55, DBTYPE_STR, m_szText[51])
		COLUMN_ENTRY_TYPE(56, DBTYPE_STR, m_szText[52])
		COLUMN_ENTRY_TYPE(57, DBTYPE_STR, m_szText[53])
		COLUMN_ENTRY_TYPE(58, DBTYPE_STR, m_szText[54])
		COLUMN_ENTRY_TYPE(59, DBTYPE_STR, m_szText[55])
		COLUMN_ENTRY_TYPE(60, DBTYPE_STR, m_szText[56])
		COLUMN_ENTRY_TYPE(61, DBTYPE_STR, m_szText[57])
		COLUMN_ENTRY_TYPE(62, DBTYPE_STR, m_szText[58])
		COLUMN_ENTRY_TYPE(63, DBTYPE_STR, m_szText[59])
		COLUMN_ENTRY_TYPE(64, DBTYPE_STR, m_szText[60])
		COLUMN_ENTRY_TYPE(65, DBTYPE_STR, m_szText[61])
		COLUMN_ENTRY_TYPE(66, DBTYPE_STR, m_szText[62])
		COLUMN_ENTRY_TYPE(67, DBTYPE_STR, m_szText[63])
	END_COLUMN_MAP()
};

class CTextSet6 : public CCommand<CAccessor<CTextRow6> >
{
public:
	CTextSet6();
	virtual ~CTextSet6();
	bool OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL);
//	bool Execute(COleBeliefBase* poleBB, LPCTSTR lpszSQL, ...);

protected:
	COleBeliefBase* m_poleBB;
};

//{{AFX_INSERT_LOCATION}}

#endif

#if !defined(AFX_IMAGESET_H__212EC608_9155_4A45_AA33_D66554D11FD2__INCLUDED_)
#define AFX_IMAGESET_H__212EC608_9155_4A45_AA33_D66554D11FD2__INCLUDED_

#pragma once

class CImageEntry : public CObject
{
public:
//	CImageEntry(int nValue, ISequentialStream* pImage);
	CImageEntry(int nValue, LPCTSTR lpszFileName);
	virtual ~CImageEntry();

	bool SetImage(BYTE* pchImage);
	int GetWidth() {return m_bmi.bmiHeader.biWidth;}
	int GetHeight() {return abs(m_bmi.bmiHeader.biHeight);}
	int GetSize() {return GetWidth() * GetHeight();}
	int GetBitsPerPixel() {return m_bmi.bmiHeader.biBitCount;}
	HBITMAP GetBitmap() {return m_hBMP;}

protected:
	int m_nValue;
	BITMAPFILEHEADER m_bmfh;
	BITMAPINFO m_bmi;
	HBITMAP m_hBMP;
	BYTE* m_pchBits;
};

class CImageEntryArray : public CObArray
{
public:
	CImageEntryArray() {m_nUpperBound = -1;}
	virtual ~CImageEntryArray() {RemoveAll();}
	void RemoveAll() {if(m_nUpperBound == -1) return; for(int i = 0; i <= m_nUpperBound; ++i) delete GetAt(i); CObArray::RemoveAll(); m_nUpperBound = -1;}
	void SetAt(int nIndex, CImageEntry* newElement) {CObArray::SetAt(nIndex,newElement); m_nUpperBound = nIndex > m_nUpperBound ? nIndex : m_nUpperBound;}
	CImageEntry* GetAt(int nIndex) {return (CImageEntry*) CObArray::GetAt(nIndex);}
	int GetUpperBound() const {return m_nUpperBound;}

protected:
	int m_nUpperBound;
};

class CImageBlock : public CObject
{
public:
//	CImageBlock(int nSequence, int nSize, LPCTSTR lpszName,
//		ISequentialStream* ppImage[64]);
	CImageBlock(int nSequence, int nSize, LPCTSTR lpszName, 
		CStringList* plsoFileNames);
	virtual ~CImageBlock() {}
	CImageEntryArray* GetEntryArray() {return &m_rgImageEntry;}
	void SetSequence(int nSequence) {m_nSequence = nSequence;}
	int GetSequence() {return m_nSequence;}
	void SetName(LPCTSTR lpszName) {m_strName = lpszName;}
	LPCTSTR GetName(void) const {return(m_strName);}

protected:
	int m_nSequence;
	int m_nSize;
	CString m_strName;
	CImageEntryArray m_rgImageEntry;
};

class CImageBlockArray : public CObArray
{
public:
	CImageBlockArray() {m_nUpperBound = -1; m_bModified = false;}
	virtual ~CImageBlockArray() {RemoveAll();}
	virtual bool LoadBelief(COleBeliefBase*) {return false;}
	virtual bool SaveBelief(COleBeliefBase*) {return false;}
	void RemoveAll() {if(m_nUpperBound == -1) return; for(int i = 0; i <= m_nUpperBound; ++i) delete GetAt(i); CObArray::RemoveAll(); m_nUpperBound = -1;}
	bool LoadBelief();
	void SetAt(int nIndex, CImageBlock* newElement) {CObArray::SetAt(nIndex,newElement); m_nUpperBound = nIndex > m_nUpperBound ? nIndex : m_nUpperBound;}
	CImageBlock* GetAt(int nIndex) {return (CImageBlock*) CObArray::GetAt(nIndex);}
	int GetUpperBound() const {return m_nUpperBound;}
	void SetModified(bool bModified = true) {m_bModified = bModified;}
	bool GetModified() {return m_bModified;}

protected:
	int m_nUpperBound;
	bool m_bModified;
};

class CImageBlockArray1 : public CImageBlockArray
{
public:
	virtual bool LoadBelief(COleBeliefBase* poleBB);
//	virtual bool SaveBelief(COleBeliefBase* poleBB);
};

class CImageBlockArray2 : public CImageBlockArray
{
public:
	virtual bool LoadBelief(COleBeliefBase* poleBB);
//	virtual bool SaveBelief(COleBeliefBase* poleBB);
};

class CImageBlockArray3 : public CImageBlockArray
{
public:
	virtual bool LoadBelief(COleBeliefBase* poleBB);
//	virtual bool SaveBelief(COleBeliefBase* poleBB);
};

class CImageBlockArray6 : public CImageBlockArray
{
public:
	virtual bool LoadBelief(COleBeliefBase* poleBB);
//	virtual bool SaveBelief(COleBeliefBase* poleBB);
};

class CImageRow1
{
public:
	CImageRow1();
	virtual ~CImageRow1();

	int m_nSequence;
	TCHAR m_szName[100];
	ISequentialStream* m_pImage[2];

	BEGIN_COLUMN_MAP(CImageRow1)
		COLUMN_ENTRY_TYPE(1, DBTYPE_I4, m_nSequence)
		COLUMN_ENTRY_TYPE(2, DBTYPE_STR, m_szName)
		BLOB_ENTRY(3, IID_ISequentialStream, STGM_READ, m_pImage[0])
		BLOB_ENTRY(4, IID_ISequentialStream, STGM_READ, m_pImage[1])
	END_COLUMN_MAP()
};

class CImageSet
{
public:
	CImageSet();
	virtual ~CImageSet();

protected:
	COleBeliefBase* m_poleBB;
};

class CImageSet1 : public CImageSet, public CCommand<CAccessor<CImageRow1> >
{
public:
	CImageSet1();
	virtual ~CImageSet1();
	bool OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL);
//	bool Execute(COleBeliefBase* poleBB, LPCTSTR lpszSQL, ...);

//protected:
//	COleBeliefBase* m_poleBB;
};

class CImageRow2
{
public:
	CImageRow2();
	virtual ~CImageRow2();

	int m_nSequence;
	TCHAR m_szName[100];
	ISequentialStream* m_pImage[4];

	BEGIN_COLUMN_MAP(CImageRow2)
		COLUMN_ENTRY_TYPE(1, DBTYPE_I4, m_nSequence)
		COLUMN_ENTRY_TYPE(2, DBTYPE_STR, m_szName)
		BLOB_ENTRY( 3, IID_ISequentialStream, STGM_READ, m_pImage[ 0])
		BLOB_ENTRY( 4, IID_ISequentialStream, STGM_READ, m_pImage[ 1])
		BLOB_ENTRY( 5, IID_ISequentialStream, STGM_READ, m_pImage[ 2])
		BLOB_ENTRY( 6, IID_ISequentialStream, STGM_READ, m_pImage[ 3])
	END_COLUMN_MAP()
};

class CImageSet2 : public CImageSet, public CCommand<CAccessor<CImageRow2> >
{
public:
	CImageSet2();
	virtual ~CImageSet2();
	bool OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL);
//	bool Execute(COleBeliefBase* poleBB, LPCTSTR lpszSQL, ...);

//protected:
//	COleBeliefBase* m_poleBB;
};

class CImageRow3
{
public:
	CImageRow3();
	virtual ~CImageRow3();

	int m_nSequence;
	TCHAR m_szName[100];
	ISequentialStream* m_pImage[8];

	BEGIN_COLUMN_MAP(CImageRow3)
		COLUMN_ENTRY_TYPE(1, DBTYPE_I4, m_nSequence)
		COLUMN_ENTRY_TYPE(2, DBTYPE_STR, m_szName)
		BLOB_ENTRY( 3, IID_ISequentialStream, STGM_READ, m_pImage[ 0])
		BLOB_ENTRY( 4, IID_ISequentialStream, STGM_READ, m_pImage[ 1])
		BLOB_ENTRY( 5, IID_ISequentialStream, STGM_READ, m_pImage[ 2])
		BLOB_ENTRY( 6, IID_ISequentialStream, STGM_READ, m_pImage[ 3])
		BLOB_ENTRY( 7, IID_ISequentialStream, STGM_READ, m_pImage[ 4])
		BLOB_ENTRY( 8, IID_ISequentialStream, STGM_READ, m_pImage[ 5])
		BLOB_ENTRY( 9, IID_ISequentialStream, STGM_READ, m_pImage[ 6])
		BLOB_ENTRY(10, IID_ISequentialStream, STGM_READ, m_pImage[ 7])
	END_COLUMN_MAP()
};

class CImageSet3 : public CImageSet, public CCommand<CAccessor<CImageRow3> >
{
public:
	CImageSet3();
	virtual ~CImageSet3();
	bool OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL);
//	bool Execute(COleBeliefBase* poleBB, LPCTSTR lpszSQL, ...);

//protected:
//	COleBeliefBase* m_poleBB;
};

class CImageRow6
{
public:
	CImageRow6();
	virtual ~CImageRow6();

	int m_nSequence;
	TCHAR m_szName[100];
	ISequentialStream* m_pImage[64];

	BEGIN_COLUMN_MAP(CImageRow6)
		COLUMN_ENTRY_TYPE(1, DBTYPE_I4, m_nSequence)
		COLUMN_ENTRY_TYPE(2, DBTYPE_STR, m_szName)
		BLOB_ENTRY( 3, IID_ISequentialStream, STGM_READ, m_pImage[ 0])
		BLOB_ENTRY( 4, IID_ISequentialStream, STGM_READ, m_pImage[ 1])
		BLOB_ENTRY( 5, IID_ISequentialStream, STGM_READ, m_pImage[ 2])
		BLOB_ENTRY( 6, IID_ISequentialStream, STGM_READ, m_pImage[ 3])
		BLOB_ENTRY( 7, IID_ISequentialStream, STGM_READ, m_pImage[ 4])
		BLOB_ENTRY( 8, IID_ISequentialStream, STGM_READ, m_pImage[ 5])
		BLOB_ENTRY( 9, IID_ISequentialStream, STGM_READ, m_pImage[ 6])
		BLOB_ENTRY(10, IID_ISequentialStream, STGM_READ, m_pImage[ 7])
		BLOB_ENTRY(11, IID_ISequentialStream, STGM_READ, m_pImage[ 8])
		BLOB_ENTRY(12, IID_ISequentialStream, STGM_READ, m_pImage[ 9])
		BLOB_ENTRY(13, IID_ISequentialStream, STGM_READ, m_pImage[10])
		BLOB_ENTRY(14, IID_ISequentialStream, STGM_READ, m_pImage[11])
		BLOB_ENTRY(15, IID_ISequentialStream, STGM_READ, m_pImage[12])
		BLOB_ENTRY(16, IID_ISequentialStream, STGM_READ, m_pImage[13])
		BLOB_ENTRY(17, IID_ISequentialStream, STGM_READ, m_pImage[14])
		BLOB_ENTRY(18, IID_ISequentialStream, STGM_READ, m_pImage[15])
		BLOB_ENTRY(19, IID_ISequentialStream, STGM_READ, m_pImage[16])
		BLOB_ENTRY(20, IID_ISequentialStream, STGM_READ, m_pImage[17])
		BLOB_ENTRY(21, IID_ISequentialStream, STGM_READ, m_pImage[18])
		BLOB_ENTRY(22, IID_ISequentialStream, STGM_READ, m_pImage[19])
		BLOB_ENTRY(23, IID_ISequentialStream, STGM_READ, m_pImage[20])
		BLOB_ENTRY(24, IID_ISequentialStream, STGM_READ, m_pImage[21])
		BLOB_ENTRY(25, IID_ISequentialStream, STGM_READ, m_pImage[22])
		BLOB_ENTRY(26, IID_ISequentialStream, STGM_READ, m_pImage[23])
		BLOB_ENTRY(27, IID_ISequentialStream, STGM_READ, m_pImage[24])
		BLOB_ENTRY(28, IID_ISequentialStream, STGM_READ, m_pImage[25])
		BLOB_ENTRY(29, IID_ISequentialStream, STGM_READ, m_pImage[26])
		BLOB_ENTRY(30, IID_ISequentialStream, STGM_READ, m_pImage[27])
		BLOB_ENTRY(31, IID_ISequentialStream, STGM_READ, m_pImage[28])
		BLOB_ENTRY(32, IID_ISequentialStream, STGM_READ, m_pImage[29])
		BLOB_ENTRY(33, IID_ISequentialStream, STGM_READ, m_pImage[30])
		BLOB_ENTRY(34, IID_ISequentialStream, STGM_READ, m_pImage[31])
		BLOB_ENTRY(35, IID_ISequentialStream, STGM_READ, m_pImage[32])
		BLOB_ENTRY(36, IID_ISequentialStream, STGM_READ, m_pImage[33])
		BLOB_ENTRY(37, IID_ISequentialStream, STGM_READ, m_pImage[34])
		BLOB_ENTRY(38, IID_ISequentialStream, STGM_READ, m_pImage[35])
		BLOB_ENTRY(39, IID_ISequentialStream, STGM_READ, m_pImage[36])
		BLOB_ENTRY(40, IID_ISequentialStream, STGM_READ, m_pImage[37])
		BLOB_ENTRY(41, IID_ISequentialStream, STGM_READ, m_pImage[38])
		BLOB_ENTRY(42, IID_ISequentialStream, STGM_READ, m_pImage[39])
		BLOB_ENTRY(43, IID_ISequentialStream, STGM_READ, m_pImage[40])
		BLOB_ENTRY(44, IID_ISequentialStream, STGM_READ, m_pImage[41])
		BLOB_ENTRY(45, IID_ISequentialStream, STGM_READ, m_pImage[42])
		BLOB_ENTRY(46, IID_ISequentialStream, STGM_READ, m_pImage[43])
		BLOB_ENTRY(47, IID_ISequentialStream, STGM_READ, m_pImage[44])
		BLOB_ENTRY(48, IID_ISequentialStream, STGM_READ, m_pImage[45])
		BLOB_ENTRY(49, IID_ISequentialStream, STGM_READ, m_pImage[46])
		BLOB_ENTRY(50, IID_ISequentialStream, STGM_READ, m_pImage[47])
		BLOB_ENTRY(51, IID_ISequentialStream, STGM_READ, m_pImage[48])
		BLOB_ENTRY(52, IID_ISequentialStream, STGM_READ, m_pImage[49])
		BLOB_ENTRY(53, IID_ISequentialStream, STGM_READ, m_pImage[50])
		BLOB_ENTRY(54, IID_ISequentialStream, STGM_READ, m_pImage[51])
		BLOB_ENTRY(55, IID_ISequentialStream, STGM_READ, m_pImage[52])
		BLOB_ENTRY(56, IID_ISequentialStream, STGM_READ, m_pImage[53])
		BLOB_ENTRY(57, IID_ISequentialStream, STGM_READ, m_pImage[54])
		BLOB_ENTRY(58, IID_ISequentialStream, STGM_READ, m_pImage[55])
		BLOB_ENTRY(59, IID_ISequentialStream, STGM_READ, m_pImage[56])
		BLOB_ENTRY(60, IID_ISequentialStream, STGM_READ, m_pImage[57])
		BLOB_ENTRY(61, IID_ISequentialStream, STGM_READ, m_pImage[58])
		BLOB_ENTRY(62, IID_ISequentialStream, STGM_READ, m_pImage[59])
		BLOB_ENTRY(63, IID_ISequentialStream, STGM_READ, m_pImage[60])
		BLOB_ENTRY(64, IID_ISequentialStream, STGM_READ, m_pImage[61])
		BLOB_ENTRY(65, IID_ISequentialStream, STGM_READ, m_pImage[62])
		BLOB_ENTRY(66, IID_ISequentialStream, STGM_READ, m_pImage[63])
	END_COLUMN_MAP()
};

class CImageSet6 : public CImageSet, public CCommand<CAccessor<CImageRow6> >
{
public:
	CImageSet6();
	virtual ~CImageSet6();
	bool OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL);
//	bool Execute(COleBeliefBase* poleBB, LPCTSTR lpszSQL, ...);

//protected:
//	COleBeliefBase* m_poleBB;
};

//{{AFX_INSERT_LOCATION}}

#endif

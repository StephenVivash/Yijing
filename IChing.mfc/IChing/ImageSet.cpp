#include "StdAfx.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

class CLoadImages : public CProcessPath
{                      
public:             
	CLoadImages(CStringList* plssImages) {m_plssImages = plssImages;}
	virtual ~CLoadImages() {}
	virtual bool ProcessFile(LPCTSTR lpszPath, LPCTSTR lpszFile, int nPathNo, int nFileNo);
protected:
	CStringList* m_plssImages;
};

bool CLoadImages::ProcessFile(LPCTSTR lpszPath, LPCTSTR lpszFile, int nPathNo, int nFileNo)
{
	m_plssImages->AddTail(::ConcatPath(lpszPath,lpszFile));
	return true;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

CImageEntry::CImageEntry(int nValue, LPCTSTR lpszFileName)
{
	BYTE pchBuffer[102400];
	UINT nLength;
	CFile F;
	m_hBMP = NULL;
	m_nValue = nValue;
	if(F.Open(lpszFileName,CFile::modeRead))
	{
		nLength = F.Read(pchBuffer,102400);
		for(int i = 0; i < 500; ++i)
		{
			BITMAPFILEHEADER* pbmfh = (BITMAPFILEHEADER*) &pchBuffer[i];
			if(pbmfh->bfType == 'MB')
			{
				BITMAPINFO* pbmi = (BITMAPINFO*) (&pchBuffer[i] + sizeof BITMAPFILEHEADER);
				if((pbmi->bmiHeader.biSize == sizeof BITMAPINFOHEADER) && 
					(pbmi->bmiHeader.biCompression == BI_RGB))
				{
					SetImage(&pchBuffer[i]);
					break;
				}
			}
		}
		F.Close();
	}
}
/*
CImageEntry::CImageEntry(int nValue, ISequentialStream* pImage)
{
	BYTE pchBuffer[102400];
	ULONG ulBytesRead;
	m_hBMP = NULL;
	m_nValue = nValue;
	pImage->Read(pchBuffer,102400,&ulBytesRead);
	for(int i = 78; i < 500; ++i)
	{
		BITMAPFILEHEADER* pbmfh = (BITMAPFILEHEADER*) &pchBuffer[i];
		if(pbmfh->bfType == 'MB')
		{
			BITMAPINFO* pbmi = (BITMAPINFO*) (&pchBuffer[i] + sizeof BITMAPFILEHEADER);
			if((pbmi->bmiHeader.biSize == sizeof BITMAPINFOHEADER) && 
				(pbmi->bmiHeader.biCompression == BI_RGB))
			{
				SetImage(&pchBuffer[i]);
				break;
			}
		}
	}
}
*/
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

CImageEntry::~CImageEntry()
{
	::DeleteObject(m_hBMP);
}

bool CImageEntry::SetImage(BYTE* pchImage)
{
	BITMAPFILEHEADER* pbmfh = (BITMAPFILEHEADER*) pchImage;
	if(pbmfh->bfType == 'MB')
	{
		BITMAPINFO* pbmi = (BITMAPINFO*) (pchImage + sizeof BITMAPFILEHEADER);
		if((pbmi->bmiHeader.biSize == sizeof BITMAPINFOHEADER) && (pbmi->bmiHeader.biPlanes == 1) &&
			(pbmi->bmiHeader.biCompression == BI_RGB) && 
			(pbmi->bmiHeader.biHeight > 0) && ((pbmi->bmiHeader.biWidth % 4) == 0))
		{
			memcpy(&m_bmfh,pbmfh,sizeof BITMAPFILEHEADER);
			memcpy(&m_bmi,pbmi,sizeof BITMAPINFO);
			pbmi->bmiHeader.biHeight = pbmi->bmiHeader.biHeight * -1; // Upside down ?
			if(m_hBMP)
				::DeleteObject(m_hBMP);
			m_hBMP = ::CreateDIBSection(::GetDC(NULL),pbmi,DIB_RGB_COLORS,(void**) 
				&m_pchBits,NULL,0);
			int r,cx = GetWidth() * GetBitsPerPixel() / 8;
			BYTE* pchBuffer = pchImage + pbmfh->bfSize - cx;

			int nHieght = abs(pbmi->bmiHeader.biHeight);
			for(r = 0; r < nHieght; ++r) 
				memcpy(m_pchBits + (r * cx),pchBuffer - (r * cx),cx);
			return true;
		}
	}
	return false;
}

CImageBlock::CImageBlock(int nSequence, int nSize, LPCTSTR lpszName, 
	 CStringList* plsoFileNames)
{
	m_nSequence = nSequence;
	m_nSize = nSize;
	m_strName = lpszName;
	m_rgImageEntry.SetSize(nSize);
	POSITION Pos = plsoFileNames->GetHeadPosition();
	for(int i = 0; (i < nSize) && (Pos != NULL); ++i)
		m_rgImageEntry.SetAt(i,new CImageEntry(i,plsoFileNames->GetNext(Pos)));
}
/*
CImageBlock::CImageBlock(int nSequence, int nSize, LPCTSTR lpszName, 
	 ISequentialStream* ppImage[64])
{
	m_nSequence = nSequence;
	m_nSize = nSize;
	m_strName = lpszName;
	m_rgImageEntry.SetSize(nSize);
	for(int i = 0; i < nSize; ++i)
		m_rgImageEntry.SetAt(i,new CImageEntry(i,ppImage[i]));
}
*/
bool CImageBlockArray1::LoadBelief(COleBeliefBase* poleBB)
{
	int nRow = 0;
	CString strTemp;
	CStringList lssImages;
	RemoveAll();
	SetSize(2,2);
	for(int i = 0; i < 2; ++i)
	{
		strTemp.Format(_T("Tarot%d.bmp"),i);
		lssImages.AddTail(::ConcatPaths(::GetProgramFolder(),_T("Images"),_T("Tarot"),_T("Bits"),strTemp,NULL));
	}
	SetAt(nRow++,new CImageBlock(1,2,_T("Tarot"),&lssImages));
	return true;
}
/*
bool CImageBlockArray1::LoadBelief(COleBeliefBase* poleBB)
{
	int nRow = 0;
	CImageSet1 ImageSet1;
	RemoveAll();
	SetSize(20,2);
	if(ImageSet1.OpenRowset(poleBB,_T("select * from Image1 order by Sequence")))
		do
		{
			SetAt(nRow++,new CImageBlock(ImageSet1.m_nSequence,2,
				ImageSet1.m_szName,ImageSet1.m_pImage));
		} while(ImageSet1.MoveNext() == S_OK);
	return true;
}

bool CImageBlockArray1::SaveBelief(COleBeliefBase* poleBB)
{
	int i,j;
	CImageSet1 ImageSet1;
	CImageBlock* pLS;
	CImageEntryArray* prgLE = NULL;
	CImageEntry* pLE;
	for(i = 0; i <= GetUpperBound(); ++i)
	{
		pLS = GetAt(i);
		prgLE = pLS->GetEntryArray();
		if(prgLE->GetModified())
			for(j = 0; j <= prgLE->GetUpperBound(); ++j)
			{
				pLE = prgLE->GetAt(j);
			}
	}
	ImageSet1.Execute(poleBB,_T("update Image1 set Active = 0"));
	return true;
}
*/

bool CImageBlockArray2::LoadBelief(COleBeliefBase* poleBB)
{
	int nRow = 0;
	CString strTemp;
	CStringList lssImages;
	RemoveAll();
	SetSize(2,2);
	for(int i = 0; i < 4; ++i)
	{
		strTemp.Format(_T("Tarot%d.bmp"),i);
		lssImages.AddTail(::ConcatPaths(::GetProgramFolder(),_T("Images"),_T("Tarot"),_T("Lines"),strTemp,NULL));
	}
	SetAt(nRow++,new CImageBlock(1,4,_T("Tarot"),&lssImages));
	return true;
}
/*
bool CImageBlockArray2::LoadBelief(COleBeliefBase* poleBB)
{
	int nRow = 0;
	CImageSet2 ImageSet2;
	RemoveAll();
	SetSize(10,2);
	if(ImageSet2.OpenRowset(poleBB,_T("select * from Image2 order by Sequence")))
		do
		{
			SetAt(nRow++,new CImageBlock(ImageSet2.m_nSequence,4,
				ImageSet2.m_szName,ImageSet2.m_pImage));
		} while(ImageSet2.MoveNext() == S_OK);
	return true;
}
*/
//bool CImageBlockArray2::SaveBelief(COleBeliefBase* poleBB)
//{
//	return true;
//}

bool CImageBlockArray3::LoadBelief(COleBeliefBase* poleBB)
{
	int nRow = 0;
	CString strTemp;
	CStringList lssImages;
	RemoveAll();
	SetSize(2,2);
	for(int i = 0; i < 8; ++i)
	{
		strTemp.Format(_T("Tarot%d.bmp"),i);
		lssImages.AddTail(::ConcatPaths(::GetProgramFolder(),_T("Images"),_T("Tarot"),_T("Trigrams"),strTemp,NULL));
	}
	SetAt(nRow++,new CImageBlock(1,8,_T("Tarot"),&lssImages));
	return true;
}
/*
bool CImageBlockArray3::LoadBelief(COleBeliefBase* poleBB)
{
	int nRow = 0;
	CImageSet3 ImageSet3;
	RemoveAll();
	SetSize(5,2);
	if(ImageSet3.OpenRowset(poleBB,_T("select * from Image3 order by Sequence")))
		do
		{
			SetAt(nRow++,new CImageBlock(ImageSet3.m_nSequence,8,
				ImageSet3.m_szName,ImageSet3.m_pImage));
		} while(ImageSet3.MoveNext() == S_OK);
	return true;
}
*/
//bool CImageBlockArray3::SaveBelief(COleBeliefBase* poleBB)
//{
//	return true;
//}

bool CImageBlockArray6::LoadBelief(COleBeliefBase* poleBB)
{
	int nRow = 0;
	CString strTemp;
	CStringList lssImages;
	RemoveAll();
	SetSize(2,2);
	for(int i = 0; i < 64; ++i)
	{
		strTemp.Format(_T("Tarot%02d.bmp"),i);
		lssImages.AddTail(::ConcatPaths(::GetProgramFolder(),_T("Images"),_T("Tarot"),_T("Hexagrams"),strTemp,NULL));
	}
	SetAt(nRow++,new CImageBlock(1,64,_T("Tarot"),&lssImages));
	return true;
}
/*
bool CImageBlockArray6::LoadBelief(COleBeliefBase* poleBB)
{
	int nRow = 0;
	CImageSet6 ImageSet6;
	RemoveAll();
	SetSize(5,2);
	if(ImageSet6.OpenRowset(poleBB,_T("select * from Image6 order by Sequence")))
		do
		{
			SetAt(nRow++,new CImageBlock(ImageSet6.m_nSequence,64,
				ImageSet6.m_szName,ImageSet6.m_pImage));
		} while(ImageSet6.MoveNext() == S_OK);
	return true;
}
*/
//bool CImageBlockArray6::SaveBelief(COleBeliefBase* poleBB)
//{
//	return true;
//}

CImageRow1::CImageRow1()
{
	memset((void*) this,0,sizeof(*this));
};

CImageRow1::~CImageRow1()
{
}

CImageSet::CImageSet()
{
}

CImageSet::~CImageSet()
{
}

CImageSet1::CImageSet1() 
{
}

CImageSet1::~CImageSet1() 
{
}

bool CImageSet1::OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL)
{
	HRESULT hr;
	m_poleBB = poleBB;
	CDBPropSet propset(DBPROPSET_ROWSET);
	propset.AddProperty(DBPROP_CANFETCHBACKWARDS, true);
	propset.AddProperty(DBPROP_IRowsetScroll, true);
	propset.AddProperty(DBPROP_IRowsetChange, true);
	propset.AddProperty(DBPROP_UPDATABILITY, DBPROPVAL_UP_CHANGE | DBPROPVAL_UP_INSERT | DBPROPVAL_UP_DELETE );
	if(SUCCEEDED(hr = CCommand<CAccessor<CImageRow1> >::Open(m_poleBB->GetSession(),lpszSQL,&propset)))
		if(SUCCEEDED(hr = MoveNext()))
			return true;
	::AppMessage(_T("CImageSet1::OpenRowset failed\n\n") + GetWinErrorMsg(hr) + _T("\n\n") + lpszSQL,MB_OK | MB_ICONWARNING);
	return false;
}
/*
bool CImageSet1::Execute(COleBeliefBase* poleBB, LPCTSTR lpszSQL, ...)
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
		::AppMessage(_T("CImageSet1::Execute failed\n\n") + GetWinErrorMsg(hr) + _T("\n\n") + strTemp,MB_OK | MB_ICONWARNING);
	va_end(argList);
	return bRC;
}
*/
CImageRow2::CImageRow2()
{
	memset((void*) this,0,sizeof(*this));
};

CImageRow2::~CImageRow2()
{
}

CImageSet2::CImageSet2() 
{
}

CImageSet2::~CImageSet2() 
{
}

bool CImageSet2::OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL)
{
	HRESULT hr;
	m_poleBB = poleBB;
	CDBPropSet propset(DBPROPSET_ROWSET);
	propset.AddProperty(DBPROP_CANFETCHBACKWARDS, true);
	propset.AddProperty(DBPROP_IRowsetScroll, true);
	propset.AddProperty(DBPROP_IRowsetChange, true);
	propset.AddProperty(DBPROP_UPDATABILITY, DBPROPVAL_UP_CHANGE | DBPROPVAL_UP_INSERT | DBPROPVAL_UP_DELETE );
	if(SUCCEEDED(hr = CCommand<CAccessor<CImageRow2> >::Open(m_poleBB->GetSession(),lpszSQL,&propset)))
		if(SUCCEEDED(hr = MoveNext()))
			return true;
	::AppMessage(_T("CImageSet2::OpenRowset failed\n\n") + GetWinErrorMsg(hr) + _T("\n\n") + lpszSQL,MB_OK | MB_ICONWARNING);
	return false;
}
/*
bool CImageSet2::Execute(COleBeliefBase* poleBB, LPCTSTR lpszSQL, ...)
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
		::AppMessage(_T("CImageSet2::Execute failed\n\n") + GetWinErrorMsg(hr) + _T("\n\n") + strTemp,MB_OK | MB_ICONWARNING);
	va_end(argList);
	return bRC;
}
*/
CImageRow3::CImageRow3()
{
	memset((void*) this,0,sizeof(*this));
};

CImageRow3::~CImageRow3()
{
}

CImageSet3::CImageSet3() 
{
}

CImageSet3::~CImageSet3() 
{
}

bool CImageSet3::OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL)
{
	HRESULT hr;
	m_poleBB = poleBB;
	CDBPropSet propset(DBPROPSET_ROWSET);
	propset.AddProperty(DBPROP_CANFETCHBACKWARDS, true);
	propset.AddProperty(DBPROP_IRowsetScroll, true);
	propset.AddProperty(DBPROP_IRowsetChange, true);
	propset.AddProperty(DBPROP_UPDATABILITY, DBPROPVAL_UP_CHANGE | DBPROPVAL_UP_INSERT | DBPROPVAL_UP_DELETE );
	if(SUCCEEDED(hr = CCommand<CAccessor<CImageRow3> >::Open(m_poleBB->GetSession(),lpszSQL,&propset)))
		if(SUCCEEDED(hr = MoveNext()))
			return true;
	::AppMessage(_T("CImageSet3::OpenRowset failed\n\n") + GetWinErrorMsg(hr) + _T("\n\n") + lpszSQL,MB_OK | MB_ICONWARNING);
	return false;
}
/*
bool CImageSet3::Execute(COleBeliefBase* poleBB, LPCTSTR lpszSQL, ...)
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
		::AppMessage(_T("CImageSet3::Execute failed\n\n") + GetWinErrorMsg(hr) + _T("\n\n") + strTemp,MB_OK | MB_ICONWARNING);
	va_end(argList);
	return bRC;
}
*/
CImageRow6::CImageRow6()
{
	memset((void*) this,0,sizeof(*this));
};

CImageRow6::~CImageRow6()
{
}

CImageSet6::CImageSet6() 
{
}

CImageSet6::~CImageSet6() 
{
}

bool CImageSet6::OpenRowset(COleBeliefBase* poleBB, LPCTSTR lpszSQL)
{
	HRESULT hr;
	m_poleBB = poleBB;
	CDBPropSet propset(DBPROPSET_ROWSET);
	propset.AddProperty(DBPROP_CANFETCHBACKWARDS, true);
	propset.AddProperty(DBPROP_IRowsetScroll, true);
	propset.AddProperty(DBPROP_IRowsetChange, true);
	propset.AddProperty(DBPROP_UPDATABILITY, DBPROPVAL_UP_CHANGE | DBPROPVAL_UP_INSERT | DBPROPVAL_UP_DELETE );
	if(SUCCEEDED(hr = CCommand<CAccessor<CImageRow6> >::Open(m_poleBB->GetSession(),lpszSQL,&propset)))
		if(SUCCEEDED(hr = MoveNext()))
			return true;
	::AppMessage(_T("CImageSet6::OpenRowset failed\n\n") + GetWinErrorMsg(hr) + _T("\n\n") + lpszSQL,MB_OK | MB_ICONWARNING);
	return false;
}
/*
bool CImageSet6::Execute(COleBeliefBase* poleBB, LPCTSTR lpszSQL, ...)
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
		::AppMessage(_T("CImageSet6::Execute failed\n\n") + GetWinErrorMsg(hr) + _T("\n\n") + strTemp,MB_OK | MB_ICONWARNING);
	va_end(argList);
	return bRC;
}
*/

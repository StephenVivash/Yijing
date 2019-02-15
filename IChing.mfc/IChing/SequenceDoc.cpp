#include "StdAfx.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

BEGIN_MESSAGE_MAP(CSequenceDoc, CDocument)
	//{{AFX_MSG_MAP(CSequenceDoc)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

IMPLEMENT_DYNCREATE(CSequenceDoc, CDocument)

CSequenceDoc::CSequenceDoc()
{
}

CSequenceDoc::~CSequenceDoc()
{
}

BOOL CSequenceDoc::OnNewDocument()
{
	if(!CDocument::OnNewDocument())
		return FALSE;
	return TRUE;
}

void CSequenceDoc::Serialize(CArchive& ar)
{
	if(ar.IsStoring())
	{
	}
	else
	{
	}
}

BOOL CSequenceDoc::OnSaveDocument(LPCTSTR lpszPathName) 
{
	return CDocument::OnSaveDocument(lpszPathName);
}

CSequenceView* CSequenceDoc::GetView()
{
	POSITION Pos = GetFirstViewPosition();
	while(Pos)
		return (CSequenceView*) GetNextView(Pos);
	return NULL;
}

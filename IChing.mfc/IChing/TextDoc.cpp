#include "StdAfx.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

BEGIN_MESSAGE_MAP(CTextDoc, CDocument)
	//{{AFX_MSG_MAP(CTextDoc)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

IMPLEMENT_DYNCREATE(CTextDoc, CDocument)

CTextDoc::CTextDoc()
{
}

CTextDoc::~CTextDoc()
{
}

BOOL CTextDoc::OnNewDocument()
{
	if(!CDocument::OnNewDocument())
		return FALSE;
	return TRUE;
}

void CTextDoc::Serialize(CArchive& ar)
{
	if(ar.IsStoring())
	{
	}
	else
	{
	}
}

CTextView* CTextDoc::GetView()
{
	POSITION Pos = GetFirstViewPosition();
	while(Pos)
		return (CTextView*) GetNextView(Pos);
	return NULL;
}

#include "StdAfx.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

BEGIN_MESSAGE_MAP(CLabelDoc, CDocument)
	//{{AFX_MSG_MAP(CLabelDoc)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

IMPLEMENT_DYNCREATE(CLabelDoc, CDocument)

CLabelDoc::CLabelDoc()
{
}

CLabelDoc::~CLabelDoc()
{
}

BOOL CLabelDoc::OnNewDocument()
{
	if(!CDocument::OnNewDocument())
		return FALSE;
	return TRUE;
}

void CLabelDoc::Serialize(CArchive& ar)
{
	if(ar.IsStoring())
	{
	}
	else
	{
	}
}

BOOL CLabelDoc::OnSaveDocument(LPCTSTR lpszPathName) 
{
	return CDocument::OnSaveDocument(lpszPathName);
}

CLabelView* CLabelDoc::GetView()
{
	POSITION Pos = GetFirstViewPosition();
	while(Pos)
		return (CLabelView*) GetNextView(Pos);
	return NULL;
}

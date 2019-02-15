#if !defined(AFX_LABELDOC_H__081D7FFF_1AC9_4753_9ADB_1CE5917E1155__INCLUDED_)
#define AFX_LABELDOC_H__081D7FFF_1AC9_4753_9ADB_1CE5917E1155__INCLUDED_

#pragma once

class CLabelDoc : public CDocument
{
public:
	CLabelDoc();
	virtual ~CLabelDoc();
	CLabelView* GetView();

	//{{AFX_VIRTUAL(CLabelDoc)
	virtual BOOL OnNewDocument();
	virtual void Serialize(CArchive& ar);
	virtual BOOL OnSaveDocument(LPCTSTR lpszPathName);
	//}}AFX_VIRTUAL

protected:
	//{{AFX_MSG(CLabelDoc)
	//}}AFX_MSG

	DECLARE_MESSAGE_MAP()
	DECLARE_DYNCREATE(CLabelDoc)
};

//{{AFX_INSERT_LOCATION}}

#endif

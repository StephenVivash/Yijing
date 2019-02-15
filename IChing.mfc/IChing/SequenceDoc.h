#if !defined(AFX_SEQUENCEDOC_H__081D7FFF_1AC9_4753_9ADB_1CE5917E1155__INCLUDED_)
#define AFX_SEQUENCEDOC_H__081D7FFF_1AC9_4753_9ADB_1CE5917E1155__INCLUDED_

#pragma once

class CSequenceDoc : public CDocument
{
public:
	CSequenceDoc();
	virtual ~CSequenceDoc();
	CSequenceView* GetView();

	//{{AFX_VIRTUAL(CSequenceDoc)
	virtual BOOL OnNewDocument();
	virtual void Serialize(CArchive& ar);
	virtual BOOL OnSaveDocument(LPCTSTR lpszPathName);
	//}}AFX_VIRTUAL

protected:
	//{{AFX_MSG(CSequenceDoc)
	//}}AFX_MSG

	DECLARE_MESSAGE_MAP()
	DECLARE_DYNCREATE(CSequenceDoc)
};

//{{AFX_INSERT_LOCATION}}

#endif

#if !defined(AFX_TEXTDOC_H__F77EC682_4C82_4271_88FA_7E450BF00DA2__INCLUDED_)
#define AFX_TEXTDOC_H__F77EC682_4C82_4271_88FA_7E450BF00DA2__INCLUDED_

#pragma once

class CTextDoc : public CDocument
{
public:
	CTextDoc();
	virtual ~CTextDoc();
	CTextView* GetView();

	//{{AFX_VIRTUAL(CTextDoc)
	virtual BOOL OnNewDocument();
	virtual void Serialize(CArchive& ar);
	//}}AFX_VIRTUAL

protected:
	//{{AFX_MSG(CTextDoc)
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
	DECLARE_DYNCREATE(CTextDoc)
};

//{{AFX_INSERT_LOCATION}}

#endif

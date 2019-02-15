#if !defined(AFX_TEXTVIEW_H__E6FF6158_9204_4BD9_9D22_3C018873847D__INCLUDED_)
#define AFX_TEXTVIEW_H__E6FF6158_9204_4BD9_9D22_3C018873847D__INCLUDED_

#pragma once

class CTextView : public CHtmlView
{
public:
	CTextView();
	virtual ~CTextView();
	CTextDoc* GetDocument() {return (CTextDoc*)m_pDocument;}
	void Update();
	void Navigate(LPCTSTR lpszUrl);

	//{{AFX_VIRTUAL(CTextView)
	virtual void OnDraw(CDC* pDC);
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	virtual void OnInitialUpdate();
	//}}AFX_VIRTUAL

protected:
	//{{AFX_MSG(CTextView)
	//}}AFX_MSG

	DECLARE_MESSAGE_MAP()
	DECLARE_DYNCREATE(CTextView)
};

//{{AFX_INSERT_LOCATION}}

#endif

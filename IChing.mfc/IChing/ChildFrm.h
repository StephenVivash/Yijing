#pragma once

class CChildFrame : public CMDIChildWnd
{
public:
	CChildFrame();
	virtual ~CChildFrame();

	virtual void ActivateFrame(int nCmdShow);
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);

protected:

	afx_msg void OnClose();

	DECLARE_MESSAGE_MAP()
	DECLARE_DYNCREATE(CChildFrame)
};

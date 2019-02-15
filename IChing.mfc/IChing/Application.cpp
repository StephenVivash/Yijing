#include "StdAfx.h"
#include "MainFrm.h"
#include "Application.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

CApplication::CApplication()
{
	CComBSTR bstrTemp(_T("IChing.ValueSequencer"));
	if(m_spIValueSequencer.CoCreateInstance(bstrTemp,NULL,CLSCTX_LOCAL_SERVER) == S_OK)
	{
	}
}

CApplication::~CApplication()
{
}

HRESULT CApplication::get_Application(IApplication** ppIApplication) 
{
	AddRef();
	*ppIApplication = dynamic_cast<IApplication*>(this);
	return S_OK;
}

HRESULT CApplication::get_Parent(IDispatch** ppIDispatch) 
{
	AddRef();
	*ppIDispatch = dynamic_cast<IDispatch*>(this);
	return S_OK;
}

HRESULT CApplication::get_Name(BSTR* pbstrName)
{
	*pbstrName = ::GetAppName().AllocSysString();
	return S_OK;
}

HRESULT CApplication::get_Hexagram(IValueSequencer** ppIValueSequencer)
{
	m_spIValueSequencer.p->AddRef();
	*ppIValueSequencer = m_spIValueSequencer;
	return S_OK;
}

HRESULT CApplication::Quit(void)
{
	return S_OK;
}

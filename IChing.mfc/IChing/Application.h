#ifndef __APPLICATION_H_
#define __APPLICATION_H_

#pragma once

class ATL_NO_VTABLE CApplication : 
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CApplication, &CLSID_Application>,
//	public IConnectionPointContainerImpl<CApplication>,
	public IDispatchImpl<IApplication, &IID_IApplication, &LIBID_IChingLib>
{
public:
	CApplication();
	virtual ~CApplication();

    HRESULT __stdcall get_Application(IApplication** ppIApplication);
    HRESULT __stdcall get_Parent(IDispatch** ppIDispatch);
    HRESULT __stdcall get_Name(BSTR* pbstrName);
	HRESULT __stdcall get_Hexagram(IValueSequencer** ppIValueSequencer);
    HRESULT __stdcall Quit(void);

	DECLARE_REGISTRY_RESOURCEID(IDR_APPLICATION)
//	DECLARE_NOT_AGGREGATABLE(CApplication)
//	DECLARE_PROTECT_FINAL_CONSTRUCT()

	BEGIN_COM_MAP(CApplication)
		COM_INTERFACE_ENTRY(IApplication)
		COM_INTERFACE_ENTRY(IDispatch)
//		COM_INTERFACE_ENTRY(IConnectionPointContainer)
	END_COM_MAP()

//	BEGIN_CONNECTION_POINT_MAP(CApplication)
//	END_CONNECTION_POINT_MAP()

protected:
    CComPtr<IValueSequencer> m_spIValueSequencer;
};

#endif

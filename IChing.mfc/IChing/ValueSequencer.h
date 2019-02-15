#if !defined(AFX_ICHING_H__C9B8108C_0E89_11D6_894C_FE7D3B55052B__INCLUDED_)
#define AFX_ICHING_H__C9B8108C_0E89_11D6_894C_FE7D3B55052B__INCLUDED_

#pragma once

class CValueSequencerCtrl;

class CValueSequencerPane
{
public:
	virtual void Update() = 0;
	virtual void SetCurrentSequencerCtrl(CValueSequencerCtrl* pvscCurrentSequencer) = 0;
};

class CValueSequencer
{
public:
	CValueSequencer(int nInnerSequencers = 0, int nValues = 0, int nValue = 0);
	CValueSequencer(const CValueSequencer& rhs) {*this = rhs;}
	virtual ~CValueSequencer();

	CValueSequencer& operator[](int nIndex) const;
	CValueSequencer& operator=(int nValue);
	CValueSequencer& operator=(const CValueSequencer& rhs);
	bool operator==(int nValue) const;
	bool operator==(const CValueSequencer& rhs);
	CValueSequencer& operator++();
	CValueSequencer operator++(int);
	CValueSequencer& operator--();
	CValueSequencer operator--(int);

	virtual void operator>>(CValueSequencer& rhs) const;
	virtual void UpdateInnerValues() {}
	virtual void UpdateOuterValues() {}

	void First();
	void Last();
	void Inverse(void);
	void Opposite(void);
	void Transverse(void);
	void Nuclear(void);
	void Secondary(void);

	bool SetEntryArrays(int nIndex);
	void SetParent(CValueSequencer* pvsParent) {m_pvsParent = pvsParent;}
	CValueSequencer* GetParent() const {return m_pvsParent;}
	void SetValue(int nValue) {m_nValue = nValue;}
	int GetValue() const {return m_nValue;}
	void SetSequence(int nSequence) {m_nSequence = nSequence;}
	int GetSequence() const {return m_nSequence;}
	int GetNoValues() {return m_nValues;}
	int GetNoInnerSequencers() {return m_nInnerSequencers;}
	LPCTSTR GetLabel() const {return GetLabel(m_nValue);}
	LPCTSTR GetLabel(int nValue) const {return m_prgLabelEntry->GetAt(nValue)->GetLabel();}
	HBITMAP GetBitmap() const {return GetBitmap(m_nValue);}
	HBITMAP GetBitmap(int nValue) const {return m_prgImageEntry->GetAt(nValue)->GetBitmap();}
	CLabelEntryArray* GetLabelEntry() {return m_prgLabelEntry;}
	CSequenceEntryArray* GetSequenceEntry() {return m_prgSequenceEntry;}
	CImageEntryArray* GetImageEntry() {return m_prgImageEntry;}

protected:
	int m_nInnerSequencers;
	int m_nValues;
	int m_nValue;
	int m_nSequence;

	CLabelEntryArray* m_prgLabelEntry;
	CSequenceEntryArray* m_prgSequenceEntry;
	CImageEntryArray* m_prgImageEntry;
	CValueSequencer* m_pvsParent;
	CValueSequencer* m_pvsInner;
};

class CBitValueSequencer : public CValueSequencer
{
public:
	CBitValueSequencer(int nValue = 0);
	CBitValueSequencer& operator=(int nValue);
	CBitValueSequencer& operator=(const CBitValueSequencer& rhs);
	virtual void operator>>(CBitValueSequencer& rhs) const;
	virtual void UpdateInnerValues();
	virtual void UpdateOuterValues();
};

class CLineValueSequencer : public CValueSequencer
{
public:
	CLineValueSequencer(int nValue = 0);
	CBitValueSequencer& operator[](int nIndex) const;
	CLineValueSequencer& operator=(int nValue);
	CLineValueSequencer& operator=(const CLineValueSequencer& rhs);
	virtual void operator>>(CLineValueSequencer& rhs) const;
	virtual void UpdateInnerValues();
	virtual void UpdateOuterValues();
};

class CTrigramValueSequencer : public CValueSequencer
{
public:
	CTrigramValueSequencer(int nValue = 0);
	CLineValueSequencer& operator[](int nIndex) const;
	CTrigramValueSequencer& operator=(int nValue);
	CTrigramValueSequencer& operator=(const CTrigramValueSequencer& rhs);
	virtual void operator>>(CTrigramValueSequencer& rhs) const;
	virtual void UpdateInnerValues();
	virtual void UpdateOuterValues();
};

class CHexagramValueSequencer : public CValueSequencer
{
public:
	CHexagramValueSequencer(int nValue = 0);
	CTrigramValueSequencer& operator[](int nIndex) const;
	CHexagramValueSequencer& operator=(int nValue);
	CHexagramValueSequencer& operator=(const CHexagramValueSequencer& rhs);
	virtual void operator>>(CHexagramValueSequencer& rhs) const;
	virtual void UpdateInnerValues();
	virtual void UpdateOuterValues();
};

enum Direction {eFirst,ePrevious,eNext,eLast};

class CValueSequencerCtrl : public CButton
{
public:
	CValueSequencerCtrl(void);
	virtual ~CValueSequencerCtrl(void) {}
	BOOL Create(DWORD dwStyle, const RECT& rect, CWnd* pParentWnd, UINT nCtrlId, 
		CValueSequencer* pvsValue, bool bImageButton = false);
	BOOL Initialise(CWnd *pwndParent, UINT nCtrlId, CValueSequencer* pvsValue, 
		bool bImageButton = false);
	CValueSequencer* GetValueSequencer() {return m_pvsValue;}

	void SetDirection(Direction eDirection) {m_eDirection = eDirection;}
	void SetMode(bool bImageButton = false);
	void SetMoveable(bool bMoveable) {m_bMoveable = bMoveable;}
	void Move();
	void Show();
	void Update();

	afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnLButtonUp(UINT nFlags, CPoint point);

	afx_msg void OnContextFirst(void);
	afx_msg void OnContextPrevious(void);
	afx_msg void OnContextNext(void);
	afx_msg void OnContextLast(void);
	afx_msg void OnContextInverse(void);
	afx_msg void OnContextOpposite(void);
	afx_msg void OnContextTransverse(void);
	afx_msg void OnContextNuclear(void);
	afx_msg void OnContextSecondary(void);
	afx_msg void OnContextSpecific(UINT nId);
	afx_msg void OnContextProperties(void);

protected:
	BOOL m_bSubClass;

	void DrawItem(LPDRAWITEMSTRUCT lpDrawItemStruct);
	afx_msg HBRUSH CtlColor(CDC* pDC, UINT nCtlColor); 
	afx_msg void OnRButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnRButtonUp(UINT nFlags, CPoint point);
	afx_msg BOOL OnSetCursor(CWnd* pWnd, UINT nHitTest, UINT message);

	UINT m_nCtrlId;
	bool m_bMoveable;
	bool m_bImageButton;
	bool m_bLButtonDown;
	bool m_bRButtonDown;
	int m_nShowCursor;
	HCURSOR m_hFirst;
	HCURSOR m_hPrevious;
	HCURSOR m_hNext;
	HCURSOR m_hLast;
	Direction m_eDirection;
	CValueSequencer* m_pvsValue;

	CValueSequencerPane* m_pvsPane;

	COLORREF m_clrText;
	COLORREF m_clrBackground;
	CBrush m_brBackground;

	DECLARE_MESSAGE_MAP()
};

class ATL_NO_VTABLE CComValueSequencer : 
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CComValueSequencer, &CLSID_ValueSequencer>,
//	public IConnectionPointContainerImpl<CComValueSequencer>,
//	public IObjectSafetyImpl<CComValueSequencer, INTERFACESAFE_FOR_UNTRUSTED_CALLER>,
	public IDispatchImpl<IValueSequencer, &IID_IValueSequencer, &LIBID_IChingLib>
{
public:
	CComValueSequencer();
	virtual ~CComValueSequencer();

	HRESULT __stdcall get_Application(IApplication** ppIApplication);
	HRESULT __stdcall get_Parent(IDispatch** ppIDispatch);
	HRESULT __stdcall get_Name(BSTR* pbstrName);
	HRESULT __stdcall get_Value(int* pnValue);
	HRESULT __stdcall put_Value(int nValue);
	HRESULT __stdcall get_Text(BSTR bstrType, BSTR* pbstrText);
	HRESULT __stdcall get_IsMovingLine(int nLine, VARIANT_BOOL* pbValue);
	HRESULT __stdcall get_HasText(BSTR bstrType, VARIANT_BOOL* pbValue);
	HRESULT __stdcall get_SequenceName(BSTR* pbstrName);
	HRESULT __stdcall get_SecondaryName(int nLine, BSTR* pbstrName);

	DECLARE_REGISTRY_RESOURCEID(IDR_VALUESEQUENCER)
//	DECLARE_NOT_AGGREGATABLE(CComValueSequencer)
//	DECLARE_PROTECT_FINAL_CONSTRUCT()

	BEGIN_COM_MAP(CComValueSequencer)
		COM_INTERFACE_ENTRY(IValueSequencer)
		COM_INTERFACE_ENTRY(IDispatch)
//		COM_INTERFACE_ENTRY(IObjectSafety)
//		COM_INTERFACE_ENTRY(IConnectionPointContainer)
	END_COM_MAP()

//	BEGIN_CONNECTION_POINT_MAP(CComValueSequencer)
//	END_CONNECTION_POINT_MAP()
};

HRESULT CreateComponentCategory(CATID catid, BSTR bstrDescription);
HRESULT RegisterCLSIDInCategory(REFCLSID clsid, CATID catid);
HRESULT UnRegisterCLSIDInCategory(REFCLSID clsid, CATID catid);
HRESULT RegisterControl();
HRESULT UnRegisterControl();

#endif

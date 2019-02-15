#include "StdAfx.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

CValueSequencer::CValueSequencer(int nInnerSequencers, int nNoValues, int nValue) 
{
	ASSERT((nNoValues == 2) || (nNoValues == 4) || (nNoValues == 8) || (nNoValues == 64));
	m_nInnerSequencers = nInnerSequencers;
	m_nValues = nNoValues;
	m_pvsParent = NULL;
	m_pvsInner = NULL;
	SetEntryArrays(0);
	*this = nValue;
}

CValueSequencer::~CValueSequencer()
{
	delete [] m_pvsInner;
}

CValueSequencer& CValueSequencer::operator[](int nIndex) const
{
	return m_pvsInner[nIndex];
}

CValueSequencer& CValueSequencer::operator=(int nValue)
{
	m_nValue = nValue;
	m_nSequence = m_prgSequenceEntry->GetAt(m_nValue)->GetSequence();
	return *this;
}

CValueSequencer& CValueSequencer::operator=(const CValueSequencer& rhs) 
{
	*this = rhs.m_nValue;
	return *this;
}

bool CValueSequencer::operator==(int nValue) const
{
	return m_nValue == nValue;
}

bool CValueSequencer::operator==(const CValueSequencer& rhs)
{
	return m_nValue == rhs.m_nValue;
}

void CValueSequencer::operator>>(CValueSequencer& rhs) const
{
	rhs = *this;
}

CValueSequencer& CValueSequencer::operator++()
{
	*this = m_prgSequenceEntry->GetNextSequence(m_nSequence);
	UpdateInnerValues();
	UpdateOuterValues();
	return *this;
}

CValueSequencer CValueSequencer::operator++(int)
{
	CValueSequencer vs = *this;
	++*this;
	return vs;
}

CValueSequencer& CValueSequencer::operator--()
{
	*this = m_prgSequenceEntry->GetPreviousSequence(m_nSequence);
	UpdateInnerValues();
	UpdateOuterValues();
	return *this;
}

CValueSequencer CValueSequencer::operator--(int)
{
	CValueSequencer vs = *this;
	--*this;
	return vs;
}

void CValueSequencer::First() 
{
	*this = m_prgSequenceEntry->GetFirstSequence();
	UpdateInnerValues();
	UpdateOuterValues();
}

void CValueSequencer::Last() 
{
	*this = m_prgSequenceEntry->GetLastSequence();
	UpdateInnerValues();
	UpdateOuterValues();
}

void CValueSequencer::Inverse(void)
{
	switch(m_nValues)
	{
	case 2:
//		*this = ~m_nValue & 1;
		break;
	case 4:
		switch(m_nValue)
		{
		case 0:
			*this = 1;
			break;
		case 1:
			*this = 0;
			break;
		case 2:
			*this = 3;
			break;
		case 3:
			*this = 2;
			break;
		}
		break;
	case 8:
		*this = (m_pvsInner[2].GetValue() % 2 == 0 ? 0 : 1) + 
			   ((m_pvsInner[1].GetValue() % 2 == 0 ? 0 : 1) * 2) + 
			   ((m_pvsInner[0].GetValue() % 2 == 0 ? 0 : 1) * 4);
		break;
	case 64:
		*this = ((m_pvsInner[1][2].GetValue() % 2 == 0 ? 0 : 1) + 
			    ((m_pvsInner[1][1].GetValue() % 2 == 0 ? 0 : 1) * 2) + 
			    ((m_pvsInner[1][0].GetValue() % 2 == 0 ? 0 : 1) * 4)) +
			  ((((m_pvsInner[0][2].GetValue() % 2 == 0 ? 0 : 1) + 
			    ((m_pvsInner[0][1].GetValue() % 2 == 0 ? 0 : 1) * 2) + 
			    ((m_pvsInner[0][0].GetValue() % 2 == 0 ? 0 : 1) * 4))) * 8);
		break;
	}
	UpdateInnerValues();
	UpdateOuterValues();
}

void CValueSequencer::Opposite(void)
{
	*this = ~m_nValue & (m_nValues - 1);
	UpdateInnerValues();
	UpdateOuterValues();
}

void CValueSequencer::Transverse(void)
{
	int nTemp;
	if(m_nValues == 64)
	{
		nTemp = m_pvsInner[0].GetValue();
		m_pvsInner[0] = m_pvsInner[1].GetValue();
		m_pvsInner[1] = nTemp;
		*this = m_pvsInner[0].GetValue() + m_pvsInner[1].GetValue() * 8;
	}
	UpdateInnerValues();
	UpdateOuterValues();
}

void CValueSequencer::Nuclear(void)
{
	int nTemp;
	if(m_nValues == 64)
	{
		nTemp = m_pvsInner[1][0].GetValue();
		m_pvsInner[1][2] = m_pvsInner[1][1].GetValue();
		m_pvsInner[1][1] = m_pvsInner[1][0].GetValue();
		m_pvsInner[1][0] = m_pvsInner[0][2].GetValue();
		m_pvsInner[0][0] = m_pvsInner[0][1].GetValue();
		m_pvsInner[0][1] = m_pvsInner[0][2].GetValue();
		m_pvsInner[0][2] = nTemp;
		*this = ((((m_pvsInner[1][2].GetValue() % 2 == 0 ? 0 : 1) * 4) + 
			((m_pvsInner[1][1].GetValue() % 2 == 0 ? 0 : 1) * 2) + 
			(m_pvsInner[1][0].GetValue() % 2 == 0 ? 0 : 1)) * 8) +
			((m_pvsInner[0][2].GetValue() % 2 == 0 ? 0 : 1) * 4) + 
			((m_pvsInner[0][1].GetValue() % 2 == 0 ? 0 : 1) * 2) + 
			(m_pvsInner[0][0].GetValue() % 2 == 0 ? 0 : 1);
	}
	UpdateInnerValues();
	UpdateOuterValues();
}

bool CValueSequencer::SetEntryArrays(int nIndex)
{
	int nLabelIndex = nIndex;
	int SequencenIndex = nIndex;
	CLabelBlock* pLB = NULL;
	CSequenceBlock* pSB = NULL;
	CImageBlock* pIB = NULL;
	switch(m_nValues)
	{
	case 2:
		if(nLabelIndex == -1)
			nLabelIndex = ::GetPropertyCtrl()->GetBitLabelIndex();
		pLB = ::GetApp()->GetLabelBlock1()->GetAt(nLabelIndex);
		if(SequencenIndex == -1)
			SequencenIndex = ::GetPropertyCtrl()->GetBitSequenceIndex();
		pSB = ::GetApp()->GetSequenceBlock1()->GetAt(SequencenIndex);
		pIB = ::GetApp()->GetImageBlock1()->GetAt(0);
		break;
	case 4:
		if(nLabelIndex == -1)
			nLabelIndex = ::GetPropertyCtrl()->GetLineLabelIndex();
		pLB = ::GetApp()->GetLabelBlock2()->GetAt(nLabelIndex);
		if(SequencenIndex == -1)
			SequencenIndex = ::GetPropertyCtrl()->GetLineSequenceIndex();
		pSB = ::GetApp()->GetSequenceBlock2()->GetAt(SequencenIndex);
		pIB = ::GetApp()->GetImageBlock2()->GetAt(0);
		break;
	case 8:
		if(nLabelIndex == -1)
			nLabelIndex = ::GetPropertyCtrl()->GetTrigramLabelIndex();
		pLB = ::GetApp()->GetLabelBlock3()->GetAt(nLabelIndex);
		if(SequencenIndex == -1)
			SequencenIndex = ::GetPropertyCtrl()->GetTrigramSequenceIndex();
		pSB = ::GetApp()->GetSequenceBlock3()->GetAt(SequencenIndex);
		pIB = ::GetApp()->GetImageBlock3()->GetAt(0);
		break;
	case 64:
		if(nLabelIndex == -1)
			nLabelIndex = ::GetPropertyCtrl()->GetHexagramLabelIndex();
		pLB = ::GetApp()->GetLabelBlock6()->GetAt(nLabelIndex);
		if(SequencenIndex == -1)
			SequencenIndex = ::GetPropertyCtrl()->GetHexagramSequenceIndex();
		pSB = ::GetApp()->GetSequenceBlock6()->GetAt(SequencenIndex);
		pIB = ::GetApp()->GetImageBlock6()->GetAt(0);
		break;
	}
	m_prgLabelEntry = pLB->GetEntryArray();
	m_prgSequenceEntry = pSB->GetEntryArray();
	m_prgImageEntry = pIB->GetEntryArray();
	return true;
}

/////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////

CBitValueSequencer::CBitValueSequencer(int nValue) : CValueSequencer(0,2,nValue)
{
}

CBitValueSequencer& CBitValueSequencer::operator=(int nValue) 
{
	CValueSequencer::operator=(nValue);
//	UpdateInnerValues();
	UpdateOuterValues();
	return *this;
}

CBitValueSequencer& CBitValueSequencer::operator=(const CBitValueSequencer& rhs) 
{
	CBitValueSequencer::operator=(rhs.GetValue());
	return *this;
}

void CBitValueSequencer::operator>>(CBitValueSequencer& rhs) const
{
	rhs = *this;
}

void CBitValueSequencer::UpdateInnerValues()
{
}

void CBitValueSequencer::UpdateOuterValues()
{
	m_pvsParent->UpdateOuterValues();
}

/////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////

CLineValueSequencer::CLineValueSequencer(int nValue) : CValueSequencer(3,4,nValue)
{
	m_pvsInner = new CBitValueSequencer[3];
	m_pvsInner[2].SetParent(this);
	m_pvsInner[1].SetParent(this);
	m_pvsInner[0].SetParent(this);
}

CBitValueSequencer& CLineValueSequencer::operator[](int nIndex) const
{
	return (CBitValueSequencer&) m_pvsInner[nIndex];
}

CLineValueSequencer& CLineValueSequencer::operator=(int nValue) 
{
	CValueSequencer::operator=(nValue);
	UpdateInnerValues();
	UpdateOuterValues();
	return *this;
}

CLineValueSequencer& CLineValueSequencer::operator=(const CLineValueSequencer& rhs) 
{
	CLineValueSequencer::operator=(rhs.GetValue());
	return *this;
}

void CLineValueSequencer::operator>>(CLineValueSequencer& rhs) const
{
	rhs = *this;
	m_pvsInner[2] >> rhs.m_pvsInner[2];
	m_pvsInner[1] >> rhs.m_pvsInner[1];
	m_pvsInner[0] >> rhs.m_pvsInner[0];
}

void CLineValueSequencer::UpdateInnerValues()
{
	switch(m_nValue)
	{
	case 0:
		m_pvsInner[2] = 0;
		m_pvsInner[1] = 0;
		m_pvsInner[0] = 0;
		break;
	case 1:
		m_pvsInner[2] = 0;
		m_pvsInner[1] = 1;
		m_pvsInner[0] = 0;
		break;
	case 2:
		m_pvsInner[2] = 1;
		m_pvsInner[1] = 0;
		m_pvsInner[0] = 1;
		break;
	case 3:
		m_pvsInner[2] = 1;
		m_pvsInner[1] = 1;
		m_pvsInner[0] = 1;
		break;
	}
	m_pvsInner[2].UpdateInnerValues();
	m_pvsInner[1].UpdateInnerValues();
	m_pvsInner[0].UpdateInnerValues();
}

void CLineValueSequencer::UpdateOuterValues()
{
	CValueSequencer::operator=(m_pvsInner[0].GetValue() + m_pvsInner[1].GetValue() + 
		m_pvsInner[2].GetValue());
	m_pvsParent->UpdateOuterValues();
}

/////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////

CTrigramValueSequencer::CTrigramValueSequencer(int nValue) : CValueSequencer(3,8,nValue)
{
	m_pvsInner = new CLineValueSequencer[3];
	m_pvsInner[2].SetParent(this);
	m_pvsInner[1].SetParent(this);
	m_pvsInner[0].SetParent(this);
}

CLineValueSequencer& CTrigramValueSequencer::operator[](int nIndex) const
{
	return (CLineValueSequencer&) m_pvsInner[nIndex];
}

CTrigramValueSequencer& CTrigramValueSequencer::operator=(int nValue) 
{
	CValueSequencer::operator=(nValue);
	UpdateInnerValues();
	UpdateOuterValues();
	return *this;
}

CTrigramValueSequencer& CTrigramValueSequencer::operator=(const CTrigramValueSequencer& rhs) 
{
	CTrigramValueSequencer::operator=(rhs.GetValue());
	return *this;
}

void CTrigramValueSequencer::operator>>(CTrigramValueSequencer& rhs) const
{
	rhs = *this;
	m_pvsInner[2] >> rhs.m_pvsInner[2];
	m_pvsInner[1] >> rhs.m_pvsInner[1];
	m_pvsInner[0] >> rhs.m_pvsInner[0];
}

int m_nYinLine = 2;
int m_nYangLine = 1;

void CTrigramValueSequencer::UpdateInnerValues()
{
	switch(m_nValue)
	{
	case 0:
		m_pvsInner[2] = m_nYinLine;
		m_pvsInner[1] = m_nYinLine;
		m_pvsInner[0] = m_nYinLine;
		break;
	case 1:
		m_pvsInner[2] = m_nYinLine;
		m_pvsInner[1] = m_nYinLine;
		m_pvsInner[0] = m_nYangLine;
		break;
	case 2:
		m_pvsInner[2] = m_nYinLine;
		m_pvsInner[1] = m_nYangLine;
		m_pvsInner[0] = m_nYinLine;
		break;
	case 3:
		m_pvsInner[2] = m_nYinLine;
		m_pvsInner[1] = m_nYangLine;
		m_pvsInner[0] = m_nYangLine;
		break;
	case 4:
		m_pvsInner[2] = m_nYangLine;
		m_pvsInner[1] = m_nYinLine;
		m_pvsInner[0] = m_nYinLine;
		break;
	case 5:
		m_pvsInner[2] = m_nYangLine;
		m_pvsInner[1] = m_nYinLine;
		m_pvsInner[0] = m_nYangLine;
		break;
	case 6:
		m_pvsInner[2] = m_nYangLine;
		m_pvsInner[1] = m_nYangLine;
		m_pvsInner[0] = m_nYinLine;
		break;
	case 7:
		m_pvsInner[2] = m_nYangLine;
		m_pvsInner[1] = m_nYangLine;
		m_pvsInner[0] = m_nYangLine;
		break;
	}
	m_pvsInner[2].UpdateInnerValues();
	m_pvsInner[1].UpdateInnerValues();
	m_pvsInner[0].UpdateInnerValues();
}

void CTrigramValueSequencer::UpdateOuterValues()
{
	CValueSequencer::operator=((m_pvsInner[0].GetValue() % 2 == 0 ? 0 : 1) + 
		((m_pvsInner[1].GetValue() % 2 == 0 ? 0 : 1) * 2) + 
		((m_pvsInner[2].GetValue() % 2 == 0 ? 0 : 1) * 4));
	m_pvsParent->UpdateOuterValues();
}

/////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////

CHexagramValueSequencer::CHexagramValueSequencer(int nValue) : CValueSequencer(2,64,nValue)
{
	m_pvsInner = new CTrigramValueSequencer[2];
	m_pvsInner[1].SetParent(this);
	m_pvsInner[0].SetParent(this);
}

CTrigramValueSequencer& CHexagramValueSequencer::operator[](int nIndex) const
{
	return (CTrigramValueSequencer&) m_pvsInner[nIndex];
}

CHexagramValueSequencer& CHexagramValueSequencer::operator=(int nValue) 
{
	CValueSequencer::operator=(nValue);
	UpdateInnerValues();
//	UpdateOuterValues();
	return *this;
}

CHexagramValueSequencer& CHexagramValueSequencer::operator=(const CHexagramValueSequencer& rhs) 
{
	CHexagramValueSequencer::operator=(rhs.GetValue());
	return *this;
}

void CHexagramValueSequencer::operator>>(CHexagramValueSequencer& rhs) const
{
	rhs = *this;
	m_pvsInner[1] >> rhs.m_pvsInner[1];
	m_pvsInner[0] >> rhs.m_pvsInner[0];
}

void CHexagramValueSequencer::UpdateInnerValues()
{
	m_pvsInner[1] = m_nValue / 8;
	m_pvsInner[0] = m_nValue % 8;
	m_pvsInner[1].UpdateInnerValues();
	m_pvsInner[0].UpdateInnerValues();
}

void CHexagramValueSequencer::UpdateOuterValues()
{
	CValueSequencer::operator=(m_pvsInner[0].GetValue() + 
		(m_pvsInner[1].GetValue() * 8));
}

////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////

BEGIN_MESSAGE_MAP(CValueSequencerCtrl, CButton)
	ON_WM_LBUTTONDOWN()
	ON_WM_LBUTTONUP()
	ON_WM_RBUTTONDOWN()
	ON_WM_RBUTTONUP()
	ON_WM_SETCURSOR()

	ON_COMMAND(ID_VSCTX_FIRST,OnContextFirst)
	ON_COMMAND(ID_VSCTX_PREVIOUS,OnContextPrevious)
	ON_COMMAND(ID_VSCTX_NEXT,OnContextNext)
	ON_COMMAND(ID_VSCTX_LAST,OnContextLast)
	ON_COMMAND(ID_VSCTX_INVERSE,OnContextInverse)
	ON_COMMAND(ID_VSCTX_OPPOSITE,OnContextOpposite)
	ON_COMMAND(ID_VSCTX_TRANSVERSE,OnContextTransverse)
	ON_COMMAND(ID_VSCTX_NUCLEAR,OnContextNuclear)
	ON_COMMAND(ID_VSCTX_SECONDARY,OnContextSecondary)
	ON_COMMAND_RANGE(ID_VSCTX_SPECIFIC,ID_VSCTX_SPECIFIC + 63,OnContextSpecific)
	ON_COMMAND(ID_VSCTX_PROPERTIES,OnContextProperties)
END_MESSAGE_MAP()

CValueSequencerCtrl::CValueSequencerCtrl(void)
{
	m_bMoveable = true;
	m_bLButtonDown = false;
	m_bRButtonDown = false;
	m_nShowCursor = 0;
	m_eDirection = eNext;
	m_clrText = RGB(0,0,0xFF);
	m_clrBackground = RGB(0xFF,0xFF,0);
	m_brBackground.CreateSolidBrush(m_clrBackground);
}

BOOL CValueSequencerCtrl::Create(DWORD dwStyle, const RECT& rect, CWnd* pParentWnd, 
	UINT nCtrlId, CValueSequencer* pvsValue, bool bImageButton)
{
	m_bSubClass = FALSE;
	m_nCtrlId = nCtrlId;
	m_pvsPane = dynamic_cast<CValueSequencerPane*>(pParentWnd);
	if(CButton::Create(_T(""),dwStyle,rect,pParentWnd,nCtrlId))
		return Initialise(pParentWnd,nCtrlId,pvsValue,bImageButton);
	return FALSE;
}

BOOL CValueSequencerCtrl::Initialise(CWnd *pwndParent, UINT nCtrlId, 
	CValueSequencer* pvsValue, bool bImageButton)
{
	m_nCtrlId = nCtrlId;
	m_pvsValue = pvsValue;
	m_bImageButton = bImageButton;
	if(m_bSubClass)
		if(!SubclassDlgItem(nCtrlId,pwndParent))
		{
			::AppMessage(_T("Failed to sub class CValueSequencerCtrl."),MB_OK | MB_ICONSTOP,::GetMainFrame());
			return FALSE;
		}
	m_hFirst = ::GetApp()->LoadCursor(IDR_FIRST);
	m_hPrevious = ::GetApp()->LoadCursor(IDR_PREVIOUS);
	m_hNext = ::GetApp()->LoadCursor(IDR_NEXT);
	m_hLast = ::GetApp()->LoadCursor(IDR_LAST);
	Show();
	return TRUE;
}

void CValueSequencerCtrl::SetMode(bool bImageButton)
{
	m_bImageButton = bImageButton;
	if(m_bImageButton)
		ModifyStyle(BS_OWNERDRAW,BS_BITMAP,0);
	else
		ModifyStyle((!::GetPropertyCtrl()->IsBitImages() ? BS_OWNERDRAW : 0) | BS_BITMAP,BS_OWNERDRAW,0);
}

void CValueSequencerCtrl::DrawItem(LPDRAWITEMSTRUCT lpDrawItemStruct) 
{
	BYTE c[2][3] = {{0,0,0},{0,0,0}};
	COLORREF crDiagram = RGB(0,0,0);
	COLORREF crBackground = (COLORREF) GetSysColor(COLOR_BTNFACE);
	CString strTemp;
	strTemp.Format(_T("%d. %s (%d)"),m_pvsValue->GetSequence() + 1,m_pvsValue->GetLabel(),m_pvsValue->GetValue());
	if(m_pvsValue->GetNoValues() == 2)
		crDiagram = RGB(m_pvsValue->GetValue() == 1 ? 0xFF : 0,m_pvsValue->GetValue() == 1 ? 0xFF : 0,
			m_pvsValue->GetValue() == 1 ? 0xFF : 0);
	else
	if(m_pvsValue->GetNoValues() == 4)
		if(::GetPropertyCtrl()->GetLineDisplay() == _T("Black"))
			crDiagram = RGB(0,0,0);
		else
			switch(m_pvsValue->GetValue())
			{
			case 0:
				crDiagram = RGB(0,0,0);
				break;
			case 1:
				crDiagram = RGB(0xAA,0xAA,0xAA);
				break;
			case 2:
				crDiagram = RGB(0x55,0x55,0x55);
				break;
			case 3:
				crDiagram = RGB(0xFF,0xFF,0xFF);
				break;
			}
	else
	if(m_pvsValue->GetNoValues() == 8)
		if(::GetPropertyCtrl()->GetTrigramDisplay() == _T("Black"))
			crDiagram = RGB(0,0,0);
		else
		{
			c[0][0] = ((*m_pvsValue)[0].GetValue() % 2) == 1 ? 0xFF : 0;
			c[0][1] = ((*m_pvsValue)[1].GetValue() % 2) == 1 ? 0xFF : 0;
			c[0][2] = ((*m_pvsValue)[2].GetValue() % 2) == 1 ? 0xFF : 0;
			crDiagram = RGB(c[0][0],c[0][1],c[0][2]);
		}
	else
	if(m_pvsValue->GetNoValues() == 64)
		if(::GetPropertyCtrl()->GetHexagramDisplay() == _T("Black"))
			crDiagram = RGB(0,0,0);
		else
		{
			c[0][0] = ((*m_pvsValue)[0][0].GetValue() % 2) == 1 ? 1 : 0;
			c[1][0] = ((*m_pvsValue)[1][0].GetValue() % 2) == 1 ? 1 : 0;
			if((c[0][0] == 0) && (c[1][0] == 0))
				c[0][0] = 0x00;
			else
			if((c[0][0] == 1) && (c[1][0] == 0))
				c[0][0] = 0x55;
			else
			if((c[0][0] == 0) && (c[1][0] == 1))
				c[0][0] = 0xAA;
			else
			if((c[0][0] == 1) && (c[1][0] == 1))
				c[0][0] = 0xFF;

			c[0][1] = ((*m_pvsValue)[0][1].GetValue() % 2) == 1 ? 1 : 0;
			c[1][1] = ((*m_pvsValue)[1][1].GetValue() % 2) == 1 ? 1 : 0;
			if((c[0][1] == 0) && (c[1][1] == 0))
				c[0][1] = 0x00;
			else
			if((c[0][1] == 1) && (c[1][1] == 0))
				c[0][1] = 0x55;
			else
			if((c[0][1] == 0) && (c[1][1] == 1))
				c[0][1] = 0xAA;
			else
			if((c[0][1] == 1) && (c[1][1] == 1))
				c[0][1] = 0xFF;
			
			c[0][2] = ((*m_pvsValue)[0][2].GetValue() % 2) == 1 ? 1 : 0;
			c[1][2] = ((*m_pvsValue)[1][2].GetValue() % 2) == 1 ? 1 : 0;
			if((c[0][2] == 0) && (c[1][2] == 0))
				c[0][2] = 0x00;
			else
			if((c[0][2] == 1) && (c[1][2] == 0))
				c[0][2] = 0x55;
			else
			if((c[0][2] == 0) && (c[1][2] == 1))
				c[0][2] = 0xAA;
			else
			if((c[0][2] == 1) && (c[1][2] == 1))
				c[0][2] = 0xFF;
			
			crDiagram = RGB(c[0][0],c[0][1],c[0][2]);
		}

	CDC dc;
	dc.Attach(lpDrawItemStruct->hDC);

	CPen penDiagram(PS_SOLID | PS_GEOMETRIC,1,crDiagram);
	CPen penBackground(PS_SOLID | PS_GEOMETRIC,1,crBackground);
	CPen* ppenOld;
	CBrush brDiagram(crDiagram);
	CBrush brBackground(crBackground);
	CBrush* pbrOld;

	CRect rect1,rect2(&lpDrawItemStruct->rcItem);
	int nWidth;

	ppenOld = dc.SelectObject(&penDiagram);
	pbrOld = dc.SelectObject(&brDiagram);

	rect2.top += 2; 
	rect2.bottom -= 2;
	rect1 = rect2;
	nWidth = rect2.Width();

	if(m_pvsValue->GetNoValues() == 2)
	{
		rect1.left += 2;
		rect1.right -= 2; 
		dc.RoundRect(rect1,CPoint(20,20));
	}
	else
	if(m_pvsValue->GetNoValues() == 4)
	{
		if(m_pvsValue->GetValue() % 2 == 0)
		{
			rect1 = rect2;
			rect1.right = rect1.Width() / 2 - 25;
			dc.RoundRect(rect1,CPoint(20,20));
			rect1.right = rect2.right;
			rect1.left = rect2.left;
			rect1.left = rect1.Width() / 2 + 25;
			dc.RoundRect(rect1,CPoint(20,20));
		}
		else
			dc.RoundRect(rect1,CPoint(20,20));
		rect1 = rect2;
		if(m_pvsValue->GetValue() == 0)
		{
			dc.Arc(CRect(CPoint(rect1.Width() / 2 - 21,rect1.Height() / 2 + 3 - 21),CSize(42,42)),CPoint(0,0),CPoint(0,0));
			dc.FloodFill(rect1.Width() / 2,rect1.Height() / 2 + 3,crDiagram);
		}
		else
		if(m_pvsValue->GetValue() == 3)
		{
			dc.SelectObject(&penBackground);
			dc.SelectObject(&brBackground);
			dc.Arc(CRect(CPoint(rect1.Width() / 2 - 21,rect1.Height() / 2 + 3 - 21),CSize(42,42)),CPoint(0,0),CPoint(0,0));
			dc.FloodFill(rect1.Width() / 2,rect1.Height() / 2 + 3,crBackground);
			dc.SelectObject(&penDiagram);
			dc.SelectObject(&brDiagram);
		}
	}
	else
	if(m_pvsValue->GetNoValues() == 8)
	{
		rect1.right = rect2.right;
		rect1.left = rect2.left;
		rect1.bottom = rect1.top + 49;
		if((*m_pvsValue)[2].GetValue() % 2 == 0)
		{
			rect1.right = nWidth / 2 - 25;
			dc.RoundRect(rect1,CPoint(20,20));
			rect1.right = rect2.right;
			rect1.left = nWidth / 2 + 25;
			dc.RoundRect(rect1,CPoint(20,20));
		}
		else
			dc.RoundRect(rect1,CPoint(20,20));
		rect1.right = rect2.right;
		rect1.left = rect2.left;
		if((*m_pvsValue)[2].GetValue() == 0)
		{
			dc.Arc(CRect(CPoint(rect1.Width() / 2 - 21,rect1.Height() / 2 + 3 - 21),CSize(42,42)),CPoint(0,0),CPoint(0,0));
			dc.FloodFill(rect1.Width() / 2,rect1.Height() / 2,crDiagram);
		}
		else
		if((*m_pvsValue)[2].GetValue() == 3)
		{
			dc.SelectObject(&penBackground);
			dc.SelectObject(&brBackground);
			dc.Arc(CRect(CPoint(rect1.Width() / 2 - 21,rect1.Height() / 2 + 3 - 21),CSize(42,42)),CPoint(0,0),CPoint(0,0));
			dc.FloodFill(rect1.Width() / 2,rect1.Height() / 2,crBackground);
			dc.SelectObject(&penDiagram);
			dc.SelectObject(&brDiagram);
		}
		rect1.top = rect1.bottom + 4;
		rect1.bottom = rect1.top + 49;
		if((*m_pvsValue)[1].GetValue() % 2 == 0)
		{
			rect1.right = nWidth / 2 - 25;
			dc.RoundRect(rect1,CPoint(20,20));
			rect1.right = rect2.right;
			rect1.left = nWidth / 2 + 25;
			dc.RoundRect(rect1,CPoint(20,20));
		}
		else
			dc.RoundRect(rect1,CPoint(20,20));
		rect1.right = rect2.right;
		rect1.left = rect2.left;
		if((*m_pvsValue)[1].GetValue() == 0)
		{
			dc.Arc(CRect(CPoint(rect1.Width() / 2 - 21,rect1.Height() / 2 + 54 - 21),CSize(42,42)),CPoint(0,0),CPoint(0,0));
			dc.FloodFill(rect1.Width() / 2,rect1.Height() / 2 + 54,crDiagram);
		}
		else
		if((*m_pvsValue)[1].GetValue() == 3)
		{
			dc.SelectObject(&penBackground);
			dc.SelectObject(&brBackground);
			dc.Arc(CRect(CPoint(rect1.Width() / 2 - 21,rect1.Height() / 2 + 54 - 21),CSize(42,42)),CPoint(0,0),CPoint(0,0));
			dc.FloodFill(rect1.Width() / 2,rect1.Height() / 2 + 54,crBackground);
			dc.SelectObject(&penDiagram);
			dc.SelectObject(&brDiagram);
		}
		rect1.top = rect1.bottom + 4;
		rect1.bottom = rect1.top + 49;
		if((*m_pvsValue)[0].GetValue() % 2 == 0)
		{
			rect1.right = nWidth / 2 - 25;
			dc.RoundRect(rect1,CPoint(20,20));
			rect1.right = rect2.right;
			rect1.left = nWidth / 2 + 25;
			dc.RoundRect(rect1,CPoint(20,20));
		}
		else
			dc.RoundRect(rect1,CPoint(20,20));
		rect1.right = rect2.right;
		rect1.left = rect2.left;
		if((*m_pvsValue)[0].GetValue() == 0)
		{
			dc.Arc(CRect(CPoint(rect1.Width() / 2 - 21,rect1.Height() / 2 + 108 - 21),CSize(42,42)),CPoint(0,0),CPoint(0,0));
			dc.FloodFill(rect1.Width() / 2,rect1.Height() / 2 + 108,crDiagram);
		}
		else
		if((*m_pvsValue)[0].GetValue() == 3)
		{
			dc.SelectObject(&penBackground);
			dc.SelectObject(&brBackground);
			dc.Arc(CRect(CPoint(rect1.Width() / 2 - 21,rect1.Height() / 2 + 108 - 21),CSize(42,42)),CPoint(0,0),CPoint(0,0));
			dc.FloodFill(rect1.Width() / 2,rect1.Height() / 2 + 108,crBackground);
			dc.SelectObject(&penDiagram);
			dc.SelectObject(&brDiagram);
		}
	}
	else
	if(m_pvsValue->GetNoValues() == 64)
	{
		rect1.right = rect2.right;
		rect1.left = rect2.left;
		rect1.bottom = rect1.top + 49;
		if((*m_pvsValue)[1][2].GetValue() % 2 == 0)
		{
			rect1.right = nWidth / 2 - 25;
			dc.RoundRect(rect1,CPoint(20,20));
			rect1.right = rect2.right;
			rect1.left = nWidth / 2 + 25;
			dc.RoundRect(rect1,CPoint(20,20));
		}
		else
			dc.RoundRect(rect1,CPoint(20,20));
		rect1.right = rect2.right;
		rect1.left = rect2.left;

		if((*m_pvsValue)[1][2].GetValue() == 0)
		{
			dc.Arc(CRect(CPoint(rect1.Width() / 2 - 21,rect1.Height() / 2 + 3 - 21),CSize(42,42)),CPoint(0,0),CPoint(0,0));
			dc.FloodFill(rect1.Width() / 2,rect1.Height() / 2,crDiagram);
		}
		else
		if((*m_pvsValue)[1][2].GetValue() == 3)
		{
			dc.SelectObject(&penBackground);
			dc.SelectObject(&brBackground);
			dc.Arc(CRect(CPoint(rect1.Width() / 2 - 21,rect1.Height() / 2 + 3 - 21),CSize(42,42)),CPoint(0,0),CPoint(0,0));
			dc.FloodFill(rect1.Width() / 2,rect1.Height() / 2,crBackground);
			dc.SelectObject(&penDiagram);
			dc.SelectObject(&brDiagram);
		}

		rect1.top = rect1.bottom + 4;
		rect1.bottom = rect1.top + 49;
		if((*m_pvsValue)[1][1].GetValue() % 2 == 0)
		{
			rect1.right = nWidth / 2 - 25;
			dc.RoundRect(rect1,CPoint(20,20));
			rect1.right = rect2.right;
			rect1.left = nWidth / 2 + 25;
			dc.RoundRect(rect1,CPoint(20,20));
		}
		else
			dc.RoundRect(rect1,CPoint(20,20));
		rect1.right = rect2.right;
		rect1.left = rect2.left;
		
		if((*m_pvsValue)[1][1].GetValue() == 0)
		{
			dc.Arc(CRect(CPoint(rect1.Width() / 2 - 21,rect1.Height() / 2 + 54 - 21),CSize(42,42)),CPoint(0,0),CPoint(0,0));
			dc.FloodFill(rect1.Width() / 2,rect1.Height() / 2 + 54,crDiagram);
		}
		else
		if((*m_pvsValue)[1][1].GetValue() == 3)
		{
			dc.SelectObject(&penBackground);
			dc.SelectObject(&brBackground);
			dc.Arc(CRect(CPoint(rect1.Width() / 2 - 21,rect1.Height() / 2 + 54 - 21),CSize(42,42)),CPoint(0,0),CPoint(0,0));
			dc.FloodFill(rect1.Width() / 2,rect1.Height() / 2 + 54,crBackground);
			dc.SelectObject(&penDiagram);
			dc.SelectObject(&brDiagram);
		}
		
		rect1.top = rect1.bottom + 4;
		rect1.bottom = rect1.top + 49;
		if((*m_pvsValue)[1][0].GetValue() % 2 == 0)
		{
			rect1.right = nWidth / 2 - 25;
			dc.RoundRect(rect1,CPoint(20,20));
			rect1.right = rect2.right;
			rect1.left = nWidth / 2 + 25;
			dc.RoundRect(rect1,CPoint(20,20));
		}
		else
			dc.RoundRect(rect1,CPoint(20,20));
		rect1.right = rect2.right;
		rect1.left = rect2.left;
		
		if((*m_pvsValue)[1][0].GetValue() == 0)
		{
			dc.Arc(CRect(CPoint(rect1.Width() / 2 - 21,rect1.Height() / 2 + 108 - 21),CSize(42,42)),CPoint(0,0),CPoint(0,0));
			dc.FloodFill(rect1.Width() / 2,rect1.Height() / 2 + 108,crDiagram);
		}
		else
		if((*m_pvsValue)[1][0].GetValue() == 3)
		{
			dc.SelectObject(&penBackground);
			dc.SelectObject(&brBackground);
			dc.Arc(CRect(CPoint(rect1.Width() / 2 - 21,rect1.Height() / 2 + 108 - 21),CSize(42,42)),CPoint(0,0),CPoint(0,0));
			dc.FloodFill(rect1.Width() / 2,rect1.Height() / 2 + 108,crBackground);
			dc.SelectObject(&penDiagram);
			dc.SelectObject(&brDiagram);
		}
		
		rect1.top = rect1.bottom + 4;
		rect1.bottom = rect1.top + 49;
		if((*m_pvsValue)[0][2].GetValue() % 2 == 0)
		{
			rect1.right = nWidth / 2 - 25;
			dc.RoundRect(rect1,CPoint(20,20));
			rect1.right = rect2.right;
			rect1.left = nWidth / 2 + 25;
			dc.RoundRect(rect1,CPoint(20,20));
		}
		else
			dc.RoundRect(rect1,CPoint(20,20));
		rect1.right = rect2.right;
		rect1.left = rect2.left;
		
		if((*m_pvsValue)[0][2].GetValue() == 0)
		{
			dc.Arc(CRect(CPoint(rect1.Width() / 2 - 21,rect1.Height() / 2 + 161 - 21),CSize(42,42)),CPoint(0,0),CPoint(0,0));
			dc.FloodFill(rect1.Width() / 2,rect1.Height() / 2 + 161,crDiagram);
		}
		else
		if((*m_pvsValue)[0][2].GetValue() == 3)
		{
			dc.SelectObject(&penBackground);
			dc.SelectObject(&brBackground);
			dc.Arc(CRect(CPoint(rect1.Width() / 2 - 21,rect1.Height() / 2 + 161 - 21),CSize(42,42)),CPoint(0,0),CPoint(0,0));
			dc.FloodFill(rect1.Width() / 2,rect1.Height() / 2 + 161,crBackground);
			dc.SelectObject(&penDiagram);
			dc.SelectObject(&brDiagram);
		}
		
		rect1.top = rect1.bottom + 4;
		rect1.bottom = rect1.top + 49;
		if((*m_pvsValue)[0][1].GetValue() % 2 == 0)
		{
			rect1.right = nWidth / 2 - 25;
			dc.RoundRect(rect1,CPoint(20,20));
			rect1.right = rect2.right;
			rect1.left = nWidth / 2 + 25;
			dc.RoundRect(rect1,CPoint(20,20));
		}
		else
			dc.RoundRect(rect1,CPoint(20,20));
		rect1.right = rect2.right;
		rect1.left = rect2.left;
		
		if((*m_pvsValue)[0][1].GetValue() == 0)
		{
			dc.Arc(CRect(CPoint(rect1.Width() / 2 - 21,rect1.Height() / 2 + 214 - 21),CSize(42,42)),CPoint(0,0),CPoint(0,0));
			dc.FloodFill(rect1.Width() / 2,rect1.Height() / 2 + 214,crDiagram);
		}
		else
		if((*m_pvsValue)[0][1].GetValue() == 3)
		{
			dc.SelectObject(&penBackground);
			dc.SelectObject(&brBackground);
			dc.Arc(CRect(CPoint(rect1.Width() / 2 - 21,rect1.Height() / 2 + 214 - 21),CSize(42,42)),CPoint(0,0),CPoint(0,0));
			dc.FloodFill(rect1.Width() / 2,rect1.Height() / 2 + 214,crBackground);
			dc.SelectObject(&penDiagram);
			dc.SelectObject(&brDiagram);
		}
		
		rect1.top = rect1.bottom + 4;
		rect1.bottom = rect1.top + 49;
		if((*m_pvsValue)[0][0].GetValue() % 2 == 0)
		{
			rect1.right = nWidth / 2 - 25;
			dc.RoundRect(rect1,CPoint(20,20));
			rect1.right = rect2.right;
			rect1.left = nWidth / 2 + 25;
			dc.RoundRect(rect1,CPoint(20,20));
		}
		else
			dc.RoundRect(rect1,CPoint(20,20));

		rect1.right = rect2.right;
		rect1.left = rect2.left;

		if((*m_pvsValue)[0][0].GetValue() == 0)
		{
			dc.Arc(CRect(CPoint(rect1.Width() / 2 - 21,rect1.Height() / 2 + 267 - 21),CSize(42,42)),CPoint(0,0),CPoint(0,0));
			dc.FloodFill(rect1.Width() / 2,rect1.Height() / 2 + 267,crDiagram);
		}
		else
		if((*m_pvsValue)[0][0].GetValue() == 3)
		{
			dc.SelectObject(&penBackground);
			dc.SelectObject(&brBackground);
			dc.Arc(CRect(CPoint(rect1.Width() / 2 - 21,rect1.Height() / 2 + 267 - 21),CSize(42,42)),CPoint(0,0),CPoint(0,0));
			dc.FloodFill(rect1.Width() / 2,rect1.Height() / 2 + 267,crBackground);
			dc.SelectObject(&penDiagram);
			dc.SelectObject(&brDiagram);
		}
	}

	dc.SelectObject(pbrOld);
	dc.SelectObject(ppenOld);

	dc.SetBkMode(TRANSPARENT);
	if((crDiagram == 0))
		dc.SetTextColor(RGB(0xAA,0xAA,0xAA)); 
	dc.SetBkColor(crDiagram);
	dc.DrawText(strTemp,strTemp.GetLength(),rect2,DT_SINGLELINE | DT_CENTER | DT_VCENTER);
//	dc.DrawFocusRect(&lpDrawItemStruct->rcItem);
}

void CValueSequencerCtrl::OnLButtonDown(UINT nFlags, CPoint point)
{
	if(m_bMoveable)
	{
//		if(nFlags != -999)
			m_bLButtonDown = true;
		::GetConsultationDlg()->KillTimer(1);
		::GetExplorationCtrl()->KillTimer(1);
		::GetLogicCtrl()->KillTimer(1);
		if((m_nShowCursor == 0) && !::GetPropertyCtrl()->GetAnimation())
			m_nShowCursor = ::ShowCursor(FALSE);
		m_pvsPane->SetCurrentSequencerCtrl(this);
		if((m_eDirection == ePrevious) || (m_eDirection == eNext))
		{
			CConsultationDlg::m_nLineDelay = 0;
			CExplorationCtrl::m_nLineDelay = 0;
			GetParent()->SetTimer(1,::GetPropertyCtrl()->GetSpeed(),NULL);
		}
		Move();
	}
	CButton::OnLButtonDown(nFlags,point);
}

void CValueSequencerCtrl::OnLButtonUp(UINT nFlags, CPoint point)
{
	m_bLButtonDown = false;
	if(m_bMoveable && !::GetPropertyCtrl()->GetAnimation())
	{
		if(m_nShowCursor == -1)
			m_nShowCursor = ::ShowCursor(TRUE);
		::GetConsultationDlg()->KillTimer(1);
		::GetExplorationCtrl()->KillTimer(1);
		::GetLogicCtrl()->KillTimer(1);
		if(GetConsultationDlg()->GetSafeHwnd() == NULL)
			::GetMainFrame()->PostMessage(WM_UPDATE_TEXT);
		else
			::GetConsultationDlg()->SendMessage(WM_UPDATE_CTRL);
	}
	CButton::OnLButtonUp(nFlags,point);
}

void CValueSequencerCtrl::OnRButtonDown(UINT nFlags, CPoint point)
{
	CMenu menuContext;              
	CMenu menuSpecific;
	CString strTemp;
	m_bRButtonDown = true;
	ClientToScreen(&point); 
	CButton::OnRButtonDown(nFlags,point);
	if(m_bMoveable && !::GetPropertyCtrl()->GetAnimation())
		if(menuContext.CreatePopupMenu())
			if(menuSpecific.CreatePopupMenu())
			{
				for(int i = 0; i < m_pvsValue->GetNoValues(); ++i)
				{
					strTemp.Format(_T("%d %s"),i + 1,m_pvsValue->GetLabelEntry()->GetAt(m_pvsValue->GetSequenceEntry()->
						GetNextSequence(i == 0 ? 999 : i - 1))->GetLabel());
					menuSpecific.AppendMenu(MF_STRING | MF_ENABLED,ID_VSCTX_SPECIFIC + i,strTemp);
				}
				menuContext.AppendMenu(MF_STRING | MF_ENABLED,ID_VSCTX_FIRST,_T("&First"));
				menuContext.AppendMenu(MF_STRING | MF_ENABLED,ID_VSCTX_PREVIOUS,_T("&Previous"));
				menuContext.AppendMenu(MF_STRING | MF_ENABLED,ID_VSCTX_NEXT,_T("&Next"));
				menuContext.AppendMenu(MF_STRING | MF_ENABLED,ID_VSCTX_LAST,_T("&Last"));
				menuContext.AppendMenu(MF_STRING | MF_ENABLED,ID_VSCTX_INVERSE,_T("&Inverse"));
				menuContext.AppendMenu(MF_STRING | MF_ENABLED,ID_VSCTX_OPPOSITE,_T("&Opposite"));
				if(dynamic_cast<CHexagramValueSequencer*>(m_pvsValue) != NULL)
				{
					menuContext.AppendMenu(MF_STRING | MF_ENABLED,ID_VSCTX_TRANSVERSE,_T("&Transverse"));
					menuContext.AppendMenu(MF_STRING | MF_ENABLED,ID_VSCTX_NUCLEAR,_T("N&uclear"));
					menuContext.AppendMenu(MF_STRING | MF_ENABLED,ID_VSCTX_SECONDARY,_T("&Secondary"));
				}
				menuContext.AppendMenu(MF_POPUP | MF_ENABLED,(UINT) menuSpecific.GetSafeHmenu(),_T("Sp&ecific"));
				menuContext.AppendMenu(MF_SEPARATOR);
				menuContext.AppendMenu(MF_STRING | MF_GRAYED,ID_VSCTX_PROPERTIES,_T("&Properties"));
				menuContext.TrackPopupMenu(TPM_LEFTALIGN | TPM_LEFTBUTTON,point.x,point.y,this); 
				menuContext.DestroyMenu();
			}
}

void CValueSequencerCtrl::OnRButtonUp(UINT nFlags, CPoint point)
{
	m_bRButtonDown = false;
	CButton::OnRButtonUp(nFlags,point);
}

void CValueSequencerCtrl::OnContextFirst(void)
{
	m_pvsValue->First();
	m_pvsPane->Update();
	::GetApp()->UpdateAllTextViews();
}

void CValueSequencerCtrl::OnContextPrevious(void)
{
	--*m_pvsValue;
	m_pvsPane->Update();
	::GetApp()->UpdateAllTextViews();
}

void CValueSequencerCtrl::OnContextNext(void)
{
	++*m_pvsValue;
	m_pvsPane->Update();
	::GetApp()->UpdateAllTextViews();
}

void CValueSequencerCtrl::OnContextLast(void)
{
	m_pvsValue->Last();
	m_pvsPane->Update();
	::GetApp()->UpdateAllTextViews();
}

void CValueSequencerCtrl::OnContextInverse(void)
{
	m_pvsValue->Inverse();
	m_pvsPane->Update();
	::GetApp()->UpdateAllTextViews();
}

void CValueSequencerCtrl::OnContextOpposite(void)
{
	m_pvsValue->Opposite();
	m_pvsPane->Update();
	::GetApp()->UpdateAllTextViews();
}

void CValueSequencerCtrl::OnContextTransverse(void)
{
	m_pvsValue->Transverse();
	m_pvsPane->Update();
	::GetApp()->UpdateAllTextViews();
}

void CValueSequencerCtrl::OnContextNuclear(void)
{
	m_pvsValue->Nuclear();
	m_pvsPane->Update();
	::GetApp()->UpdateAllTextViews();
}

void CValueSequencerCtrl::OnContextSecondary(void)
{
	if(((*m_pvsValue)[1][2].GetValue() == 0) || ((*m_pvsValue)[1][2].GetValue() == 3))
		++(*m_pvsValue)[1][2];
	if(((*m_pvsValue)[1][1].GetValue() == 0) || ((*m_pvsValue)[1][1].GetValue() == 3))
		++(*m_pvsValue)[1][1];
	if(((*m_pvsValue)[1][0].GetValue() == 0) || ((*m_pvsValue)[1][0].GetValue() == 3))
		++(*m_pvsValue)[1][0];
	if(((*m_pvsValue)[0][2].GetValue() == 0) || ((*m_pvsValue)[0][2].GetValue() == 3))
		++(*m_pvsValue)[0][2];
	if(((*m_pvsValue)[0][1].GetValue() == 0) || ((*m_pvsValue)[0][1].GetValue() == 3))
		++(*m_pvsValue)[0][1];
	if(((*m_pvsValue)[0][0].GetValue() == 0) || ((*m_pvsValue)[0][0].GetValue() == 3))
		++(*m_pvsValue)[0][0];
	m_pvsPane->Update();
	::GetApp()->UpdateAllTextViews();
}
/*
	pCmdUI->Enable(((*pvsHS)[1][2].GetValue() == 0) || ((*pvsHS)[1][2].GetValue() == 3) ||
		((*pvsHS)[1][1].GetValue() == 0) || ((*pvsHS)[1][1].GetValue() == 3) ||
		((*pvsHS)[1][0].GetValue() == 0) || ((*pvsHS)[1][0].GetValue() == 3) ||
		((*pvsHS)[0][2].GetValue() == 0) || ((*pvsHS)[0][2].GetValue() == 3) ||
		((*pvsHS)[0][1].GetValue() == 0) || ((*pvsHS)[0][1].GetValue() == 3) ||
		((*pvsHS)[0][0].GetValue() == 0) || ((*pvsHS)[0][0].GetValue() == 3));
*/
void CValueSequencerCtrl::OnContextSpecific(UINT nId)
{
	CHexagramValueSequencer* phvsValue;
	CTrigramValueSequencer* ptvsValue;
	CLineValueSequencer* plvsValue;
	CBitValueSequencer* pcvsValue;
	if((phvsValue = dynamic_cast<CHexagramValueSequencer*>(m_pvsValue)) != NULL)
		*phvsValue = m_pvsValue->GetSequenceEntry()->FindValue(nId - ID_VSCTX_SPECIFIC);
	if((ptvsValue = dynamic_cast<CTrigramValueSequencer*>(m_pvsValue)) != NULL)
		*ptvsValue = m_pvsValue->GetSequenceEntry()->FindValue(nId - ID_VSCTX_SPECIFIC);
	if((plvsValue = dynamic_cast<CLineValueSequencer*>(m_pvsValue)) != NULL)
		*plvsValue = m_pvsValue->GetSequenceEntry()->FindValue(nId - ID_VSCTX_SPECIFIC);
	if((pcvsValue = dynamic_cast<CBitValueSequencer*>(m_pvsValue)) != NULL)
		*pcvsValue = m_pvsValue->GetSequenceEntry()->FindValue(nId - ID_VSCTX_SPECIFIC);
	m_pvsPane->Update();
	::GetApp()->UpdateAllTextViews();
}

void CValueSequencerCtrl::OnContextProperties(void)
{
}

BOOL CValueSequencerCtrl::OnSetCursor(CWnd* pWnd, UINT nHitTest, UINT message)
{
	CRect rect;
	GetWindowRect(&rect);
	if(m_bMoveable)
		if(!m_bRButtonDown)
			if(GetCurrentMessage()->pt.x > rect.CenterPoint().x)
				if((GetKeyState(VK_CONTROL) & 128) == 128)
				{
					m_eDirection = eLast;
					::SetCursor(m_hLast);
				}
				else
				{
					m_eDirection = eNext;
					::SetCursor(m_hNext);
				}
			else
				if((GetKeyState(VK_CONTROL) & 128) == 128)
				{
					m_eDirection = eFirst;
					::SetCursor(m_hFirst);
				}
				else
				{
					m_eDirection = ePrevious;
					::SetCursor(m_hPrevious);
				}
		else
			return CButton::OnSetCursor(pWnd,nHitTest,message);
	else
		return CButton::OnSetCursor(pWnd,nHitTest,message);
	return FALSE;
}

void CValueSequencerCtrl::Move()
{
	if(m_eDirection == eFirst)
		m_pvsValue->First();
	else
		if(m_eDirection == ePrevious)
			--*m_pvsValue;
		else
			if(m_eDirection == eNext)
				++*m_pvsValue;
			else
				if(m_eDirection == eLast)
					m_pvsValue->Last();
	m_pvsPane->Update();
}

void CValueSequencerCtrl::Show()
{
	CString strTemp;
	if(!m_bImageButton)
		Invalidate();
	else
		CButton::SetBitmap(m_pvsValue->GetBitmap());
}

void CValueSequencerCtrl::Update()
{
	m_pvsValue->SetEntryArrays(-1);
	Show();
}

/////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////

CComValueSequencer::CComValueSequencer()
{
}

CComValueSequencer::~CComValueSequencer()
{
}

HRESULT CComValueSequencer::get_Application(IApplication** ppIApplication)
{
	*ppIApplication = NULL;
	return S_OK;
}

HRESULT CComValueSequencer::get_Parent(IDispatch** ppIDispatch)
{
	*ppIDispatch = NULL;
	return S_OK;
}

HRESULT CComValueSequencer::get_Name(BSTR* pbstrName)
{
	CString strLabel = ::GetApp()->GetCurrentHexagram()->GetLabel();
	*pbstrName = strLabel.AllocSysString();
	return S_OK;
}

HRESULT CComValueSequencer::get_Value(int* pnValue)
{
	*pnValue = ::GetApp()->GetCurrentHexagram()->GetValue();
	return S_OK;
}

HRESULT CComValueSequencer::put_Value(int nValue)
{
	CHexagramValueSequencer* phvsCurrent = ::GetApp()->GetCurrentHexagram();
	phvsCurrent->operator=(nValue);
	::GetApp()->UpdateExplorationCtrl();
	::GetTextDoc()->SetTitle(::GetApp()->GetCurrentHexagram()->GetLabel());
	return S_OK;
}

int GetMoveIndex(LPCTSTR lpszName, int nSize, int nIndex)
{
	if(::GetPropertyCtrl()->GetUserGender() == _T("Female")) 
		if((_tcscmp(lpszName,_T("Gender")) == 0) && (nSize == 2))
			return 1 - nIndex;
	return nIndex;
}

HRESULT CComValueSequencer::get_Text(BSTR bstrType, BSTR* pbstrText)
{
	CLabelBlockArray* prgLS1 = ::GetApp()->GetLabelBlock1();
	CLabelBlockArray* prgLS2 = ::GetApp()->GetLabelBlock2();
	CLabelEntryArray* prgLE1;
	CLabelEntryArray* prgLE2;
	CLabelEntryArray* prgLE3;
	CLabelEntryArray* prgLE6;
	CString strTemp1,strTemp2,strTemp3 = bstrType;
	CTextColumnSet TCS;

	CString strDesignation[5] = 
	{
		_T("Superior Man"),
		_T("Noble One"),
		_T("Saintly Person"),
		_T("Celestial Master"), 
		_T("Disciple of Wisdom")
	};

	strTemp2 = ::GetPropertyCtrl()->GetTextSource();
	strTemp1.Format(_T("select V%d from Text6 where Type = '%s' and Name = '%s'"),
		::GetApp()->GetCurrentHexagram()->GetValue(),strTemp3,strTemp2);
	if(TCS.OpenRowset(::GetApp()->GetBeliefBase(),strTemp1))
	{
		strTemp1 = TCS.m_szText;

		for(int i = 0; i < 5; ++i)
			strTemp1 = ReplaceWholeWord(strTemp1,::GetPropertyCtrl()->GetDesignation(),strDesignation[i],NULL);

		if(::GetPropertyCtrl()->GetDynamicHyperlink() && (strTemp2 != _T("Andrade")))
		{
			prgLE6 = ::GetApp()->GetLabelBlock6()->GetAt(::GetAtiveHexagramLabelIndex())->GetEntryArray();
			for(int j = 0; j <= prgLE6->GetUpperBound(); ++j)
			{
				strTemp3.Format(_T("Hexagram.html?Name=%s\" onclick=\"LoadHexagram(%d)"),prgLE6->GetAt(j)->GetLabel(),j);
				strTemp1 = ReplaceWholeWord(strTemp1,prgLE6->GetAt(j)->GetLabel(),
					prgLE6->GetAt(j)->GetLabel(),strTemp3);
			}
			prgLE3 = ::GetApp()->GetLabelBlock3()->GetAt(::GetAtiveTrigramLabelIndex())->GetEntryArray();
			for(int j = 0; j <= prgLE3->GetUpperBound(); ++j)
				strTemp1 = ReplaceWholeWord(strTemp1,prgLE3->GetAt(j)->GetLabel(),
					prgLE3->GetAt(j)->GetLabel(),_T("http://en.wikipedia.org/wiki/") + CString(prgLE3->GetAt(j)->GetLabel()));
			for(int i = 2; i <= prgLS2->GetUpperBound(); ++i)
			{
				prgLE2 = prgLS2->GetAt(i)->GetEntryArray();
				for(int j = 0; j <= prgLE2->GetUpperBound(); ++j)
					strTemp1 = ReplaceWholeWord(strTemp1,prgLE2->GetAt(j)->GetLabel(),
						prgLE2->GetAt(j)->GetLabel(),_T("http://en.wikipedia.org/wiki/") + CString(prgLE2->GetAt(j)->GetLabel()));
			}
			for(int i = 2; i <= prgLS1->GetUpperBound(); ++i)
			{
				prgLE1 = prgLS1->GetAt(i)->GetEntryArray();
				for(int j = 0; j <= prgLE1->GetUpperBound(); ++j)
				{
					int nIndex = GetMoveIndex(prgLS1->GetAt(i)->GetName(),prgLE1->GetUpperBound() + 1,j);
					strTemp1 = ReplaceWholeWord(strTemp1,prgLE1->GetAt(nIndex)->GetLabel(),
						prgLE1->GetAt(j)->GetLabel(),_T("http://en.wikipedia.org/wiki/") + CString(prgLE1->GetAt(j)->GetLabel()));
				}
			}
		}
		*pbstrText = strTemp1.AllocSysString();
	}
	return S_OK;
}

HRESULT CComValueSequencer::get_IsMovingLine(int nLine, VARIANT_BOOL* pbValue)
{
	int nValue = (*::GetApp()->GetCurrentHexagram())[nLine / 3][nLine % 3].GetValue();
	*pbValue = (nValue == 0) || (nValue == 3) ? VARIANT_TRUE : VARIANT_FALSE;
	return S_OK;
}

HRESULT CComValueSequencer::get_HasText(BSTR bstrType, VARIANT_BOOL* pbValue)
{
	*pbValue = VARIANT_TRUE; 
	CString strTemp1,strTemp2 = bstrType;
	strTemp1 = ::GetPropertyCtrl()->GetTextSource();
	if((strTemp2 != _T("Text")) && (strTemp1 == _T("Andrade")))
		*pbValue = VARIANT_FALSE; 
	if((strTemp2 != _T("Text")) && (strTemp1 == _T("Heyboer")))
		*pbValue = VARIANT_FALSE; 
	if((strTemp2 != _T("Text")) && (strTemp1 == _T("Harvard Yenching")))
		*pbValue = VARIANT_FALSE; 
	if((strTemp2 != _T("Text")) && (strTemp1 == _T("YellowBridge")))
		*pbValue = VARIANT_FALSE; 
	if((strTemp2 != _T("Text")) && (strTemp1 == _T("Stackhouse")))
		*pbValue = VARIANT_FALSE; 
	if(((strTemp2 == _T("Judgment")) || (strTemp2 == _T("Image"))) && (strTemp1 == _T("Legge")))
		*pbValue = VARIANT_FALSE; 
	return S_OK;
}

HRESULT CComValueSequencer::get_SequenceName(BSTR* pbstrName)
{
	CString strName = ::GetPropertyCtrl()->GetTextSource();
	*pbstrName = strName.AllocSysString();
	return S_OK;
}

HRESULT CComValueSequencer::get_SecondaryName(int nLine, BSTR* pbstrName)
{
	CHexagramValueSequencer vsHexagram;
	int nValue = (*::GetApp()->GetCurrentHexagram())[nLine / 3][nLine % 3].GetValue();
	vsHexagram.SetValue(::GetApp()->GetCurrentHexagram()->GetValue());
	vsHexagram.UpdateInnerValues();
	if(nValue == 0)
		vsHexagram[nLine / 3][nLine % 3].SetValue(1);
	else
	if(nValue == 3)
		vsHexagram[nLine / 3][nLine % 3].SetValue(2);
	vsHexagram[nLine / 3][nLine % 3].UpdateInnerValues();
	vsHexagram[nLine / 3][nLine % 3].UpdateOuterValues();
	CString strTemp1,strTemp2,strTemp3 = ::GetApp()->GetLabelBlock6()->GetAt(::GetAtiveHexagramLabelIndex())->GetEntryArray()->GetAt(vsHexagram.GetValue())->GetLabel();
	strTemp2.Format(_T("Hexagram.html?Name=%s\" onclick=\"LoadHexagram(%d)"),strTemp3,vsHexagram.GetValue());
	strTemp1 = ReplaceWholeWord(strTemp3,strTemp3,strTemp3,strTemp2);
	*pbstrName = strTemp1.AllocSysString();
	return S_OK;
}

/////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////

HRESULT CreateComponentCategory(CATID catid, BSTR bstrDescription)
{
	CATEGORYINFO catinfo;
	ICatRegister* pcr = NULL;
	HRESULT hr;
	if(SUCCEEDED(hr = CoCreateInstance(CLSID_StdComponentCategoriesMgr,NULL,
		CLSCTX_INPROC_SERVER,IID_ICatRegister,(void**) &pcr)))
	{
		catinfo.catid = catid;
		catinfo.lcid = 0x0409;
//		wcsncpy(catinfo.szDescription,catDescription,len);
		catinfo.szDescription[0] = 0;
		hr = pcr->RegisterCategories(1,&catinfo);
		pcr->Release();
	}
	return hr;
}

HRESULT RegisterCLSIDInCategory(REFCLSID clsid, CATID catid)
{
	CATID rgcatid[1];
	ICatRegister* pcr = NULL;
	HRESULT hr;
	if(SUCCEEDED(hr = CoCreateInstance(CLSID_StdComponentCategoriesMgr,NULL,
		CLSCTX_INPROC_SERVER,IID_ICatRegister,(void**) &pcr)))
	{
		rgcatid[0] = catid;
		hr = pcr->RegisterClassImplCategories(clsid,1,rgcatid);
		pcr->Release();
	}
	return hr;
}

HRESULT UnRegisterCLSIDInCategory(REFCLSID clsid, CATID catid)
{
	CATID rgcatid[1];
	ICatRegister* pcr = NULL;
	HRESULT hr;
	if(SUCCEEDED(hr = CoCreateInstance(CLSID_StdComponentCategoriesMgr,NULL,
		CLSCTX_INPROC_SERVER,IID_ICatRegister,(void**) &pcr)))
	{
		rgcatid[0] = catid;
		hr = pcr->UnRegisterClassImplCategories(clsid,1,rgcatid);
		pcr->Release();
	}
	return hr;
}

HRESULT RegisterControl()
{
	HRESULT hr;
	CComBSTR bstrTemp1 = _T("Controls safely initializable from persistent data!");
	CComBSTR bstrTemp2 = _T("Controls safely scriptable!");
	if(SUCCEEDED(hr = CreateComponentCategory(CATID_SafeForInitializing,bstrTemp1)))
	if(SUCCEEDED(hr = RegisterCLSIDInCategory(CLSID_ValueSequencer,CATID_SafeForInitializing)))
	if(SUCCEEDED(hr = RegisterCLSIDInCategory(CLSID_Application,CATID_SafeForInitializing)))
	if(SUCCEEDED(hr = CreateComponentCategory(CATID_SafeForScripting,bstrTemp2)))
	if(SUCCEEDED(hr = RegisterCLSIDInCategory(CLSID_ValueSequencer,CATID_SafeForScripting)))
	if(SUCCEEDED(hr = RegisterCLSIDInCategory(CLSID_Application,CATID_SafeForScripting)))
		return S_OK;
	return hr;
}

HRESULT UnRegisterControl()
{
	HRESULT hr;
	if(SUCCEEDED(hr = UnRegisterCLSIDInCategory(CLSID_ValueSequencer,CATID_SafeForInitializing)))
	if(SUCCEEDED(hr = UnRegisterCLSIDInCategory(CLSID_Application,CATID_SafeForScripting)))
		return S_OK;
	return hr;
}

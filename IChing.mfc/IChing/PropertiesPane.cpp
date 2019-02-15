#include "StdAfx.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

BEGIN_MESSAGE_MAP(CPropertiesPane, CWnd)
	ON_WM_CREATE()
	ON_WM_SIZE()
	ON_COMMAND(ID_PANEPROPERTIES_CATEGORIZED, OnPanePropertiesCategorized)
	ON_UPDATE_COMMAND_UI(ID_PANEPROPERTIES_CATEGORIZED, OnUpdatePanePropertiesCategorized)
	ON_COMMAND(ID_PANEPROPERTIES_ALPHABETIC, OnPanePropertiesAlphabetic)
	ON_UPDATE_COMMAND_UI(ID_PANEPROPERTIES_ALPHABETIC, OnUpdatePanePropertiesAlphabetic)
	ON_WM_SETFOCUS()
	ON_MESSAGE(XTPWM_PROPERTYGRID_NOTIFY, OnGridNotify)
END_MESSAGE_MAP()

CPropertiesPane::CPropertiesPane()
{
	m_bInitialised =  false;
	m_nSpeed = 500;
}

CPropertiesPane::~CPropertiesPane()
{
}

int CPropertiesPane::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if((CWnd::OnCreate(lpCreateStruct) == -1) || !m_wndToolBar.CreateToolBar(WS_TABSTOP | WS_VISIBLE | WS_CHILD | 
		CBRS_TOOLTIPS,this) || !m_wndToolBar.LoadToolBar(IDR_PANE_PROPERTIES))
		return -1;
	return 0;
}

void CPropertiesPane::CreateAndLoadControls()
{
	CLabelBlockArray* prgLB;
	CSequenceBlockArray* prgSB;
	if(m_wndPropertyGrid.GetSafeHwnd() == 0)
	{
		m_wndPropertyGrid.Create(CRect(0,0,0,0),this,0);
		m_wndPropertyGrid.SetOwner(this);
		m_wndPropertyGrid.SetTheme(xtpGridThemeWhidbey);

		m_pgiApplication = m_wndPropertyGrid.AddCategory(_T("Application Settings"));
		m_pgiBeliefBase = m_pgiApplication->AddChildItem(new CXTPPropertyGridItem(_T("Belief Base"),_T("Athiest")));
		CXTPPropertyGridItemConstraints* pgicBeliefBase = m_pgiBeliefBase->GetConstraints();
		m_pgiBeliefBase->SetDescription(_T("Select the Belief Database you want to use."));
		m_pgiBeliefBase->SetFlags(xtpGridItemHasComboButton);
		m_pgiApplication->Expand();
		pgicBeliefBase->AddConstraint(_T("Athiest"));

		m_pgiUser = m_wndPropertyGrid.AddCategory(_T("User Settings"));
		m_pgiUserName = m_pgiUser->AddChildItem(new CXTPPropertyGridItem(_T("Name"),_T("Stephen Vivash")));
		m_pgiUserGender = m_pgiUser->AddChildItem(new CXTPPropertyGridItem(_T("Gender"),_T("Male")));
		CXTPPropertyGridItemConstraints* pgicUserGender = m_pgiUserGender->GetConstraints();
		m_pgiUserName->SetDescription(_T("Enter your name."));
		m_pgiUserGender->SetDescription(_T("Select your gender."));
		m_pgiUserGender->SetFlags(xtpGridItemHasComboButton);
		m_pgiUser->Expand();
		pgicUserGender->AddConstraint(_T("Male"));
		pgicUserGender->AddConstraint(_T("Female"));

		m_pgiLogicgrams = m_wndPropertyGrid.AddCategory(_T("Logicgram Settings"));
		m_pgiAnimation = m_pgiLogicgrams->AddChildItem(new CXTPPropertyGridItemBool(_T("Animation"),FALSE));
		m_pgiSpeed = m_pgiLogicgrams->AddChildItem(new CXTPPropertyGridItem(_T("Speed"),_T("Medium")));
		CXTPPropertyGridItemConstraints* pgicSpeed = m_pgiSpeed->GetConstraints();
		m_pgiAnimation->SetDescription(_T("Animate the Logicgrams."));
		m_pgiSpeed->SetDescription(_T("Select the Logicgram movement speed."));
		m_pgiAnimation->SetFlags(xtpGridItemHasComboButton);
		m_pgiSpeed->SetFlags(xtpGridItemHasComboButton);
		m_pgiLogicgrams->Expand();
		pgicSpeed->AddConstraint(_T("Slow"));
		pgicSpeed->AddConstraint(_T("Medium"));
		pgicSpeed->AddConstraint(_T("Fast"));

		m_pgiBits = m_wndPropertyGrid.AddCategory(_T("Bit Settings"));
		m_pgiBitLabels = m_pgiBits->AddChildItem(new CXTPPropertyGridItem(_T("Bit Labels"),_T("Bit")));
		m_pgiBitSequence = m_pgiBits->AddChildItem(new CXTPPropertyGridItem(_T("Bit Sequence"),_T("Reverse")));
		m_pgiBitDisplay = m_pgiBits->AddChildItem(new CXTPPropertyGridItem(_T("Bit Display"),_T("Black & White")));
		CXTPPropertyGridItemConstraints* pgicBitLabels = m_pgiBitLabels->GetConstraints();
		CXTPPropertyGridItemConstraints* pgicBitSequence = m_pgiBitSequence->GetConstraints();
		CXTPPropertyGridItemConstraints* pgicBitDisplay = m_pgiBitDisplay->GetConstraints();
		m_pgiBitLabels->SetDescription(_T("Select the active bit label set."));
		m_pgiBitSequence->SetDescription(_T("Select the active bit sequence."));
		m_pgiBitDisplay->SetDescription(_T("Select the bit display mode."));
		m_pgiBitLabels->SetFlags(xtpGridItemHasComboButton);
		m_pgiBitSequence->SetFlags(xtpGridItemHasComboButton);
		m_pgiBitDisplay->SetFlags(xtpGridItemHasComboButton);
		m_pgiBits->Expand();
		prgLB = ::GetLabelBlock1();
		for(int i = 0; i <= prgLB->GetUpperBound(); ++i)
			pgicBitLabels->AddConstraint(prgLB->GetAt(i)->GetName());
		prgSB = ::GetSequenceBlock1();
		for(int i = 0; i <= prgSB->GetUpperBound(); ++i)
			pgicBitSequence->AddConstraint(prgSB->GetAt(i)->GetName());
		pgicBitDisplay->AddConstraint(_T("Black & White"));
		pgicBitDisplay->AddConstraint(_T("Tarot Images"));

		m_pgiLines = m_wndPropertyGrid.AddCategory(_T("Line Settings"));
		m_pgiLineLabels = m_pgiLines->AddChildItem(new CXTPPropertyGridItem(_T("Line Labels"),_T("Line")));
		m_pgiLineSequence = m_pgiLines->AddChildItem(new CXTPPropertyGridItem(_T("Line Sequence"),_T("Natural")));
		m_pgiLineDisplay = m_pgiLines->AddChildItem(new CXTPPropertyGridItem(_T("Line Display"),_T("Grey Scale")));
		m_pgiLineFastRatio = m_pgiLines->AddChildItem(new CXTPPropertyGridItem(_T("Fast Ratio"),_T("1:3:3:1")));
		CXTPPropertyGridItemConstraints* pgicLineLabels = m_pgiLineLabels->GetConstraints();
		CXTPPropertyGridItemConstraints* pgicLineSequence = m_pgiLineSequence->GetConstraints();
		CXTPPropertyGridItemConstraints* pgicLineDisplay = m_pgiLineDisplay->GetConstraints();
		CXTPPropertyGridItemConstraints* pgicLineFastRatio = m_pgiLineFastRatio->GetConstraints();
		m_pgiLineLabels->SetDescription(_T("Select the active line label set."));
		m_pgiLineSequence->SetDescription(_T("Select the active line sequence."));
		m_pgiLineDisplay->SetDescription(_T("Select the line display mode."));
		m_pgiLineFastRatio->SetDescription(_T("Select the line fast movement ying / yang / young / old ratio."));
		m_pgiLineLabels->SetFlags(xtpGridItemHasComboButton);
		m_pgiLineSequence->SetFlags(xtpGridItemHasComboButton);
		m_pgiLineDisplay->SetFlags(xtpGridItemHasComboButton);
		m_pgiLineFastRatio->SetFlags(xtpGridItemHasComboButton);
		m_pgiLines->Expand();
		prgLB = ::GetLabelBlock2();
		for(int i = 0; i <= prgLB->GetUpperBound(); ++i)
			pgicLineLabels->AddConstraint(prgLB->GetAt(i)->GetName());
		prgSB = ::GetSequenceBlock2();
		for(int i = 0; i <= prgSB->GetUpperBound(); ++i)
			pgicLineSequence->AddConstraint(prgSB->GetAt(i)->GetName());
		pgicLineDisplay->AddConstraint(_T("Black"));
		pgicLineDisplay->AddConstraint(_T("Grey Scale"));
		pgicLineDisplay->AddConstraint(_T("Tarot Images"));
		pgicLineFastRatio->AddConstraint(_T("1:3:3:1"));

		m_pgiTrigrams = m_wndPropertyGrid.AddCategory(_T("Trigram Settings"));
		m_pgiTrigramLabels = m_pgiTrigrams->AddChildItem(new CXTPPropertyGridItem(_T("Trigram Labels"),_T("Trigram")));
		m_pgiTrigramSequence = m_pgiTrigrams->AddChildItem(new CXTPPropertyGridItem(_T("Trigram Sequence"),_T("Primal")));
		m_pgiTrigramDisplay = m_pgiTrigrams->AddChildItem(new CXTPPropertyGridItem(_T("Trigram Display"),_T("Colour")));
		CXTPPropertyGridItemConstraints* pgicTrigramLabels = m_pgiTrigramLabels->GetConstraints();
		CXTPPropertyGridItemConstraints* pgicTrigramSequence = m_pgiTrigramSequence->GetConstraints();
		CXTPPropertyGridItemConstraints* pgicTrigramDisplay = m_pgiTrigramDisplay->GetConstraints();
		m_pgiTrigramLabels->SetDescription(_T("Select the active trigram label set."));
		m_pgiTrigramSequence->SetDescription(_T("Select the active trigram sequence."));
		m_pgiTrigramDisplay->SetDescription(_T("Select the trigram display mode."));
		m_pgiTrigramLabels->SetFlags(xtpGridItemHasComboButton);
		m_pgiTrigramSequence->SetFlags(xtpGridItemHasComboButton);
		m_pgiTrigramDisplay->SetFlags(xtpGridItemHasComboButton);
		m_pgiTrigrams->Expand();
		prgLB = ::GetLabelBlock3();
		for(int i = 0; i <= prgLB->GetUpperBound(); ++i)
			pgicTrigramLabels->AddConstraint(prgLB->GetAt(i)->GetName());
		prgSB = ::GetSequenceBlock3();
		for(int i = 0; i <= prgSB->GetUpperBound(); ++i)
			pgicTrigramSequence->AddConstraint(prgSB->GetAt(i)->GetName());
		pgicTrigramDisplay->AddConstraint(_T("Black"));
		pgicTrigramDisplay->AddConstraint(_T("Colour"));
		pgicTrigramDisplay->AddConstraint(_T("Tarot Images"));

		m_pgiHexagrams = m_wndPropertyGrid.AddCategory(_T("Hexagram Settings"));
		m_pgiHexagramLabels = m_pgiHexagrams->AddChildItem(new CXTPPropertyGridItem(_T("Hexagram Labels"),_T("Vivash")));
		m_pgiHexagramSequence = m_pgiHexagrams->AddChildItem(new CXTPPropertyGridItem(_T("Hexagram Sequence"),_T("Wen")));
		m_pgiHexagramDisplay = m_pgiHexagrams->AddChildItem(new CXTPPropertyGridItem(_T("Hexagram Display"),_T("Colour")));
		CXTPPropertyGridItemConstraints* pgicHexagramLabels = m_pgiHexagramLabels->GetConstraints();
		CXTPPropertyGridItemConstraints* pgicHexagramSequence = m_pgiHexagramSequence->GetConstraints();
		CXTPPropertyGridItemConstraints* pgicHexagramDisplay = m_pgiHexagramDisplay->GetConstraints();
		m_pgiHexagramLabels->SetDescription(_T("Select the active hexagram label set."));
		m_pgiHexagramSequence->SetDescription(_T("Select the active hexagram sequence."));
		m_pgiHexagramDisplay->SetDescription(_T("Select the hexagram display mode."));
		m_pgiHexagramLabels->SetFlags(xtpGridItemHasComboButton);
		m_pgiHexagramSequence->SetFlags(xtpGridItemHasComboButton);
		m_pgiHexagramDisplay->SetFlags(xtpGridItemHasComboButton);
		m_pgiHexagrams->Expand();
		prgLB = ::GetLabelBlock6();
		for(int i = 0; i <= prgLB->GetUpperBound(); ++i)
			pgicHexagramLabels->AddConstraint(prgLB->GetAt(i)->GetName());
		prgSB = ::GetSequenceBlock6();
		for(int i = 0; i <= prgSB->GetUpperBound(); ++i)
			pgicHexagramSequence->AddConstraint(prgSB->GetAt(i)->GetName());
		pgicHexagramDisplay->AddConstraint(_T("Black"));
		pgicHexagramDisplay->AddConstraint(_T("Colour"));
		pgicHexagramDisplay->AddConstraint(_T("Tarot Images"));

		m_pgiText = m_wndPropertyGrid.AddCategory(_T("Text Settings"));
		m_pgiTextSource = m_pgiText->AddChildItem(new CXTPPropertyGridItem(_T("Source"),_T("Processed Wilhelm")));
		m_pgiDynamicHyperlink = m_pgiText->AddChildItem(new CXTPPropertyGridItemBool(_T("Dynamic Hyperlink"),true));
		m_pgiDesignation = m_pgiText->AddChildItem(new CXTPPropertyGridItem(_T("Designation"),_T("Superior Man")));
		CXTPPropertyGridItemConstraints* pgicTextSource = m_pgiTextSource->GetConstraints();
		CXTPPropertyGridItemConstraints* pgicDesignation = m_pgiDesignation->GetConstraints();
		m_pgiTextSource->SetDescription(_T("Select the text source."));
		m_pgiDynamicHyperlink->SetDescription(_T("Dynamicly hyperlink local text."));
		m_pgiDesignation->SetDescription(_T("Select the designation."));
		m_pgiTextSource->SetFlags(xtpGridItemHasComboButton);
		m_pgiDynamicHyperlink->SetFlags(xtpGridItemHasComboButton);
		m_pgiDesignation->SetFlags(xtpGridItemHasComboButton);
		m_pgiText->Expand();
		pgicTextSource->AddConstraint(_T("Processed Wilhelm"));
		pgicTextSource->AddConstraint(_T("Wilhelm"));
		pgicTextSource->AddConstraint(_T("Legge"));
		pgicTextSource->AddConstraint(_T("Andrade"));
		pgicTextSource->AddConstraint(_T("Heyboer"));
		pgicTextSource->AddConstraint(_T("Harvard Yenching"));
		pgicTextSource->AddConstraint(_T("YellowBridge"));
		pgicTextSource->AddConstraint(_T("Stackhouse"));

		pgicDesignation->AddConstraint(_T("Superior Man"));
		pgicDesignation->AddConstraint(_T("Noble One"));
		pgicDesignation->AddConstraint(_T("Saintly Person"));
		pgicDesignation->AddConstraint(_T("Celestial Master")); 
		pgicDesignation->AddConstraint(_T("Disciple of Wisdom"));

		m_wndPropertyGrid.HighlightChangedItems(TRUE);
	}
	m_bInitialised = true;
}

void CPropertiesPane::OnSize(UINT nType, int cx, int cy)
{
	CWnd::OnSize(nType, cx, cy);
	int nTop = 0;
	if(m_wndToolBar.GetSafeHwnd())
	{
		CSize sz = m_wndToolBar.CalcDockingLayout(cx,/*LM_HIDEWRAP |*/ LM_HORZDOCK | LM_HORZ | LM_COMMIT);
		m_wndToolBar.MoveWindow(0, nTop, cx, sz.cy);
		m_wndToolBar.Invalidate(FALSE);
		nTop += sz.cy;
	}
	if(m_wndPropertyGrid.GetSafeHwnd())
	{
		m_wndPropertyGrid.MoveWindow(0, nTop, cx, cy - nTop);
		m_wndPropertyGrid.Invalidate(FALSE);
	}
}

LRESULT CPropertiesPane::OnGridNotify(WPARAM wParam, LPARAM lParam)
{
	if(m_bInitialised)
	if(wParam == XTP_PGN_ITEMVALUE_CHANGED)
	{
		CXTPPropertyGridItem* pItem = (CXTPPropertyGridItem*) lParam;
		if(pItem->GetCaption() == _T("Animation"))
		{
			if(pItem->GetValue() == _T("True"))
			{
				::GetExplorationCtrl()->GetHexagramSequencerCtrl()->SetDirection(eNext);
				::GetExplorationCtrl()->GetHexagramSequencerCtrl()->OnLButtonDown(-999,CPoint(0,0));
			}
			else
				::GetExplorationCtrl()->GetHexagramSequencerCtrl()->OnLButtonUp(0,CPoint(0,0));
			return 0;
		}
		else
		if(pItem->GetCaption() == _T("Speed"))
		{
			if(pItem->GetValue() == _T("Slow"))
				m_nSpeed = 1000;
			else
			if(pItem->GetValue() == _T("Medium"))
				m_nSpeed = 500;
			else
			if(pItem->GetValue() == _T("Fast"))
				m_nSpeed = 10;
			if(::GetPropertyCtrl()->GetAnimation())
				::GetExplorationCtrl()->GetHexagramSequencerCtrl()->OnLButtonDown(-999,CPoint(0,0));
//			::GetApp()->WriteProfileInt(_T("Settings"),_T("Speed"),1000);
			return 0;
		}
		else
		if(pItem->GetCaption() == _T("Bit Display"))
			::GetExplorationCtrl()->UpdateBits(pItem->GetValue() == _T("Tarot Images"));
		else
		if(pItem->GetCaption() == _T("Line Display"))
			::GetExplorationCtrl()->UpdateLines(pItem->GetValue() == _T("Tarot Images"));
		else
		if(pItem->GetCaption() == _T("Trigram Display"))
			::GetExplorationCtrl()->UpdateTrigrams(pItem->GetValue() == _T("Tarot Images"));
		else
		if(pItem->GetCaption() == _T("Hexagram Display"))
		{
			::GetExplorationCtrl()->UpdateHexagrams(pItem->GetValue() == _T("Tarot Images"));
			::GetLogicCtrl()->UpdateHexagrams(pItem->GetValue() == _T("Tarot Images"));
			::GetApp()->UpdateLogicCtrl();
//			::GetApp()->WriteProfileInt(_T("Settings"),_T("HexagramImages"),m_bHexagramImages ? 1 : 0);
		}
		::GetApp()->UpdateAllViews();
	}
	return 0;
}

void CPropertiesPane::OnPanePropertiesCategorized()
{
	m_wndPropertyGrid.SetPropertySort(xtpGridSortCategorized);
}

void CPropertiesPane::OnUpdatePanePropertiesCategorized(CCmdUI* pCmdUI)
{
	pCmdUI->SetCheck(m_wndPropertyGrid.GetPropertySort() == xtpGridSortCategorized);
}

void CPropertiesPane::OnPanePropertiesAlphabetic()
{
	m_wndPropertyGrid.SetPropertySort(xtpGridSortAlphabetical);
}

void CPropertiesPane::OnUpdatePanePropertiesAlphabetic(CCmdUI* pCmdUI)
{
	pCmdUI->SetCheck(m_wndPropertyGrid.GetPropertySort() == xtpGridSortAlphabetical);
}

void CPropertiesPane::OnSetFocus(CWnd* /*pOldWnd*/)
{
	m_wndPropertyGrid.SetFocus();
}
/*
	m_bAnimation = false; // ::GetApp()->GetProfileInt(_T("Settings"),_T("Animation"),0) == 1;
	m_nSpeed = ::GetApp()->GetProfileInt(_T("Settings"),_T("Speed"),400);
	m_bColour = ::GetApp()->GetProfileInt(_T("Settings"),_T("Colour"),1) == 1;
	m_bBitImages = ::GetApp()->GetProfileInt(_T("Settings"),_T("BitImages"),0) == 1;
	m_bLineImages = ::GetApp()->GetProfileInt(_T("Settings"),_T("LineImages"),0) == 1;
	m_bTrigramImages = ::GetApp()->GetProfileInt(_T("Settings"),_T("TrigramImages"),0) == 1;
	m_bHexagramImages = ::GetApp()->GetProfileInt(_T("Settings"),_T("HexagramImages"),0) == 1;

void CMainFrame::OnColour()
{
	m_bColour = !m_bColour;
	::GetApp()->WriteProfileInt(_T("Settings"),_T("Colour"),m_bColour ? 1 : 0);
	::GetExplorationCtrl()->UpdateBits(m_bBitImages);
	::GetExplorationCtrl()->UpdateLines(m_bLineImages);
	::GetExplorationCtrl()->UpdateTrigrams(m_bTrigramImages);
	::GetExplorationCtrl()->UpdateHexagrams(m_bHexagramImages);
	::GetApp()->UpdateExplorationCtrl();
	::GetLogicCtrl()->UpdateHexagrams(m_bHexagramImages);
	::GetApp()->UpdateLogicCtrl();
}
*/


void CPropertiesPane::SetAnimation(LPCTSTR lpszAnimation) 
{
	m_pgiAnimation->SetValue(lpszAnimation); 
	if(_tcscmp(lpszAnimation,_T("True")) == 0)
	{
		::GetExplorationCtrl()->GetHexagramSequencerCtrl()->SetDirection(eNext);
		::GetExplorationCtrl()->GetHexagramSequencerCtrl()->OnLButtonDown(-999,CPoint(0,0));
	}
	else
		::GetExplorationCtrl()->GetHexagramSequencerCtrl()->OnLButtonUp(0,CPoint(0,0));
}

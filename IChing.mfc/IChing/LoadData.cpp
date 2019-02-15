#include "StdAfx.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

enum TextState {tsNone,tsText,tsJudgment,tsImage,tsLines,tsLine1,tsLine2,tsLine3,tsLine4,
	tsLine5,tsLine6,tsWansExplanation,tsWansExplanationText,tsKausExplanation,tsKausExplanationText};

CLoadData::CLoadData()
{
}

CLoadData::~CLoadData()
{
}

//http://www.yijing.nl/i_ching/hex_1-16/hex_e_01.htm

void CLoadData::LoadAll()
{
//	LoadNewTables();
//	LoadNewData();

//	ConvertNewBB();
//	ConvertOldBB();

//	CleanBB(_T("Wilhelm"));
//	LoadWilhelmText2BB(); 

//	CleanBB(_T("Legge"));
//	LoadLeggeText2BB(); 

//	CleanBB(_T("Vivash"));
//	CopyWilhelmBB2VivashBB();

//	LoadNamedSequences();
//	LoadNumberedSequences();
//	AddSequenceTables();

//	ConvertLabelTables();

//	GenerateLsl();
//	GenerateXml();

	GenerateCSharp();
}

void CLoadData::LoadNewTables()
{
	CTextColumnSet TCS;
	CString strTemp1,strTemp2,strTemp3;

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("drop table Configuration"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("create table Configuration (Name Text, Value1 Text)"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("alter table Configuration add constraint pkConfiguration primary key (Name)")); 
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Configuration (Name, Value1) values('Schema Version','1.0.0')"));

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("drop table Consultation"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("create table Consultation (Id Integer, ParentId Integer, Title Text, Type Text, Label Text, Source Text, Question Text, Notes Text, Result Text)"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("alter table Consultation add constraint pkConsultation primary key (Id)")); 

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Consultation (Id, ParentId, Title, Type, Label, Source, Question, Notes, Result) values(1,0,'Title1','Relationship','Vivash','Processed Wilhelm','Question1','Notes1','1.1.2')"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Consultation (Id, ParentId, Title, Type, Label, Source, Question, Notes, Result) values(2,1,'Title2','Relationship','Vivash','Processed Wilhelm','Question2','Notes2','2.1.2')"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Consultation (Id, ParentId, Title, Type, Label, Source, Question, Notes, Result) values(3,1,'Title2','Relationship','Vivash','Processed Wilhelm','Question3','Notes3','3.1.2')"));

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Consultation (Id, ParentId, Title, Type, Label, Source, Question, Notes, Result) values(4,0,'Title4','Business','Karcher','Stackhouse','Question4','Notes4','4.1.2')"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Consultation (Id, ParentId, Title, Type, Label, Source, Question, Notes, Result) values(5,4,'Title5','Business','Karcher','Stackhouse','Question5','Notes5','5.1.2')"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Consultation (Id, ParentId, Title, Type, Label, Source, Question, Notes, Result) values(6,4,'Title6','Business','Karcher','Stackhouse','Question6','Notes6','6.1.2')"));
}

int nGetoJack[64] = 
{
	 1,58,30,51,57,29,52, 2,
	43,10,55,21,48,59,15,23,
	14,54,13,17,18, 7,53, 8,
	34,38,49,25,46, 4,39,20,
	 9,60,22,24,44,47,56,16,
	 5,61,36,27,28, 6,62,35,
	26,19,37, 3,50,40,33,45,
	11,41,63,42,32,64,31,12
};

int FindGetoJackIndex(int nSequence)
{
	for(int i = 0; i < 64; ++i)
		if(nGetoJack[i] == nSequence)
			return i;
	return -1;
}

TCHAR szLatticePath[64][7] = {
	"000000","100000","110000","010000","011000","111000","101000","001000",
	"001100","101100","111100","011100","010100","110100","100100","000100",
	"000110","100110","110110","010110","011110","111110","101110","001110",
	"001010","101010","111010","011010","010010","110010","100010","000010",
	"000011","100011","110011","010011","011011","111011","101011","001011",
	"001111","101111","111111","011111","010111","110111","100111","000111",
	"000101","100101","110101","010101","011101","111101","101101","001101",
	"001001","101001","111001","011001","010001","110001","100001","000001"
};

TCHAR szShaoYung[64][7] = {
	"000000","000001","000010","000011","000100","000101","000110","000111",
	"001000","001001","001010","001011","001100","001101","001110","001111",
	"010000","010001","010010","010011","010100","010101","010110","010111",
	"011000","011001","011010","011011","011100","011101","011110","011111",
	"100000","100001","100010","100011","100100","100101","100110","100111",
	"101000","101001","101010","101011","101100","101101","101110","101111",
	"110000","110001","110010","110011","110100","110101","110110","110111",
	"111000","111001","111010","111011","111100","111101","111110","111111"
};

CString strRitsema[64] = {
	"Energy",					
	"Space",					
	"Sprouting",				
	"Envelopment",				
	"Attending",				
	"Arguing",					
	"The Legions",				
	"Grouping",				
	"The Small Accumulating",  
	"Treading",				
	"Compenetration",			
	"Obstruction",				
	"Concording People",		
	"The Great Possessing",	
	"Humbling",				
	"Providing",				
	"Following",				
	"Decay",					
	"Nearing",					
	"Overseeing",				
	"Gnawing and Biting",		
	"Adorning",				
	"Stripping",				
	"Return",					
	"Without Entanglement",	
	"The Great Accumulating",  
	"The Jaws",				
	"The Great Exceeding",		
	"The Gorge",				
	"The Radiance",			
	"Conjunction",				
	"Persevering",				
	"Retiring",				
	"The Great''s Vigor",		
	"Prospering",				
	"Brightness Hidden",		
	"Household People",		
	"Polarizing",				
	"Limping",					
	"Unraveling",				
	"Diminishing",				
	"Augmenting",				
	"Parting",					
	"Coupling",				
	"Clustering",				
	"Ascending",				
	"Confinement",				
	"The Well",				
	"Skinning",				
	"The Vessel",				
	"The Shake",				
	"The Bound",				
	"Infiltrating",			
	"Converting Maidenhood",	
	"Abounding",				
	"Sojourning",				
	"The Root",				
	"The Open",				
	"Dispersing",				
	"Articulating",			
	"The Center Conforming",	
	"The Small Exceeding",		
	"Already Fording",			
	"Not Yet Fording"
};

CString strRutt[64] = {
	"Active", 
	"Earth", 
	"Massed", 
	"Dodder", 
	"Waiting", 
	"Dispute", 
	"Troops", 
	"Joining", 
	"Farming: Minor", 
	"Stepping", 
	"Great", 
	"Bad", 
	"Mustering", 
	"Large, There", 
	"Rat", 
	"Elephant", 
	"Pursuit", 
	"Mildew", 
	"Keening", 
	"Observing", 
	"Biting", 
	"Bedight", 
	"Flaying", 
	"Returning", 
	"Unexpected", 
	"Farming: Major", 
	"Molars", 
	"Passing: Major", 
	"Pit", 
	"Oriole", 
	"Chopping", 
	"Fixing", 
	"Pig", 
	"Big Injury", 
	"Advancing", 
	"Crying Pheasant", 
	"Household", 
	"Espy", 
	"Stumbling", 
	"Unloosing", 
	"Diminishing", 
	"Enriching", 
	"Skipping", 
	"Locking", 
	"Together", 
	"Going Up", 
	"Beset", 
	"Well", 
	"Leather", 
	"Tripod-Bowl", 
	"Thunder", 
	"Cleaving", 
	"Settling", 
	"Marriage", 
	"Thick", 
	"Sojourner", 
	"Food Offerings", 
	"Satisfaction", 
	"Gushing", 
	"Juncture", 
	"Trying Captives", 
	"Passing: Minor", 
	"Already Across", 
	"Not Yet Across" 
};

void CLoadData::LoadNewData()
{
	int nValue,nSequence;
	CString strTemp1,strTemp2,strTemp3;
	CTextColumnSet TCS;
	CSequenceBlockArray* prgSB6 = ::GetApp()->GetSequenceBlock6();
	CSequenceEntryArray* prgSE6 = prgSB6->GetAt(2)->GetEntryArray();

	int nPosition[6] = {1,2,4,8,16,32};
	int nValues[64];

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("delete from Sequence6 where Name = 'Lattice Path'"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Sequence6 (Sequence, Name) values(900,'Lattice Path')"));
	for(int i = 0; i < 64; ++i)
	{
		nValues[i] = 0;
		for(int j = 0; j < 6; ++j)
			nValues[i] += (szLatticePath[i][j] == '1' ? 1 : 0) * nPosition[j];
	}
	strTemp3 = _T("update Sequence6 set");
	for(int i = 0; i < 64; ++i)
	{
		nValue = prgSE6->GetAt(i)->GetValue();
		nSequence = prgSE6->GetAt(i)->GetSequence();
		strTemp2.Format(_T(" S%d = %d%s "),nValues[i],i,i == 63 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'Lattice Path'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("delete from Sequence6 where Name = 'Shao Yung'"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Sequence6 (Sequence, Name) values(1000,'Shao Yung')"));
	for(int i = 0; i < 64; ++i)
	{
		nValues[i] = 0;
		for(int j = 0; j < 6; ++j)
			nValues[i] += (szShaoYung[i][j] == '1' ? 1 : 0) * nPosition[j];
	}
	strTemp3 = _T("update Sequence6 set");
	for(int i = 0; i < 64; ++i)
	{
		nValue = prgSE6->GetAt(i)->GetValue();
		nSequence = prgSE6->GetAt(i)->GetSequence();
		strTemp2.Format(_T(" S%d = %d%s "),nValues[i],i,i == 63 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'Shao Yung'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("delete from Label6 where Name = 'Ritsema'"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Label6 (Sequence, Name) values(800,'Ritsema')"));
	strTemp3 = _T("update Label6 set");
	for(int i = 0; i < 64; ++i)
	{
		nValue = prgSE6->GetAt(i)->GetValue();
		nSequence = prgSE6->GetAt(i)->GetSequence();
		strTemp2.Format(_T(" V%d = '%s'%s "),i,strRitsema[nSequence],i == 63 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'Ritsema'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("delete from Label6 where Name = 'Rutt'"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Label6 (Sequence, Name) values(900,'Rutt')"));
	strTemp3 = _T("update Label6 set");
	for(int i = 0; i < 64; ++i)
	{
		nValue = prgSE6->GetAt(i)->GetValue();
		nSequence = prgSE6->GetAt(i)->GetSequence();
		strTemp2.Format(_T(" V%d = '%s'%s "),i,strRutt[nSequence],i == 63 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'Rutt'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);
}

//	http://www.sacred-texts.com/ich/ic01.htm

/*
void CLoadData::LoadNewData()
{
	CString strTemp1,strTemp2;
	CSequenceBlockArray* prgSB6 = ::GetApp()->GetSequenceBlock6();
	CSequenceEntryArray* prgSE6;
	CStdioFile F;

	if(F.Open(_T("C:\\Tmp\\Sequences.txt"),CFile::modeCreate | CFile::modeWrite | CFile::typeText))
	{
		for(int i = 0; i < 9; ++i)
		{
			prgSE6 = prgSB6->GetAt(i)->GetEntryArray();
			strTemp1 = CString(_T("\n")) + prgSB6->GetAt(i)->GetName() + CString(_T("\n"));
			for(int j = 0; j < 64; ++j)
			{
				if((j != 0) && ((j % 8) == 0))
					strTemp1 += _T("\n");
				if(prgSE6->GetAt(j)->GetSequence() == -1)
					strTemp2 = _T("  .");
				else
					strTemp2.Format(_T(" %2d"),prgSE6->GetAt(j)->GetSequence());
				strTemp1 += strTemp2;
			}
			strTemp1 += _T("\n");
			F.WriteString(strTemp1);
		}
		F.Close();
	}

	int nValue,nSequence;
	CString strTemp1,strTemp2,strTemp3;
	CTextColumnSet TCS;
	CSequenceBlockArray* prgSB6 = ::GetApp()->GetSequenceBlock6();
	CSequenceEntryArray* prgSE6 = prgSB6->GetAt(2)->GetEntryArray();

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("delete from Sequence6 where Name = 'GetoJack'"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Sequence6 (Sequence, Name) values(720,'GetoJack')"));

	strTemp3 = _T("update Sequence6 set");
	for(int i = 0; i < 64; ++i)
	{
		nValue = prgSE6->GetAt(i)->GetValue();
		nSequence = prgSE6->GetAt(i)->GetSequence();
		strTemp2.Format(_T(" S%d = %d%s "),i,FindGetoJackIndex(nSequence + 1),i == 63 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'GetoJack'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);
}

void CLoadData::LoadNewData()
{
	CTextColumnSet TCS;
	int nSequence;
	CString strTemp1,strTemp2,strTemp3;
	CSequenceBlockArray* prgSB6 = ::GetApp()->GetSequenceBlock6();
	CSequenceEntryArray* prgSE6 = prgSB6->GetAt(2)->GetEntryArray();

	CString strHarvardYenching[64] = {
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi1.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi2.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi3.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi4.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi5.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi6.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi7.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi8.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi9.html"),

		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi10.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi11.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi12.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi13.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi14.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi15.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi16.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi17.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi18.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi19.html"),

		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi20.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi21.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi22.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi23.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi24.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi25.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi26.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi27.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi28.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi29.html"),

		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi30.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi31.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi32.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi33.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi34.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi35.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi36.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi37.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi38.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi39.html"),

		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi40.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi41.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi42.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi43.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi44.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi45.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi46.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi47.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi48.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi49.html"),

		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi50.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi51.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi52.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi53.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi54.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi55.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi56.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi57.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi58.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi59.html"),

		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi60.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi61.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi62.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi63.html"),
		_T("http://www.harvard-yenching.net/ruxue/zhuzuo/zhouyi/zhouyi64.html"),
	};

	CString strYellowBridge[64] = {
		_T("http://www.yellowbridge.com/onlinelit/yijing01.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing02.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing03.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing04.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing05.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing06.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing07.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing08.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing09.php"),

		_T("http://www.yellowbridge.com/onlinelit/yijing10.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing11.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing12.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing13.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing14.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing15.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing16.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing17.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing18.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing19.php"),

		_T("http://www.yellowbridge.com/onlinelit/yijing20.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing21.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing22.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing23.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing24.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing25.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing26.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing27.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing28.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing29.php"),

		_T("http://www.yellowbridge.com/onlinelit/yijing30.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing31.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing32.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing33.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing34.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing35.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing36.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing37.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing38.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing39.php"),

		_T("http://www.yellowbridge.com/onlinelit/yijing40.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing41.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing42.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing43.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing44.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing45.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing46.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing47.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing48.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing49.php"),

		_T("http://www.yellowbridge.com/onlinelit/yijing50.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing51.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing52.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing53.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing54.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing55.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing56.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing57.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing58.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing59.php"),

		_T("http://www.yellowbridge.com/onlinelit/yijing60.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing61.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing62.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing63.php"),
		_T("http://www.yellowbridge.com/onlinelit/yijing64.php"),
	};

	CString strStackhouse[64] = {
		_T("http://208.109.67.174/iching/hex01.html"),
		_T("http://208.109.67.174/iching/hex02.html"),
		_T("http://208.109.67.174/iching/hex03.html"),
		_T("http://208.109.67.174/iching/hex04.html"),
		_T("http://208.109.67.174/iching/hex05.html"),
		_T("http://208.109.67.174/iching/hex06.html"),
		_T("http://208.109.67.174/iching/hex07.html"),
		_T("http://208.109.67.174/iching/hex08.html"),
		_T("http://208.109.67.174/iching/hex09.html"),

		_T("http://208.109.67.174/iching/hex10.html"),
		_T("http://208.109.67.174/iching/hex11.html"),
		_T("http://208.109.67.174/iching/hex12.html"),
		_T("http://208.109.67.174/iching/hex13.html"),
		_T("http://208.109.67.174/iching/hex14.html"),
		_T("http://208.109.67.174/iching/hex15.html"),
		_T("http://208.109.67.174/iching/hex16.html"),
		_T("http://208.109.67.174/iching/hex17.html"),
		_T("http://208.109.67.174/iching/hex18.html"),
		_T("http://208.109.67.174/iching/hex19.html"),

		_T("http://208.109.67.174/iching/hex20.html"),
		_T("http://208.109.67.174/iching/hex21.html"),
		_T("http://208.109.67.174/iching/hex22.html"),
		_T("http://208.109.67.174/iching/hex23.html"),
		_T("http://208.109.67.174/iching/hex24.html"),
		_T("http://208.109.67.174/iching/hex25.html"),
		_T("http://208.109.67.174/iching/hex26.html"),
		_T("http://208.109.67.174/iching/hex27.html"),
		_T("http://208.109.67.174/iching/hex28.html"),
		_T("http://208.109.67.174/iching/hex29.html"),

		_T("http://208.109.67.174/iching/hex30.html"),
		_T("http://208.109.67.174/iching/hex31.html"),
		_T("http://208.109.67.174/iching/hex32.html"),
		_T("http://208.109.67.174/iching/hex33.html"),
		_T("http://208.109.67.174/iching/hex34.html"),
		_T("http://208.109.67.174/iching/hex35.html"),
		_T("http://208.109.67.174/iching/hex36.html"),
		_T("http://208.109.67.174/iching/hex37.html"),
		_T("http://208.109.67.174/iching/hex38.html"),
		_T("http://208.109.67.174/iching/hex39.html"),

		_T("http://208.109.67.174/iching/hex40.html"),
		_T("http://208.109.67.174/iching/hex41.html"),
		_T("http://208.109.67.174/iching/hex42.html"),
		_T("http://208.109.67.174/iching/hex43.html"),
		_T("http://208.109.67.174/iching/hex44.html"),
		_T("http://208.109.67.174/iching/hex45.html"),
		_T("http://208.109.67.174/iching/hex46.html"),
		_T("http://208.109.67.174/iching/hex47.html"),
		_T("http://208.109.67.174/iching/hex48.html"),
		_T("http://208.109.67.174/iching/hex49.html"),

		_T("http://208.109.67.174/iching/hex50.html"),
		_T("http://208.109.67.174/iching/hex51.html"),
		_T("http://208.109.67.174/iching/hex52.html"),
		_T("http://208.109.67.174/iching/hex53.html"),
		_T("http://208.109.67.174/iching/hex54.html"),
		_T("http://208.109.67.174/iching/hex55.html"),
		_T("http://208.109.67.174/iching/hex56.html"),
		_T("http://208.109.67.174/iching/hex57.html"),
		_T("http://208.109.67.174/iching/hex58.html"),
		_T("http://208.109.67.174/iching/hex59.html"),

		_T("http://208.109.67.174/iching/hex60.html"),
		_T("http://208.109.67.174/iching/hex61.html"),
		_T("http://208.109.67.174/iching/hex62.html"),
		_T("http://208.109.67.174/iching/hex63.html"),
		_T("http://208.109.67.174/iching/hex64.html"),
	};

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("delete from Text6 where Name = 'Harvard Yenching'"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Text6 (Type, Sequence, Name) values('Text',600,'Harvard Yenching')"));

	strTemp3 = _T("update Text6 set");
	for(int i = 0; i < 64; ++i)
	{
		nSequence = prgSE6->GetAt(i)->GetSequence();
		strTemp2.Format(_T(" V%d = '%s'%s "),i,strHarvardYenching[nSequence],i == 63 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'Harvard Yenching' and Type = 'Text'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("delete from Text6 where Name = 'YellowBridge'"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Text6 (Type, Sequence, Name) values('Text',700,'YellowBridge')"));

	strTemp3 = _T("update Text6 set");
	for(int i = 0; i < 64; ++i)
	{
		nSequence = prgSE6->GetAt(i)->GetSequence();
		strTemp2.Format(_T(" V%d = '%s'%s "),i,strYellowBridge[nSequence],i == 63 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'YellowBridge' and Type = 'Text'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("delete from Text6 where Name = 'Stackhouse'"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Text6 (Type, Sequence, Name) values('Text',800,'Stackhouse')"));

	strTemp3 = _T("update Text6 set");
	for(int i = 0; i < 64; ++i)
	{
		nSequence = prgSE6->GetAt(i)->GetSequence();
		strTemp2.Format(_T(" V%d = '%s'%s "),i,strStackhouse[nSequence],i == 63 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'Stackhouse' and Type = 'Text'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);
}

void CLoadData::ConvertNewBB()
{
//	CTextColumnSet TCS;
//	TCS.Execute(::GetApp()->GetBeliefBase(),_T("drop table Test1"));
//	TCS.Execute(::GetApp()->GetBeliefBase(),_T("drop table TextOptions"));
//	TCS.Execute(::GetApp()->GetBeliefBase(),_T("update Text6 set Name = 'Processed Wilhelm' where Name = 'Vivash'"));
}

void CLoadData::CleanBB(LPCTSTR lpszName)
{
	for(int nHexagram = 0; nHexagram < 64; ++nHexagram)
	{
		CTextColumnSet TCS;
		TCS.Execute(::GetApp()->GetBeliefBase(),_T("update Text6 set V%d = 'yyy' where Name = '%s'"),nHexagram,lpszName);
	}
}

void CLoadData::LoadWilhelmText2BB()
{
	CLabelBlockArray* prgLB6 = ::GetApp()->GetLabelBlock6();
	CLabelEntryArray* prgLE6 = prgLB6->GetAt(3)->GetEntryArray();
	CString strTemp1,strTemp2,strFilename;
	CTextSet6 TS6;
	TextState eTS;
	CStdioFile F;

	CString strText = _T("*****Hexagram %d");
	CString strJudgment = _T("THE JUDGMENT");
	CString strImage = _T("THE IMAGE");
	CString strLines = _T("THE LINES");

	CString strLine1 = _T("at the beginning means:");
	CString strLine2 = _T("in the second place means:");
	CString strLine3 = _T("in the third place means:");
	CString strLine4 = _T("in the fourth place means:");
	CString strLine5 = _T("in the fifth place means:");
	CString strLine6 = _T("at the top means:");

	bool bCommentry = false;
	bool bParagraph = false;

	int nHexagramRead = -1;
	int nHexagramCount = -1;
	int nLastHexagram = -1;
	int nHexagramValue = -1;
	int nHexagramValues[64] = {
		-1,-1,-1,-1,-1,-1,-1,-1,
		-1,-1,-1,-1,-1,-1,-1,-1,
		-1,-1,-1,-1,-1,-1,-1,-1,
		-1,-1,-1,-1,-1,-1,-1,-1,
		-1,-1,-1,-1,-1,-1,-1,-1,
		-1,-1,-1,-1,-1,-1,-1,-1,
		-1,-1,-1,-1,-1,-1,-1,-1,
		-1,-1,-1,-1,-1,-1,-1,-1,
	};

	strFilename.Format(_T("C:\\Src1\\IChing\\Resources\\Text\\Wilhelm\\Akira Rabelais  Book of Changes.txt"));
	if(F.Open(strFilename,CFile::modeRead))
	{
		eTS = tsLine6;
		while(F.ReadString(strTemp1))
		{
			if(strTemp1.GetLength() > 1)
				if(((eTS == tsText) || bCommentry) && (strTemp1[0] == _T(' ')))
					bParagraph = true;
			strTemp1.Trim();
			if(strTemp1.IsEmpty())
				if(!strTemp2.IsEmpty())
				{
					if(eTS == tsText)
						bCommentry = false;
					else
						if(!bCommentry)
						{
							bCommentry = true;
							strTemp2 += _T("<br/>");
							continue;
						}
						else
							bCommentry = false;

					strTemp2 = ReplaceString(strTemp2,_T("''"),_T("'"));
					switch(eTS)
					{
					case tsText:
						if(nLastHexagram + 1 == nHexagramCount)
						{
							TS6.Execute(::GetApp()->GetBeliefBase(),
								_T("update Text6 set V%d = '%s' where Name = 'Wilhelm' and Type = 'Text'"),
								nHexagramValue,strTemp2);
						}
						else
							ASSERT(FALSE);
						nLastHexagram = nHexagramCount;
						break;
					case tsJudgment:
						TS6.Execute(::GetApp()->GetBeliefBase(),
							_T("update Text6 set V%d = '%s' where Name = 'Wilhelm' and Type = 'Judgment'"),
							nHexagramValue,strTemp2);
						break;
					case tsImage:
						TS6.Execute(::GetApp()->GetBeliefBase(),
							_T("update Text6 set V%d = '%s' where Name = 'Wilhelm' and Type = 'Image'"),
							nHexagramValue,strTemp2);
						break;
					case tsLine1:
						TS6.Execute(::GetApp()->GetBeliefBase(),
							_T("update Text6 set V%d = '%s' where Name = 'Wilhelm' and Type = 'Line0'"),
							nHexagramValue,strTemp2);
						break;
					case tsLine2:
						TS6.Execute(::GetApp()->GetBeliefBase(),
							_T("update Text6 set V%d = '%s' where Name = 'Wilhelm' and Type = 'Line1'"),
							nHexagramValue,strTemp2);
						break;
					case tsLine3:
						TS6.Execute(::GetApp()->GetBeliefBase(),
							_T("update Text6 set V%d = '%s' where Name = 'Wilhelm' and Type = 'Line2'"),
							nHexagramValue,strTemp2);
						break;
					case tsLine4:
						TS6.Execute(::GetApp()->GetBeliefBase(),
							_T("update Text6 set V%d = '%s' where Name = 'Wilhelm' and Type = 'Line3'"),
							nHexagramValue,strTemp2);
						break;
					case tsLine5:
						TS6.Execute(::GetApp()->GetBeliefBase(),
							_T("update Text6 set V%d = '%s' where Name = 'Wilhelm' and Type = 'Line4'"),
							nHexagramValue,strTemp2);
						break;
					case tsLine6:
						TS6.Execute(::GetApp()->GetBeliefBase(),
							_T("update Text6 set V%d = '%s' where Name = 'Wilhelm' and Type = 'Line5'"),
							nHexagramValue,strTemp2);
						break;
					}
					strTemp2.Empty();
				}
				else
					/ *ASSERT(FALSE)* /;
			else
			if(_stscanf(strTemp1,strText,&nHexagramRead) == 1)
				if(eTS == tsLine6)
				{
					if(nHexagramRead - 1 == ++nHexagramCount)
						nHexagramValue = prgLE6->FindValue(nHexagramCount);
					else
						ASSERT(FALSE);
					if(nHexagramValues[nHexagramValue] == -1)
						nHexagramValues[nHexagramValue] = nHexagramCount;
					else
						ASSERT(FALSE);
					eTS = tsText;
				}
				else
					ASSERT(FALSE);
			else
			if(_tcscmp(strTemp1,strJudgment) == 0)
				if(eTS == tsText)
					eTS = tsJudgment;
				else
					ASSERT(FALSE);
			else
			if(_tcscmp(strTemp1,strImage) == 0)
				if(eTS == tsJudgment)
					eTS = tsImage;
				else
					ASSERT(FALSE);
			else
			if(StrStrI(strTemp1,strLine1) != NULL)
				if(eTS == tsImage)
					eTS = tsLine1;
				else
					ASSERT(FALSE);
			else
			if(StrStrI(strTemp1,strLine2) != NULL)
				if(eTS == tsLine1)
					eTS = tsLine2;
				else
					ASSERT(FALSE);
			else
			if(StrStrI(strTemp1,strLine3) != NULL)
				if(eTS == tsLine2)
					eTS = tsLine3;
				else
					ASSERT(FALSE);
			else
			if(StrStrI(strTemp1,strLine4) != NULL)
				if(eTS == tsLine3)
					eTS = tsLine4;
				else
					ASSERT(FALSE);
			else
			if(StrStrI(strTemp1,strLine5) != NULL)
				if(eTS == tsLine4)
					eTS = tsLine5;
				else
					ASSERT(FALSE);
			else
			if(StrStrI(strTemp1,strLine6) != NULL)
				if(eTS == tsLine5)
					eTS = tsLine6;
				else
					ASSERT(FALSE);
			else
			{
				if(bParagraph)
					strTemp2 += _T("<br/><br/>");
				strTemp2 += (strTemp2.IsEmpty() || bParagraph ? _T("") : _T(" ")) + strTemp1;
				if((eTS != tsText) && !bCommentry)
					strTemp2 += _T("<br/>");
				bParagraph = false;
			}
		}
		TS6.Execute(::GetApp()->GetBeliefBase(),
			_T("update Text6 set V%d = '%s' where Name = 'Wilhelm' and Type = 'Line5'"),
			nHexagramValue,strTemp2);
		F.Close();
	}
}

void CLoadData::LoadLeggeText2BB()
{
	TCHAR szTemp[100];
	CString strTemp1,strTemp2,strFilename;
	CString strText,strLine1,strLine2,strLine3,strLine4,strLine5,strLine6;
	CTextSet6 TS6;
	TextState eTS;
	CStdioFile F;

	CString strStart = _T("**********");
	CString strLines = _T("THE LINES");
	CString strWansExplanation = _T("EXPLANATION OF THE HEXAGRAM");
	CString strKausExplanation = _T("EXPLANATION OF THE LINES");

	for(int nHexagram = 0; nHexagram < 64; ++nHexagram)
	{
		eTS = tsNone;

		strText.Empty();
		strLine1.Empty();
		strLine2.Empty();
		strLine3.Empty();
		strLine4.Empty();
		strLine5.Empty();
		strLine6.Empty();

		strTemp2.Empty();
		_itoa(nHexagram,szTemp,2);
		for(int i = 0; i < 6 - (int) _tcslen(szTemp); ++i)
			strTemp2 += _T("0");
		strTemp2 += szTemp;
		strFilename.Format(_T("C:\\Src1\\IChing\\Resources\\Text\\Legge\\%s.html"),strTemp2);

		if(F.Open(strFilename,CFile::modeRead))
		{
			int nLine = -1;
			while(F.ReadString(strTemp1))
			{
				strTemp1.Trim();
				if(!strTemp1.IsEmpty())
				{
					strTemp1 = ReplaceString(strTemp1,_T("''"),_T("'"));

					strTemp1 = ReplaceString(strTemp1,_T(""),_T("<P>"));
					strTemp1 = ReplaceString(strTemp1,_T(""),_T("</P>"));
					strTemp1 = ReplaceString(strTemp1,_T(""),_T("<B>"));
					strTemp1 = ReplaceString(strTemp1,_T(""),_T("</B>"));
					strTemp1 = ReplaceString(strTemp1,_T(""),_T("<I>"));
					strTemp1 = ReplaceString(strTemp1,_T(""),_T("</I>"));
					strTemp1 = ReplaceString(strTemp1,_T(""),_T("<BR>"));
					strTemp1 = ReplaceString(strTemp1,_T(""),_T("<LI>"));
					strTemp1 = ReplaceString(strTemp1,_T(""),_T("</LI>"));

					if((eTS > tsNone) || StrStrI(strTemp1,strStart) != NULL)
					{
						if(eTS == tsNone)
							eTS = tsText;
						else						
						if(eTS == tsText)
						{
							eTS = tsLines;
							strText = strTemp1;
						}
						else
						if((eTS == tsLines) && StrStrI(strTemp1,strLines) != NULL)
							eTS = tsLine1;
						else
						if(eTS == tsLine1)
						{
							eTS = tsLine2;
							strLine1 = strTemp1;
						}
						else
						if(eTS == tsLine2)
						{
							eTS = tsLine3;
							strLine2 = strTemp1;
						}
						else
						if(eTS == tsLine3)
						{
							eTS = tsLine4;
							strLine3 = strTemp1;
						}
						else
						if(eTS == tsLine4)
						{
							eTS = tsLine5;
							strLine4 = strTemp1;
						}
						else
						if(eTS == tsLine5)
						{
							eTS = tsLine6;
							strLine5 = strTemp1;
						}
						else
						if(eTS == tsLine6)
						{
							eTS = tsWansExplanation;
							strLine6 = strTemp1;
						}
						else
						if((eTS == tsWansExplanation) && StrStrI(strTemp1,strWansExplanation) != NULL)
							eTS = tsWansExplanationText;
						else
						if((eTS == tsWansExplanationText) && StrStrI(strTemp1,strKausExplanation) != NULL)
							eTS = tsKausExplanationText;
						else
						if(eTS == tsWansExplanationText)
							strText += _T("<br/><br/>") + strTemp1;
						else
						if(eTS == tsKausExplanationText)
						{
							switch(++nLine)
							{
							case 0:
								strText += _T("<br/><br/>") + strTemp1;
								break;
							case 1:
								strLine1 += _T("<br/><br/>") + strTemp1;
								break;
							case 2:
								strLine2 += _T("<br/><br/>") + strTemp1;
								break;
							case 3:
								strLine3 += _T("<br/><br/>") + strTemp1;
								break;
							case 4:
								strLine4 += _T("<br/><br/>") + strTemp1;
								break;
							case 5:
								strLine5 += _T("<br/><br/>") + strTemp1;
								break;
							case 6:
								strLine6 += _T("<br/><br/>") + strTemp1;
								break;
							}
						}
					}
				}
			}

			if((eTS != tsKausExplanationText) || strText.IsEmpty() || strLine1.IsEmpty() || strLine2.IsEmpty() || 
				strLine3.IsEmpty() || strLine4.IsEmpty() || strLine5.IsEmpty() || strLine6.IsEmpty())
				ASSERT(FALSE);

			TS6.Execute(::GetApp()->GetBeliefBase(),
				_T("update Text6 set V%d = '%s' where Name = 'Legge' and Type = 'Text'"),nHexagram,strText);
			TS6.Execute(::GetApp()->GetBeliefBase(),
				_T("update Text6 set V%d = '%s' where Name = 'Legge' and Type = 'Line0'"),nHexagram,strLine1);
			TS6.Execute(::GetApp()->GetBeliefBase(),
				_T("update Text6 set V%d = '%s' where Name = 'Legge' and Type = 'Line1'"),nHexagram,strLine2);
			TS6.Execute(::GetApp()->GetBeliefBase(),
				_T("update Text6 set V%d = '%s' where Name = 'Legge' and Type = 'Line2'"),nHexagram,strLine3);
			TS6.Execute(::GetApp()->GetBeliefBase(),
				_T("update Text6 set V%d = '%s' where Name = 'Legge' and Type = 'Line3'"),nHexagram,strLine4);
			TS6.Execute(::GetApp()->GetBeliefBase(),
				_T("update Text6 set V%d = '%s' where Name = 'Legge' and Type = 'Line4'"),nHexagram,strLine5);
			TS6.Execute(::GetApp()->GetBeliefBase(),
				_T("update Text6 set V%d = '%s' where Name = 'Legge' and Type = 'Line5'"),nHexagram,strLine6);
			F.Close();
		}
		else
			ASSERT(FALSE);
	}
}

void CLoadData::CopyWilhelmBB2VivashBB()
{
	CString strTemp1,strTemp2,strTemp3;

	CTextColumnSet TCS;
	CLabelBlockArray* prgLB6 = ::GetApp()->GetLabelBlock6();
	CLabelEntryArray* prgLE6V = prgLB6->GetAt(3)->GetEntryArray();
	CLabelEntryArray* prgLE6W1 = prgLB6->GetAt(4)->GetEntryArray();

	for(int nTextType = 0; !g_strTextTypes[nTextType].IsEmpty(); ++nTextType)
	{
		CTextSet6 TS6;
		strTemp1.Format(_T("select * from Text6 where Name = 'Wilhelm' and Type = '%s'"),
			g_strTextTypes[nTextType]);
		if(TS6.OpenRowset(::GetApp()->GetBeliefBase(),strTemp1))
			for(int nHexagram = 0; nHexagram < 64; ++nHexagram)
			{
				strTemp1 = TS6.m_szText[nHexagram];

				for(int i = 1; i < 65; ++i)
				{
					strTemp2.Format(_T(" (%d)"),i);
					strTemp1 = ReplaceString(strTemp1,_T(""),strTemp2);
				}
				strTemp1 = ReplaceString(strTemp1,_T("''"),_T("'"));
				strTemp1 = ReplaceString(strTemp1,_T(""),_T("\""));
				strTemp1 = ReplaceString(strTemp1,_T(", "),_T("--"));
				strTemp1 = ReplaceString(strTemp1,_T(" "),_T("-"));
				strTemp1 = ReplaceString(strTemp1,_T("."),_T(":"));
				strTemp1 = ReplaceString(strTemp1,_T(","),_T(";"));
				strTemp1 = ReplaceString(strTemp1,_T(""),_T("["));
				strTemp1 = ReplaceString(strTemp1,_T(""),_T("]"));
				strTemp1 = ReplaceString(strTemp1,_T(""),_T("("));
				strTemp1 = ReplaceString(strTemp1,_T(""),_T(")"));
				strTemp1 = ReplaceString(strTemp1,_T(","),_T(" ,"));
				strTemp1 = ReplaceString(strTemp1,_T("."),_T(" ."));
				strTemp1 = ReplaceString(strTemp1,_T(","),_T(",,"));
				strTemp1 = ReplaceString(strTemp1,_T(" "),_T("' "));

				strTemp1 = ReplaceWholeWord(strTemp1,_T("xxx"),_T("Ch''ien"));
				strTemp1 = ReplaceWholeWord(strTemp1,_T("xxx"),_T("Tui"));
				strTemp1 = ReplaceWholeWord(strTemp1,_T("xxx"),_T("Li"));
				strTemp1 = ReplaceWholeWord(strTemp1,_T("xxx"),_T("Chên"));
				strTemp1 = ReplaceWholeWord(strTemp1,_T("xxx"),_T("Sun"));
				strTemp1 = ReplaceWholeWord(strTemp1,_T("xxx"),_T("K''an"));
				strTemp1 = ReplaceWholeWord(strTemp1,_T("xxx"),_T("Kên"));
				strTemp1 = ReplaceWholeWord(strTemp1,_T("xxx"),_T("K''un"));
				strTemp1 = ReplaceString(strTemp1,_T(""),_T("xxx"));

				strTemp1 = ReplaceString(strTemp1,_T(""),_T(", Chun,"));
				strTemp1 = ReplaceString(strTemp1,_T(""),_T("ku "));
				strTemp1 = ReplaceString(strTemp1,_T(""),_T("lin "));
				strTemp1 = ReplaceString(strTemp1,_T(""),_T("Hsiao Ch''u"));
				strTemp1 = ReplaceString(strTemp1,_T(""),_T("Pi, "));
				strTemp1 = ReplaceString(strTemp1,_T(""),_T("Chin, "));
				strTemp1 = ReplaceString(strTemp1,_T(""),_T("Ting, "));
				strTemp1 = ReplaceString(strTemp1,_T(""),_T("Ta Kuo, "));
				strTemp1 = ReplaceString(strTemp1,_T(""),_T("T''ai"));
				strTemp1 = ReplaceString(strTemp1,_T(""),_T("P''i"));
				strTemp1 = ReplaceString(strTemp1,_T(""),_T("Shih, "));
				strTemp1 = ReplaceString(strTemp1,_T(""),_T("Sung, "));
				strTemp1 = ReplaceString(strTemp1,_T(""),_T("K''uei, "));
				strTemp1 = ReplaceString(strTemp1,_T(""),_T("Hêng, "));
				strTemp1 = ReplaceString(strTemp1,_T(""),_T("Chien, "));
				strTemp1 = ReplaceString(strTemp1,_T(""),_T("Shih Ho, "));
				strTemp1 = ReplaceString(strTemp1,_T(""),_T("fu "));
				strTemp1 = ReplaceString(strTemp1,_T(""),_T("Hsien, "));

				strTemp1 = ReplaceString(strTemp1,_T(""),_T("or Chung Fu, "));
				strTemp1 = ReplaceString(strTemp1,_T(""),_T("hexagram I, providing "));
				strTemp1 = ReplaceString(strTemp1,_T(""),_T("the ninth hexagram, "));
				strTemp1 = ReplaceString(strTemp1,_T("Nourishment"),_T("an open mouth cf. hexagram 27"));

				strTemp1 = ReplaceString(strTemp1,_T(" "),_T("  "));
				strTemp1 = ReplaceString(strTemp1,_T(""),_T(" ,"));

				strTemp1 = ReplaceStringCase(strTemp1,_T("it"),_T("IT"));
				strTemp1 = ReplaceStringCase(strTemp1,_T("The"),_T("THE"));
				strTemp1 = ReplaceStringCase(strTemp1,_T("youth"),_T("YOUTH"));
				strTemp1 = ReplaceStringCase(strTemp1,_T("Decay"),_T("DECAY"));
				strTemp1 = ReplaceStringCase(strTemp1,_T("Return"),_T("TURNING POINT"));
				strTemp1 = ReplaceStringCase(strTemp1,_T("providing"),_T("PROVIDING"));
				strTemp1 = ReplaceStringCase(strTemp1,_T("Breakthrough"),_T("BREAK THROUGH"));
				strTemp1 = ReplaceStringCase(strTemp1,_T("Oppression"),_T("EXHAUSTION"));
				strTemp1 = ReplaceStringCase(strTemp1,_T("Arouse"),_T("SHOCK"));
				strTemp1 = ReplaceStringCase(strTemp1,_T("Faith"),_T("GENTLY PENETRATING"));

				for(int i = 0; i < 64; ++i)
				{
					strTemp2 = prgLE6V->GetAt(i)->GetLabel();
					if((strTemp2 == _T("Creative")) || (strTemp2 == _T("Receptive")) || 
						(strTemp2 == _T("Army")) || (strTemp2 == _T("Wanderer")))
						strTemp2 = CString(_T("The ")) + prgLE6V->GetAt(i)->GetLabel();
					else
					if(strTemp2 == _T("Order"))
						strTemp2 = _T("The Family");
					else
					if(strTemp2 == _T("Reserve"))
						strTemp2 = _T("The Well");
					strTemp1 = ReplaceString(strTemp1,strTemp2,prgLE6W1->GetAt(i)->GetLabel());
				}

				strTemp1 = ReplaceString(strTemp1,_T("chaos"),_T("disorder"));
				strTemp1 = ReplaceString(strTemp1,_T("chaos"),_T("confusion"));
				strTemp1 = ReplaceString(strTemp1,_T("paternal"),_T("male-paternal"));
				strTemp1 = ReplaceString(strTemp1,_T("maternal"),_T("female-maternal"));

				TCS.Execute(::GetApp()->GetBeliefBase(),
					_T("update Text6 set V%d = '%s' where Name = 'Vivash' and Type = '%s'"),
					nHexagram,strTemp1,g_strTextTypes[nTextType]);
			}
		}
}

void CLoadData::LoadNamedSequences()
{
	int nValue,nSequence;
	CString strTemp1,strTemp2,strTemp3;
	CTextColumnSet TCS;
	CLabelBlockArray* prgLB6 = ::GetApp()->GetLabelBlock6();
	CLabelEntryArray* prgLE6 = prgLB6->GetAt(3)->GetEntryArray();

	CString strWilhelm[64] = {
		_T("The Creative"),	
		_T("The Receptive"),		   
		_T("Difficulty at the Beginning"),		   
		_T("Youthful Folly"),	
		_T("Waiting"),	   
		_T("Conflict"),		   
		_T("The Army"),	
		_T("Holding Together"),	
		_T("The Taming Power of the Small"),	   
		_T("Treading"),		   
		_T("Peace"),	   
		_T("Standstill"),	  
		_T("Fellowship with Men"),		   
		_T("Possession in Great Measure"),	  
		_T("Modesty"),	   
		_T("Enthusiasm"),	   
		_T("Following"),	   
		_T("Work on What Has Been Spoiled"),	   
		_T("Approach"),	   
		_T("Contemplation"),	   
		_T("Biting Through"),	   
		_T("Grace"),	  
		_T("Splitting Apart"),	  
		_T("Return"),	
		_T("Innocence"),	  
		_T("The Taming Power of the Great"),	   
		_T("The Corners of the Mouth"),	
		_T("Preponderance of the Great"),		   
		_T("The Abysmal"),		   
		_T("The Clinging"),	
		_T("Influence"),		   
		_T("Duration"),	
		_T("Retreat"),	   	 
		_T("The Power of the Great"),
		_T("Progress"),
		_T("Darkening of the Light"),	   
		_T("The Family"),	
		_T("Opposition"),
		_T("Obstruction"),	   
		_T("Deliverance"),	   
		_T("Decrease"),	
		_T("Increase"),
		_T("Break-through"),	
		_T("Coming to Meet"),	 
		_T("Gathering Together"),
		_T("Pushing Upward"),	 
		_T("Oppression"),	
		_T("The Well"),	
		_T("Revolution"),	
		_T("The Cauldron"),	
		_T("The Arousing"),	
		_T("Keeping Still"),	
		_T("Development"),	
		_T("The Marrying Maiden"),	 
		_T("Abundance"),	 
		_T("The Wanderer"),	   
		_T("The Gentle"),	 
		_T("The Joyous"),	
		_T("Dispersion"),	   
		_T("Limitation"),
		_T("Inner Truth"),
		_T("Preponderance of the Small"),	   
		_T("After Completion"),
		_T("Before Completion"),
		};

	CString strAndrade[64] = {
		_T("http://www.yitoons.com/yibook2/images/YiBook2-111.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-112.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-113.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-114.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-115.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-116.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-117.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-118.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-119.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-121.jpg"),

		_T("http://www.yitoons.com/yibook2/images/YiBook2-122.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-123.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-124.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-125.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-126.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-127.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-128.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-129.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-130.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-131.jpg"),

		_T("http://www.yitoons.com/yibook2/images/YiBook2-132.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-133.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-134.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-135.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-136.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-137.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-138.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-139.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-140.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-141.jpg"),

		_T("http://www.yitoons.com/yibook2/images/YiBook2-142.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-143.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-144.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-145.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-146.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-147.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-148.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-149.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-150.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-151.jpg"),

		_T("http://www.yitoons.com/yibook2/images/YiBook2-152.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-153.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-154.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-155.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-156.jpg"),
		_T("http://www.yitoons.com/yibook2/images/YiBook2-157.jpg"),
		_T("http://www.yitoons.com/yibook3/images/YiBook3-102.jpg"),
		_T("http://www.yitoons.com/yibook3/images/YiBook3-103.jpg"),
		_T("http://www.yitoons.com/yibook3/images/YiBook3-104.jpg"),
		_T("http://www.yitoons.com/yibook3/images/YiBook3-105.jpg"),

		_T("http://www.yitoons.com/yibook3/images/YiBook3-106.jpg"),
		_T("http://www.yitoons.com/yibook3/images/YiBook3-107.jpg"),
		_T("http://www.yitoons.com/yibook3/images/YiBook3-108.jpg"),
		_T("http://www.yitoons.com/yibook3/images/YiBook3-109.jpg"),
		_T("http://www.yitoons.com/yibook3/images/YiBook3-110.jpg"),
		_T("http://www.yitoons.com/yibook3/images/YiBook3-111.jpg"),
		_T("http://www.yitoons.com/yibook3/images/YiBook3-112.jpg"),
		_T("http://www.yitoons.com/yibook3/images/YiBook3-113.jpg"),
		_T("http://www.yitoons.com/yibook3/images/YiBook3-114.jpg"),
		_T("http://www.yitoons.com/yibook3/images/YiBook3-115.jpg"),

		_T("http://www.yitoons.com/yibook3/images/YiBook3-116.jpg"),
		_T("http://www.yitoons.com/yibook3/images/YiBook3-117.jpg"),
		_T("http://www.yitoons.com/yibook3/images/YiBook3-118.jpg"),
		_T("http://www.yitoons.com/yibook3/images/YiBook3-119.jpg"),
	};

	CString strHeyboer1[64] = {
		_T("The Structure Of Heaven"),
		_T("The Energy Of Earth"),
		_T("The Spark Of Life"),
		_T("Not Knowing"),
		_T("Waiting For The Rain To Stop"),
		_T("The Gong Speaks"),
		_T("Legion"),
		_T("Stand By"),
		_T("Tending Small Livestock"),
		_T("The Footprints Of The Ancestors"), 
		_T("Mount Tai"),
		_T("To Say No"),
		_T("Mankind"),
		_T("Great Assets"),
		_T("Give And Take"),
		_T("Weaving Images"),
		_T("Follow Without Resistance"),
		_T("Can O Worms"),
		_T("The Caring Eye"),
		_T("The Heron"),
		_T("Biting Through"),
		_T("Pot Of Herbs"),
		_T("The Wine Skin And The Knife"), 
		_T("Return To Your Town"),
		_T("Natural"),
		_T("Raise And Tame The Big Bull"),
		_T("Jaws"),
		_T("Beyond The Great Pass"),
		_T("The Teachings Of Danger"), 
		_T("To Catch The Bird Of Brightness"),
		_T("Affect And Affection"),
		_T("Steady The Helm Of The Heart"),
		_T("Save Your Bacon"),
		_T("A Man Of Stone"),
		_T("The Gift"),
		_T("Shooting Down The Surplus Suns"),
		_T("Family Of Man"),
		_T("Looking Askance"),
		_T("Cold Feet"),
		_T("Take The Horns"),
		_T("The Empty Cauldron"),
		_T("The Bowl Of The Rain God"),
		_T("The Speaking Staff"),
		_T("A Lawful Heir"),
		_T("Gathering"),
		_T("Step By Step"),
		_T("Enclosed Tree"),
		_T("The Well"),
		_T("Skinning"), 
		_T("The Ritual Cauldron"),
		_T("Thunderbolt"),
		_T("Inaccessible"),
		_T("The Waterwheel"),
		_T("Marrying Maiden"),
		_T("Drums Of Victory"),
		_T("Itinerant Troops"),
		_T("Seals Bestowed"),
		_T("Exchange"),
		_T("The Flood"),
		_T("Bamboo Segments"),
		_T("Inner Truth"),
		_T("Beyond The Small Pass"),
		_T("Already Across"),
		_T("Not Yet Across"),
	};

	CString strHeyboer2[64] = {
		_T("http://www.anton-heyboer.org/i_ching/hex_1-16/hex_e_01.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_1-16/hex_e_02.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_1-16/hex_e_03.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_1-16/hex_e_04.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_1-16/hex_e_05.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_1-16/hex_e_06.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_1-16/hex_e_07.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_1-16/hex_e_08.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_1-16/hex_e_09.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_1-16/hex_e_10.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_1-16/hex_e_11.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_1-16/hex_e_12.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_1-16/hex_e_13.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_1-16/hex_e_14.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_1-16/hex_e_15.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_1-16/hex_e_16.htm"),

		_T("http://www.anton-heyboer.org/i_ching/hex_17-32/hex_e_17.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_17-32/hex_e_18.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_17-32/hex_e_19.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_17-32/hex_e_20.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_17-32/hex_e_21.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_17-32/hex_e_22.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_17-32/hex_e_23.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_17-32/hex_e_24.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_17-32/hex_e_25.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_17-32/hex_e_26.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_17-32/hex_e_27.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_17-32/hex_e_28.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_17-32/hex_e_29.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_17-32/hex_e_30.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_17-32/hex_e_31.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_17-32/hex_e_32.htm"),

		_T("http://www.anton-heyboer.org/i_ching/hex_33-48/hex_e_33.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_33-48/hex_e_34.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_33-48/hex_e_35.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_33-48/hex_e_36.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_33-48/hex_e_37.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_33-48/hex_e_38.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_33-48/hex_e_39.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_33-48/hex_e_40.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_33-48/hex_e_41.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_33-48/hex_e_42.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_33-48/hex_e_43.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_33-48/hex_e_44.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_33-48/hex_e_45.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_33-48/hex_e_46.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_33-48/hex_e_47.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_33-48/hex_e_48.htm"),

		_T("http://www.anton-heyboer.org/i_ching/hex_49-64/hex_e_49.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_49-64/hex_e_50.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_49-64/hex_e_51.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_49-64/hex_e_52.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_49-64/hex_e_53.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_49-64/hex_e_54.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_49-64/hex_e_55.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_49-64/hex_e_56.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_49-64/hex_e_57.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_49-64/hex_e_58.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_49-64/hex_e_59.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_49-64/hex_e_60.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_49-64/hex_e_61.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_49-64/hex_e_62.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_49-64/hex_e_63.htm"),
		_T("http://www.anton-heyboer.org/i_ching/hex_49-64/hex_e_64.htm"),
	};

	CString strKarcher1[8] = {
		_T("Force"),		//heaven 
		_T("Open"),			//lake 
		_T("Radiance"),		//fire 
		_T("Shake"),		//thunder 
		_T("Ground"),		//wind 
		_T("Gorge"),		//water 
		_T("Bound"),		//mountain 
		_T("Field"),		//earth 
	};

	CString strKarcher2[64] = {
		_T("Force"),					
		_T("Field"),					
		_T("Sprouting"),				
		_T("Enveloping"),				
		_T("Attending"),				
		_T("Arguing"),					
		_T("Leading"),					
		_T("Grouping"),				
		_T("Small Accumulating"),		
		_T("Treading"),				
		_T("Prevading"),				
		_T("Obstruction"),				
		_T("Concording People"),		
		_T("Great Possessing"),		
		_T("Humbling"),				
		_T("Providing-For"),			
		_T("Following"),				
		_T("Corrupting"),				
		_T("Nearing"),					
		_T("Viewing"),					
		_T("Gnawing Bite"),			
		_T("Adorning"),				
		_T("Stripping"),				
		_T("Returning"),				
		_T("Without Embroiling"),		
		_T("Great Accumulating"),		
		_T("Swallowing"),				
		_T("Great Exceeding"),			
		_T("Gorge"),					
		_T("Radiance"),				
		_T("Persevering"),				
		_T("Conjoining"),				
		_T("Retiring"),				
		_T("Great Invigorating"),		
		_T("Prospering"),				
		_T("Brightness Hiding"),		
		_T("Dwelling People"),			
		_T("Polarising"),				
		_T("Limping"),					
		_T("Taking-Apart"),			
		_T("Diminishing"),				
		_T("Augmenting"),				
		_T("Parting"),					
		_T("Coupling"),				
		_T("Clustering"),				
		_T("Ascending"),				
		_T("Confining"),				
		_T("Welling"),					
		_T("Skinning"),				
		_T("Holding"),					
		_T("Shake"),					
		_T("Bound"),					
		_T("Infiltrating"),			
		_T("Converting The Maiden"),	
		_T("Abounding"),				
		_T("Sojourning"),				
		_T("Ground"),					
		_T("Open"),					
		_T("Dispersing"),				
		_T("Articulating"),			
		_T("Centre Confirming"),		
		_T("Small Exceeding"),			
		_T("Already Fording"),			
		_T("Not-Yet Fording"),
		};

	CString strQabalah[64] = {
		_T("Chokmah in Yetzirah"),
		_T("Binah in Assiah"), 
		_T("Geburah in Briah"),
		_T("Yod"),
		_T("Chokmah in Briah"),
		_T("Yesod in Yetzirah"), 
		_T("Yesod in Assiah"),
		_T("Binah in Briah"), 
		_T("Malkuth in Yetzirah"), 
		_T("Netzach in Yetzirah"), 
		_T("Chokmah in Assiah"), 
		_T("Briah in Yetzirah"), 
		_T("Tipareth in Yetzirah"), 
		_T("Chokmah in Atziluth"), 
		_T("Chesed in Assiah"), 
		_T("Kether in Atziluth"), 
		_T("Briah of Atziluth"), 
		_T("Assiah of Yetzirah"), 
		_T("Netzach in Assiah"), 
		_T("Kether in Yetzirah"), 
		_T("Geburah in Atziluth"), 
		_T("Ayin"), 
		_T("Kether in Assiah"), 
		_T("Geburah in Assiah"), 
		_T("Geburah in Yetzirah"), 
		_T("Malkuth in Assiah"), 
		_T("Assiah of Atziluth"), 
		_T("Briah of Yetzirah"), 
		_T("Yesod in Briah"), 
		_T("Tipareth in Atziluth"), 
		_T("Briah of Atziluth"), 
		_T("Atziluth of Yetzirah"), 
		_T("Chesed in Yetzirah"), 
		_T("Malkuth in Assiah"), 
		_T("Binah in Atziluth"), 
		_T("Tipareth in Assiah"), 
		_T("Lamed"), 
		_T("Netzach in Atziluth"), 
		_T("Chesed in Briah"), 
		_T("Samech"), 
		_T("Assiah of Briah"), 
		_T("Yetzirah of Atziluth"), 
		_T("Malkuth in Briah"), 
		_T("Hod in Yetzirah"), 
		_T("Kether in Briah"), 
		_T("Hod in Assiah"), 
		_T("Qoph"), 
		_T("Hod in Briah"), 
		_T("Hheth"),
		_T("Hod in Atzituth"), 
		_T("Atziluth of Atziluth"), 
		_T("Assiah of Assiah"), 
		_T("Yetzirah of Atziluth"), 
		_T("Atziluth of Briah"), 
		_T("He"), 
		_T("Chesed in Atziluth"), 
		_T("Yetzirah of Yetzirah"), 
		_T("Briah of Briah"), 
		_T("Zain"), 
		_T("Netzach in Briah"), 
		_T("Yetzirah of Briah"), 
		_T("Atziluth of Assiah"), 
		_T("Tipareth in Briah"), 
		_T("Yesod in Atziluth"),
	}; 

	CString strTarot[64] = {
		_T("Two of Swords"),
		_T("Three of Pentacles"),
		_T("Five of Cups"),
		_T("The Hermit"),
		_T("Two of Cups"),
		_T("Nine of Swords"),
		_T("Nine of Pentacles"), 
		_T("Three of Cups"), 
		_T("Ten of Swords"), 
		_T("Seven of Swords"), 
		_T("Two of Pentacles"), 
		_T("Three of Swords"), 
		_T("Six of Swords"), 
		_T("Two of Wands"), 
		_T("Four of Pentacles"), 
		_T("Ace of Wands"), 
		_T("Queen of Wands"), 
		_T("Princess of Swords"), 
		_T("Seven of Pentacles"), 
		_T("Ace of Swords"), 
		_T("Five of Wands"), 
		_T("The Devil"), 
		_T("Ace of Pentacles"), 
		_T("Five of Pentacles"), 
		_T("Five of Swords"), 
		_T("Ten of Pentacles"), 
		_T("Princess of Wands"), 
		_T("Queen of Swords"), 
		_T("Nine of Cups"), 
		_T("Six of Wands"), 
		_T("Queen of Pentacles"), 
		_T("King of Swords"), 
		_T("Four of Swords"), 
		_T("Ten of Wands"), 
		_T("Three of Wands"), 
		_T("Six of Pentacles"), 
		_T("Justice"), 
		_T("Seven of Wands"), 
		_T("Four of Cups"), 
		_T("Temperance"), 
		_T("Princess of Cups"), 
		_T("Prince of Wands"), 
		_T("Ten of Cups"), 
		_T("Eight of Swords"), 
		_T("Ace of Cups"), 
		_T("The Moon"), 
		_T("Eight of Pentacles"), 
		_T("Eight of Cups"), 
		_T("The Chariot"), 
		_T("Eight of Wands"), 
		_T("King of Wands"), 
		_T("Princess of Pentacles"), 
		_T("Prince of Pentacles"), 
		_T("King of Cups"), 
		_T("The Emperor"), 
		_T("Four of Wands"), 
		_T("Prince of Swords"), 
		_T("Queen of Cups"), 
		_T("The Lovers"), 
		_T("Seven of Cups"), 
		_T("Prince of Cups"), 
		_T("King of Pentacles"), 
		_T("Six of Cups"), 
		_T("Nine of Wands"),
	}; 

	CString strAstrology[64] = {
		_T("Uranus in Air"),
		_T("Neptune in Earth"),
		_T("Mars in Water"),
		_T("Sixth House, Mutable Earth, Virgo"),
		_T("Uranus in Water"),
		_T("Luna in Air"),
		_T("Luna in Earth"),
		_T("Neptune in Water"), 
		_T("Pluto in Air"), 
		_T("Venus in Air"), 
		_T("Uranus in Earth"), 
		_T("Neptune in Air"), 
		_T("Sol in Air"), 
		_T("Uranus in Fire"), 
		_T("Jupiter in Earth"), 
		_T("Saturn in Fire"), 
		_T("Sagittarius Ascending, Mutable Fire"), 
		_T("Caput Draconis in Air"), 
		_T("Venus in Earth"), 
		_T("Saturn in Air"), 
		_T("Mars in Fire"), 
		_T("Tenth House, Cardinal Earth, Capricorn"), 
		_T("Saturn in Earth"), 
		_T("Mars in Earth"), 
		_T("Mars in Air"), 
		_T("Pluto in Earth"), 
		_T("Caput Draconis in Fire"), 
		_T("Gemini Ascending, Mutable Air"), 
		_T("Luna in Water"), 
		_T("Sol in Fire"), 
		_T("Virgo Ascending, Mutable Earth"), 
		_T("Libra Ascending, Cardinal Air"), 
		_T("Jupiter in Air"), 
		_T("Pluto in Fire"), 
		_T("Neptune in Fire"), 
		_T("Sol in Earth"), 
		_T("Seventh House, Cardinal Air, Libra"), 
		_T("Venus in Fire"), 
		_T("Jupiter in Water"), 
		_T("Ninth House, Mutable Fire, Sagittarius"), 
		_T("Caput Draconis in Water"),
		_T("Leo Ascending, Fixed Fire"), 
		_T("Pluto in Water"), 
		_T("Mercury in Air"), 
		_T("Saturn in Water"), 
		_T("Mercury in Earth"), 
		_T("Twelfth House, Mutable Water, Pisces"), 
		_T("Mercury in Water"), 
		_T("Fourth House, Cardinal Fire, Cancer"), 
		_T("Mercury in Fire"), 
		_T("Aries Ascending, Mutable Fire"), 
		_T("Caput Draconis in Earth"), 
		_T("Taurus Ascending, Fixed Earth"), 
		_T("Cancer Ascending, Cardinal Water"), 
		_T("First House, Cardinal Fire, Aries"), 
		_T("Jupiter in Fire"), 
		_T("Aquarius Ascending, Fixed Air"), 
		_T("Pisces Ascending, Mutable Water"), 
		_T("Third House, Mutable Air, Gemini"), 
		_T("Venus in Water"), 
		_T("Scorpio Ascending, Fixed Water"), 
		_T("Capricorn Ascending, Cardinal Earth"), 
		_T("Sol in Water"), 
		_T("Luna in Fire"),
	};

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("delete from Label6 where Name = 'Andrade'"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("delete from Label6 where Name = 'Heyboer'"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("delete from Label6 where Name = 'Karcher'"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("delete from Label6 where Name = 'Qabalah'"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("delete from Label6 where Name = 'Tarot'"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("delete from Label6 where Name = 'Astrology'"));

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Label6 (Sequence, Name, Active) values(700,'Andrade',0)"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Label6 (Sequence, Name, Active) values(750,'Heyboer',0)"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Label6 (Sequence, Name, Active) values(760,'Karcher',0)"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Label6 (Sequence, Name, Active) values(1200,'Qabalah',0)"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Label6 (Sequence, Name, Active) values(1300,'Tarot',0)"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Label6 (Sequence, Name, Active) values(1400,'Astrology',0)"));

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	strTemp3 = _T("update Label6 set");
	for(int i = 0; i < 32; ++i)
	{
		nValue = prgLE6->GetAt(i)->GetValue();
		nSequence = prgLE6->GetAt(i)->GetSequence();
		strTemp1 = prgLE6->GetAt(i)->GetLabel();
		strTemp2.Format(_T(" S%d = %d, V%d = '%s'%s "),i,nSequence,i,strTemp1,i == 31 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'Andrade'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);
	strTemp3 = _T("update Label6 set");
	for(int i = 32; i < 64; ++i)
	{
		nValue = prgLE6->GetAt(i)->GetValue();
		nSequence = prgLE6->GetAt(i)->GetSequence();
		strTemp1 = prgLE6->GetAt(i)->GetLabel();
		strTemp2.Format(_T(" S%d = %d, V%d = '%s'%s "),i,nSequence,i,strTemp1,i == 63 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'Andrade'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("delete from Text6 where Name = 'Andrade'"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Text6 (Type, Sequence, Name) values('Text',400,'Andrade')"));

	strTemp3 = _T("update Text6 set");
	for(int i = 0; i < 64; ++i)
	{
		strTemp1 = prgLE6->GetAt(i)->GetLabel();
		strTemp2.Format(_T(" V%d = '<center><img src=\"%s\" alt=\"%s\"></center>'%s "),i,strAndrade[i],strTemp1,i == 63 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'Andrade' and Type = 'Text'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	strTemp3 = _T("update Label6 set");
	for(int i = 0; i < 32; ++i)
	{
		nValue = prgLE6->GetAt(i)->GetValue();
		nSequence = prgLE6->GetAt(i)->GetSequence();
		strTemp1 = prgLE6->GetAt(i)->GetLabel();
		strTemp2.Format(_T(" S%d = %d, V%d = '%s'%s "),i,nSequence,i,strHeyboer1[nSequence],i == 31 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'Heyboer'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);
	strTemp3 = _T("update Label6 set");
	for(int i = 32; i < 64; ++i)
	{
		nValue = prgLE6->GetAt(i)->GetValue();
		nSequence = prgLE6->GetAt(i)->GetSequence();
		strTemp1 = prgLE6->GetAt(i)->GetLabel();
		strTemp2.Format(_T(" S%d = %d, V%d = '%s'%s "),i,nSequence,i,strHeyboer1[nSequence],i == 63 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'Heyboer'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("delete from Text6 where Name = 'Heyboer'"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Text6 (Type, Sequence, Name) values('Text',500,'Heyboer')"));

	strTemp3 = _T("update Text6 set");
	for(int i = 0; i < 64; ++i)
	{
		strTemp1 = prgLE6->GetAt(i)->GetLabel();
		nSequence = prgLE6->GetAt(i)->GetSequence();
		strTemp2.Format(_T(" V%d = '%s'%s "),i,strHeyboer2[nSequence],i == 63 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'Heyboer' and Type = 'Text'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	strTemp3 = _T("update Label6 set");
	for(int i = 0; i < 32; ++i)
	{
		nValue = prgLE6->GetAt(i)->GetValue();
		nSequence = prgLE6->GetAt(i)->GetSequence();
		strTemp1 = prgLE6->GetAt(i)->GetLabel();
		strTemp2.Format(_T(" S%d = %d, V%d = '%s'%s "),i,nSequence,i,strKarcher2[nSequence],i == 31 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'Karcher'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);
	strTemp3 = _T("update Label6 set");
	for(int i = 32; i < 64; ++i)
	{
		nValue = prgLE6->GetAt(i)->GetValue();
		nSequence = prgLE6->GetAt(i)->GetSequence();
		strTemp1 = prgLE6->GetAt(i)->GetLabel();
		strTemp2.Format(_T(" S%d = %d, V%d = '%s'%s "),i,nSequence,i,strKarcher2[nSequence],i == 63 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'Karcher'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	strTemp3 = _T("update Label6 set");
	for(int i = 0; i < 32; ++i)
	{
		nValue = prgLE6->GetAt(i)->GetValue();
		nSequence = prgLE6->GetAt(i)->GetSequence();
		strTemp1 = prgLE6->GetAt(i)->GetLabel();
		strTemp2.Format(_T(" S%d = %d, V%d = '%s'%s "),i,nSequence,i,strQabalah[nSequence],i == 31 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'Qabalah'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);
	strTemp3 = _T("update Label6 set");
	for(int i = 32; i < 64; ++i)
	{
		nValue = prgLE6->GetAt(i)->GetValue();
		nSequence = prgLE6->GetAt(i)->GetSequence();
		strTemp1 = prgLE6->GetAt(i)->GetLabel();
		strTemp2.Format(_T(" S%d = %d, V%d = '%s'%s "),i,nSequence,i,strQabalah[nSequence],i == 63 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'Qabalah'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	strTemp3 = _T("update Label6 set");
	for(int i = 0; i < 32; ++i)
	{
		nValue = prgLE6->GetAt(i)->GetValue();
		nSequence = prgLE6->GetAt(i)->GetSequence();
		strTemp1 = prgLE6->GetAt(i)->GetLabel();
		strTemp2.Format(_T(" S%d = %d, V%d = '%s'%s "),i,nSequence,i,strTarot[nSequence],i == 31 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'Tarot'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);
	strTemp3 = _T("update Label6 set");
	for(int i = 32; i < 64; ++i)
	{
		nValue = prgLE6->GetAt(i)->GetValue();
		nSequence = prgLE6->GetAt(i)->GetSequence();
		strTemp1 = prgLE6->GetAt(i)->GetLabel();
		strTemp2.Format(_T(" S%d = %d, V%d = '%s'%s "),i,nSequence,i,strTarot[nSequence],i == 63 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'Tarot'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	strTemp3 = _T("update Label6 set");
	for(int i = 0; i < 32; ++i)
	{
		nValue = prgLE6->GetAt(i)->GetValue();
		nSequence = prgLE6->GetAt(i)->GetSequence();
		strTemp1 = prgLE6->GetAt(i)->GetLabel();
		strTemp2.Format(_T(" S%d = %d, V%d = '%s'%s "),i,nSequence,i,strAstrology[nSequence],i == 31 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'Astrology'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);
	strTemp3 = _T("update Label6 set");
	for(int i = 32; i < 64; ++i)
	{
		nValue = prgLE6->GetAt(i)->GetValue();
		nSequence = prgLE6->GetAt(i)->GetSequence();
		strTemp1 = prgLE6->GetAt(i)->GetLabel();
		strTemp2.Format(_T(" S%d = %d, V%d = '%s'%s "),i,nSequence,i,strAstrology[nSequence],i == 63 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'Astrology'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);
}

void CLoadData::ConvertOldBB()
{
	CTextColumnSet TCS;

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("drop table Image1"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("drop table Image2"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("drop table Image3"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("drop table Image6"));

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("update Label6 set V6 = 'Ascent' where Name = 'Vivash' and V6 = 'Ascending'"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("update Label6 set V9 = 'Arouse' where Name = 'Vivash' and V9 = 'Shock'"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("update Label6 set V23 = 'Delay' where Name = 'Vivash' and V23 = 'Waiting'"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("update Label6 set V24 = 'Accumulation' where Name = 'Vivash' and V24 = 'Massing'"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("update Label6 set V25 = 'Pursuit' where Name = 'Vivash' and V25 = 'Following'"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("update Label6 set V53 = 'Order' where Name = 'Vivash' and V53 = 'Family'"));

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("update Label6 set Sequence = 600 where Name = 'Legge'"));

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Text6 (Type, Sequence, Name) values('Image',300,'Legge')"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Text6 (Type, Sequence, Name) values('Judgment',300,'Legge')"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Text6 (Type, Sequence, Name) values('Line0',300,'Legge')"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Text6 (Type, Sequence, Name) values('Line1',300,'Legge')"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Text6 (Type, Sequence, Name) values('Line2',300,'Legge')"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Text6 (Type, Sequence, Name) values('Line3',300,'Legge')"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Text6 (Type, Sequence, Name) values('Line4',300,'Legge')"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Text6 (Type, Sequence, Name) values('Line5',300,'Legge')"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Text6 (Type, Sequence, Name) values('LineAll',300,'Legge')"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Text6 (Type, Sequence, Name) values('Text',300,'Legge')"));
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//Portable Dragon - Xun Shuang 

int nMystery[64] = 
{
	 1,44,13,10, 9,14,43,33,
	25,61,26,34, 6,37,38, 5,
	57,30,58,50,49,28,12,42,
	41,11,59,22,54,53,21,60,
	18,55,56,17,32,31,47,48,
	63,64,20,27,19, 4,36,52,
	51,35, 3,46,62,45,29,39,
	40,24, 7,15,16, 8,23, 2
};

int nFuxi[64] = 
{
	 2,23, 8,20,16,35,45,12,
	15,52,39,53,62,56,31,33,
	 7, 4,29,59,40,64,47, 6,
	46,18,48,57,32,50,28,44,
	24,27, 3,42,51,21,17,25,
	36,22,63,37,55,30,49,13,
	19,41,60,61,54,38,58,10,
	11,26, 5, 9,34,14,43, 1
};

int n8Palaces[64] = 
{
	 1,44,33,12,20,23,35,14,
	51,16,40,32,46,48,28,17,
	29,60, 3,63,49,55,36, 7,
	52,22,26,41,38,10,61,53,
	 2,24,19,11,34,43, 5, 8,
	57, 9,37,42,25,21,27,18,
	30,56,50,64, 4,59, 6,13,
	58,47,45,31,39,15,62,54
};

int nMawangdui[64] = 
{
	 1,12,33,10,6,13,25,44,
	52,26,23,41,4,22,27,18,
	29, 5, 8,39,60,63,3,48,
	51,34,16,62,54,40,55,32,
	 2,11,15,19, 7,36,24,46,
	58,43,45,31,47,49,17,28,
	30,14,35,56,38,64,21,50,
	57, 9,20,53,61,59,37,42
};

int FindMysteryIndex(int nSequence)
{
	for(int i = 0; i < 64; ++i)
		if(nMystery[i] == nSequence)
			return i;
	return -1;
}

int FindFuxiIndex(int nSequence)
{
	for(int i = 0; i < 64; ++i)
		if(nFuxi[i] == nSequence)
			return i;
	return -1;
}

int Find8PalacesIndex(int nSequence)
{
	for(int i = 0; i < 64; ++i)
		if(n8Palaces[i] == nSequence)
			return i;
	return -1;
}

int FindMawangduiIndex(int nSequence)
{
	for(int i = 0; i < 64; ++i)
		if(nMawangdui[i] == nSequence)
			return i;
	return -1;
}

void CLoadData::LoadNumberedSequences()
{
	int nValue,nSequence;
	CString strTemp1,strTemp2,strTemp3;
	CTextColumnSet TCS;
	CLabelBlockArray* prgLB6 = ::GetApp()->GetLabelBlock6();
	CLabelEntryArray* prgLE6 = prgLB6->GetAt(3)->GetEntryArray();

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("delete from Label6 where Name = 'Mystery'"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("delete from Label6 where Name = 'Fuxi'"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("delete from Label6 where Name = '8Palaces'"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("delete from Label6 where Name = 'Mawangdui'"));

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Label6 (Sequence, Name, Active) values(780,'Mystery',0)"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Label6 (Sequence, Name, Active) values(781,'Fuxi',0)"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Label6 (Sequence, Name, Active) values(782,'8Palaces',0)"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Label6 (Sequence, Name, Active) values(783,'Mawangdui',0)"));

	strTemp3 = _T("update Label6 set");
	for(int i = 0; i < 32; ++i)
	{
		nValue = prgLE6->GetAt(i)->GetValue();
		nSequence = prgLE6->GetAt(i)->GetSequence();
		strTemp1 = prgLE6->GetAt(i)->GetLabel();
		strTemp2.Format(_T(" S%d = %d, V%d = '%s'%s "),i,FindMysteryIndex(nSequence + 1),i,strTemp1,i == 31 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'Mystery'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);
	strTemp3 = _T("update Label6 set");
	for(int i = 32; i < 64; ++i)
	{
		nValue = prgLE6->GetAt(i)->GetValue();
		nSequence = prgLE6->GetAt(i)->GetSequence();
		strTemp1 = prgLE6->GetAt(i)->GetLabel();
		strTemp2.Format(_T(" S%d = %d, V%d = '%s'%s "),i,FindMysteryIndex(nSequence + 1),i,strTemp1,i == 63 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'Mystery'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);

	strTemp3 = _T("update Label6 set");
	for(int i = 0; i < 32; ++i)
	{
		nValue = prgLE6->GetAt(i)->GetValue();
		nSequence = prgLE6->GetAt(i)->GetSequence();
		strTemp1 = prgLE6->GetAt(i)->GetLabel();
		strTemp2.Format(_T(" S%d = %d, V%d = '%s'%s "),i,FindFuxiIndex(nSequence + 1),i,strTemp1,i == 31 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'Fuxi'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);
	strTemp3 = _T("update Label6 set");
	for(int i = 32; i < 64; ++i)
	{
		nValue = prgLE6->GetAt(i)->GetValue();
		nSequence = prgLE6->GetAt(i)->GetSequence();
		strTemp1 = prgLE6->GetAt(i)->GetLabel();
		strTemp2.Format(_T(" S%d = %d, V%d = '%s'%s "),i,FindFuxiIndex(nSequence + 1),i,strTemp1,i == 63 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'Fuxi'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);

	strTemp3 = _T("update Label6 set");
	for(int i = 0; i < 32; ++i)
	{
		nValue = prgLE6->GetAt(i)->GetValue();
		nSequence = prgLE6->GetAt(i)->GetSequence();
		strTemp1 = prgLE6->GetAt(i)->GetLabel();
		strTemp2.Format(_T(" S%d = %d, V%d = '%s'%s "),i,Find8PalacesIndex(nSequence + 1),i,strTemp1,i == 31 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = '8Palaces'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);
	strTemp3 = _T("update Label6 set");
	for(int i = 32; i < 64; ++i)
	{
		nValue = prgLE6->GetAt(i)->GetValue();
		nSequence = prgLE6->GetAt(i)->GetSequence();
		strTemp1 = prgLE6->GetAt(i)->GetLabel();
		strTemp2.Format(_T(" S%d = %d, V%d = '%s'%s "),i,Find8PalacesIndex(nSequence + 1),i,strTemp1,i == 63 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = '8Palaces'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);

	strTemp3 = _T("update Label6 set");
	for(int i = 0; i < 32; ++i)
	{
		nValue = prgLE6->GetAt(i)->GetValue();
		nSequence = prgLE6->GetAt(i)->GetSequence();
		strTemp1 = prgLE6->GetAt(i)->GetLabel();
		strTemp2.Format(_T(" S%d = %d, V%d = '%s'%s "),i,FindMawangduiIndex(nSequence + 1),i,strTemp1,i == 31 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'Mawangdui'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);
	strTemp3 = _T("update Label6 set");
	for(int i = 32; i < 64; ++i)
	{
		nValue = prgLE6->GetAt(i)->GetValue();
		nSequence = prgLE6->GetAt(i)->GetSequence();
		strTemp1 = prgLE6->GetAt(i)->GetLabel();
		strTemp2.Format(_T(" S%d = %d, V%d = '%s'%s "),i,FindMawangduiIndex(nSequence + 1),i,strTemp1,i == 63 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'Mawangdui'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);

	int nPosition[6] = {32,16,8,4,2,1};
	int nEpi[64];
	TCHAR szEpi[64][7] = 
	{
		"||||||","|||:||",":::|||","::::||","::||||","::|:||","||:|||","||::||",
		":|:|||",":|::||","|:||||","|:|:||","|::|||","|:::||",":|||||",":||:||",
		"|||:::","::::::","::|:::","|||::|",":::::|","||::::","::|::|","||:::|",
		":|:::|","|:|::|",":|::::","|::::|",":||::|","|:|:::","|:::::",":||:::",
		"|||||:",":::||:","|||:|:","::::|:","::|:|:","||::|:","::|||:","||:||:",
		":|::|:","|:|:|:",":|:||:","|:|||:","|::||:",":||||:","|:::|:",":||:|:",
		"||||::","||||:|",":::|:|",":::|::","::||::","::||:|","||:|:|","||:|::",
		":|:|::",":|:|:|","|:||:|","|:||::","|::|::","|::|:|",":|||:|",":|||::"
	};

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("delete from Label6 where Name = 'Epi'"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Label6 (Sequence, Name, Active) values(784,'Epi',0)"));

	for(int i = 0; i < 64; ++i)
	{
		nEpi[i] = 0;
		for(int j = 5; j >= 0; --j)
			nEpi[i] += (szEpi[i][j] == '|' ? 1 : 0) * nPosition[j];
	}

	strTemp3 = _T("update Label6 set");
	for(int i = 0; i < 32; ++i)
	{
		nValue = prgLE6->GetAt(i)->GetValue();
		nSequence = prgLE6->GetAt(i)->GetSequence();
		strTemp1 = prgLE6->GetAt(nEpi[i])->GetLabel();
		strTemp2.Format(_T(" S%d = %d, V%d = '%s'%s "),nEpi[i],i,nEpi[i],strTemp1,i == 31 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'Epi'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);
	strTemp3 = _T("update Label6 set");
	for(int i = 32; i < 64; ++i)
	{
		nValue = prgLE6->GetAt(i)->GetValue();
		nSequence = prgLE6->GetAt(i)->GetSequence();
		strTemp1 = prgLE6->GetAt(nEpi[i])->GetLabel();
		strTemp2.Format(_T(" S%d = %d, V%d = '%s'%s "),nEpi[i],i,nEpi[i],strTemp1,i == 63 ? _T("") : _T(","));
		strTemp3 += strTemp2;
	}
	strTemp3 += _T("where Name = 'Epi'");
	TCS.Execute(::GetApp()->GetBeliefBase(),strTemp3);
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void CLoadData::AddSequenceTables()
{
	CString strTemp1,strTemp2,strTemp3;
	CTextColumnSet TCS;
	CLabelBlockArray* prgLB1 = ::GetApp()->GetLabelBlock1();
	CLabelEntryArray* prgLE1;
	CLabelBlockArray* prgLB2 = ::GetApp()->GetLabelBlock2();
	CLabelEntryArray* prgLE2;
	CLabelBlockArray* prgLB3 = ::GetApp()->GetLabelBlock3();
	CLabelEntryArray* prgLE3;
	CLabelBlockArray* prgLB6 = ::GetApp()->GetLabelBlock6();
	CLabelEntryArray* prgLE6;

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("drop table Sequence1"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("drop table Sequence2"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("drop table Sequence3"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("drop table Sequence6"));

	TCS.Execute(::GetApp()->GetBeliefBase(),_T(
		"create table Sequence1("
		"	Sequence Integer,"
		"	Name Text,"
		"	S0 Integer,"
		"	S1 Integer)"));

	TCS.Execute(::GetApp()->GetBeliefBase(),_T(
		"create table Sequence2("
		"	Sequence Integer,"
		"	Name Text,"
		"	S0 Integer,"
		"	S1 Integer,"
		"	S2 Integer,"
		"	S3 Integer)"));

	TCS.Execute(::GetApp()->GetBeliefBase(),_T(
		"create table Sequence3("
		"	Sequence Integer,"
		"	Name Text,"
		"	S0 Integer,"
		"	S1 Integer,"
		"	S2 Integer,"
		"	S3 Integer,"
		"	S4 Integer,"
		"	S5 Integer,"
		"	S6 Integer,"
		"	S7 Integer)"));

	TCS.Execute(::GetApp()->GetBeliefBase(),_T(
		"create table Sequence6("
		"	Sequence Integer,"
		"	Name Text,"
		"	S0 Integer,"
		"	S1 Integer,"
		"	S2 Integer,"
		"	S3 Integer,"
		"	S4 Integer,"
		"	S5 Integer,"
		"	S6 Integer,"
		"	S7 Integer,"
		"	S8 Integer,"
		"	S9 Integer,"
		"	S10 Integer,"
		"	S11 Integer,"
		"	S12 Integer,"
		"	S13 Integer,"
		"	S14 Integer,"
		"	S15 Integer,"
		"	S16 Integer,"
		"	S17 Integer,"
		"	S18 Integer,"
		"	S19 Integer,"
		"	S20 Integer,"
		"	S21 Integer,"
		"	S22 Integer,"
		"	S23 Integer,"
		"	S24 Integer,"
		"	S25 Integer,"
		"	S26 Integer,"
		"	S27 Integer,"
		"	S28 Integer,"
		"	S29 Integer,"
		"	S30 Integer,"
		"	S31 Integer,"
		"	S32 Integer,"
		"	S33 Integer,"
		"	S34 Integer,"
		"	S35 Integer,"
		"	S36 Integer,"
		"	S37 Integer,"
		"	S38 Integer,"
		"	S39 Integer,"
		"	S40 Integer,"
		"	S41 Integer,"
		"	S42 Integer,"
		"	S43 Integer,"
		"	S44 Integer,"
		"	S45 Integer,"
		"	S46 Integer,"
		"	S47 Integer,"
		"	S48 Integer,"
		"	S49 Integer,"
		"	S50 Integer,"
		"	S51 Integer,"
		"	S52 Integer,"
		"	S53 Integer,"
		"	S54 Integer,"
		"	S55 Integer,"
		"	S56 Integer,"
		"	S57 Integer,"
		"	S58 Integer,"
		"	S59 Integer,"
		"	S60 Integer,"
		"	S61 Integer,"
		"	S62 Integer,"
		"	S63 Integer)"));

	for(int i = 0; i <= prgLB1->GetUpperBound(); ++i)
	{
		strTemp1 = _T("insert into Sequence1 (Sequence,Name");
		strTemp2.Format(_T(" values(%d,'%s'"),i,prgLB1->GetAt(i)->GetName());
		prgLE1 = prgLB1->GetAt(i)->GetEntryArray();
		for(int j = 0; j <= prgLE1->GetUpperBound(); ++j)
		{
			strTemp3.Format(_T(",S%d"),j);
			strTemp1 += strTemp3;
			strTemp3.Format(_T(",%d"),prgLE1->GetAt(j)->GetSequence());
			strTemp2 += strTemp3;
		}
		strTemp1 += _T(")");
		strTemp2 += _T(")");
		strTemp1 += strTemp2;
		TCS.Execute(::GetApp()->GetBeliefBase(),strTemp1);
	}

	for(int i = 0; i <= prgLB2->GetUpperBound(); ++i)
	{
		strTemp1 = _T("insert into Sequence2 (Sequence,Name");
		strTemp2.Format(_T(" values(%d,'%s'"),i,prgLB2->GetAt(i)->GetName());
		prgLE2 = prgLB2->GetAt(i)->GetEntryArray();
		for(int j = 0; j <= prgLE2->GetUpperBound(); ++j)
		{
			strTemp3.Format(_T(",S%d"),j);
			strTemp1 += strTemp3;
			strTemp3.Format(_T(",%d"),prgLE2->GetAt(j)->GetSequence());
			strTemp2 += strTemp3;
		}
		strTemp1 += _T(")");
		strTemp2 += _T(")");
		strTemp1 += strTemp2;
		TCS.Execute(::GetApp()->GetBeliefBase(),strTemp1);
	}

	for(int i = 0; i <= prgLB3->GetUpperBound(); ++i)
	{
		strTemp1 = _T("insert into Sequence3 (Sequence,Name");
		strTemp2.Format(_T(" values(%d,'%s'"),i,prgLB3->GetAt(i)->GetName());
		prgLE3 = prgLB3->GetAt(i)->GetEntryArray();
		for(int j = 0; j <= prgLE3->GetUpperBound(); ++j)
		{
			strTemp3.Format(_T(",S%d"),j);
			strTemp1 += strTemp3;
			strTemp3.Format(_T(",%d"),prgLE3->GetAt(j)->GetSequence());
			strTemp2 += strTemp3;
		}
		strTemp1 += _T(")");
		strTemp2 += _T(")");
		strTemp1 += strTemp2;
		TCS.Execute(::GetApp()->GetBeliefBase(),strTemp1);
	}

	for(int i = 0; i <= prgLB6->GetUpperBound(); ++i)
	{
		strTemp1 = _T("insert into Sequence6 (Sequence,Name");
		strTemp2.Format(_T(" values(%d,'%s'"),i,prgLB6->GetAt(i)->GetName());
		prgLE6 = prgLB6->GetAt(i)->GetEntryArray();
		for(int j = 0; j <= prgLE6->GetUpperBound(); ++j)
		{
			strTemp3.Format(_T(",S%d"),j);
			strTemp1 += strTemp3;
			strTemp3.Format(_T(",%d"),prgLE6->GetAt(j)->GetSequence());
			strTemp2 += strTemp3;
		}
		strTemp1 += _T(")");
		strTemp2 += _T(")");
		strTemp1 += strTemp2;
		TCS.Execute(::GetApp()->GetBeliefBase(),strTemp1);
	}
}
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void CLoadData::ConvertLabelTables()
{
	CString strTemp1,strTemp2,strTemp3;
	CTextColumnSet TCS;

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("alter table Sequence1 add constraint pkSequence1 primary key (Sequence)")); 
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("alter table Sequence2 add constraint pkSequence2 primary key (Sequence)")); 
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("alter table Sequence3 add constraint pkSequence3 primary key (Sequence)")); 
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("alter table Sequence6 add constraint pkSequence6 primary key (Sequence)")); 

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("alter table Label1 drop column Active"));
	for(int i = 0; i < 2; ++i)
	{
		strTemp1.Format(_T("alter table Label1 drop column S%d"),i);
		TCS.Execute(::GetApp()->GetBeliefBase(),strTemp1);
	}

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("alter table Label2 drop column Active"));
	for(int i = 0; i < 4; ++i)
	{
		strTemp1.Format(_T("alter table Label2 drop column S%d"),i);
		TCS.Execute(::GetApp()->GetBeliefBase(),strTemp1);
	}

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("alter table Label3 drop column Active"));
	for(int i = 0; i < 8; ++i)
	{
		strTemp1.Format(_T("alter table Label3 drop column S%d"),i);
		TCS.Execute(::GetApp()->GetBeliefBase(),strTemp1);
	}

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("alter table Label6 drop column Active"));
	for(int i = 0; i < 64; ++i)
	{
		strTemp1.Format(_T("alter table Label6 drop column S%d"),i);
		TCS.Execute(::GetApp()->GetBeliefBase(),strTemp1);
	}

	TCS.Execute(::GetApp()->GetBeliefBase(),_T("drop table Test1"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("create table Test1 (Sequence Integer, Name Text)"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Test1 (Sequence, Name) values(1,'A')"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Test1 (Sequence, Name) values(2,'B')"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("create unique index idxSequence on Test1 (Sequence asc)")); 
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("alter table Test1 add constraint pkTest1 primary key (Sequence)")); 
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Test1 (Sequence, Name) values(2,'Bb')"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("alter table Test1 add Id Integer"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("insert into Test1 (Sequence, Name, Id) values(3,'C',1)"));
	TCS.Execute(::GetApp()->GetBeliefBase(),_T("alter table Test1 drop column Id"));

	//alter table production.transactionhistoryarchive drop constraint pk_transactionhistoryarchive_transactionid
	//alter table Test1 add constraint fk_contactbacup_contact foreign key (contactid) references person.contact (contactid)
}
*/

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
Hatcher

Bit Labels

Shadow 
Light

Line Labels

Water 
Metal
Wood 
Fire

Trigram Labels

Accepting 
Stillness 
Exposure 
Adaptation 
Arousal
Arising
Satisfaction
Creating

Hexagram Labels

Creating 
Accepting 
Rallying 
Inexperience 
Anticipation 
Contention 
The Militia 
Belonging 
Raising Small Beasts 
Respectful Conduct 
Interplay 
Separating 
Fellowship With Others 
Big Domain 
Authenticity 
Readiness 
Following 
Detoxifying 
Taking Charge 
Perspective 
Biting Through 
Adornment 
Decomposing 
Returning 
Without Pretense 
Raising Great Beasts 
Hungry Mouth 
Greatness in Excess 
Exposure 
Arising 
Reciprocity 
Continuity 
Distancing
Big and Strong
Expansion
Brightness Obscured
Family Members
Estrangement
Impasse
Release
Decreasing
Increasing
Decisiveness
Dissipation
Collectedness
Advancement
Exhaustion
The Well
Seasonal Change
The Cauldron
Arousal
Stillness
Gradual Progress
Little Sister’s Marriage
Abundance
The Wanderer
Adaptation
Satisfaction
Scattering
Boundaries
The Truth Within
Smallness in Excess
Already Complete
Not Yet Complete

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

Abraham

Chen, Arousing (Zhen, Quake)
Sun, Gentle (Sun, Compliance)
Li, Clinging (Li, Cohesion)
K’un, Receptive (Kun, Earth)
Tui, Joyous (Dui, Joy)
Ch’ien, Creative, (Qian, Pure Yang)
K’an, Abysmal (Kan, Sink Hole)
Ken, Keeping Still (Gen, Restraint)

New moon 0 to 45* Emergence, ::/ Chen
Crescent, 45 to 90* Expansion, //: Sun
First quarter 90 to 135* Action, /:/ Li
Gibbous 135 to 180* Evercoming, ::: K’un
Full moon 180 to 225* Fulfillment, :// Tui
Disseminating 225* to 270* Overcoming, /// Ch’ien
Last quarter, 270 to 315* Reorienting, :/: K’an
Balsamic 315 to 360* Release, /:: Ken

> 6. Li Moon
> 5. Sun Mercury
> 4. Tui Venus
> 3. K'an Mars
> 2. Chen Jupiter
> 1. Ken Saturn
*/

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void CLoadData::GenerateLsl()
{
	CStdioFile F;
	CTextSet6 TS6;
	CString strTemp1,strTemp2,strTemp3;
	CLabelBlockArray* prgLB6 = ::GetApp()->GetLabelBlock6();
	CLabelEntryArray* prgLE6 = prgLB6->GetAt(2)->GetEntryArray();

	if(F.Open(_T("C:\\Src\\IChing.308\\IChingSl\\IChing1.lsl"),CFile::modeCreate | CFile::modeWrite | CFile::typeText))
	{
		F.WriteString(_T("\n"));
		F.WriteString(_T("///////////////////////////////////////////////////////////////////////////////////////////////\n"));
		F.WriteString(_T("///////////////////////////////////////////////////////////////////////////////////////////////\n"));
		F.WriteString(_T("//	Global Variables\n"));
		F.WriteString(_T("///////////////////////////////////////////////////////////////////////////////////////////////\n"));
		F.WriteString(_T("///////////////////////////////////////////////////////////////////////////////////////////////\n\n"));

		F.WriteString(_T("list hexagramLabels = [\n"));
		for(int i = 0; i  < 64; ++i)
		{
			strTemp1.Format(_T("	\"%s\"%s\n"),prgLE6->GetAt(i)->GetLabel(),i == 63 ? _T("") : _T(","));
			F.WriteString(strTemp1);
		}
		F.WriteString(_T("];\n\n"));

		F.WriteString(_T("list hexagramImages = [\n"));
		strTemp1.Format(_T("select * from Text6 where Name = 'Processed Wilhelm' and Type = 'Image'"));
		if(TS6.OpenRowset(::GetApp()->GetBeliefBase(),strTemp1))
			for(int i = 0; i < 64; ++i)
			{
				strTemp2 = ReplaceString(TS6.m_szText[i],_T("\\n"),_T("<br/> "));
				strTemp2 = ReplaceString(strTemp2,_T("\\n"),_T("<br/>"));
				strTemp2 = ReplaceString(strTemp2,_T("disciple of wisdom"),_T("superior man"));
				strTemp2.GetBufferSetLength(200);
				strTemp2.ReleaseBuffer();
				strTemp1.Format(_T("	\"%s\"%s\n"),strTemp2,i == 63 ? _T("") : _T(","));
				F.WriteString(strTemp1);
			}
		F.WriteString(_T("];\n\n"));

		F.WriteString(_T("///////////////////////////////////////////////////////////////////////////////////////////////\n"));
		F.WriteString(_T("///////////////////////////////////////////////////////////////////////////////////////////////\n"));
		F.WriteString(_T("//	Global Functions\n"));
		F.WriteString(_T("///////////////////////////////////////////////////////////////////////////////////////////////\n"));
		F.WriteString(_T("///////////////////////////////////////////////////////////////////////////////////////////////\n\n"));

		F.WriteString(_T("integer bitValue()\n"));
		F.WriteString(_T("{\n"));
		F.WriteString(_T("	return (integer) (llFrand(0.01) * 100);\n"));
		F.WriteString(_T("}\n\n"));

		F.WriteString(_T("integer lineValue()\n"));
		F.WriteString(_T("{\n"));
		F.WriteString(_T("	return bitValue() + bitValue() + bitValue();\n"));
		F.WriteString(_T("}\n\n"));

		F.WriteString(_T("string hexagramLabel(integer i)\n"));
		F.WriteString(_T("{\n"));
		F.WriteString(_T("	return llList2String(hexagramLabels,i);\n"));
		F.WriteString(_T("}\n\n"));

		F.WriteString(_T("string hexagramImage(integer i)\n"));
		F.WriteString(_T("{\n"));
		F.WriteString(_T("	return llList2String(hexagramImages,i);\n"));
		F.WriteString(_T("}\n\n"));

		F.WriteString(_T("///////////////////////////////////////////////////////////////////////////////////////////////\n"));
		F.WriteString(_T("///////////////////////////////////////////////////////////////////////////////////////////////\n"));
		F.WriteString(_T("//	States\n"));
		F.WriteString(_T("///////////////////////////////////////////////////////////////////////////////////////////////\n"));
		F.WriteString(_T("///////////////////////////////////////////////////////////////////////////////////////////////\n\n"));

		F.WriteString(_T("default\n"));
		F.WriteString(_T("{\n"));
		F.WriteString(_T("	state_entry()\n"));
		F.WriteString(_T("	{\n"));
		F.WriteString(_T("		llSay(0,\"Reset\");\n"));
		F.WriteString(_T("	}\n\n"));

		F.WriteString(_T("	touch_start(integer total_number)\n"));
		F.WriteString(_T("	{\n"));
		F.WriteString(_T("		integer i;\n"));
		F.WriteString(_T("		rotation Y_10 = llEuler2Rot(<0,10 * DEG_TO_RAD,0>);\n"));
		F.WriteString(_T("		for(i = 1; i < 19; i++)\n"));
		F.WriteString(_T("		{\n"));
		F.WriteString(_T("			rotation newRotation = llGetRot() * Y_10;\n"));
		F.WriteString(_T("			llSetRot(newRotation);\n"));
		F.WriteString(_T("		} \n"));
		F.WriteString(_T("		i = (integer) (llFrand(0.63) * 100);\n"));
		F.WriteString(_T("		llSay(0,hexagramLabel(i) + \"\\n\" + hexagramImage(i));\n"));
		F.WriteString(_T("//		llSay(0,hexagramImage(i));\n"));
		F.WriteString(_T("	}\n"));
		F.WriteString(_T("}\n\n"));

		F.Close();
	}
}

void CLoadData::GenerateXml()
{
	int nPower[] = {1,2,3,6};
	CStdioFile F;
	CTextSet6 TS6;
	CSequenceBlockArray* prgSB;
	CSequenceEntryArray* prgSE;
	CLabelBlockArray* prgLB;
	CLabelEntryArray* prgLE;
	CString strTemp1,strTemp2,strTemp3;

	for(int i = 0; i < 4; ++i)
	{
		switch(i)
		{
		case 0:
			prgSB = ::GetApp()->GetSequenceBlock1();
			prgLB = ::GetApp()->GetLabelBlock1();
			break;
		case 1:
			prgSB = ::GetApp()->GetSequenceBlock2();
			prgLB = ::GetApp()->GetLabelBlock2();
			break;
		case 2:
			prgSB = ::GetApp()->GetSequenceBlock3();
			prgLB = ::GetApp()->GetLabelBlock3();
			break;
		case 3:
			prgSB = ::GetApp()->GetSequenceBlock6();
			prgLB = ::GetApp()->GetLabelBlock6();
			break;
		}

		strTemp1.Format(_T("C:\\Src\\IChing.308\\Resources\\My Documents\\IChing\\Xml\\Sequence%d.xml"),nPower[i]);
		if(F.Open(strTemp1,CFile::modeCreate | CFile::modeWrite | CFile::typeText))
		{
			F.WriteString(_T("<?xml version=\"1.0\"?>\n"));
			strTemp1.Format(_T("<Sequence%d xmlns=\"http://tempuri.org/Sequence%d.xsd\">\n"),nPower[i],nPower[i]);
			F.WriteString(strTemp1);
			for(int j = 0; j <= prgSB->GetUpperBound(); ++j)
			{
				prgSE = prgSB->GetAt(j)->GetEntryArray();
				strTemp1.Format(_T("	<Sequence Id=\"%d\" Name=\"%s\" "),(j + 1) * 10,prgSB->GetAt(j)->GetName());
				F.WriteString(strTemp1); 
				for(int k = 0; k < int(ldexp(1.0,nPower[i])); ++k)
				{
					strTemp1.Format(_T("S%d=\"%d\" "),k,prgSE->GetAt(k)->GetSequence());
					F.WriteString(strTemp1);
				}
			F.WriteString(_T("/>\n"));
			}
			strTemp1.Format(_T("</Sequence%d>\n"),nPower[i]);
			F.WriteString(strTemp1);
			F.Close();
		}

		strTemp1.Format(_T("C:\\Src\\IChing.308\\Resources\\My Documents\\IChing\\Xml\\Label%d.xml"),nPower[i]);
		if(F.Open(strTemp1,CFile::modeCreate | CFile::modeWrite | CFile::typeText))
		{
			F.WriteString(_T("<?xml version=\"1.0\"?>\n"));
			strTemp1.Format(_T("<Label%d xmlns=\"http://tempuri.org/Label%d.xsd\">\n"),nPower[i],nPower[i]);
			F.WriteString(strTemp1);
			for(int j = 0; j <= prgLB->GetUpperBound(); ++j)
			{
				prgLE = prgLB->GetAt(j)->GetEntryArray();
				strTemp1.Format(_T("	<Label Id=\"%d\" Name=\"%s\" "),(j + 1) * 10,prgLB->GetAt(j)->GetName());
				F.WriteString(strTemp1); 
				for(int k = 0; k < int(ldexp(1.0,nPower[i])); ++k)
				{
					strTemp1.Format(_T("L%d=\"%s\" "),k,prgLE->GetAt(k)->GetLabel());
					F.WriteString(strTemp1);
				}
			F.WriteString(_T("/>\n"));
			}
			strTemp1.Format(_T("</Label%d>\n"),nPower[i]);
			F.WriteString(strTemp1);
			F.Close();
		}
	}

/*
			F.WriteString(_T("list hexagramImages = [\n"));
			strTemp1.Format(_T("select * from Text6 where Name = 'Processed Wilhelm' and Type = 'Image'"));
			if(TS6.OpenRowset(::GetApp()->GetBeliefBase(),strTemp1))
				for(int i = 0; i < 64; ++i)
				{
					strTemp2 = ReplaceString(TS6.m_szText[i],_T("\\n"),_T("<br/> "));
					F.WriteString(strTemp1);
				}
			F.WriteString(_T("];\n\n"));
*/
}

void CLoadData::GenerateCSharp()
{
	int nPower[] = {1,2,3,6};
	CStdioFile F;
	CTextSet6 TS6;
	CSequenceBlockArray* prgSB;
	CSequenceEntryArray* prgSE;
	CLabelBlockArray* prgLB;
	CLabelEntryArray* prgLE;
	CString strTemp1,strTemp2,strTemp3;
	CString strDiagram[] = {_T("Line"),_T("Trigram"),_T("Hexagram")};

	if(F.Open(_T("C:\\Src\\YiJing.wp7\\YiJing\\Sequence.cs"),CFile::modeCreate | CFile::modeWrite | CFile::typeText))
	{
		F.WriteString(_T("using System;\n\n"));
		F.WriteString(_T("namespace YiJing\n"));
		F.WriteString(_T("{\n"));
		F.WriteString(_T("	public class Sequences\n"));
		F.WriteString(_T("	{\n"));

		F.WriteString(_T("\n		public static String[,] strDiagramSequences = {\n			{"));
		for(int i = 1; i < 4; ++i)
		{
			switch(i)
			{
			case 1:
				prgSB = ::GetApp()->GetSequenceBlock2();
				break;
			case 2:
				prgSB = ::GetApp()->GetSequenceBlock3();
				break;
			case 3:
				prgSB = ::GetApp()->GetSequenceBlock6();
				break;
			}

			for(int j = 0; j <= prgSB->GetUpperBound(); ++j)
			{
				strTemp1.Format(_T("\"%s\"%s"),prgSB->GetAt(j)->GetName(),j == prgSB->GetUpperBound() ? _T("") : _T(","));
				F.WriteString(strTemp1.MakeLower()); 
			}
			strTemp1.Format(_T("},\n			{"));
			F.WriteString(strTemp1);
		}

		F.WriteString(_T("\n\n		public static String[,] strDiagramLabels = {\n			{"));
		for(int i = 1; i < 4; ++i)
		{
			switch(i)
			{
			case 1:
				prgLB = ::GetApp()->GetLabelBlock2();
				break;
			case 2:
				prgLB = ::GetApp()->GetLabelBlock3();
				break;
			case 3:
				prgLB = ::GetApp()->GetLabelBlock6();
				break;
			}

			for(int j = 0; j <= prgLB->GetUpperBound(); ++j)
			{
				strTemp1.Format(_T("\"%s\"%s"),prgLB->GetAt(j)->GetName(),j == prgLB->GetUpperBound() ? _T("") : _T(","));
				F.WriteString(strTemp1.MakeLower()); 
			}
			strTemp1.Format(_T("},\n			{"));
			F.WriteString(strTemp1);
		}

		for(int i = 1; i < 4; ++i)
		{
			switch(i)
			{
			case 1:
				prgSB = ::GetApp()->GetSequenceBlock2();
				prgLB = ::GetApp()->GetLabelBlock2();
				break;
			case 2:
				prgSB = ::GetApp()->GetSequenceBlock3();
				prgLB = ::GetApp()->GetLabelBlock3();
				break;
			case 3:
				prgSB = ::GetApp()->GetSequenceBlock6();
				prgLB = ::GetApp()->GetLabelBlock6();
				break;
			}

			F.WriteString(_T("\n\n		"));
			strTemp1.Format(_T("public static Int32[,] n%sSequences = {\n			{"),strDiagram[i - 1]);
			F.WriteString(strTemp1);
			for(int j = 0; j <= prgSB->GetUpperBound(); ++j)
			{
				prgSE = prgSB->GetAt(j)->GetEntryArray();
				for(int k = 0; k < int(ldexp(1.0,nPower[i])); ++k)
				{
					strTemp1.Format(_T("%d%s"),prgSE->GetAt(k)->GetSequence(),k == int(ldexp(1.0,nPower[i])) - 1 ? _T("") : _T(","));
					F.WriteString(strTemp1);
				}
				strTemp1.Format(_T("}%s\n			%s"),j == prgSB->GetUpperBound() ? _T("") : _T(","),j == prgSB->GetUpperBound() ? _T("};\n") : _T("{"));
				F.WriteString(strTemp1);
			}

			F.WriteString(_T("\n		"));
			strTemp1.Format(_T("public static String[,] str%sLabels = {\n			{"),strDiagram[i - 1]);
			F.WriteString(strTemp1);
			for(int j = 0; j <= prgLB->GetUpperBound(); ++j)
			{
				prgLE = prgLB->GetAt(j)->GetEntryArray();
				for(int k = 0; k < int(ldexp(1.0,nPower[i])); ++k)
				{
					strTemp1.Format(_T("\"%s\"%s"),prgLE->GetAt(k)->GetLabel(),k == int(ldexp(1.0,nPower[i])) - 1 ? _T("") : _T(","));
					F.WriteString(strTemp1.MakeLower());
				}
				strTemp1.Format(_T("}%s\n			%s"),j == prgLB->GetUpperBound() ? _T("") : _T(","),j == prgLB->GetUpperBound() ? _T("};\n") : _T("{"));
				F.WriteString(strTemp1);
			}
		}
		F.WriteString(_T("	}\n"));
		F.WriteString(_T("}\n"));
		F.Close();
	}
}

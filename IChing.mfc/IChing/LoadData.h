#if !defined(AFX_LOADDATA_H__BDCFE6B9_8E0C_4656_995F_2FE5554FBE56__INCLUDED_)
#define AFX_LOADDATA_H__BDCFE6B9_8E0C_4656_995F_2FE5554FBE56__INCLUDED_

#pragma once

class CLoadData
{
public:
	CLoadData();
	virtual ~CLoadData();

	void LoadAll();

	void LoadNewTables();
	void LoadNewData();

	void GenerateLsl();
	void GenerateXml();
	void GenerateCSharp();

//	void ConvertNewBB();
//	void ConvertOldBB();
//	void AddSequenceTables();
//	void ConvertLabelTables();

//	void LoadNamedSequences();
//	void LoadNumberedSequences();

//	void CleanBB(LPCTSTR lpszName);

//	void LoadWilhelmText2BB();
//	void LoadLeggeText2BB();
//	void CopyWilhelmBB2VivashBB();
};

//{{AFX_INSERT_LOCATION}}

#endif

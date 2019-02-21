//
//  main.swift
//  Swift1
//
//  Created by Stephen Vivash on 21/2/19.
//  Copyright © 2019 Stephen Vivash. All rights reserved.
//

import Foundation

/*
public class CValueSequencer
{
	public CValueSequencer(int nInnerSequencers, int nValues, int nValue)
{
	m_nInnerSequencers = nInnerSequencers;
	m_nValues = nValues;
	m_pvsParent = null;
	m_pvsInner = null;
	//SetValue(nValue);
	}
	
	//~CValueSequencer() { }
	
	public int Value
{
	get { return m_nValue; }
	set { SetValue(value); }
	}
	
	public int Sequence
{
	get { return m_nSequence; }
	set { SetValue(FindValue(value)); }
	}
	
	public String ValueStr
{
	get { return Sequences.nHexagramSequences[0, Value].ToString();	}
	}
	
	public String SequenceStr
{
	get	{ return (Sequence + 1).ToString(); }
	}
	
	public String Label
{
	get { return GetLabel(); }
	}
	
	public bool IsMoving
{
	get { return GetMoving(); }
	}
	
	public virtual CValueSequencer First()
{
	SetValue(GetFirstSequence());
	UpdateInnerValues();
	UpdateOuterValues();
	return this;
	}
	
	public virtual CValueSequencer Previous(bool bRatio = false)
{
	if (!bRatio || (--m_nRatio == 0))
	{
	SetValue(GetPreviousSequence(m_nSequence));
	UpdateInnerValues();
	UpdateOuterValues();
	}
	return this;
	}
	
	public virtual CValueSequencer Next(bool bRatio = false)
{
	if (!bRatio || (--m_nRatio == 0))
	{
	SetValue(GetNextSequence(m_nSequence));
	UpdateInnerValues();
	UpdateOuterValues();
	}
	return this;
	}
	
	public virtual CValueSequencer Last()
{
	SetValue(GetLastSequence());
	UpdateInnerValues();
	UpdateOuterValues();
	return this;
	}
	
	public virtual CValueSequencer Inverse() { return this; }
	
	public virtual CValueSequencer Opposite()
{
	SetValue(~m_nValue & (m_nValues - 1));
	UpdateInnerValues();
	UpdateOuterValues();
	return this;
	}
	
	public virtual CValueSequencer Transverse() { return this; }
	public virtual CValueSequencer Nuclear() { return this; }
	public virtual CValueSequencer Move() { return this; }
	public virtual CValueSequencer Young() { return this; }
	public virtual CValueSequencer Old() { return this; }
	
	public virtual CValueSequencer Update()
{
	SetValue(m_nValue);
	UpdateInnerValues();
	UpdateOuterValues();
	return this;
	}
	
	public virtual void UpdateInnerValues() { }
	public virtual void UpdateOuterValues() { }
	
	public void SetParent(CValueSequencer pvsParent) { m_pvsParent = pvsParent; }
	protected CValueSequencer GetParent() { return m_pvsParent; }
	
	protected CValueSequencer GetChild(int nIndex)
	{
	return m_pvsInner[nIndex];
	}
	
	private CValueSequencer SetValue(int nValue)
{
	if((nValue >= 0) && (nValue <= m_nValues))
	{
	m_nValue = nValue;
	m_nSequence = m_nSequences[GetCurrentSequence(), nValue];
	m_nRatio = m_nRatios[GetCurrentRatio(), nValue];
	}
	return this;
	}
	
	protected int FindValue(int nSequence)
	{
	for (int i = 0; i < m_nValues; ++i)
	if (m_nSequences[GetCurrentSequence(), i] == nSequence)
	return i;
	return -1;
	}
	
	protected int GetFirstSequence()
	{
	return FindValue(0);
	}
	
	protected int GetPreviousSequence(int nSequence)
	{
	if (nSequence > 0)
	return FindValue(--nSequence);
	return GetLastSequence();
	}
	
	protected int GetNextSequence(int nSequence)
	{
	if (nSequence < m_nValues - 1)
	if ((nSequence = FindValue(++nSequence)) != -1)
	return nSequence;
	return GetFirstSequence();
	}
	
	protected int GetLastSequence()
	{
	int nSequence1,nSequence2 = m_nValues;
	while(nSequence2 > -1)
	if ((nSequence1 = FindValue(--nSequence2)) != -1)
	return nSequence1;
	return -1;
	}
	
	protected virtual String GetLabel() { return ""; }
	protected virtual bool GetMoving() { return false; }
	
	protected virtual int GetCurrentSequence() { return 0; }
	protected virtual int GetCurrentRatio() { return 0; }
	protected virtual int GetCurrentLabel() { return 0; }
	
	protected int m_nValue;
	protected int m_nSequence;
	protected int m_nRatio;
	
	protected int m_nInnerSequencers;
	protected int m_nValues;
	
	protected int[,] m_nSequences;
	protected int[,] m_nRatios;
	protected CValueSequencer m_pvsParent;
	protected CValueSequencer[] m_pvsInner;
}*/

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

public class CValueSequencer {
	
	init(nInnerSequencers: Int, nValues: Int, nValue: Int) {
		m_nInnerSequencers = nInnerSequencers
		m_nValues = nValues
		//m_pvsParent = CValueSequencer(nInnerSequencers: 0, nValues: 0, nValue: 0)
        //m_pvsInner = [CValueSequencer(nInnerSequencers: 0, nValues: 0, nValue: 0)]
        ///////SetValue(nValue);
	}

	public var Value: Int {
		get {return m_nValue}
		set(value) {SetValue(nValue: value)}
	}
	
	public var Sequence: Int {
		get {return m_nSequence}
		set(value) {SetValue(nValue: FindValue(nSequence: value))}
	}
	
	private func SetValue(nValue: Int) -> CValueSequencer {
		if (nValue >= 0) && (nValue <= m_nValues)
		{
			m_nValue = nValue
			m_nSequence = m_nSequences[GetCurrentSequence()][nValue]
			m_nRatio = m_nRatios[GetCurrentRatio()][nValue]
		}
		return self
	}
	
	private func FindValue(nSequence: Int) -> Int {
		for i in 0 ... m_nValues {
			if m_nSequences[GetCurrentSequence()][i] == nSequence {
				return i
			}
		}
		return -1
	}

	public var ValueStr: String {
		get { return "XXX" /*Sequences.nHexagramSequences[0][Value].ToString()*/}
	}
	
	public var SequenceStr: String {
		get	{ return "XXX" /*(Sequence + 1).ToString()*/ }
	}
	
	public var Label: String {
		get { return GetLabel() }
	}
	
	public var IsMoving: Bool {
		get { return GetMoving() }
	}
	
	public func Transverse() -> CValueSequencer { return self; }
	public func Nuclear() -> CValueSequencer { return self; }
	public func Move() -> CValueSequencer { return self; }
	public func Young() -> CValueSequencer { return self; }
	public func Old() -> CValueSequencer { return self; }
	
	public func Update() -> CValueSequencer {
		SetValue(nValue: m_nValue)
		UpdateInnerValues()
		UpdateOuterValues()
		return self
	}
	
	public func UpdateInnerValues() {}
	public func UpdateOuterValues() {}
	
	public func SetParent(pvsParent: CValueSequencer) { m_pvsParent = pvsParent }
	public func GetParent() -> CValueSequencer? { return m_pvsParent }
	
	public func GetChild(nIndex: Int) -> CValueSequencer? {
        return m_pvsInner?[nIndex]
	}
	
	public func GetLabel() -> String { return ""; }
	public func GetMoving() -> Bool { return false; }
	
	public func GetCurrentSequence() -> Int { return 0; }
	public func GetCurrentRatio() -> Int { return 0; }
	public func GetCurrentLabel() -> Int { return 0; }

	private var m_nValue = 0
	private var m_nSequence: Int = 0
	private var m_nRatio: Int = 0
	
	private var m_nInnerSequencers: Int = 0
	private var m_nValues: Int = 0
	
	private var m_nSequences: [[Int]] = [[0,1],[0,1]]
	private var m_nRatios: [[Int]] = [[0,1],[0,1]]
	private var m_pvsParent: CValueSequencer? // = CValueSequencer(nInnerSequencers: 0, nValues: 0, nValue: 0)
	private var  m_pvsInner: [CValueSequencer]? // = [CValueSequencer(nInnerSequencers: 0, nValues: 0, nValue: 0)]
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

class Test1 {
    func Test() -> Int {
        return i;
    }
    var i: Int = 1
}

class Test2: Test1 {
    override func Test() -> Int {
        return j;
    }
    var j: Int = 2
}

func Test() {
    var t1a: Test1 = Test1()
    var t1b: Test1 = Test2()
    var t2a: Test2 = Test2()
    
    print("Hello, world!")

    print (t1b.Test())
    t1b = t1a
    print (t1b.Test())

	var vs: CValueSequencer = CValueSequencer(nInnerSequencers: 0, nValues: 4, nValue: 0)
	vs.Value = 0
	vs.Value = 1

}

Test()
//
//  ValueSequencer.swift
//  Swift1
//
//  Created by Stephen Vivash on 23/2/19.
//  Copyright © 2019 Stephen Vivash. All rights reserved.
//

//import Foundation

public class CValueSequencer
{
	init(nInnerSequencers: Int, nValues: Int, nValue: Int) {
		m_nInnerSequencers = nInnerSequencers
		m_nValues = nValues
		//m_pvsParent = CValueSequencer(nInnerSequencers: 0, nValues: 0, nValue: 0)
		//m_pvsInner = [CValueSequencer(nInnerSequencers: 0, nValues: 0, nValue: 0)]
		///////SetValue(nValue)
	}

	public var Value: Int {
		get {return m_nValue}
		set(value) {SetValue(nValue: value)}
	}
	
	public var Sequence: Int {
		get {return m_nSequence}
		set(value) {SetValue(nValue: FindValue(nSequence: value))}
	}
	
	public var ValueStr: String {
		get { return "XXX" /*Sequences.nHexagramSequences[0][Value].ToString()*/}
	}
	
	public var SequenceStr: String {
		get	{return String(Sequence + 1)}
	}
	
	public var Label: String {
		get { return GetLabel() }
	}
	
	public var IsMoving: Bool {
		get { return GetMoving() }
	}
	
	public func First() -> CValueSequencer {
		SetValue(nValue: GetFirstSequence())
		UpdateInnerValues()
		UpdateOuterValues()
		return self
	}

	public func Previous(bRatio: Bool = false) -> CValueSequencer {
		m_nRatio = m_nRatio - 1
		if !bRatio || (m_nRatio >= 0) {
			SetValue(nValue: GetPreviousSequence(nSequence: m_nSequence))
			UpdateInnerValues()
			UpdateOuterValues()
		}
		return self
	}

	public func Next(bRatio: Bool = false) -> CValueSequencer {
		m_nRatio = m_nRatio - 1
		if !bRatio || (m_nRatio >= 0) {
			SetValue(nValue: GetNextSequence(nSequence: m_nSequence))
			UpdateInnerValues()
			UpdateOuterValues()
		}
		return self
	}

	public func Last() -> CValueSequencer {
		SetValue(nValue: GetLastSequence())
		UpdateInnerValues()
		UpdateOuterValues()
		return self
	}

	public func Inverse() -> CValueSequencer { return self }

	public func Opposite() -> CValueSequencer {
		SetValue(nValue: ~m_nValue & (m_nValues - 1))
		UpdateInnerValues()
		UpdateOuterValues()
		return self
	}

	public func Transverse() -> CValueSequencer { return self }
	public func Nuclear() -> CValueSequencer { return self }
	public func Move() -> CValueSequencer { return self }
	public func Young() -> CValueSequencer { return self }
	public func Old() -> CValueSequencer { return self }

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
	
	@discardableResult
	private func SetValue(nValue: Int) -> CValueSequencer {
		if (nValue >= 0) && (nValue <= m_nValues) {
			m_nValue = nValue
			m_nSequence = m_nSequences[GetCurrentSequence()][nValue]
			m_nRatio = m_nRatios[GetCurrentRatio()][nValue]
		}
		return self
	}

	private func FindValue(nSequence: Int) -> Int {
		for i in 0 ... m_nValues - 1 {
			if m_nSequences[GetCurrentSequence()][i] == nSequence {
				return i
			}
		}
		return -1
	}
	
	private func GetFirstSequence() -> Int {
		return FindValue(nSequence: 0)
	}

	private func GetPreviousSequence(nSequence: Int) -> Int {
		if nSequence > 0 {
			let s = nSequence - 1
			return FindValue(nSequence: s)
		}
		return GetLastSequence()
	}

	private func GetNextSequence(nSequence: Int) -> Int	{
		if nSequence < m_nValues - 1 {
			var s = nSequence + 1
			s = FindValue(nSequence: s)
			if s != -1 {
				return s
			}
		}
		return GetFirstSequence()
	}

	private func GetLastSequence() -> Int {
		var nSequence1 = 0
		var nSequence2 = m_nValues
		while nSequence2 > -1 {
			nSequence2 = nSequence2 - 1
			nSequence1 = FindValue(nSequence: nSequence2)
			if nSequence1 != -1 {
				return nSequence1
			}
		}
		return -1
	}
	
	public func GetLabel() -> String { return "" }
	public func GetMoving() -> Bool { return false }
	
	public func GetCurrentSequence() -> Int { return 1 } /////////////////////// ????????????????????????????
	public func GetCurrentRatio() -> Int { return 0 }
	public func GetCurrentLabel() -> Int { return 0 }

	private var m_nValue = 0
	private var m_nSequence: Int = 0
	private var m_nRatio: Int = 0
	
	private var m_nInnerSequencers: Int = 0
	private var m_nValues: Int = 0
	
	var m_nSequences: [[Int]] = [[0,1,2,3],[2,3,1,0]]
	var m_nRatios: [[Int]] = [[1,1,1,1],[1,3,3,1],[1,5,7,3],[1,4,4,1],[5,1,10,1],[1,10,1,5]]
    
	var m_pvsParent: CValueSequencer? // = CValueSequencer(nInnerSequencers: 0, nValues: 0, nValue: 0)
	var  m_pvsInner: [CValueSequencer]? // = [CValueSequencer(nInnerSequencers: 0, nValues: 0, nValue: 0)]
}

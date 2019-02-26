public class CLineValueSequencer : CValueSequencer
{
	public init(nValue: Int) {
		super.init(nInnerSequencers: 3, nValues: 4, nValue: nValue)
		
		m_pvsInner = [CBitValueSequencer(nValue: 0) as CValueSequencer,CBitValueSequencer(nValue: 0) as CValueSequencer,
			CBitValueSequencer(nValue: 0) as CValueSequencer]
		//m_pvsInner?[2] = CBitValueSequencer(nValue: 0)
 		//m_pvsInner?[1] = CBitValueSequencer(nValue: 0)
 		//m_pvsInner?[0] = CBitValueSequencer(nValue: 0)
		Bit(nIndex: 2).SetParent(pvsParent: self)
 		Bit(nIndex: 1).SetParent(pvsParent: self)
 		Bit(nIndex: 0).SetParent(pvsParent: self)
 		//m_nSequences = Sequences.nLineSequences
 		//m_nRatios = Sequences.nLineRatios
		
	}

	public func Bit(nIndex: Int) -> CBitValueSequencer {
 		return m_pvsInner![nIndex] as! CBitValueSequencer
 	}

	@discardableResult
	public override func Inverse() -> CValueSequencer {
 		switch (Value)
 		{
 	 		case 0:
 	 			Value = 1
 	 			//break
 	 		case 1:
 	 			Value = 0
 	 			break
 	 		case 2:
 	 			Value = 3
 	 			break
 	 		case 3:
 	 			Value = 2
 	 			break
			default:
				let _ = 0
 		}
 		UpdateInnerValues()
 		UpdateOuterValues()
 		return self
 	}

 	@discardableResult
	public override func Move() -> CValueSequencer {
		if Value == 0 {
 	 		Value = 1
		}
 		else if Value == 3 {
			Value = 2
		}
 		UpdateInnerValues()
 		UpdateOuterValues()
 		return self
 	}

 	@discardableResult
	public override func Young() -> CValueSequencer {
		if Value == 0 {
 	 		Value = 2
		}
		else if Value == 3 {
			Value = 1
		}
 		UpdateInnerValues()
 		UpdateOuterValues()
 		return self
 	}

 	@discardableResult
	public override func Old() -> CValueSequencer {
		if Value == 2 {
 	 		Value = 0
		}
		else if Value == 1 {
			Value = 3
		}
 		UpdateInnerValues()
 		UpdateOuterValues()
 		return self
 	}

 	public override func UpdateInnerValues() {
 		switch (Value)
 		{
 	 		case 0:
				Bit(nIndex: 2).Value = 0
 	 			Bit(nIndex: 1).Value = 0
 	 			Bit(nIndex: 0).Value = 0
 	 			//break
 	 		case 1:
 	 			Bit(nIndex: 2).Value = 0
 	 			Bit(nIndex: 1).Value = 1
 	 			Bit(nIndex: 0).Value = 0
 	 			break
 	 		case 2:
 	 			Bit(nIndex: 2).Value = 1
 	 			Bit(nIndex: 1).Value = 0
 	 			Bit(nIndex: 0).Value = 1
 	 			break
 	 		case 3:
 	 			Bit(nIndex: 2).Value = 1
 	 			Bit(nIndex: 1).Value = 1
 	 			Bit(nIndex: 0).Value = 1
 	 			break
			default:
				let _ = 0
 		}
 		Bit(nIndex: 2).UpdateInnerValues()
 		Bit(nIndex: 1).UpdateInnerValues()
 		Bit(nIndex: 0).UpdateInnerValues()
 	}

 	public override func UpdateOuterValues() {
 		Value = Bit(nIndex: 0).Value + Bit(nIndex: 1).Value + Bit(nIndex: 2).Value
 		m_pvsParent?.UpdateOuterValues()
 	}

	public static func SetCurrentSequence(nSequence: Int) {
 		m_nCurrentSequence = nSequence
 	}

	public static func SetCurrentRatio(nRatio: Int) {
 		m_nCurrentRatio = nRatio
 	}

	public static func SetCurrentLabel(nLabel: Int) {
 		m_nCurrentLabel = nLabel
 	}

 	public override func GetLabel() -> String {
 		return "Sequences.strLineLabels[GetCurrentLabel(),Value]"
 	}

 	public override func GetMoving() -> Bool {
 		return (Value == 0) || (Value == 3)
 	}

	public override func GetCurrentSequence() -> Int { return CLineValueSequencer.m_nCurrentSequence }
	public override func GetCurrentRatio() -> Int { return CLineValueSequencer.m_nCurrentRatio }
	public override func GetCurrentLabel() -> Int { return CLineValueSequencer.m_nCurrentLabel }

	private static var m_nCurrentSequence: Int = 1 /////////// ?????????????????
 	private static var m_nCurrentRatio: Int = 0
 	private static var m_nCurrentLabel: Int = 0
}

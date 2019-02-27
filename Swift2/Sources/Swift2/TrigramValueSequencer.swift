
public class CTrigramValueSequencer : CValueSequencer {
	
	init(nValue: Int) {
		super.init(nInnerSequencers: 3, nValues: 8, nValue: nValue)
		m_pvsInner = [CLineValueSequencer(nValue: 0) as CValueSequencer,CLineValueSequencer(nValue: 0) as CValueSequencer,
			CLineValueSequencer(nValue: 0) as CValueSequencer]
		//m_pvsInner[2] = new CLineValueSequencer(0)
		//m_pvsInner[1] = new CLineValueSequencer(0)
		//m_pvsInner[0] = new CLineValueSequencer(0)
		Line(nIndex: 2).SetParent(pvsParent: self)
		Line(nIndex: 1).SetParent(pvsParent: self)
		Line(nIndex: 0).SetParent(pvsParent: self)
		m_nSequences = Sequences.nTrigramSequences
		m_nRatios = Sequences.nTrigramRatios
	}

	public func Line(nIndex: Int) -> CLineValueSequencer {
 		return m_pvsInner[nIndex] as! CLineValueSequencer
	}

	@discardableResult
	public override func Inverse() -> CValueSequencer {
		var bMoving: [Bool] = [ false, false, false ]
		SaveMoving(bMoving: &bMoving)
		Value = (Line(nIndex: 2).Value % 2 == 0 ? 0 : 1) +
			((Line(nIndex: 1).Value % 2 == 0 ? 0 : 1) * 2) +
			((Line(nIndex: 0).Value % 2 == 0 ? 0 : 1) * 4)
		UpdateInnerValues()
		UpdateOuterValues()
		RestoreMoving(bMoving: &bMoving, bInverseLine: true)
		return self
	}

	@discardableResult
	public override func Opposite() -> CValueSequencer {
		var bMoving: [Bool] = [ false, false, false ]
		SaveMoving(bMoving: &bMoving)
		super.Opposite()
		RestoreMoving(bMoving: &bMoving, bInverseLine: false)
		return self
	}

	@discardableResult
	public override func Move() -> CValueSequencer {
		Line(nIndex: 2).Move()
		Line(nIndex: 1).Move()
		Line(nIndex: 0).Move()
		return self
	}

	@discardableResult
	public override func Young() -> CValueSequencer {
		Line(nIndex: 2).Young()
		Line(nIndex: 1).Young()
		Line(nIndex: 0).Young()
		return self
	}

	public override func UpdateInnerValues() {
		switch Value {
		case 0:
			Line(nIndex: 2).Value = CTrigramValueSequencer.m_nYinLine
			Line(nIndex: 1).Value = CTrigramValueSequencer.m_nYinLine
			Line(nIndex: 0).Value = CTrigramValueSequencer.m_nYinLine
			break
		case 1:
			Line(nIndex: 2).Value = CTrigramValueSequencer.m_nYinLine
			Line(nIndex: 1).Value = CTrigramValueSequencer.m_nYinLine
			Line(nIndex: 0).Value = CTrigramValueSequencer.m_nYangLine
			break
		case 2:
			Line(nIndex: 2).Value = CTrigramValueSequencer.m_nYinLine
			Line(nIndex: 1).Value = CTrigramValueSequencer.m_nYangLine
			Line(nIndex: 0).Value = CTrigramValueSequencer.m_nYinLine
			break
		case 3:
			Line(nIndex: 2).Value = CTrigramValueSequencer.m_nYinLine
			Line(nIndex: 1).Value = CTrigramValueSequencer.m_nYangLine
			Line(nIndex: 0).Value = CTrigramValueSequencer.m_nYangLine
			break
		case 4:
			Line(nIndex: 2).Value = CTrigramValueSequencer.m_nYangLine
			Line(nIndex: 1).Value = CTrigramValueSequencer.m_nYinLine
			Line(nIndex: 0).Value = CTrigramValueSequencer.m_nYinLine
			break
		case 5:
			Line(nIndex: 2).Value = CTrigramValueSequencer.m_nYangLine
			Line(nIndex: 1).Value = CTrigramValueSequencer.m_nYinLine
			Line(nIndex: 0).Value = CTrigramValueSequencer.m_nYangLine
			break
		case 6:
			Line(nIndex: 2).Value = CTrigramValueSequencer.m_nYangLine
			Line(nIndex: 1).Value = CTrigramValueSequencer.m_nYangLine
			Line(nIndex: 0).Value = CTrigramValueSequencer.m_nYinLine
			break
		case 7:
			Line(nIndex: 2).Value = CTrigramValueSequencer.m_nYangLine
			Line(nIndex: 1).Value = CTrigramValueSequencer.m_nYangLine
			Line(nIndex: 0).Value = CTrigramValueSequencer.m_nYangLine
			break
		default:
			let _ = 0
		}
		Line(nIndex: 2).UpdateInnerValues()
		Line(nIndex: 1).UpdateInnerValues()
		Line(nIndex: 0).UpdateInnerValues()
	}

	public override func UpdateOuterValues() {
		Value = (Line(nIndex: 0).Value % 2 == 0 ? 0 : 1) +
			((Line(nIndex: 1).Value % 2 == 0 ? 0 : 1) * 2) +
			((Line(nIndex: 2).Value % 2 == 0 ? 0 : 1) * 4)
		m_pvsParent?.UpdateOuterValues()
	}

	private func SaveMoving(bMoving: inout [Bool]) {
		for l in 0 ... 2 {
			if (Line(nIndex: l).Value == 0) || (Line(nIndex: l).Value == 3) {
				bMoving[l] = true
			}
		}
	}

	private func RestoreMoving(bMoving: inout [Bool], bInverseLine: Bool) {
		for l in 0 ... 2 {
			if bMoving[l]	{
				var l1 = l
				if bInverseLine {
					if l1 == 0 {
						l1 = 2
					}
					else
					if l1 == 2 {
						l1 = 0
					}
				Line(nIndex: l1).Old()
				}
			}
		}
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
		return Sequences.strTrigramLabels[GetCurrentLabel()][Value]
	}

	public override func GetMoving() -> Bool {
		return Line(nIndex: 0).IsMoving || Line(nIndex: 1).IsMoving || Line(nIndex: 2).IsMoving
	}

	public override func GetCurrentSequence() -> Int { return CTrigramValueSequencer.m_nCurrentSequence }
	public override func GetCurrentRatio() -> Int { return CTrigramValueSequencer.m_nCurrentRatio }
	public override func GetCurrentLabel() -> Int{ return CTrigramValueSequencer.m_nCurrentLabel }

	private static var m_nCurrentSequence: Int = 0
	private static var m_nCurrentRatio: Int = 0
	private static var m_nCurrentLabel: Int = 0

	private static var m_nYinLine: Int = 2
	private static var m_nYangLine: Int = 1
}

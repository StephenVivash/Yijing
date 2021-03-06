
public class CHexagramValueSequencer : CValueSequencer {
	
	init(nValue: Int = 0) {
		super.init(nInnerSequencers: 2, nValues: 64, nValue: nValue)
		m_pvsInner = [CTrigramValueSequencer(nValue: 0) as CValueSequencer,CTrigramValueSequencer(nValue: 0) as CValueSequencer]
		//m_pvsInner[1] = new CTrigramValueSequencer(-1)
		//m_pvsInner[0] = new CTrigramValueSequencer(-1)
		Trigram(nIndex: 1).SetParent(pvsParent: self)
		Trigram(nIndex: 0).SetParent(pvsParent: self)
		m_nSequences = Sequences.nHexagramSequences
		m_nRatios = Sequences.nHexagramRatios
		Value = nValue
		UpdateInnerValues()
		//UpdateOuterValues()
	}

	convenience init(strValue: String) {
		let s = strValue.trimmingCharacters(in: [" "])
		if let fValue = Double(s) {
			self.init(nValue: Int(fValue))
			let s1 = s.split(separator: ".")
			if s1.count == 2 {
				var line = -1
				for l in "123456" {
					line += 1
					if s1[1].contains(l) {
						Trigram(nIndex: line / 3).Line(nIndex: line % 3).Next()
					}
				}
			}
		}
		else {
			self.init()
		}
	}

	convenience init(hvs: CHexagramValueSequencer) {
		self.init() // (nInnerSequencers: 2, nValues: 64, nValue: 0)
		Trigram(nIndex: 1).Line(nIndex: 2).Value = hvs.Trigram(nIndex: 1).Line(nIndex: 2).Value
		Trigram(nIndex: 1).Line(nIndex: 1).Value = hvs.Trigram(nIndex: 1).Line(nIndex: 1).Value
		Trigram(nIndex: 1).Line(nIndex: 0).Value = hvs.Trigram(nIndex: 1).Line(nIndex: 0).Value
		Trigram(nIndex: 0).Line(nIndex: 2).Value = hvs.Trigram(nIndex: 0).Line(nIndex: 2).Value
		Trigram(nIndex: 0).Line(nIndex: 1).Value = hvs.Trigram(nIndex: 0).Line(nIndex: 1).Value
		Trigram(nIndex: 0).Line(nIndex: 0).Value = hvs.Trigram(nIndex: 0).Line(nIndex: 0).Value

		Trigram(nIndex: 1).Line(nIndex: 2).UpdateInnerValues()
		Trigram(nIndex: 1).Line(nIndex: 1).UpdateInnerValues()
		Trigram(nIndex: 1).Line(nIndex: 0).UpdateInnerValues()
		Trigram(nIndex: 0).Line(nIndex: 2).UpdateInnerValues()
		Trigram(nIndex: 0).Line(nIndex: 1).UpdateInnerValues()
		Trigram(nIndex: 0).Line(nIndex: 0).UpdateInnerValues()

		Trigram(nIndex: 1).Line(nIndex: 2).UpdateOuterValues()
		Trigram(nIndex: 1).Line(nIndex: 1).UpdateOuterValues()
		Trigram(nIndex: 1).Line(nIndex: 0).UpdateOuterValues()
		Trigram(nIndex: 0).Line(nIndex: 2).UpdateOuterValues()
		Trigram(nIndex: 0).Line(nIndex: 1).UpdateOuterValues()
		Trigram(nIndex: 0).Line(nIndex: 0).UpdateOuterValues()

		//UpdateInnerValues()
		//UpdateOuterValues()
	}

	public func Trigram(nIndex: Int) -> CTrigramValueSequencer {
 		return m_pvsInner[nIndex] as! CTrigramValueSequencer
	}

	@discardableResult
	public override func Inverse() -> CValueSequencer {
		var bMoving: [[Bool]] = [[ false, false, false ], [ false, false, false ]]
		SaveMoving(bMoving: &bMoving)
		Value = ((Trigram(nIndex: 1).Line(nIndex: 2).Value % 2 == 0 ? 0 : 1) +
			((Trigram(nIndex: 1).Line(nIndex: 1).Value % 2 == 0 ? 0 : 1) * 2) +
			((Trigram(nIndex: 1).Line(nIndex: 0).Value % 2 == 0 ? 0 : 1) * 4)) +
			((((Trigram(nIndex: 0).Line(nIndex: 2).Value % 2 == 0 ? 0 : 1) +
			((Trigram(nIndex: 0).Line(nIndex: 1).Value % 2 == 0 ? 0 : 1) * 2) +
			((Trigram(nIndex: 0).Line(nIndex: 0).Value % 2 == 0 ? 0 : 1) * 4))) * 8)
		UpdateInnerValues()
		UpdateOuterValues()
		RestoreMoving(bMoving: &bMoving,bInverseTrigram: true, bInverseLine: true)
		return self
	}

	@discardableResult
	public override func Opposite() -> CValueSequencer {
		var bMoving: [[Bool]] = [[ false, false, false ], [ false, false, false ]]
		SaveMoving(bMoving: &bMoving)
		super.Opposite()
		RestoreMoving(bMoving: &bMoving, bInverseTrigram: false, bInverseLine: false)
		return self
	}

	@discardableResult
	public override func Transverse() -> CValueSequencer {
		var nTemp: Int
		var bMoving: [[Bool]] = [[ false, false, false ], [ false, false, false ]]
		SaveMoving(bMoving: &bMoving)
		nTemp = Trigram(nIndex: 0).Value
		Trigram(nIndex: 0).Value = Trigram(nIndex: 1).Value
		Trigram(nIndex: 1).Value = nTemp
		Value = Trigram(nIndex: 0).Value + Trigram(nIndex: 1).Value * 8
		UpdateInnerValues()
		UpdateOuterValues()
		RestoreMoving(bMoving: &bMoving, bInverseTrigram: true, bInverseLine: false)
		return self
	}

	@discardableResult
	public override func Nuclear() -> CValueSequencer {
		var nTemp: Int
		var bMoving: [[Bool]] = [[ false, false, false ], [ false, false, false ]]
		SaveMoving(bMoving: &bMoving)
		nTemp = Trigram(nIndex: 1).Line(nIndex: 0).Value
		Trigram(nIndex: 1).Line(nIndex: 2).Value = Trigram(nIndex: 1).Line(nIndex: 1).Value
		Trigram(nIndex: 1).Line(nIndex: 1).Value = Trigram(nIndex: 1).Line(nIndex: 0).Value
		Trigram(nIndex: 1).Line(nIndex: 0).Value = Trigram(nIndex: 0).Line(nIndex: 2).Value
		Trigram(nIndex: 0).Line(nIndex: 0).Value = Trigram(nIndex: 0).Line(nIndex: 1).Value
		Trigram(nIndex: 0).Line(nIndex: 1).Value = Trigram(nIndex: 0).Line(nIndex: 2).Value
		Trigram(nIndex: 0).Line(nIndex: 2).Value = nTemp
		Value = ((((Trigram(nIndex: 1).Line(nIndex: 2).Value % 2 == 0 ? 0 : 1) * 4) +
			((Trigram(nIndex: 1).Line(nIndex: 1).Value % 2 == 0 ? 0 : 1) * 2) +
			(Trigram(nIndex: 1).Line(nIndex: 0).Value % 2 == 0 ? 0 : 1)) * 8) +
			((Trigram(nIndex: 0).Line(nIndex: 2).Value % 2 == 0 ? 0 : 1) * 4) +
			((Trigram(nIndex: 0).Line(nIndex: 1).Value % 2 == 0 ? 0 : 1) * 2) +
			(Trigram(nIndex: 0).Line(nIndex: 0).Value % 2 == 0 ? 0 : 1)
		UpdateInnerValues()
		UpdateOuterValues()
		if bMoving[1][1] {
			Trigram(nIndex: 1).Line(nIndex: 2).Old()
		}
		if bMoving[1][0] {
			Trigram(nIndex: 1).Line(nIndex: 0).Old()
			Trigram(nIndex: 1).Line(nIndex: 1).Old()
		}
		if bMoving[0][2] {
			Trigram(nIndex: 0).Line(nIndex: 2).Old()
			Trigram(nIndex: 0).Line(nIndex: 1).Old()
		}
		if bMoving[0][1] {
			Trigram(nIndex: 0).Line(nIndex: 0).Old()
		}
		return self
	}

	@discardableResult
	public override func Move() -> CValueSequencer {
		Trigram(nIndex: 1).Move()
		Trigram(nIndex: 0).Move()
		return self
	}

	@discardableResult
	public override func Young() -> CValueSequencer {
		Trigram(nIndex: 1).Young()
		Trigram(nIndex: 0).Young()
		return self
	}

	public override func UpdateInnerValues() {
		Trigram(nIndex: 1).Value = Value / 8
		Trigram(nIndex: 0).Value = Value % 8
		Trigram(nIndex: 1).UpdateInnerValues()
		Trigram(nIndex: 0).UpdateInnerValues()
	}

	public override func UpdateOuterValues() {
		Value = Trigram(nIndex: 0).Value + (Trigram(nIndex: 1).Value * 8)
	}

	private func SaveMoving(bMoving: inout [[Bool]]) {
		for t in 0 ... 1 {
			for l in 0 ... 2 {
				if (Trigram(nIndex: t).Line(nIndex: l).Value == 0) || (Trigram(nIndex: t).Line(nIndex: l).Value == 3) {
					bMoving[t][l] = true
				}
			}
		}
	}

	private func RestoreMoving(bMoving: inout [[Bool]], bInverseTrigram: Bool, bInverseLine: Bool) {
		for t in 0 ... 1 {
			for l in 0 ... 2 {
				if bMoving[t][l] {
					var l1 = l
					if bInverseLine {
						if (l1 == 0) {
							l1 = 2
						}
						else
							if l1 == 2 {
								l1 = 0
							}
					}
					Trigram(nIndex: bInverseTrigram ? 1 - t : t).Line(nIndex: l1).Old()
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
		return Sequences.strHexagramLabels[GetCurrentLabel()][Value]
	}

	public override func GetMoving() -> Bool {
		return Trigram(nIndex: 0).IsMoving || Trigram(nIndex: 1).IsMoving
	}

	public func HexagramId(bValueId: Bool = false) -> String {
		var s = String(format: "%2d", bValueId ?  Value : Sequence + 1)
		if IsMoving	{
			s = s + "."
			for l in 0 ... 5 {
				if Trigram(nIndex: l / 3).Line(nIndex: l % 3).IsMoving {
					s = s + String(l + 1)
				}
			}
		}
		return s;
	}

	public func DescribePrimary(bValueId: Bool = false, bIncludeValue: Bool = false) -> String {
		return HexagramId(bValueId: bValueId) + " " + Label + (bIncludeValue ? " (" + ValueStr + ")" : "")
	}

	public func DescibeSecondary(bValueId: Bool = false, bIncludeValue: Bool = false) -> String {
		if IsMoving	{
			let hvsPrimary: CHexagramValueSequencer = self
			let hvsSeconday: CHexagramValueSequencer = CHexagramValueSequencer(hvs: hvsPrimary)
			hvsSeconday.Move()
			return hvsSeconday.HexagramId(bValueId: bValueId) + " " + hvsSeconday.Label + (bIncludeValue ? " (" + hvsSeconday.ValueStr + ")" : "")
		}
		return ""
	}

	public func DescribeCast(bValueId: Bool = false, bIncludeValue: Bool = false) -> String	{
		return DescribePrimary(bValueId: bValueId, bIncludeValue: bIncludeValue) + (IsMoving ? " > " +
			DescibeSecondary(bValueId: bValueId, bIncludeValue: bIncludeValue) : "")
	}

	public override func GetCurrentSequence() -> Int { return CHexagramValueSequencer.m_nCurrentSequence }
	public override func GetCurrentRatio() -> Int { return CHexagramValueSequencer.m_nCurrentRatio }
	public override func GetCurrentLabel() -> Int { return CHexagramValueSequencer.m_nCurrentLabel }

	private static var m_nCurrentSequence: Int = 0
	private static var m_nCurrentRatio: Int = 0
	private static var m_nCurrentLabel: Int = 0
}

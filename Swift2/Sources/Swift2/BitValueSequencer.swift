
public class CBitValueSequencer : CValueSequencer {

	init(nValue: Int) {
		super.init(nInnerSequencers: 0, nValues: 2, nValue: nValue)
		m_nSequences = Sequences.nBitSequences;
		m_nRatios = Sequences.nBitRatios;
		Value = nValue
	}

	public override func UpdateOuterValues() {
		m_pvsParent.UpdateOuterValues()
	}
}


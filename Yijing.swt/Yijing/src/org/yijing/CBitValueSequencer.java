package org.yijing;

public class CBitValueSequencer extends CValueSequencer
{
	public CBitValueSequencer(int nValue) {
		super(0, 2, nValue);
		m_nSequences = Sequences.nBitSequences;
		m_nRatios = Sequences.nBitRatios;
	}
	@Override
	public void UpdateOuterValues() {
		m_pvsParent.UpdateOuterValues();
	}
}

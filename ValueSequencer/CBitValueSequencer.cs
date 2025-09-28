using System;

namespace ValueSequencer
{
	public class CBitValueSequencer : CValueSequencer
	{
		public CBitValueSequencer(int nValue) : base(0, 2, nValue)
		{
			m_nSequences = Sequences.nBitSequences;
			m_nRatios = Sequences.nBitRatios;
		}

		public override void UpdateOuterValues()
		{
			m_pvsParent.UpdateOuterValues();
		}
	}
}

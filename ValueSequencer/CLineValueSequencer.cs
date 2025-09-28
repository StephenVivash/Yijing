using System;

namespace ValueSequencer
{
	public class CLineValueSequencer : CValueSequencer
	{
		public CLineValueSequencer(int nValue) : base(3, 4, nValue)
		{
			m_pvsInner = new CBitValueSequencer[3];
			m_pvsInner[2] = new CBitValueSequencer(0);
			m_pvsInner[1] = new CBitValueSequencer(0);
			m_pvsInner[0] = new CBitValueSequencer(0);
			Bit(2).SetParent(this);
			Bit(1).SetParent(this);
			Bit(0).SetParent(this);
			m_nSequences = Sequences.nLineSequences;
			m_nRatios = Sequences.nLineRatios;
		}

		public CBitValueSequencer Bit(int nIndex)
		{
			return (CBitValueSequencer)m_pvsInner[nIndex];
		}

		public override CValueSequencer Inverse()
		{
			switch (m_nValue)
			{
				case 0:
					Value = 1;
					break;
				case 1:
					Value = 0;
					break;
				case 2:
					Value = 3;
					break;
				case 3:
					Value = 2;
					break;
			}
			UpdateInnerValues();
			UpdateOuterValues();
			return this;
		}

		public override CValueSequencer Move()
		{
			if (Value == 0)
				Value = 1;
			else
				if (Value == 3)
				Value = 2;
			UpdateInnerValues();
			UpdateOuterValues();
			return this;
		}

		public override CValueSequencer Young()
		{
			if (Value == 0)
				Value = 2;
			else
				if (Value == 3)
				Value = 1;
			UpdateInnerValues();
			UpdateOuterValues();
			return this;
		}

		public override CValueSequencer Old()
		{
			if (Value == 2)
				Value = 0;
			else
				if (Value == 1)
				Value = 3;
			UpdateInnerValues();
			UpdateOuterValues();
			return this;
		}

		public override void UpdateInnerValues()
		{
			switch (m_nValue)
			{
				case 0:
					Bit(2).Value = 0;
					Bit(1).Value = 0;
					Bit(0).Value = 0;
					break;
				case 1:
					Bit(2).Value = 0;
					Bit(1).Value = 1;
					Bit(0).Value = 0;
					break;
				case 2:
					Bit(2).Value = 1;
					Bit(1).Value = 0;
					Bit(0).Value = 1;
					break;
				case 3:
					Bit(2).Value = 1;
					Bit(1).Value = 1;
					Bit(0).Value = 1;
					break;
			}
			Bit(2).UpdateInnerValues();
			Bit(1).UpdateInnerValues();
			Bit(0).UpdateInnerValues();
		}

		public override void UpdateOuterValues()
		{
			Value = Bit(0).Value + Bit(1).Value + Bit(2).Value;
			m_pvsParent.UpdateOuterValues();
		}

		public static void SetCurrentSequence(int nSequence)
		{
			m_nCurrentSequence = nSequence;
		}

		public static void SetCurrentRatio(int nRatio)
		{
			m_nCurrentRatio = nRatio;
		}

		public static void SetCurrentLabel(int nLabel)
		{
			m_nCurrentLabel = nLabel;
		}

		protected override String GetLabel()
		{
			return Sequences.strLineLabels[GetCurrentLabel(), Value];
		}

		protected override bool GetMoving()
		{
			return (Value == 0) || (Value == 3);
		}

		protected override int GetCurrentSequence() { return m_nCurrentSequence; }
		protected override int GetCurrentRatio() { return m_nCurrentRatio; }
		protected override int GetCurrentLabel() { return m_nCurrentLabel; }

		protected static int m_nCurrentSequence = 0;
		protected static int m_nCurrentRatio = 0;
		protected static int m_nCurrentLabel = 0;
	}
}

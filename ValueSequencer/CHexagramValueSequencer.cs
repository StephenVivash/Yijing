using System;

namespace ValueSequencer
{
	public class CHexagramValueSequencer : CValueSequencer
	{
		public CHexagramValueSequencer(int nValue) : base(2, 64, nValue)
		{
			m_pvsInner = new CTrigramValueSequencer[2];
			m_pvsInner[1] = new CTrigramValueSequencer(-1);
			m_pvsInner[0] = new CTrigramValueSequencer(-1);
			Trigram(1).SetParent(this);
			Trigram(0).SetParent(this);
			m_nSequences = Sequences.nHexagramSequences;
			m_nRatios = Sequences.nHexagramRatios;
			Value = nValue;
			UpdateInnerValues();
			//UpdateOuterValues();
		}

		public CHexagramValueSequencer(ref CHexagramValueSequencer hvs) : this(0)
		{
			for (int t = 0; t < 2; ++t)
				for (int l = 0; l < 3; ++l)
				{
					var line = Trigram(t).Line(l);
					line.Value = hvs.Trigram(t).Line(l).Value;
					line.UpdateInnerValues();
					line.UpdateOuterValues();
				}
		//UpdateInnerValues();
		//UpdateOuterValues();
		}
			   
/*			   
		public CHexagramValueSequencer(ref CHexagramValueSequencer hvs) : this(0)
		{
			Trigram(1).Line(2).Value = hvs.Trigram(1).Line(2).Value;
			Trigram(1).Line(1).Value = hvs.Trigram(1).Line(1).Value;
			Trigram(1).Line(0).Value = hvs.Trigram(1).Line(0).Value;
			Trigram(0).Line(2).Value = hvs.Trigram(0).Line(2).Value;
			Trigram(0).Line(1).Value = hvs.Trigram(0).Line(1).Value;
			Trigram(0).Line(0).Value = hvs.Trigram(0).Line(0).Value;

			Trigram(1).Line(2).UpdateInnerValues();
			Trigram(1).Line(1).UpdateInnerValues();
			Trigram(1).Line(0).UpdateInnerValues();
			Trigram(0).Line(2).UpdateInnerValues();
			Trigram(0).Line(1).UpdateInnerValues();
			Trigram(0).Line(0).UpdateInnerValues();

			Trigram(1).Line(2).UpdateOuterValues();
			Trigram(1).Line(1).UpdateOuterValues();
			Trigram(1).Line(0).UpdateOuterValues();
			Trigram(0).Line(2).UpdateOuterValues();
			Trigram(0).Line(1).UpdateOuterValues();
			Trigram(0).Line(0).UpdateOuterValues();

			//UpdateInnerValues();
			//UpdateOuterValues();
		}
*/

		public CTrigramValueSequencer Trigram(int nIndex)
		{
			return (CTrigramValueSequencer)m_pvsInner[nIndex];
		}

		public override CValueSequencer Inverse()
		{
			bool[,] bMoving = { { false, false, false }, { false, false, false } };
			SaveMoving(ref bMoving);
			Value = ((Trigram(1).Line(2).Value % 2 == 0 ? 0 : 1) +
				((Trigram(1).Line(1).Value % 2 == 0 ? 0 : 1) * 2) +
				((Trigram(1).Line(0).Value % 2 == 0 ? 0 : 1) * 4)) +
				((((Trigram(0).Line(2).Value % 2 == 0 ? 0 : 1) +
				((Trigram(0).Line(1).Value % 2 == 0 ? 0 : 1) * 2) +
				((Trigram(0).Line(0).Value % 2 == 0 ? 0 : 1) * 4))) * 8);
			UpdateInnerValues();
			UpdateOuterValues();
			RestoreMoving(bMoving,true,true);
			return this;
		}

		public override CValueSequencer Opposite()
		{
			bool[,] bMoving = { { false, false, false }, { false, false, false } };
			SaveMoving(ref bMoving);
			base.Opposite();
			RestoreMoving(bMoving, false, false);
			return this;
		}

		public override CValueSequencer Transverse()
		{
			int nTemp;
			bool[,] bMoving = { { false, false, false }, { false, false, false } };
			SaveMoving(ref bMoving);
			nTemp = Trigram(0).Value;
			Trigram(0).Value = Trigram(1).Value;
			Trigram(1).Value = nTemp;
			Value = Trigram(0).Value + Trigram(1).Value * 8;
			UpdateInnerValues();
			UpdateOuterValues();
			RestoreMoving(bMoving, true, false);
			return this;
		}

		public override CValueSequencer Nuclear()
		{
			int nTemp;
			bool[,] bMoving = { { false, false, false }, { false, false, false } };
			SaveMoving(ref bMoving);
			nTemp = Trigram(1).Line(0).Value;
			Trigram(1).Line(2).Value = Trigram(1).Line(1).Value;
			Trigram(1).Line(1).Value = Trigram(1).Line(0).Value;
			Trigram(1).Line(0).Value = Trigram(0).Line(2).Value;
			Trigram(0).Line(0).Value = Trigram(0).Line(1).Value;
			Trigram(0).Line(1).Value = Trigram(0).Line(2).Value;
			Trigram(0).Line(2).Value = nTemp;
			Value = ((((Trigram(1).Line(2).Value % 2 == 0 ? 0 : 1) * 4) +
				((Trigram(1).Line(1).Value % 2 == 0 ? 0 : 1) * 2) +
				(Trigram(1).Line(0).Value % 2 == 0 ? 0 : 1)) * 8) +
				((Trigram(0).Line(2).Value % 2 == 0 ? 0 : 1) * 4) +
				((Trigram(0).Line(1).Value % 2 == 0 ? 0 : 1) * 2) +
				(Trigram(0).Line(0).Value % 2 == 0 ? 0 : 1);
			UpdateInnerValues();
			UpdateOuterValues();
			if (bMoving[1,1])
				Trigram(1).Line(2).Old();
			if (bMoving[1,0])
			{
				Trigram(1).Line(0).Old();
				Trigram(1).Line(1).Old();
			}
			if (bMoving[0,2])
			{
				Trigram(0).Line(2).Old();
				Trigram(0).Line(1).Old();
			}
			if (bMoving[0,1])
				Trigram(0).Line(0).Old();
			return this;
		}

		public override CValueSequencer Move()
		{
			Trigram(1).Move();
			Trigram(0).Move();
			return this;
		}

		public override CValueSequencer Young()
		{
			Trigram(1).Young();
			Trigram(0).Young();
			return this;
		}

		public override void UpdateInnerValues()
		{
			Trigram(1).Value = Value / 8;
			Trigram(0).Value = Value % 8;
			Trigram(1).UpdateInnerValues();
			Trigram(0).UpdateInnerValues();
		}

		public override void UpdateOuterValues()
		{
			Value = Trigram(0).Value + (Trigram(1).Value * 8);
		}

		private void SaveMoving(ref bool[,] bMoving)
		{
			for (int t = 0; t < 2; ++t)
				for (int l = 0; l < 3; ++l)
					if ((Trigram(t).Line(l).Value == 0) || (Trigram(t).Line(l).Value == 3))
						bMoving[t, l] = true;
		}

		private void RestoreMoving(bool[,] bMoving, bool bInverseTrigram, bool bInverseLine)
		{
			for (int t = 0; t < 2; ++t)
				for (int l = 0; l < 3; ++l)
					if (bMoving[t, l])
					{
						int l1 = l;
						if (bInverseLine)
							if (l1 == 0)
								l1 = 2;
							else
								if (l1 == 2)
									l1 = 0;
						Trigram(bInverseTrigram ? 1 - t : t).Line(l1).Old();
					}
		}

		public static void SetCurrentSequence(int nCurrentSequence)
		{
			m_nCurrentSequence = nCurrentSequence;
		}

		public static void SetCurrentRatio(int nRatio) {
			m_nCurrentRatio = nRatio;
		}
	
		public static void SetCurrentLabel(int nLabel) {
			m_nCurrentLabel = nLabel;
		}
	
		protected override String GetLabel()
		{ 
			return Sequences.strHexagramLabels[GetCurrentLabel(),Value];	
		}
	
		protected override bool GetMoving()
		{
			return Trigram(0).IsMoving || Trigram(1).IsMoving;
		}

		public String HexagramId(bool bValue = false)
		{
			String s = String.Format("{0,2}", bValue ?  Value : Sequence + 1);
			if (IsMoving)
			{
				s = s + ".";
				for (int l = 0; l < 6; ++l)
					if (Trigram(l / 3).Line(l % 3).IsMoving)
						s = s + (l + 1).ToString();
			}
			return s;
		}

		public String DescribePrimary(bool bValue = false)
		{
			return HexagramId() + " " + Label + (bValue ? " (" + ValueStr + ")" : "");
		}

		public String DescribeSecondary(bool bValue = false)
		{
			if (IsMoving)
			{
				CHexagramValueSequencer hvsPrimary = this;
				CHexagramValueSequencer hvsSeconday = new CHexagramValueSequencer(ref hvsPrimary);
				hvsSeconday.Move();
				return hvsSeconday.HexagramId() + " " + hvsSeconday.Label + (bValue ? " (" + hvsSeconday.ValueStr + ")" : "");
			}
			return "";
		}

		public String DescribeCast(bool bValue = false)
		{
			return DescribePrimary(bValue) + (IsMoving ? " > " + DescribeSecondary(bValue) : "");
		}

		protected override int GetCurrentSequence() { return m_nCurrentSequence; }
		protected override int GetCurrentRatio() { return m_nCurrentRatio; }
		protected override int GetCurrentLabel() { return m_nCurrentLabel; }

		protected static int m_nCurrentSequence = 0;
		protected static int m_nCurrentRatio = 0;
		protected static int m_nCurrentLabel = 0;
	}
}

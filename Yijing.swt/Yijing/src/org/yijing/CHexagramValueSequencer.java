package org.yijing;

public class CHexagramValueSequencer extends CValueSequencer
{
	public CHexagramValueSequencer(int nValue) {
		super(2, 64, nValue);
		m_pvsInner = new CTrigramValueSequencer[2];
		m_pvsInner[1] = new CTrigramValueSequencer(0);
		m_pvsInner[0] = new CTrigramValueSequencer(0);
		Trigram(1).SetParent(this);
		Trigram(0).SetParent(this);
		m_nSequences = Sequences.nHexagramSequences;
		m_nRatios = Sequences.nHexagramRatios;
	}

	public CHexagramValueSequencer(CHexagramValueSequencer hvs) {
		this(0);
		Trigram(1).Line(2).Value(hvs.Trigram(1).Line(2).Value());
		Trigram(1).Line(1).Value(hvs.Trigram(1).Line(1).Value());
		Trigram(1).Line(0).Value(hvs.Trigram(1).Line(0).Value());
		Trigram(0).Line(2).Value(hvs.Trigram(0).Line(2).Value());
		Trigram(0).Line(1).Value(hvs.Trigram(0).Line(1).Value());
		Trigram(0).Line(0).Value(hvs.Trigram(0).Line(0).Value());

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

	public CTrigramValueSequencer Trigram(int nIndex) {
		return (CTrigramValueSequencer)m_pvsInner[nIndex];
	}

	public CValueSequencer Inverse() {
		boolean[][] bMoving = { { false, false, false }, { false, false, false } };
		SaveMoving(bMoving);
		Value(((Trigram(1).Line(2).Value() % 2 == 0 ? 0 : 1) +
			((Trigram(1).Line(1).Value() % 2 == 0 ? 0 : 1) * 2) +
			((Trigram(1).Line(0).Value() % 2 == 0 ? 0 : 1) * 4)) +
			((((Trigram(0).Line(2).Value() % 2 == 0 ? 0 : 1) +
			((Trigram(0).Line(1).Value() % 2 == 0 ? 0 : 1) * 2) +
			((Trigram(0).Line(0).Value() % 2 == 0 ? 0 : 1) * 4))) * 8));
		UpdateInnerValues();
		UpdateOuterValues();
		RestoreMoving(bMoving,true,true);
		return this;
	}

	public CValueSequencer Opposite() {
		boolean[][] bMoving = { { false, false, false }, { false, false, false } };
		SaveMoving(bMoving);
		super.Opposite();
		RestoreMoving(bMoving, false, false);
		return this;
	}

	public CValueSequencer Transverse() {
		int nTemp;
		boolean[][] bMoving = { { false, false, false }, { false, false, false } };
		SaveMoving(bMoving);
		nTemp = Trigram(0).Value();
		Trigram(0).Value(Trigram(1).Value());
		Trigram(1).Value(nTemp);
		Value(Trigram(0).Value() + Trigram(1).Value() * 8);
		UpdateInnerValues();
		UpdateOuterValues();
		RestoreMoving(bMoving, true, false);
		return this;
	}

	public CValueSequencer Nuclear() {
		int nTemp;
		boolean[][] bMoving = { { false, false, false }, { false, false, false } };
		SaveMoving(bMoving);
		nTemp = Trigram(1).Line(0).Value();
		Trigram(1).Line(2).Value(Trigram(1).Line(1).Value());
		Trigram(1).Line(1).Value(Trigram(1).Line(0).Value());
		Trigram(1).Line(0).Value(Trigram(0).Line(2).Value());
		Trigram(0).Line(0).Value(Trigram(0).Line(1).Value());
		Trigram(0).Line(1).Value(Trigram(0).Line(2).Value());
		Trigram(0).Line(2).Value(nTemp);
		Value(((((Trigram(1).Line(2).Value() % 2 == 0 ? 0 : 1) * 4) +
			((Trigram(1).Line(1).Value() % 2 == 0 ? 0 : 1) * 2) +
			(Trigram(1).Line(0).Value() % 2 == 0 ? 0 : 1)) * 8) +
			((Trigram(0).Line(2).Value() % 2 == 0 ? 0 : 1) * 4) +
			((Trigram(0).Line(1).Value() % 2 == 0 ? 0 : 1) * 2) +
			(Trigram(0).Line(0).Value() % 2 == 0 ? 0 : 1));
		UpdateInnerValues();
		UpdateOuterValues();
		if (bMoving[1][1])
			Trigram(1).Line(2).Old();
		if (bMoving[1][0])
		{
			Trigram(1).Line(0).Old();
			Trigram(1).Line(1).Old();
		}
		if (bMoving[0][2])
		{
			Trigram(0).Line(2).Old();
			Trigram(0).Line(1).Old();
		}
		if (bMoving[0][1])
			Trigram(0).Line(0).Old();
		return this;
	}

	public CValueSequencer Move() {
		Trigram(1).Move();
		Trigram(0).Move();
		return this;
	}

	public CValueSequencer Young() {
		Trigram(1).Young();
		Trigram(0).Young();
		return this;
	}

	protected void UpdateInnerValues() {
		Trigram(1).Value(Value() / 8);
		Trigram(0).Value(Value() % 8);
		Trigram(1).UpdateInnerValues();
		Trigram(0).UpdateInnerValues();
	}

	protected void UpdateOuterValues() {
		Value(Trigram(0).Value() + (Trigram(1).Value() * 8));
	}

	private void SaveMoving(boolean[][] bMoving) {
		for (int t = 0; t < 2; ++t)
			for (int l = 0; l < 3; ++l)
				if ((Trigram(t).Line(l).Value() == 0) || (Trigram(t).Line(l).Value() == 3))
					bMoving[t][l] = true;
	}

	private void RestoreMoving(boolean[][] bMoving, boolean bInverseTrigram, boolean bInverseLine) {
		for (int t = 0; t < 2; ++t)
			for (int l = 0; l < 3; ++l)
				if (bMoving[t][l])
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

	public static void SetCurrentSequence(int nSequence) {
		m_nCurrentSequence = nSequence;
	}

	public static void SetCurrentRatio(int nRatio) {
		m_nCurrentRatio = nRatio;
	}
	
	public static void SetCurrentLabel(int nLabel) {
		m_nCurrentLabel = nLabel;
	}
	
	protected String GetLabel() { 
		return Sequences.strHexagramLabels[GetCurrentLabel()][Value()];
				
	}
	
	protected boolean GetMoving() {
		return Trigram(0).IsMoving() || Trigram(1).IsMoving();
	}

	protected int GetCurrentSequence() { return m_nCurrentSequence; }
	protected int GetCurrentRatio() { return m_nCurrentRatio; }
	protected int GetCurrentLabel() { return m_nCurrentLabel; }

	protected static int m_nCurrentSequence = 0;
	protected static int m_nCurrentRatio = 0;
	protected static int m_nCurrentLabel = 0;
}

package org.yijing;

public class CTrigramValueSequencer extends CValueSequencer
{
	public CTrigramValueSequencer(int nValue) {
		super(3, 8, nValue);
		m_pvsInner = new CLineValueSequencer[3];
		m_pvsInner[2] = new CLineValueSequencer(0);
		m_pvsInner[1] = new CLineValueSequencer(0);
		m_pvsInner[0] = new CLineValueSequencer(0);
		Line(2).SetParent(this);
		Line(1).SetParent(this);
		Line(0).SetParent(this);
		m_nSequences = Sequences.nTrigramSequences;
		m_nRatios = Sequences.nTrigramRatios;
	}

	public CLineValueSequencer Line(int nIndex) {
		return (CLineValueSequencer)m_pvsInner[nIndex];
	}

	public CValueSequencer Inverse() {
		boolean[] bMoving = { false, false, false };
		SaveMoving(bMoving);
		Value((Line(2).Value() % 2 == 0 ? 0 : 1) +
			((Line(1).Value() % 2 == 0 ? 0 : 1) * 2) +
			((Line(0).Value() % 2 == 0 ? 0 : 1) * 4));
		UpdateInnerValues();
		UpdateOuterValues();
		RestoreMoving(bMoving, true);
		return this;
	}

	public CValueSequencer Opposite() {
		boolean[] bMoving = { false, false, false };
		SaveMoving(bMoving);
		super.Opposite();
		RestoreMoving(bMoving, false);
		return this;
	}

	public CValueSequencer Move() {
		Line(2).Move();
		Line(1).Move();
		Line(0).Move();
		return this;
	}

	public CValueSequencer Young() {
		Line(2).Young();
		Line(1).Young();
		Line(0).Young();
		return this;
	}

	protected void UpdateInnerValues() {
		switch (m_nValue)
		{
			case 0:
				Line(2).Value(m_nYinLine);
				Line(1).Value(m_nYinLine);
				Line(0).Value(m_nYinLine);
				break;
			case 1:
				Line(2).Value(m_nYinLine);
				Line(1).Value(m_nYinLine);
				Line(0).Value(m_nYangLine);
				break;
			case 2:
				Line(2).Value(m_nYinLine);
				Line(1).Value(m_nYangLine);
				Line(0).Value(m_nYinLine);
				break;
			case 3:
				Line(2).Value(m_nYinLine);
				Line(1).Value(m_nYangLine);
				Line(0).Value(m_nYangLine);
				break;
			case 4:
				Line(2).Value(m_nYangLine);
				Line(1).Value(m_nYinLine);
				Line(0).Value(m_nYinLine);
				break;
			case 5:
				Line(2).Value(m_nYangLine);
				Line(1).Value(m_nYinLine);
				Line(0).Value(m_nYangLine);
				break;
			case 6:
				Line(2).Value(m_nYangLine);
				Line(1).Value(m_nYangLine);
				Line(0).Value(m_nYinLine);
				break;
			case 7:
				Line(2).Value(m_nYangLine);
				Line(1).Value(m_nYangLine);
				Line(0).Value(m_nYangLine);
				break;
		}
		Line(2).UpdateInnerValues();
		Line(1).UpdateInnerValues();
		Line(0).UpdateInnerValues();
	}

	protected void UpdateOuterValues() {
		Value((Line(0).Value() % 2 == 0 ? 0 : 1) +
			((Line(1).Value() % 2 == 0 ? 0 : 1) * 2) +
			((Line(2).Value() % 2 == 0 ? 0 : 1) * 4));
		if(m_pvsParent != null)
			m_pvsParent.UpdateOuterValues();
	}

	private void SaveMoving(boolean[] bMoving) {
		for (int l = 0; l < 3; ++l)
			if ((Line(l).Value() == 0) || (Line(l).Value() == 3))
				bMoving[l] = true;
	}

	private void RestoreMoving(boolean[] bMoving, boolean bInverseLine) {
		for (int l = 0; l < 3; ++l)
			if (bMoving[l])
			{
				int l1 = l;
				if (bInverseLine)
					if (l1 == 0)
						l1 = 2;
					else
						if (l1 == 2)
							l1 = 0;
				Line(l1).Old();
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
		return Sequences.strTrigramLabels[GetCurrentLabel()][Value()];
	}
	
	protected boolean GetMoving() {
		return Line(0).IsMoving() || Line(1).IsMoving() || Line(2).IsMoving();
	}

	protected int GetCurrentSequence() { return m_nCurrentSequence; }
	protected int GetCurrentRatio() { return m_nCurrentRatio; }
	protected int GetCurrentLabel() { return m_nCurrentLabel; }

	protected static int m_nCurrentSequence = 0;
	protected static int m_nCurrentRatio = 0;
	protected static int m_nCurrentLabel = 0;

	protected int m_nYinLine = 2;
	protected int m_nYangLine = 1;
}

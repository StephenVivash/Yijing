package org.yijing;

public class CValueSequencer {
	
	public CValueSequencer(int nInnerSequencers, int nValues, int nValue) {
		m_nInnerSequencers = nInnerSequencers;
		m_nValues = nValues;
		m_pvsParent = null;
		m_pvsInner = null;
		//SetValue(nValue);
	}
	
	//~CValueSequencer() { }
	
	public int Value() {
		return m_nValue;
	}

	public void Value(int nValue) {
		SetValue(nValue);
	}

	public int Sequence() {
		return m_nSequence;
	}
	
	public void Sequence(int nSequence) {
		SetValue(FindValue(nSequence));
	}

	
	public String ValueStr()
	{
		return Integer.toString(Sequences.nHexagramSequences[0][Value()]);
	}

	public String SequenceStr()
	{
		return Integer.toString(Sequence() + 1);
	}
	
	public String Label() {
		return GetLabel();
	}

	public boolean IsMoving() {
		return GetMoving();
	}
	
	public CValueSequencer First() {
		SetValue(GetFirstSequence());
		UpdateInnerValues();
		UpdateOuterValues();
		return this;
	}

	public CValueSequencer Previous(boolean bRatio) {
		if (!bRatio || (--m_nRatio == 0))
		{
			SetValue(GetPreviousSequence(m_nSequence));
			UpdateInnerValues();
			UpdateOuterValues();
		}
		return this;
	}

	public CValueSequencer Next(boolean bRatio) {
		if (!bRatio || (--m_nRatio == 0))
		{
			SetValue(GetNextSequence(m_nSequence));
			UpdateInnerValues();
			UpdateOuterValues();
		}
		return this;
	}

	public CValueSequencer Last() {
		SetValue(GetLastSequence());
		UpdateInnerValues();
		UpdateOuterValues();
		return this;
	}

	public CValueSequencer Inverse() { return this; }

	public CValueSequencer Opposite() {
		SetValue(~m_nValue & (m_nValues - 1));
		UpdateInnerValues();
		UpdateOuterValues();
		return this;
	}

	public CValueSequencer Transverse() { return this; }
	public CValueSequencer Nuclear() { return this; }
	public CValueSequencer Move() { return this; }
	public CValueSequencer Young() { return this; }
	public CValueSequencer Old() { return this; }

	public CValueSequencer Update() {
		SetValue(m_nValue);
		UpdateInnerValues();
		UpdateOuterValues();
		return this;
	}

	protected void UpdateInnerValues() { }
	protected void UpdateOuterValues() { }

	protected void SetParent(CValueSequencer pvsParent) { m_pvsParent = pvsParent; }
	protected CValueSequencer GetParent() { return m_pvsParent; }

	protected CValueSequencer GetChild(int nIndex) {
		return m_pvsInner[nIndex];
	}

	private CValueSequencer SetValue(int nValue) {
		if((nValue >= 0) && (nValue <= m_nValues))
		{
			m_nValue = nValue;
			m_nSequence = m_nSequences[GetCurrentSequence()][nValue];
			m_nRatio = m_nRatios[GetCurrentRatio()][nValue];
		}
		return this;
	}

	protected int FindValue(int nSequence) {
		for (int i = 0; i < m_nValues; ++i)
			if (m_nSequences[GetCurrentSequence()][i] == nSequence)
				return i;
		return -1;
	}

	protected int GetFirstSequence() {
		return FindValue(0);
	}

	protected int GetPreviousSequence(int nSequence) {
		if (nSequence > 0)
			return FindValue(--nSequence);
		return GetLastSequence();
	}

	protected int GetNextSequence(int nSequence) {
		if (nSequence < m_nValues - 1)
			if ((nSequence = FindValue(++nSequence)) != -1)
				return nSequence;
		return GetFirstSequence();
	}

	protected int GetLastSequence() {
		int nSequence1,nSequence2 = m_nValues;
		while(nSequence2 > -1)
			if ((nSequence1 = FindValue(--nSequence2)) != -1)
				return nSequence1;
		return -1;
	}

	protected String GetLabel() { return ""; }
	protected boolean GetMoving() { return false; }

	protected int GetCurrentSequence() { return 0; }
	protected int GetCurrentRatio() { return 0; }
	protected int GetCurrentLabel() { return 0; }
	
	protected int m_nValue;
	protected int m_nSequence;
	protected int m_nRatio;

	protected int m_nInnerSequencers;
	protected int m_nValues;

	protected int[][] m_nSequences;
	protected int[][] m_nRatios;

	protected CValueSequencer m_pvsParent;
	protected CValueSequencer[] m_pvsInner;
}

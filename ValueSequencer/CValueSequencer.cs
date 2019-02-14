using System;

namespace ValueSequencer
{
	public class CValueSequencer
	{
		public CValueSequencer(int nInnerSequencers, int nValues, int nValue)
		{
			m_nInnerSequencers = nInnerSequencers;
			m_nValues = nValues;
			m_pvsParent = null;
			m_pvsInner = null;
			//SetValue(nValue);
		}

		//~CValueSequencer() { }

		public int Value
		{
			get { return m_nValue; }
			set { SetValue(value); }
		}

		public int Sequence
		{
			get { return m_nSequence; }
			set { SetValue(FindValue(value)); }
		}

		public String ValueStr
		{
			get { return Sequences.nHexagramSequences[0, Value].ToString();	}
		}

		public String SequenceStr
		{
			get	{ return (Sequence + 1).ToString(); }
		}

		public String Label
		{
			get { return GetLabel(); }
		}

		public bool IsMoving
		{
			get { return GetMoving(); }
		}

		public virtual CValueSequencer First()
		{
			SetValue(GetFirstSequence());
			UpdateInnerValues();
			UpdateOuterValues();
			return this;
		}

		public virtual CValueSequencer Previous(bool bRatio = false)
		{
			if (!bRatio || (--m_nRatio == 0))
			{
				SetValue(GetPreviousSequence(m_nSequence));
				UpdateInnerValues();
				UpdateOuterValues();
			}
			return this;
		}

		public virtual CValueSequencer Next(bool bRatio = false)
		{
			if (!bRatio || (--m_nRatio == 0))
			{
				SetValue(GetNextSequence(m_nSequence));
				UpdateInnerValues();
				UpdateOuterValues();
			}
			return this;
		}

		public virtual CValueSequencer Last()
		{
			SetValue(GetLastSequence());
			UpdateInnerValues();
			UpdateOuterValues();
			return this;
		}

		public virtual CValueSequencer Inverse() { return this; }

		public virtual CValueSequencer Opposite()
		{
			SetValue(~m_nValue & (m_nValues - 1));
			UpdateInnerValues();
			UpdateOuterValues();
			return this;
		}

		public virtual CValueSequencer Transverse() { return this; }
		public virtual CValueSequencer Nuclear() { return this; }
		public virtual CValueSequencer Move() { return this; }
		public virtual CValueSequencer Young() { return this; }
		public virtual CValueSequencer Old() { return this; }

		public virtual CValueSequencer Update()
		{
			SetValue(m_nValue);
			UpdateInnerValues();
			UpdateOuterValues();
			return this;
		}

		public virtual void UpdateInnerValues() { }
		public virtual void UpdateOuterValues() { }

		public void SetParent(CValueSequencer pvsParent) { m_pvsParent = pvsParent; }
		protected CValueSequencer GetParent() { return m_pvsParent; }

		protected CValueSequencer GetChild(int nIndex)
		{
			return m_pvsInner[nIndex];
		}

		private CValueSequencer SetValue(int nValue)
		{
			if((nValue >= 0) && (nValue <= m_nValues))
			{
				m_nValue = nValue;
				m_nSequence = m_nSequences[GetCurrentSequence(), nValue];
				m_nRatio = m_nRatios[GetCurrentRatio(), nValue];
			}
			return this;
		}

		protected int FindValue(int nSequence)
		{
			for (int i = 0; i < m_nValues; ++i)
				if (m_nSequences[GetCurrentSequence(), i] == nSequence)
					return i;
			return -1;
		}

		protected int GetFirstSequence()
		{
			return FindValue(0);
		}

		protected int GetPreviousSequence(int nSequence)
		{
			if (nSequence > 0)
				return FindValue(--nSequence);
			return GetLastSequence();
		}

		protected int GetNextSequence(int nSequence)
		{
			if (nSequence < m_nValues - 1)
				if ((nSequence = FindValue(++nSequence)) != -1)
					return nSequence;
			return GetFirstSequence();
		}

		protected int GetLastSequence()
		{
			int nSequence1,nSequence2 = m_nValues;
			while(nSequence2 > -1)
				if ((nSequence1 = FindValue(--nSequence2)) != -1)
					return nSequence1;
			return -1;
		}

		protected virtual String GetLabel() { return ""; }
		protected virtual bool GetMoving() { return false; }

		protected virtual int GetCurrentSequence() { return 0; }
		protected virtual int GetCurrentRatio() { return 0; }
		protected virtual int GetCurrentLabel() { return 0; }

		protected int m_nValue;
		protected int m_nSequence;
		protected int m_nRatio;

		protected int m_nInnerSequencers;
		protected int m_nValues;

		protected int[,] m_nSequences;
		protected int[,] m_nRatios;
		protected CValueSequencer m_pvsParent;
		protected CValueSequencer[] m_pvsInner;
	}
}

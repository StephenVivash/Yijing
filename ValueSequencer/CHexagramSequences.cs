using System;
using System.Collections.Generic;
using System.Text;

namespace ValueSequencer
{
	public class CHexagramSequences
	{

		public class CHexagramCounter
		{
			public CHexagramCounter(ref CHexagramValueSequencer hvsPrimary)
			{
				m_hvsPrimary = new CHexagramValueSequencer(ref hvsPrimary);
			}

			public void Add() => ++Count;

			public string DescribeCast
			{
				get { return CHexagramSequences.DescribeCast(ref m_hvsPrimary); }
			}

			public String Primary
			{
				get { return HexagramId(ref m_hvsPrimary) + " " + m_hvsPrimary.Label; }
			}

			public String Secondary
			{
				get
				{
					String s = "";
					if (m_hvsPrimary.IsMoving)
					{
						CHexagramValueSequencer hvsSeconday = new CHexagramValueSequencer(ref m_hvsPrimary);
						hvsSeconday.Move();
						s = HexagramId(ref hvsSeconday) + " " + hvsSeconday.Label;
					}
					return s;
				}
			}
/*
			public override bool Equals(object obj)
			{
				var counter = obj as CHexagramCounter;
				return counter != null &&
					   EqualityComparer<CHexagramValueSequencer>.Default.Equals(m_hvsPrimary, counter.m_hvsPrimary);
			}

			public override int GetHashCode()
			{
				return -958826402 + EqualityComparer<CHexagramValueSequencer>.Default.GetHashCode(m_hvsPrimary);
			}
*/
			public int Count { get; private set; } = 1;

			private CHexagramValueSequencer m_hvsPrimary;
		}

		public class CHexagramCounterArray
		{
			public void Add(ref CHexagramValueSequencer hvsPrimary)
			{
				m_hCounterArray[++m_nCount] = new CHexagramCounter(ref hvsPrimary);
			}

			public CHexagramCounter[] HexagramCounterArray() => m_hCounterArray;

			public CHexagramCounter this[int i] => m_hCounterArray[i];

			private int m_nCount = -1;

			private CHexagramCounter[] m_hCounterArray = new CHexagramCounter[4096];
		}

		static public CHexagramCounterArray FullCast()
		{
			CHexagramCounterArray hca = new CHexagramCounterArray();
			CHexagramValueSequencer hvsPrimary = new CHexagramValueSequencer(0);
			hvsPrimary.First();
			for (int p = 0; p < 64; ++p)
			{
				for (int s = 0; s < 64; ++s)
				{
					CHexagramValueSequencer hvs = new CHexagramValueSequencer(ref hvsPrimary);
					for (int l = 0; l < 6; ++l)
						if (((s & (1 << l)) >> l) == 1)
							hvs.Trigram(l / 3).Line(l % 3).Next(false);
					hca.Add(ref hvs);
				}
				hvsPrimary.Next();
			}
			return hca;
		}

		static public String DescribeCast(ref CHexagramValueSequencer hvsPrimary)
		{
			String s = HexagramId(ref hvsPrimary) + " " + hvsPrimary.Label;
			if (hvsPrimary.IsMoving)
			{
				CHexagramValueSequencer hvsSeconday = new CHexagramValueSequencer(ref hvsPrimary);
				hvsSeconday.Move();
				s = s + " > " + HexagramId(ref hvsSeconday) + " " + hvsSeconday.Label;
			}
			return s;
		}

		static public String HexagramId(ref CHexagramValueSequencer hvs)
		{
			String s = String.Format("{0,2}", hvs.Sequence + 1);
			if (hvs.IsMoving)
			{
				s = s + ".";
				for (int l = 0; l < 6; ++l)
					if (hvs.Trigram(l / 3).Line(l % 3).IsMoving)
						s = s + (l + 1).ToString();
			}
			return s;
		}
	}
}

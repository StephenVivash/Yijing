using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ValueSequencer
{
	public class CHexagramSequences
	{
		public class CHexagramCounter : IComparable
		{
			public CHexagramCounter(ref CHexagramValueSequencer hvsPrimary)
			{
				m_hvsPrimary = new CHexagramValueSequencer(ref hvsPrimary);
			}

			public string DescribeCast
			{
				get { return m_hvsPrimary.DescribeCast(); }
			}

			public int CompareTo(object obj)
			{
				if (obj.GetType() == typeof(CHexagramValueSequencer))
				{
					CHexagramValueSequencer hvs = (CHexagramValueSequencer)obj;
					return m_hvsPrimary.HexagramId().CompareTo(hvs.HexagramId());
				}
				CHexagramCounter hc = (CHexagramCounter)obj;
				return m_hvsPrimary.HexagramId().CompareTo(hc.m_hvsPrimary.HexagramId());
			}

			public void Add() => ++Count;
			public int Count { get; private set; } = 0;

			private CHexagramValueSequencer m_hvsPrimary;
		}

		public class CHexagramCounterArray //: IEnumerable
		{
			public CHexagramCounterArray()
			{
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
						Add(ref hvs);
					}
					hvsPrimary.Next();
				}
				Array.Sort(m_hCounterArray);
			}

			public void Add(ref CHexagramValueSequencer hvsPrimary)
			{
				m_hCounterArray.SetValue(new CHexagramCounter(ref hvsPrimary),++m_nCount);
			}

			public CHexagramCounterArray MultiCast(int nCount)
			{
				CHexagramValueSequencer hvs = new CHexagramValueSequencer(63); // more even ????????
				for (int i = 0; i < nCount; ++i)
				{
					//CHexagramValueSequencer hvs = new CHexagramValueSequencer(63);
					AutoCast(ref hvs);
					int nIndex = Array.BinarySearch(m_hCounterArray, hvs);
					if(nIndex >= 0)
						((CHexagramCounter) m_hCounterArray.GetValue(nIndex)).Add();
				}
				return this;
			}

			public CHexagramValueSequencer AutoCast(ref CHexagramValueSequencer hvs) //////// Random ?????????????????????????????
			{
				Random r = new Random(DateTime.Now.Millisecond);
				for (int l = 0; l < 6; ++l)
				{
					int count = (r.Next(5) + 1) * 1000 + r.Next(1000);
					for (int c = 0; c < count; ++c)
						hvs.Trigram(l / 3).Line(l % 3).Next(true);
				}
				return hvs;
			}

			public IEnumerator GetEnumerator()
			{
				throw new NotImplementedException();
			}

			public Array HexagramCounterArray() => m_hCounterArray;
			public CHexagramCounter this[int i] => (CHexagramCounter) m_hCounterArray.GetValue(i);

			private Array m_hCounterArray = Array.CreateInstance(typeof(CHexagramCounter), 4096);
			private int m_nCount = -1;
		}
	}
}

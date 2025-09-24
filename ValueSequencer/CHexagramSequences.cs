using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ValueSequencer
{
        public class CHexagramSequences
        {
                public CHexagramValueSequencer AutoCast(ref CHexagramValueSequencer hvs)
                {
                        Random r = Sequences.m_ranSession ?? new Random(DateTime.Now.Millisecond);
                        for (int l = 0; l < 6; ++l)
                        {
                                int count = (r.Next(5) + 1) * 100 + r.Next(100);
                                for (int c = 0; c < count; ++c)
                                        hvs.Trigram(l / 3).Line(l % 3).Next(true);
                        }
                        return hvs;
                }
        }
	public class CHexagram : IComparable
	{
		public CHexagram(ref CHexagramValueSequencer hvsPrimary)
		{
			m_hvsPrimary = new CHexagramValueSequencer(ref hvsPrimary);
		}

		public string DescribeCast
		{
			get { return m_hvsPrimary.DescribeCast(); }
		}

		public int CompareTo(object obj)
		{
			CHexagram h = (CHexagram)obj;
			return m_hvsPrimary.HexagramId().CompareTo(h.m_hvsPrimary.HexagramId());
		}

		public void Add() => ++Count;
		public int Count { get; private set; } = 0;

		private CHexagramValueSequencer m_hvsPrimary;
	}

	public class CHexagramArray //: IEnumerable
	{
		public CHexagramArray()
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
			Array.Sort<CHexagram>(m_arrHexagram);
		}

		public void Add(ref CHexagramValueSequencer hvsPrimary)
		{
			m_arrHexagram[++m_nCount] = new CHexagram(ref hvsPrimary);
		}

		public CHexagramArray MultiCast(int nCount)
		{
			CHexagramValueSequencer hvs = new CHexagramValueSequencer(63); // more even ????????
			for (int i = 0; i < nCount; ++i)
			{
				//CHexagramValueSequencer hvs = new CHexagramValueSequencer(63);
				AutoCast(ref hvs);
				CHexagram h = new CHexagram(ref hvs);
				int nIndex = Array.BinarySearch<CHexagram>(m_arrHexagram, h);
				if(nIndex >= 0)
				m_arrHexagram[nIndex].Add();
			}
			return this;
		}

		public CHexagramValueSequencer AutoCast(ref CHexagramValueSequencer hvs) //////// Random ?????????????????????????????
		{
			Random r = true ? Sequences.m_ranSession : new Random(DateTime.Now.Millisecond);
			for (int l = 0; l < 6; ++l)
			{
				int count = (r.Next(5) + 1) * 100 + r.Next(100);
				for (int c = 0; c < count; ++c)
					hvs.Trigram(l / 3).Line(l % 3).Next(true);
			}
			return hvs;
		}

		public IEnumerator GetEnumerator()
		{
			throw new NotImplementedException();
		}

		public CHexagram[] HexagramArray() => m_arrHexagram;
		public CHexagram this[int i] => m_arrHexagram[i];

		private CHexagram[] m_arrHexagram = new CHexagram[4096];
		private int m_nCount = -1;
	}
}

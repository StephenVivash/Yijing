using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

//using HexagramSequences;
using ValueSequencer;
using YijingDb;

namespace Yijing
{

	class Program
	{
		public static CHexagramValueSequencer m_hvsPrimary = null;
		public static CValueSequencer m_vsCurrent;

		static void Main(string[] args)
		{

			YijingData yd = new YijingData();
			yd.InitialseDB();

			using (YijingEntities ye = new YijingEntities())
			{
				YijingDb.Type t = ye.Types.Find(3);
				LabelSery ls2 = ye.LabelSeries.Where(ls1 => ls1.Name == "Vivash" &&
					ls1.TypeId == (int)Sequences.ValueType.Hexagram).First(); // .OrderBy(ls1 => ls1.Name)
				IEnumerable<LabelSery> iels = from ls3 in ye.LabelSeries where ls3.TypeId == (int)Sequences.ValueType.Line
					orderby ls3.Name select ls3;
				List<LabelSery> lls = iels.ToList();

				LabelSery ls4 = ye.LabelSeries.Include(ls => ls.Type).Include(ls => ls.Labels).Single(ls5 => ls5.Name == "Vivash");
				Label l1 = ls2.Labels.Single(l => l.LabelData == "Return");
				t = l1.Type;

				int x = 0;
			}
			/*
			HexagramSequences1.initValueSequencer(0);

			m_hvsPrimary = HexagramSequences1.newHexagramById(30.1234);
			string s1 = HexagramSequences1.hexagramId(m_hvsPrimary);

			m_vsCurrent = HexagramSequences1.newHexagram(63);
			string s2 = HexagramSequences1.hexagramId((CHexagramValueSequencer) m_vsCurrent);
			m_vsCurrent.Value = 23;
			m_vsCurrent.Update();

			CHexagramSequences.CHexagramCounterArray hca = CHexagramSequences.FullCast();
			foreach (CHexagramSequences.CHexagramCounter hc in hca.HexagramCounterArray())
				Console.WriteLine("{0}", hc.DescribeCast);

			string[] array2 = HexagramSequences1.multiCast(4096);
			for (int i = 0; i < array2.Length; ++i)
				Console.WriteLine("{0}", array2[i]);
			*/
		}

	}
}

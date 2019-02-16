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
		static void Main(string[] args)
		{

			YijingData yd = new YijingData();
			//yd.InitialseDB();
			
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
			}

			CLineValueSequencer.SetCurrentRatio(0); // 0 - 5 "Equal", "Coin", "Yarrow", "Marbles", "Yin", "Yang"

			CHexagramSequences.CHexagramCounterArray hca = new CHexagramSequences.CHexagramCounterArray();
			hca.MultiCast(10000);
			foreach (CHexagramSequences.CHexagramCounter hc in hca.HexagramCounterArray())
				if (hc.Count > 0)
					Console.WriteLine($"{hc.Count,4:D} {hc.DescribeCast}");

			int x = 0;
		}
	}
}

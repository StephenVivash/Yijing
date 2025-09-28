
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using ValueSequencer;

//PM> enable-migrations
//PM> add-migration CreateDB
//PM> update-database
//PM> remove-migration

namespace YijingDb
{

	public class YijingEntities : DbContext
	{
		public static String _ds;

		public virtual DbSet<Label> Labels { get; set; }
		public virtual DbSet<LabelSery> LabelSeries { get; set; }
		public virtual DbSet<Ratio> Ratios { get; set; }
		public virtual DbSet<RatioSery> RatioSeries { get; set; }
		public virtual DbSet<Sequence> Sequences { get; set; }
		public virtual DbSet<SequenceSery> SequenceSeries { get; set; }
		public virtual DbSet<Text> Texts { get; set; }
		public virtual DbSet<TextSery> TextSeries { get; set; }
		public virtual DbSet<TextType> TextTypes { get; set; }
		public virtual DbSet<Type> Types { get; set; }
		public virtual DbSet<Value> Values { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder
				//"Data Source=WALLABY\SQLEXPRESS;Initial Catalog=Yijing;Integrated Security=True;Pooling=False"
				//"Data Source=WALLABY\SQLEXPRESS;Initial Catalog=Yijing;Trusted_Connection=True;Pooling=False"
				//"Data Source=WALLABY\SQLEXPRESS;Initial Catalog=Yijing;Persist Security Info=True;User ID=sa;Password="
				//.UseSqlServer(@"Data Source=WALLABY\SQLEXPRESS;Initial Catalog=Yijing;Integrated Security=True;Pooling=False")

				.UseSqlite(_ds)
				//.UseSqlite("Data Source=C:/Src/Yijing/YijingDb/Yijing.db")					// Windows
				//.UseSqlite("Data Source=/home/stephen/Yijing.db")							// Ubuntu VM
				//.UseSqlite("Data Source=/Users/stephenvivash/Yijing.db")					// MacOS 
				//.UseSqlite("Data Source=/mnt/c/Users/Stephen Vivash/src/Yijing.db")		// WSL
				//.UseSqlite("Data Source=./Yijing.db")										// Android

				//.UseLazyLoadingProxies()
				.EnableSensitiveDataLogging(true)
				.EnableDetailedErrors(true);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			/*
			modelBuilder.Entity<Value>()
				.HasAlternateKey(v => v.ValueId)
				.ForSqlServerIsClustered(false);
				
			modelBuilder.Entity<Value>()
				.HasIndex(v => v.ValueId)
				.HasName("AK_Values_ValueId")
				.IsUnique(false);
			*/

			modelBuilder.Entity<Value>()
				.HasKey(v => new { v.TypeId, v.ValueId });

			modelBuilder.Entity<Sequence>()
				.HasOne(s => s.Type)
				.WithMany(t => t.Sequences)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Ratio>()
				.HasOne(r => r.Type)
				.WithMany(t => t.Ratios)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Label>()
				.HasOne(l => l.Type)
				.WithMany(t => t.Labels)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Text>()
				.HasOne(t => t.Type)
				.WithMany(t => t.Texts)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Sequence>()
				.HasOne(s => s.Value)
				.WithMany(v => v.Sequences)
				.HasForeignKey(s => new { s.TypeId, s.ValueId })
				.HasPrincipalKey(v => new { v.TypeId, v.ValueId })
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Ratio>()
				.HasOne(r => r.Value)
				.WithMany(v => v.Ratios)
				.HasForeignKey(r => new { r.TypeId, r.ValueId })
				.HasPrincipalKey(v => new { v.TypeId, v.ValueId })
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Label>()
				.HasOne(l => l.Value)
				.WithMany(v => v.Labels)
				.HasForeignKey(l => new { l.TypeId, l.ValueId })
				.HasPrincipalKey(v => new { v.TypeId, v.ValueId })
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Text>()
				.HasOne(t => t.Value)
				.WithMany(v => v.Texts)
				.HasForeignKey(t => new { t.TypeId, t.ValueId })
				.HasPrincipalKey(v => new { v.TypeId, v.ValueId })
				.OnDelete(DeleteBehavior.Restrict);
		}
	}

	public class YijingData
	{

		public YijingData(String file)
		{
			YijingEntities._ds = $"Data Source={file}";
			Sequences.Initialise();
		}

		public void InitialseDB()
		{
			using (YijingEntities ye = new YijingEntities())
			{
				//List<Type> lt1 = ye.Types.ToList(); // Force access

				ye.Database.EnsureDeleted();
				//ye.Database.EnsureCreated();
				ye.Database.Migrate();

				//List<Type> lt2 = ye.Types.ToList(); // Force access
			}
			using (YijingEntities ye = new YijingEntities())
			{
				IQueryable<Sequence> iqs = ye.Sequences;
				IQueryable<Ratio> iqr = ye.Ratios;
				IQueryable<Label> iql = ye.Labels;
				IQueryable<Text> iqtx = ye.Texts;

				IQueryable<SequenceSery> iqss = ye.SequenceSeries;
				IQueryable<RatioSery> iqrs = ye.RatioSeries;
				IQueryable<LabelSery> iqls = ye.LabelSeries;
				IQueryable<TextSery> iqts = ye.TextSeries;

				IQueryable<TextType> iqtt = ye.TextTypes;
				IQueryable<Value> iqv = ye.Values;
				IQueryable<Type> iqt = ye.Types;

				ye.Sequences.RemoveRange(iqs);
				ye.Ratios.RemoveRange(iqr);
				ye.Labels.RemoveRange(iql);
				ye.Texts.RemoveRange(iqtx);

				ye.SequenceSeries.RemoveRange(iqss);
				ye.RatioSeries.RemoveRange(iqrs);
				ye.LabelSeries.RemoveRange(iqls);
				ye.TextSeries.RemoveRange(iqts);

				ye.TextTypes.RemoveRange(iqtt);
				ye.Values.RemoveRange(iqv);
				ye.Types.RemoveRange(iqt);
				SaveChanges(ye);
			}

			List<Type> lt = new List<Type>();
			lt.Add(new Type() { Id = (int)Sequences.ValueType.Bit, Size = 2, Name = "Bit" });
			lt.Add(new Type() { Id = (int)Sequences.ValueType.Line, Size = 4, Name = "Line" });
			lt.Add(new Type() { Id = (int)Sequences.ValueType.Duogram, Size = 4, Name = "Duogram" });
			lt.Add(new Type() { Id = (int)Sequences.ValueType.Trigram, Size = 8, Name = "Trigram" });
			lt.Add(new Type() { Id = (int)Sequences.ValueType.Hexagram, Size = 64, Name = "Hexagram" });

			List<Value> lv = new List<Value>();
			foreach (Type t in lt)
				for (int i = 0; i < t.Size; ++i)
					lv.Add(new Value() { TypeId = t.Id, ValueId = i, });

			List<TextType> ltt = new List<TextType>();
			foreach (String s in Sequences.TextType)
				ltt.Add(new TextType() { Name = s });

			using (YijingEntities ye = new YijingEntities())
			{
				ye.Types.AddRange(lt);
				ye.Values.AddRange(lv);
				ye.TextTypes.AddRange(ltt);
				SaveChanges(ye);
			}

			using (YijingEntities ye = new YijingEntities())
			{
				List<Sequence> ls = new List<Sequence>();
				List<Ratio> lr = new List<Ratio>();
				List<Label> ll = new List<Label>();
				List<Text> ltx = new List<Text>();
				for (int dt = 0; dt < 3; ++dt)
					for (int ds = 1; ds < 14; ++ds)
					{
						int size = 0;
						String sequence = Sequences.strDiagramSettings[SettingIndex(dt, 0), ds];
						if (sequence.Length > 0)
						{
							SequenceSery ss1 = new SequenceSery() { TypeId = DiagramType(dt), Name = sequence };
							ye.SequenceSeries.Add(ss1);
							SaveChanges(ye);
							size = ye.Types.Find(ss1.TypeId).Size;
							for (int dv = 0; dv < size; ++dv)
								ls.Add(new Sequence()
								{
									TypeId = DiagramType(dt),
									ValueId = dv,
									SequenceSeryId = ss1.Id,
									SequenceData = SequenceArray(dt)[ds - 1, dv]
								});
						}

						String ratio = Sequences.strDiagramSettings[SettingIndex(dt, 1), ds];
						if (ratio.Length > 0)
						{
							RatioSery rs1 = new RatioSery() { TypeId = DiagramType(dt), Name = ratio };
							ye.RatioSeries.Add(rs1);
							SaveChanges(ye);
							size = ye.Types.Find(rs1.TypeId).Size;
							for (int dv = 0; dv < size; ++dv)
								lr.Add(new Ratio()
								{
									TypeId = DiagramType(dt),
									ValueId = dv,
									RatioSeryId = rs1.Id,
									RatioData = RatioArray(dt)[ds - 1, dv]
								});
						}

						String label = Sequences.strDiagramSettings[SettingIndex(dt, 2), ds];
						if (label.Length > 0)
						{
							LabelSery ls1 = new LabelSery() { TypeId = DiagramType(dt), Name = label };
							ye.LabelSeries.Add(ls1);
							SaveChanges(ye);
							size = ye.Types.Find(ls1.TypeId).Size;
							for (int dv = 0; dv < size; ++dv)
								ll.Add(new Label()
								{
									TypeId = DiagramType(dt),
									ValueId = dv,
									LabelSeryId = ls1.Id,
									LabelData = LabelArray(dt)[ds - 1, dv]
								});
						}

						String text = Sequences.strDiagramSettings[SettingIndex(dt, 3), ds];
						if (text.Length > 0)
						{
							TextSery ts1 = new TextSery() { TypeId = DiagramType(dt), Name = text };
							ye.TextSeries.Add(ts1);
							SaveChanges(ye);
							size = ye.Types.Find(ts1.TypeId).Size;
							int tti = ye.TextTypes.Single(tt => tt.Name == "Text").Id;
							for (int dv = 0; dv < size; ++dv)
							{
								if (text.Length > 0)
									ltx.Add(new Text()
									{
										TypeId = DiagramType(dt),
										ValueId = dv,
										TextSeryId = ts1.Id,
										TextTypeId = tti,
										TextData = Text(dt, text, "Text", dv)
									});
							}
						}
					}
				ye.Sequences.AddRange(ls);
				ye.Ratios.AddRange(lr);
				ye.Labels.AddRange(ll);
				ye.Texts.AddRange(ltx);
				SaveChanges(ye);
			}
		}

		public void QueryDB()
		{
			using (YijingEntities ye = new YijingEntities())
			{
				Type t = ye.Types.Find(3);
				LabelSery ls2 = ye.LabelSeries.Where(ls1 => ls1.Name == "Vivash" &&
					ls1.TypeId == (int)Sequences.ValueType.Hexagram).First(); // .OrderBy(ls1 => ls1.Name)
				IEnumerable<LabelSery> iels = from ls3 in ye.LabelSeries
											  where ls3.TypeId == (int)Sequences.ValueType.Line
											  orderby ls3.Name
											  select ls3;
				List<LabelSery> lls = iels.ToList();

				LabelSery ls4 = ye.LabelSeries.Include(ls => ls.Type).Include(ls => ls.Labels).Single(ls5 => ls5.Name == "Vivash");
				Label l1 = ls2.Labels.Single(l => l.LabelData == "Return");
				t = l1.Type;
			}
		}

		public int DiagramType(int i)
		{
			switch (i)
			{
				case 0: return (int)Sequences.ValueType.Line;
				case 1: return (int)Sequences.ValueType.Trigram;
				case 2: return (int)Sequences.ValueType.Hexagram;
			}
			return 0;
		}

		public int SettingIndex(int i, int o)
		{
			switch (i)
			{
				case 0: return 5 + o;
				case 1: return 9 + o;
				case 2: return 13 + o;
			}
			return 0;
		}

		public int[,] SequenceArray(int i)
		{
			switch (i)
			{
				case 0: return Sequences.nLineSequences;
				case 1: return Sequences.nTrigramSequences;
				case 2: return Sequences.nHexagramSequences;
			}
			return null;
		}

		public int[,] RatioArray(int i)
		{
			switch (i)
			{
				case 0: return Sequences.nLineRatios;
				case 1: return Sequences.nTrigramRatios;
				case 2: return Sequences.nHexagramRatios;
			}
			return null;
		}

		public String[,] LabelArray(int i)
		{
			switch (i)
			{
				case 0: return Sequences.strLineLabels;
				case 1: return Sequences.strTrigramLabels;
				case 2: return Sequences.strHexagramLabels;
			}
			return null;
		}

		public static String[] strLineText = { "Old Yin", "Young Yang", "Young Yin", "Old Yang" };
		public static String[] strTrigramText = { "Earth", "Thunder", "Water", "Lake", "Mountain", "Fire", "Wind", "Heaven" };
		public static String[] strHexagramText = { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fiveteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen", "Twenty", "Twenty One", "Twenty Two", "Twenty Three", "Twenty Four", "Twenty Five", "Twenty Six", "Twenty Seven", "Twenty Eight", "Twenty Nine", "Thirty", "Thirty One", "Thirty Two", "Thirty Three", "Thirty Four", "Thirty Five", "Thirty Six", "Thirty Seven", "Thirty Eight", "Thirty Nine", "Fourty", "Fourty One", "Fourty Two", "Fourty Three", "Fourty Four", "Fourty Five", "Fourty Six", "Fourty Seven", "Fourty Eight", "Fourty Nine", "Fifty", "Fifty One", "Fifty Two", "Fifty Three", "Fifty Four", "Fifty Five", "Fifty Six", "Fifty Seven", "Fifty Eight", "Fifty Nine", "Sixty", "Sixty One", "Sixty Two", "Sixty Three" };

		public String Text(int dt, String className, String methodName, int index)
		{
			switch (dt)
			{
				case 0:
					return strLineText[index];
				case 1:
					return strTrigramText[index];
				case 2:
					if ((className == "Wilhelm") || (className == "Legge"))
					{
						System.Type t = System.Type.GetType("ValueSequencer." + className +
							", ValueSequencer, Version=5.2.10.0, Culture=neutral, PublicKeyToken=null");
						if (t != null)
						{
							object[] p = { index };
							MethodInfo m = t.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);
							if (m != null)
								return (String)m.Invoke(null, p);
						}
					}
					break;
			}
			return strHexagramText[index];
		}

		public void ExecuteSqlCommand(String s)
		{
			using (YijingEntities ye = new YijingEntities())
			{
				try
				{
					//SqlParameter sp = new SqlParameter("@CategoryName", "Test");
					//ye.Database.ExecuteSqlCommand(s); // ,sp
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
			}
		}

		public void SaveChanges(YijingEntities ye)
		{
			try
			{
				ye.SaveChanges();
			}
			//catch (DbEntityValidationException e)
			//{
			//	Console.WriteLine(e.Message);
			//}
			catch (Exception e)
			{
				if (e.InnerException != null)
					if (e.InnerException.InnerException != null)
						Console.WriteLine(e.InnerException.InnerException.Message);
					else
						Console.WriteLine(e.InnerException.Message);
				else
					Console.WriteLine(e.Message);
			}
		}

	}
}

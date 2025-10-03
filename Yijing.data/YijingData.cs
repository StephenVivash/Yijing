
using Microsoft.EntityFrameworkCore;

//PM> add-migration CreateDB
//PM> update-database
//PM> remove-migration

namespace YijingData;

public class YijingDbContext: DbContext
{
	public static String _ds;

	public virtual DbSet<Session> Sessions { get; set; }
	public virtual DbSet<Text> Texts { get; set; }

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
	{/*
		modelBuilder.Entity<Text>()
			.HasOne(t => t.Type)
			.WithMany(t => t.Texts)
			.OnDelete(DeleteBehavior.Restrict);
		*/
	}
}

public class YijingDatabase
{
	public YijingDatabase(String file)
	{
		YijingDbContext._ds = $"Data Source={file}";
	}

	public void Initialse()
	{
		using (var yc = new YijingDbContext())
		{
			//List<Type> lt1 = ye.Types.ToList(); // Force access

			//tc.Database.EnsureDeleted();
			//ye.Database.EnsureCreated();
			yc.Database.Migrate();

			//List<Type> lt2 = ye.Types.ToList(); // Force access
		}
		using (var yc = new YijingDbContext())
		{
			IQueryable<Text> iqt = yc.Texts;
			IQueryable<Session> iqs = yc.Sessions;
			//tc.Texts.RemoveRange(iqtt);
			//SaveChanges(tc);
		}

		/*
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
		*/
	}

	public void QueryDB()
	{/*
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
		}*/
	}

	public void ExecuteSqlCommand(String s)
	{
		using (var yc = new YijingDbContext())
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

	public static void SaveChanges(YijingDbContext yc)
	{
		try
		{
			yc.SaveChanges();
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

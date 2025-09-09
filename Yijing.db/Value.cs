namespace YijingDb
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class Value
	{
		public Value()
		{
			Labels = new HashSet<Label>();
			Ratios = new HashSet<Ratio>();
			Sequences = new HashSet<Sequence>();
			Texts = new HashSet<Text>();
		}

		public int TypeId { get; set; }

		public int ValueId { get; set; }

		public virtual ICollection<Label> Labels { get; set; }

		public virtual ICollection<Ratio> Ratios { get; set; }

		public virtual ICollection<Sequence> Sequences { get; set; }

		public virtual ICollection<Text> Texts { get; set; }

		public virtual Type Type { get; set; }
	}
}

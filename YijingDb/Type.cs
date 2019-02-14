namespace YijingDb
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class Type
	{
		public Type()
		{
			Labels = new HashSet<Label>();
			LabelSeries = new HashSet<LabelSery>();
			Ratios = new HashSet<Ratio>();
			RatioSeries = new HashSet<RatioSery>();
			Sequences = new HashSet<Sequence>();
			SequenceSeries = new HashSet<SequenceSery>();
			Texts = new HashSet<Text>();
			TextSeries = new HashSet<TextSery>();
			Values = new HashSet<Value>();
		}

		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

		public int Size { get; set; }

		[Required, StringLength(100)]
		public string Name { get; set; }

		public virtual ICollection<Label> Labels { get; set; }

		public virtual ICollection<LabelSery> LabelSeries { get; set; }

		public virtual ICollection<Ratio> Ratios { get; set; }

		public virtual ICollection<RatioSery> RatioSeries { get; set; }

		public virtual ICollection<Sequence> Sequences { get; set; }

		public virtual ICollection<SequenceSery> SequenceSeries { get; set; }

		public virtual ICollection<Text> Texts { get; set; }

		public virtual ICollection<TextSery> TextSeries { get; set; }

		public virtual ICollection<Value> Values { get; set; }
	}
}

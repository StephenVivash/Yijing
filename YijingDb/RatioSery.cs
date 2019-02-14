namespace YijingDb
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class RatioSery
	{
		public RatioSery()
		{
			Ratios = new HashSet<Ratio>();
		}

		public int Id { get; set; }

		public int TypeId { get; set; }

		[Required, StringLength(100)]
		public string Name { get; set; }

		public virtual ICollection<Ratio> Ratios { get; set; }

		public virtual Type Type { get; set; }
	}
}

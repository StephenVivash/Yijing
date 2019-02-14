namespace YijingDb
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class Ratio
	{
		public int Id { get; set; }

		public int TypeId { get; set; }

		public int ValueId { get; set; }

		public int RatioSeryId { get; set; }

		public int RatioData { get; set; }

		public virtual RatioSery RatioSery { get; set; }

		public virtual Type Type { get; set; }

		public virtual Value Value { get; set; }
	}
}

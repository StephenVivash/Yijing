namespace YijingDb
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class Label
	{
		public int Id { get; set; }

		public int TypeId { get; set; }

		public int ValueId { get; set; }

		public int LabelSeryId { get; set; }

		[Required, StringLength(100)]
		public string LabelData { get; set; }

		public virtual LabelSery LabelSery { get; set; }

		public virtual Type Type { get; set; }

		public virtual Value Value { get; set; }
	}
}

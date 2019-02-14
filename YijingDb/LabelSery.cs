namespace YijingDb
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class LabelSery
	{
		public LabelSery()
		{
			Labels = new HashSet<Label>();
		}

		public int Id { get; set; }

		public int TypeId { get; set; }

		[Required, StringLength(100)]
		public string Name { get; set; }

		public virtual ICollection<Label> Labels { get; set; }

		public virtual Type Type { get; set; }
	}
}

namespace YijingDb
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class SequenceSery
	{
		public SequenceSery()
		{
			Sequences = new HashSet<Sequence>();
		}

		public int Id { get; set; }

		public int TypeId { get; set; }

		[Required, StringLength(100)]
		public string Name { get; set; }

		public virtual ICollection<Sequence> Sequences { get; set; }

		public virtual Type Type { get; set; }
	}
}

namespace YijingDb
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class Sequence
	{
		public int Id { get; set; }

		public int TypeId { get; set; }

		public int ValueId { get; set; }

		public int SequenceSeryId { get; set; }

		public int SequenceData { get; set; }

		public virtual SequenceSery SequenceSery { get; set; }

		public virtual Type Type { get; set; }

		public virtual Value Value { get; set; }
	}
}

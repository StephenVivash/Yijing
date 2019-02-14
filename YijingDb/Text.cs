namespace YijingDb
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class Text
	{
		public int Id { get; set; }

		public int TypeId { get; set; }

		public int ValueId { get; set; }

		public int TextSeryId { get; set; }

		public int TextTypeId { get; set; }

		//[Required, StringLength(100)]
		//public string TextType { get; set; }

		[Required]
		public string TextData { get; set; }

		public virtual TextSery TextSery { get; set; }

		public virtual TextType TextType { get; set; }

		public virtual Type Type { get; set; }

		public virtual Value Value { get; set; }
	}
}

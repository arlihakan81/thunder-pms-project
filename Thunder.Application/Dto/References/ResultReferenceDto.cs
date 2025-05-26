namespace Thunder.Application.Dto.References
{
	public class ResultReferenceDto
	{
		public Guid Id { get; set; }
		public required string Name { get; set; }
		public required string EmailAddress { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }

	}
}

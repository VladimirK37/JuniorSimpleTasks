using LinqToDB.Mapping;

namespace Fogsoft.SimpleTasks.Library
{
	/// <summary>
	/// Задача. Может быть назначена на <see cref="User"/>, а может быть ни на кого не назначена.
	/// </summary>
	[Table("Tasks")]
	public class Task
	{
		[Identity]
		[PrimaryKey]
		public long Id { get; set; }

		[Column, NotNull]
		public string Subject { get; set; }

		[Column, Nullable]
		public string Description { get; set; }

		[Column, Nullable]
		public long? AssigneeId { get; set; }
	}
}
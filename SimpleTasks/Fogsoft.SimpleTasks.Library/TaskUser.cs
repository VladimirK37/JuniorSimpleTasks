using LinqToDB.Mapping;

namespace Fogsoft.SimpleTasks.Library
{
	[Table("TasksUsers")]
	public class TaskUser
	{
		[PrimaryKey]
		public long UserId { get; set; }

		[PrimaryKey]
		public long TaskId { get; set; }
	}
}

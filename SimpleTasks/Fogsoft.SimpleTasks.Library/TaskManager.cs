/* Это класс, отвечающий за бизнес-логику по созданию задач.
 * В реальном проекте он использовался бы одним или несколькими сервисами, предоставляющими API для клиентских приложений.
 *
 * Чтобы не усложнять тестовое задание, приняты следующие упрощения (их дорабатывать не нужно):
 * - в методах GetX не предусмотрен пейджинг;
 * - отсутствует удаление задач и пользователей;
 * - отсутствует проверка прав, лог действий и ошибок;
 * - не предусмотрена многопользовательская работа;
 * - используется БД SQLite.
 */
using LinqToDB;
using System.Linq;

namespace Fogsoft.SimpleTasks.Library
{
	/// <summary>
	/// Предоставляет методы для работы с задачами.
	/// </summary>
	public class TaskManager
	{
		/// <summary>
		/// Добавляет новую задачу в БД.
		/// </summary>
		public Task Add(string subject, string description)
		{
			using var db = Db.Open();
			var task = new Task
			{
				Description = description,
				Subject = subject
			};
			task.Id = db.InsertWithInt64Identity(task);
			return task;
		}

		/// <summary>
		/// Назначает исполнителя на задачу.
		/// </summary>
		public void Assign(long taskId, long assigneeId)
		{
			using var db = Db.Open();
			db.Tasks
				.Where(x => x.Id == taskId)
				.Set(x => x.AssigneeId, assigneeId)
				.Update();
		}

		/// <summary>
		/// Снимает исполнителя с задачи.
		/// </summary>
		public void Unassign(long taskId)
		{
			using var db = Db.Open();
			db.Tasks
				.Where(x => x.Id == taskId)
				.Set(x => x.AssigneeId, (long?)null)
				.Update();
		}

		/// <summary>
		/// Возвращает все задачи.
		/// </summary>
		public Task[] GetAll()
		{
			using var db = Db.Open();
			return db.Tasks.ToArray();
		}

		/// <summary>
		/// Возвращает все задачи, назначенные на конкретного исполнителя.
		/// </summary>
		public Task[] GetByAssignee(long assigneeId)
		{
			using var db = Db.Open();
			return db.Tasks
				.Where(x => x.AssigneeId == assigneeId)
				.ToArray();
		}
	}
}
using System.IO;
using System.Linq;
using System.Reflection;
using Fogsoft.SimpleTasks.Library;
using LinqToDB.Data;
using NUnit.Framework;

namespace Fogsoft.SimpleTasks.Tests
{
	public class TaskManagerSpec
	{
		private long User0Id;
		private long User1Id;

		[SetUp]
		public void SetUp()
		{
			// всегда удаляем тестовую БД, если есть
			var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var dbFile = Path.Combine(currentDirectory, "db.sqlite");
			if (File.Exists(dbFile))
				File.Delete(dbFile);

			// запускаем скрипт, создающий тестовую БД
			var scriptPath = Path.Combine(currentDirectory, @"..\..\..\..\CreateTables.sql");
			var sql = File.ReadAllText(scriptPath);
			using var db = Db.Open();
			db.Execute(sql);
			User0Id = db.Users.Single(x => x.Name == "User0").Id;
			User1Id = db.Users.Single(x => x.Name == "User1").Id;

		}

		[Test]
		public void Typical_scenario_must_work()
		{
			var taskManager = new TaskManager();

			var tasks = taskManager.GetAll();
			Assert.That(tasks.Length, Is.EqualTo(0), "сначала задач не должно быть");

			var newTask = taskManager.Add("test1", "description1");
			tasks = taskManager.GetAll();
			Assert.That(tasks.Length, Is.EqualTo(1), "должна быть одна новая задача");
			AssertTask(tasks[0], newTask);

			tasks = taskManager.GetByAssignee(User0Id);
			Assert.That(tasks.Length, Is.EqualTo(0), "задача не должна быть назначена");

			taskManager.Assign(newTask.Id, User0Id);
			tasks = taskManager.GetByAssignee(User0Id);
			Assert.That(tasks.Length, Is.EqualTo(1), "у задачи появился исполнитель User0Id");
			Assert.That(tasks[0].Id, Is.EqualTo(newTask.Id), "задачи должны совпадать");

			taskManager.Assign(newTask.Id, User1Id);
			tasks = taskManager.GetByAssignee(User1Id);
			Assert.That(tasks.Length, Is.EqualTo(1), "у задачи появился исполнитель User1Id");
			Assert.That(tasks[0].Id, Is.EqualTo(newTask.Id), "обе задачи совпадать");

			taskManager.Unassign(newTask.Id, User0Id);
			tasks = taskManager.GetByAssignee(User0Id);
			Assert.That(tasks.Length, Is.EqualTo(0), "у задачи удалили исполнителя User0Id");

			taskManager.Unassign(newTask.Id, User1Id);
			tasks = taskManager.GetByAssignee(User1Id);
			Assert.That(tasks.Length, Is.EqualTo(0), "у задачи удалили исполнителя User1Id");

			tasks = taskManager.GetAll();
			Assert.That(tasks.Length, Is.EqualTo(1), "должна быть одна задача");
			AssertTask(tasks[0], newTask);
		}

		private static void AssertTask(Task actual, Task expected)
		{
			Assert.That(actual.Id, Is.EqualTo(expected.Id), "Id должно совпадать");
			Assert.That(actual.Subject, Is.EqualTo(expected.Subject), "Subject должно совпадать");
			Assert.That(actual.Description, Is.EqualTo(expected.Description), "Description должно совпадать");
		}

		[Test]
		public void Assign_to_many_users_scenario()
		{
			var taskManager = new TaskManager();

			var newTask = taskManager.Add("test2", "description2");
			var tasks = taskManager.GetAll();
			Assert.That(tasks.Length, Is.EqualTo(1), "должна быть одна новая задача");
			AssertTask(tasks[0], newTask);

			tasks = taskManager.GetByAssignee(User0Id);
			Assert.That(tasks.Length, Is.EqualTo(0), "задача не должна быть назначена");

			taskManager.Assign(newTask.Id, User0Id, User1Id);
			tasks = taskManager.GetByAssignee(User0Id);
			Assert.That(tasks.Length, Is.EqualTo(1), "у задачи появился исполнитель User0Id");
			Assert.That(tasks[0].Id, Is.EqualTo(newTask.Id), "задача должна быть равна первой задаче newTask");
			tasks = taskManager.GetByAssignee(User1Id);
			Assert.That(tasks.Length, Is.EqualTo(1), "у задачи появился исполнитель User1Id");
			Assert.That(tasks[0].Id, Is.EqualTo(newTask.Id), "задача должна быть равна первой задаче newTask");
		}
	}
}
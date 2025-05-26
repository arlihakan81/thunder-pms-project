using Microsoft.EntityFrameworkCore;
using Thunder.Domain.Entities;

namespace Thunder.Persistence.Context
{
	public class AppDbContext : DbContext
	{

		public DbSet<Domain.Entities.Task> Tasks { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Project> Projects { get; set; }
		public DbSet<Team> Teams { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<TaskTag> TaskTags { get; set; }
		public DbSet<Subtask> Subtasks { get; set; }
		public DbSet<Reference> References { get; set; }
 
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder
				.UseSqlServer(@"
					Server=(localdb)\mssqllocaldb; 
					Database=ThunderDb; 
					Trusted_Connection=True; 
					TrustServerCertificate=True"
			);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().HasMany(us => us.Tasks).WithOne(ts => ts.Assignee)
				.HasForeignKey(ts => ts.AssigneeId);

			modelBuilder.Entity<Project>().HasMany(us => us.Tasks).WithOne(ts => ts.Project)
				.HasForeignKey(ts => ts.ProjectId);

			modelBuilder.Entity<Team>().HasMany(us => us.Members).WithOne(ts => ts.Team)
				.HasForeignKey(ts => ts.TeamId);

			modelBuilder.Entity<Project>().HasMany(pr => pr.Teams).WithOne(tm => tm.Project)
				.HasForeignKey(tm => tm.ProjectId);

			modelBuilder.Entity<Domain.Entities.Task>().HasMany(ts => ts.Tags).WithMany(tg => tg.Tasks)
				.UsingEntity<TaskTag>();

			modelBuilder.Entity<Domain.Entities.Task>().HasMany(ts => ts.Subtasks).WithOne(st => st.Task)
				.HasForeignKey(st => st.TaskId);

			modelBuilder.Entity<Tag>().HasMany(tg => tg.TaskTags).WithOne(tt => tt.Tag)
				.HasForeignKey(tt => tt.TagId);

			modelBuilder.Entity<Domain.Entities.Task>().HasMany(ts => ts.TaskTags).WithOne(tt => tt.Task)
				.HasForeignKey(tt => tt.TaskId);

		}


	}
}

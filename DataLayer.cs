using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Project3.DataLayer
{
    // Модели
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CreatorId { get; set; }
        public User Creator { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int? AssigneeId { get; set; }
        public User? Assignee { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DueDate { get; set; }
    }

    public enum TaskStatus { Todo, InProgress, Done }

    // Исключения
    public class RepositoryException : Exception
    {
        public RepositoryException(string message) : base(message) { }
        public RepositoryException(string message, Exception inner) : base(message, inner) { }
    }

    // Интерфейсы репозиториев
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }

    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByUsernameAsync(string username);
    }

    public interface IProjectRepository : IRepository<Project>
    {
        Task<IEnumerable<Project>> GetByUserIdAsync(int userId);
    }

    public interface ITaskRepository : IRepository<Task>
    {
        Task<IEnumerable<Task>> GetByProjectIdAsync(int projectId);
        Task<IEnumerable<Task>> GetByAssigneeIdAsync(int assigneeId);
        Task UpdateStatusAsync(int taskId, TaskStatus status);
    }

    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IProjectRepository Projects { get; }
        ITaskRepository Tasks { get; }
        Task<int> CompleteAsync();
    }

    // Реализация репозиториев
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try { return await _dbSet.ToListAsync(); }
            catch (Exception ex) { throw new RepositoryException("Ошибка при получении всех записей", ex); }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            try { return await _dbSet.FindAsync(id); }
            catch (Exception ex) { throw new RepositoryException($"Ошибка при получении записи с ID {id}", ex); }
        }

        public async Task AddAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) { throw new RepositoryException("Ошибка при добавлении записи", ex); }
            catch (Exception ex) { throw new RepositoryException("Непредвиденная ошибка при добавлении записи", ex); }
        }

        public async Task UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) { throw new RepositoryException("Ошибка при обновлении записи", ex); }
            catch (Exception ex) { throw new RepositoryException("Непредвиденная ошибка при обновлении записи", ex); }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await GetByIdAsync(id);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex) { throw new RepositoryException("Ошибка при удалении записи", ex); }
            catch (Exception ex) { throw new RepositoryException("Непредвиденная ошибка при удалении записи", ex); }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try { return await GetByIdAsync(id) != null; }
            catch (Exception ex) { throw new RepositoryException($"Ошибка при проверке существования записи с ID {id}", ex); }
        }
    }

    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public async Task<User> GetByEmailAsync(string email)
        {
            try { return await _context.Users.FirstOrDefaultAsync(u => u.Email == email); }
            catch (Exception ex) { throw new RepositoryException($"Ошибка при получении пользователя по email {email}", ex); }
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            try { return await _context.Users.FirstOrDefaultAsync(u => u.Username == username); }
            catch (Exception ex) { throw new RepositoryException($"Ошибка при получении пользователя по логину {username}", ex); }
        }
    }

    public class ProjectRepository : BaseRepository<Project>, IProjectRepository
    {
        public ProjectRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Project>> GetByUserIdAsync(int userId)
        {
            try
            {
                return await _context.Projects
                    .Where(p => p.CreatorId == userId)
                    .ToListAsync();
            }
            catch (Exception ex) { throw new RepositoryException($"Ошибка при получении проектов пользователя с ID {userId}", ex); }
        }
    }

    public class TaskRepository : BaseRepository<Task>, ITaskRepository
    {
        public TaskRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Task>> GetByProjectIdAsync(int projectId)
        {
            try
            {
                return await _context.Tasks
                    .Where(t => t.ProjectId == projectId)
                    .ToListAsync();
            }
            catch (Exception ex) { throw new RepositoryException($"Ошибка при получении задач проекта с ID {projectId}", ex); }
        }

        public async Task<IEnumerable<Task>> GetByAssigneeIdAsync(int assigneeId)
        {
            try
            {
                return await _context.Tasks
                    .Where(t => t.AssigneeId == assigneeId)
                    .ToListAsync();
            }
            catch (Exception ex) { throw new RepositoryException($"Ошибка при получении задач исполнителя с ID {assigneeId}", ex); }
        }

        public async Task UpdateStatusAsync(int taskId, TaskStatus status)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(taskId);
                if (task != null)
                {
                    task.Status = status;
                    task.UpdatedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex) { throw new RepositoryException($"Ошибка при обновлении статуса задачи с ID {taskId}", ex); }
        }
    }

    // Контекст БД и Unit of Work
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Creator)
                .WithMany()
                .HasForeignKey(p => p.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Task>()
                .HasOne(t => t.Project)
                .WithMany()
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Task>()
                .HasOne(t => t.Assignee)
                .WithMany()
                .HasForeignKey(t => t.AssigneeId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IUserRepository _users;
        private IProjectRepository _projects;
        private ITaskRepository _tasks;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IUserRepository Users => _users ??= new UserRepository(_context);
        public IProjectRepository Projects => _projects ??= new ProjectRepository(_context);
        public ITaskRepository Tasks => _tasks ??= new TaskRepository(_context);

        public async Task<int> CompleteAsync()
        {
            try { return await _context.SaveChangesAsync(); }
            catch (Exception ex) { throw new RepositoryException("Ошибка при сохранении изменений", ex); }
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }

    // Пример сервиса
    public class ProjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Project> CreateProjectAsync(string title, string description, int creatorId)
        {
            var project = new Project
            {
                Title = title,
                Description = description,
                CreatorId = creatorId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Projects.AddAsync(project);
            await _unitOfWork.CompleteAsync();
            return project;
        }

        public async Task<IEnumerable<Project>> GetUserProjectsAsync(int userId)
        {
            return await _unitOfWork.Projects.GetByUserIdAsync(userId);
        }

        public async Task<bool> UpdateProjectAsync(int projectId, string title, string description)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(projectId);
            if (project == null) return false;

            project.Title = title;
            project.Description = description;
            project.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Projects.UpdateAsync(project);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}

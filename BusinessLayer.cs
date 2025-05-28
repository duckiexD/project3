using System;
using System.Collections.Generic;


{
    
    public interface IEntity
    {
        int Id { get; set; }
    }

    
    public abstract class BusinessLogic<T> where T : IEntity
    {
        protected readonly IRepository<T> _repository;

        protected BusinessLogic(IRepository<T> repository)
        {
            _repository = repository;
        }

        public virtual T GetById(int id)
        {
            return _repository.GetById(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public virtual void Add(T entity)
        {
            Validate(entity);
            _repository.Add(entity);
        }

        public virtual void Update(T entity)
        {
            Validate(entity);
            _repository.Update(entity);
        }

        public virtual void Delete(int id)
        {
            _repository.Delete(id);
        }

        protected abstract void Validate(T entity);
    }

    
    public interface IRepository<T> where T : IEntity
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
    }

    
    public class User : IEntity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class UserLogic : BusinessLogic<User>
    {
        public UserLogic(IRepository<User> repository) : base(repository) { }

        protected override void Validate(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username))
                throw new ArgumentException("Имя пользователя не может быть пустым");

            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ArgumentException("Email не может быть пустым");

            if (!user.Email.Contains("@"))
                throw new ArgumentException("Неверный формат email");

            if (string.IsNullOrWhiteSpace(user.PasswordHash))
                throw new ArgumentException("Пароль не может быть пустым");
        }

        public User GetByUsername(string username)
        {
            foreach (var user in _repository.GetAll())
            {
                if (user.Username == username)
                    return user;
            }
            return null;
        }

        public bool Authenticate(string username, string passwordHash)
        {
            var user = GetByUsername(username);
            if (user == null)
                return false;

            return user.PasswordHash == passwordHash && user.IsActive;
        }

        public void Deactivate(int userId)
        {
            var user = GetById(userId);
            if (user != null)
            {
                user.IsActive = false;
                _repository.Update(user);
            }
        }
    }

   
    public class Project : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CreatorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Deadline { get; set; }
        public ProjectStatus Status { get; set; }
    }

    public enum ProjectStatus
    {
        Planning,
        InProgress,
        Completed,
        OnHold,
        Cancelled
    }

    public class ProjectLogic : BusinessLogic<Project>
    {
        private readonly UserLogic _userLogic;

        public ProjectLogic(IRepository<Project> repository, UserLogic userLogic) : base(repository)
        {
            _userLogic = userLogic;
        }

        protected override void Validate(Project project)
        {
            if (string.IsNullOrWhiteSpace(project.Title))
                throw new ArgumentException("Название проекта не может быть пустым");

            if (project.CreatorId <= 0)
                throw new ArgumentException("Неверный ID создателя");

            if (project.Deadline < DateTime.Now)
                throw new ArgumentException("Срок выполнения не может быть в прошлом");

            if (_userLogic.GetById(project.CreatorId) == null)
                throw new ArgumentException("Создатель не найден");
        }

        public IEnumerable<Project> GetProjectsByUser(int userId)
        {
            var result = new List<Project>();
            foreach (var project in _repository.GetAll())
            {
                if (project.CreatorId == userId)
                    result.Add(project);
            }
            return result;
        }

        public void ChangeStatus(int projectId, ProjectStatus newStatus)
        {
            var project = GetById(projectId);
            if (project != null)
            {
                project.Status = newStatus;
                _repository.Update(project);
            }
        }

        public IEnumerable<Project> GetProjectsDueThisWeek()
        {
            var result = new List<Project>();
            var endOfWeek = DateTime.Today.AddDays(7);
            
            foreach (var project in _repository.GetAll())
            {
                if (project.Deadline >= DateTime.Today && project.Deadline <= endOfWeek)
                    result.Add(project);
            }
            return result;
        }
    }

    public class TaskItem : IEntity
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AssigneeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; }
        public PriorityLevel Priority { get; set; }
    }

    public enum TaskStatus
    {
        New,
        InProgress,
        Completed,
        OnHold,
        Cancelled
    }

    public enum PriorityLevel
    {
        Low,
        Medium,
        High,
        Critical
    }

    public class TaskLogic : BusinessLogic<TaskItem>
    {
        private readonly ProjectLogic _projectLogic;
        private readonly UserLogic _userLogic;

        public TaskLogic(IRepository<TaskItem> repository, ProjectLogic projectLogic, UserLogic userLogic) : base(repository)
        {
            _projectLogic = projectLogic;
            _userLogic = userLogic;
        }

        protected override void Validate(TaskItem task)
        {
            if (string.IsNullOrWhiteSpace(task.Title))
                throw new ArgumentException("Название задачи не может быть пустым");

            if (task.ProjectId <= 0)
                throw new ArgumentException("Неверный ID проекта");

            if (task.AssigneeId <= 0)
                throw new ArgumentException("Неверный ID исполнителя");

            if (_projectLogic.GetById(task.ProjectId) == null)
                throw new ArgumentException("Проект не найден");

            if (_userLogic.GetById(task.AssigneeId) == null)
                throw new ArgumentException("Исполнитель не найден");

            if (task.DueDate < DateTime.Now)
                throw new ArgumentException("Срок выполнения не может быть в прошлом");
        }

        public IEnumerable<TaskItem> GetTasksByProject(int projectId)
        {
            var result = new List<TaskItem>();
            foreach (var task in _repository.GetAll())
            {
                if (task.ProjectId == projectId)
                    result.Add(task);
            }
            return result;
        }

        public IEnumerable<TaskItem> GetTasksByUser(int userId)
        {
            var result = new List<TaskItem>();
            foreach (var task in _repository.GetAll())
            {
                if (task.AssigneeId == userId)
                    result.Add(task);
            }
            return result;
        }

        public void ChangeTaskStatus(int taskId, TaskStatus newStatus)
        {
            var task = GetById(taskId);
            if (task != null)
            {
                task.Status = newStatus;
                _repository.Update(task);
            }
        }

        public void ChangeTaskPriority(int taskId, PriorityLevel newPriority)
        {
            var task = GetById(taskId);
            if (task != null)
            {
                task.Priority = newPriority;
                _repository.Update(task);
            }
        }
    }
}

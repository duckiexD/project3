using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

{
    public class DatabaseService
    {
        private readonly ApplicationDbContext _context;

        public DatabaseService(ApplicationDbContext context)
        {
            _context = context;
        }

        // CREATE операции
        public async Task<User> CreateUserAsync(User user)
        {
            try
            {
                user.CreatedAt = DateTime.UtcNow;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при создании пользователя", ex);
            }
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            try
            {
                project.CreatedAt = DateTime.UtcNow;
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();
                return project;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при создании проекта", ex);
            }
        }

        public async Task<Task> CreateTaskAsync(Task task)
        {
            try
            {
                task.CreatedAt = DateTime.UtcNow;
                task.Status = TaskStatus.Todo;
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();
                return task;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при создании задачи", ex);
            }
        }

        
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.Creator)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Project>> GetProjectsByUserIdAsync(int userId)
        {
            return await _context.Projects
                .Where(p => p.CreatorId == userId)
                .ToListAsync();
        }

        public async Task<Task> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.Assignee)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Task>> GetTasksByProjectIdAsync(int projectId)
        {
            return await _context.Tasks
                .Where(t => t.ProjectId == projectId)
                .ToListAsync();
        }

       
        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateProjectAsync(Project project)
        {
            try
            {
                project.UpdatedAt = DateTime.UtcNow;
                _context.Projects.Update(project);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateTaskAsync(Task task)
        {
            try
            {
                task.UpdatedAt = DateTime.UtcNow;
                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateTaskStatusAsync(int taskId, TaskStatus status)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null) return false;

            task.Status = status;
            task.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

       
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return false;

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        
        public async Task<bool> UserExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<bool> ProjectExistsAsync(int id)
        {
            return await _context.Projects.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> TaskExistsAsync(int id)
        {
            return await _context.Tasks.AnyAsync(t => t.Id == id);
        }

        public async Task<User> AuthenticateUserAsync(string email, string passwordHash)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == passwordHash);
        }

        public async Task<List<Task>> GetUserTasksAsync(int userId)
        {
            return await _context.Tasks
                .Where(t => t.AssigneeId == userId)
                .ToListAsync();
        }
    }
}


using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Repository.Context;

namespace TaskManagement.Repository
{
    public class TaskManagementRepository : IDisposable, IRepository
    {
        private readonly TaskManagementContext _taskManagementContext;
        public TaskManagementRepository(TaskManagementContext taskManagementContext)
        {
            _taskManagementContext = taskManagementContext;
        }

        public T? Add<T>(T obj) where T : class
        {
            _taskManagementContext.Database.BeginTransaction();
            try
            {
                _taskManagementContext.Set<T>().Add(obj);
            }
            catch (Exception)
            {
                _taskManagementContext.Database.RollbackTransaction();
                return null;
            }

            _taskManagementContext.Database.CommitTransaction();
            _taskManagementContext.SaveChanges();
            return obj;
        }
        public T? Update<T>(T obj) where T : class
        {
            _taskManagementContext.Database.BeginTransaction();
            try
            {
                _taskManagementContext.Set<T>().Update(obj);
            }
            catch (Exception)
            {
                _taskManagementContext.Database.RollbackTransaction();
                return null;
            }

            _taskManagementContext.Database.CommitTransaction();
            _taskManagementContext.SaveChanges();
            return obj;
        }
        public bool Delete<T>(T obj) where T : class
        {
            _taskManagementContext.Database.BeginTransaction();
            try
            {
                _taskManagementContext.Set<T>().Remove(obj);
            }
            catch (Exception)
            {
                _taskManagementContext.Database.RollbackTransaction();
                return false;
            }

            _taskManagementContext.Database.CommitTransaction();
            _taskManagementContext.SaveChanges();
            return true;
        }
        public List<T> List<T>(int skip = 0, int take = 50, Expression<Func<T, bool>>? conditional = null) where T : class
        {
            if (conditional == null)
                conditional = p => true;

            return _taskManagementContext.Set<T>().Where(conditional).Skip(skip).Take(take).ToList();
        }

        public int? Counter<T>(Expression<Func<T, bool>>? conditional = null) where T : class
        {
            if (conditional == null)
                conditional = p => true;

            return _taskManagementContext.Set<T>().Where(conditional).Count();
        }

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _taskManagementContext.Dispose();
                }
            }

            _disposed = true;
        }
        #endregion
    }
}

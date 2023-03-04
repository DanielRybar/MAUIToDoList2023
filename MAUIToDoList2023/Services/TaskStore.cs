using MAUIToDoList2023.Data;
using MAUIToDoList2023.Interfaces;
using MAUIToDoList2023.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace MAUIToDoList2023.Services
{
    public class TaskStore : IDataStore<TaskItem>
    {
        private readonly TaskDbContext _context;
        public TaskStore(TaskDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddItemAsync(TaskItem item)
        {
            try
            {
                await _context.TaskItems.AddAsync(item);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteItemAsync(TaskItem item)
        {
            try
            {
                _context.TaskItems.Remove(item);
                await _context.SaveChangesAsync();
                
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<TaskItem> GetItemAsync(int id)
        {
            try
            {
                var item = await _context.TaskItems.FindAsync(id);
                if (item != null)               
                    return item;               
                else 
                    return default;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return default;
            }
        }

        public async Task<IEnumerable<TaskItem>> GetItemsAsync()
        {
            try
            {
                var items = await _context.TaskItems.ToListAsync();
                return items;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return default;
            }
        }

        public async Task<bool> RemoveAllItemsAsync()
        {
            try
            {
                if (_context.TaskItems.Any())
                {
                    foreach (TaskItem item in _context.TaskItems)
                    {
                        _context.Remove(item);
                    }
                }
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateItemAsync(TaskItem item)
        {
            try
            {
                _context.TaskItems.Update(item);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }
}

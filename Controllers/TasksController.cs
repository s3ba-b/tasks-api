using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TasksApi.Data;
using TasksApi.Models;
using TasksApi.Repositories;

namespace TasksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IRepository<Models.Task> _repository;

        public TasksController(IRepository<Models.Task> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpPost]
        public async Task<ActionResult<Models.Task>> PostTask(Models.Task task)
        {
            await _repository.AddAsync(task);
            await _repository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetTasks([FromQuery] TaskQueryParameters query)
        {
            Expression<Func<Models.Task, bool>> filter = p =>
        (!query.MinDueTime.HasValue || p.DueTime >= query.MinDueTime.Value) &&
        (!query.MaxDueTime.HasValue || p.DueTime <= query.MaxDueTime.Value) &&
        (!query.IsComplete.HasValue || p.IsComplete == query.IsComplete.Value);

            var tasks = await _repository.FindAsync(filter);

            if (!query.PageNumber.HasValue || !query.PageSize.HasValue)
            {
                return Ok(tasks);
            }

            var totalItems = tasks.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)query.PageSize.Value);
            var pagedTasks = tasks
                .Skip((query.PageNumber.Value - 1) * query.PageSize.Value)
                .Take(query.PageSize.Value)
                .ToList();

            var paginationMetadata = new
            {
                totalItems,
                totalPages,
                query.PageSize,
                query.PageNumber
            };

            Response.Headers.Append("X-Pagination", System.Text.Json.JsonSerializer.Serialize(paginationMetadata));

            return Ok(pagedTasks);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Task>> GetTask(int id)
        {
            var task = await _repository.GetByIdAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, Models.Task task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }

            _repository.Update(task);

            try
            {
                await _repository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _repository.ExistsAsync(id))
                {
                    return NotFound();
                }

                return Conflict("The task was modified by another request. Please refresh and try again.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _repository.GetByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _repository.Remove(task);
            await _repository.SaveChangesAsync();

            return NoContent();
        }
    }
}

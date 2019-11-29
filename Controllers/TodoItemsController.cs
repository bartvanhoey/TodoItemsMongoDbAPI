using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TodoItemsMongoDbAPI.Models;
using TodoItemsMongoDbAPI.Services;

namespace TodoItemsMongoDbAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoItemsService _todoItemService;

        public TodoItemsController(TodoItemsService todoItemService)
        {
            _todoItemService = todoItemService;
        }

        [HttpGet]
        public ActionResult<List<TodoItem>> Get()
        {
            return _todoItemService.Get();
        }

        [HttpGet("{id:length(24)}", Name = "GetTodoItem")]
        public ActionResult<TodoItem> Get(string id)
        {
            var todoItem = _todoItemService.Get(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        [HttpPost]
        public ActionResult<TodoItem> Create(TodoItem todoItem)
        {
            _todoItemService.Create(todoItem);

            return CreatedAtRoute("GetTodoItem", new { id = todoItem.Id.ToString() }, todoItem);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, TodoItem todoItemIn)
        {
            var todoItem = _todoItemService.Get(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _todoItemService.Update(id, todoItemIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var todoItem = _todoItemService.Get(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _todoItemService.Remove(todoItem.Id);

            return NoContent();
        }
    }
}
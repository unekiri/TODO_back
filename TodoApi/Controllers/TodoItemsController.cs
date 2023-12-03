using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Controllers
{
    // program.csではなく、controller側のルート属性により、ルートの指定を行う
    [Route("api/[controller]")]

    // controllerがWebAPIへのリクエストに応答する
    [ApiController]

    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        // TodoContextクラスをControllerに挿入(各メソッド内でContextクラスを使用することができる)
        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

        // OPTIONS: api/TodoItems
        [HttpOptions]
        public IActionResult Options()
        {
            return Ok();
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
          if (_context.TodoItems == null)
          {
              return NotFound();
          }

            // DB内のTodoデータを全て取得して返す
            return await _context.TodoItems.ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
          if (_context.TodoItems == null)
          {
              return NotFound();
          }
            // リクエストされたIDと一致するDB内のTodoアイテムを取得
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                // 404 Not Found
                return NotFound();
            }

            // 200 OK
            return todoItem;
        }

        // GET: api/TodoItems/Incomplete
        [HttpGet("Incomplete")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetIncompleteTodoItems()
        {
            if (_context.TodoItems == null)
            {
                return NotFound();
            }

            // Retrieve only incomplete (未完了) TODO items
            var incompleteTodoItems = await _context.TodoItems
                .Where(item => !item.IsComplete)
                .ToListAsync();

            return incompleteTodoItems;
        }

        // GET: api/TodoItems/Complete
        [HttpGet("Complete")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetCompleteTodoItems()
        {
            if (_context.TodoItems == null)
            {
                return NotFound();
            }

            // Retrieve only completed (完了) TODO items
            var completeTodoItems = await _context.TodoItems
                .Where(item => item.IsComplete)
                .ToListAsync();

            return completeTodoItems;
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            // エンティティ(todoitem)の状態をUnchangedからModifiedに変更する
            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                // データをDBに保存する
                await _context.SaveChangesAsync();
            }

            // 同時実行制御に関する例外(更新中に別のユーザーによって値が書き換えられた場合など)
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {

            if (_context.TodoItems == null)
          {
              return Problem("Entity set 'TodoContext.TodoItems'  is null.");
          }

            // POSTされたデータをContextクラスに追加する
            _context.TodoItems.Add(todoItem);

            // データをDBに保存する
            await _context.SaveChangesAsync();

            // ステータスコード201(Created)を返し、GetTodoItemメソッドを呼び出す
            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            if (_context.TodoItems == null)
            {
                return NotFound();
            }

            // リクエストされたIDと一致するDB内のTodoアイテムを取得
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            // DELETEするデータをContextクラスから取り除く
            _context.TodoItems.Remove(todoItem);

            // データをDBに保存する
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id)
        {
            return (_context.TodoItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
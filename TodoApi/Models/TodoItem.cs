namespace TodoApi.Models;
using System.ComponentModel.DataAnnotations;


public class TodoItem
{
    public long Id { get; set; }

    [StringLength(100)]
    public string? Name { get; set; }

    public bool IsComplete { get; set; }

    [Required(ErrorMessage = "日付を入力して下さい。")]
    public DateTime Date { get; set; }
}
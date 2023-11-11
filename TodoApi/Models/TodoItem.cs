namespace TodoApi.Models;
using System.ComponentModel.DataAnnotations;


public class TodoItem
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public bool IsComplete { get; set; }

    public DateTime Date { get; set; }
}
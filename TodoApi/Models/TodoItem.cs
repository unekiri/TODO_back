﻿// エンティティ(DBのテーブルに対応するクラスのインスタンス)

namespace TodoApi.Models;
using System;
using System.ComponentModel.DataAnnotations;


public class TodoItem
{
    public long Id { get; set; }

    [Required(ErrorMessage = "TODOを入力して下さい")]
    [StringLength(100, ErrorMessage = "入力は100文字以内にして下さい")]
    public string? Name { get; set; }

    public bool IsComplete { get; set; }

    [Required(ErrorMessage = "日付を入力して下さい")]
    public DateTime Date { get; set; }
}
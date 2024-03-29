﻿// TodoItemエンティティとDBを対応付ける

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace TodoApi.Models;

// DbContextクラス：DBにアクセスするための基本クラス
public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
    {
    }

    // どのエンティティがデータベースのどのテーブルに対応するかを定義する
    public DbSet<TodoItem> TodoItems { get; set; } = null!;
}
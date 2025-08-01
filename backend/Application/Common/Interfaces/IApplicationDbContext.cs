﻿using CleanArch.Domain.TodoItems;
using CleanArch.Domain.TodoLists;
using CleanArch.Domain.Users;

namespace CleanArch.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<TodoList> TodoLists { get; }
    DbSet<TodoItem> TodoItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

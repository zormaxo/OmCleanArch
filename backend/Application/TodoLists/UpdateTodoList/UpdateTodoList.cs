﻿using CleanArch.Application.Common.Interfaces;
using CleanArch.Application.Common.Models;
using CleanArch.Domain.TodoLists;

namespace CleanArch.Application.TodoLists.UpdateTodoList;

public record UpdateTodoListCommand(int Id, string Title, string UserId) : IRequest<Result>;

public class UpdateTodoListCommandHandler(IApplicationDbContext context)
    : IRequestHandler<UpdateTodoListCommand, Result>
{
    public async Task<Result> Handle(
        UpdateTodoListCommand request,
        CancellationToken cancellationToken
    )
    {
        TodoList? todoList = await context.TodoLists.SingleOrDefaultAsync(
            t => t.Id == request.Id,
            cancellationToken
        );

        if (todoList is null)
        {
            return Result.Failure<int>(TodoListErrors.NotFound(request.Id));
        }

        todoList.Title = request.Title;

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

﻿using CleanArch.Application.Common.Interfaces;
using CleanArch.Application.Common.Models;
using CleanArch.Application.Common.Security;
using CleanArch.Domain.Constants;

namespace CleanArch.Application.TodoLists.PurgeTodoLists;

[Authorize(Roles = Roles.Administrator)]
[Authorize(Policy = Policies.CanPurge)]
public record PurgeTodoListsCommand : IRequest<Result>;

public class PurgeTodoListsCommandHandler(IApplicationDbContext context)
    : IRequestHandler<PurgeTodoListsCommand, Result>
{
    public async Task<Result> Handle(
        PurgeTodoListsCommand request,
        CancellationToken cancellationToken
    )
    {
        context.TodoLists.RemoveRange(context.TodoLists);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

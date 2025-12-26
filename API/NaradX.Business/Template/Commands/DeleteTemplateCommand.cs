// <copyright file="DeleteTemplateCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Template.Commands;

using MediatR;

public class DeleteTemplateCommand(string name) : IRequest<bool>
{
    public string Name { get; } = name;
}

using SmartRide.Application.Commands;
using SmartRide.Application.DTOs;
using SmartRide.Application.Queries;
using System.Reflection;

namespace SmartRide.Application.Factories;

public static class MediatRFactory
{
    public static TQuery CreateQuery<TQuery>(BaseDTO requestDTO)
        where TQuery : IQuery, new()
    {
        var query = new TQuery();
        var requestProps = requestDTO.GetType().GetProperties();
        var queryProps = typeof(TQuery).GetProperties();

        foreach (var prop in requestProps)
        {
            var matchingProp = queryProps.FirstOrDefault(p => p.Name == prop.Name);
            if (matchingProp != null && matchingProp.CanWrite)
            {
                matchingProp.SetValue(query, prop.GetValue(requestDTO));
            }
        }

        return query;
    }

    public static TCommand CreateCommand<TCommand>(BaseDTO requestDTO)
        where TCommand : ICommand, new()
    {
        var command = new TCommand();
        var requestProps = requestDTO.GetType().GetProperties();
        var commandProps = typeof(TCommand).GetProperties();

        foreach (var prop in requestProps)
        {
            var matchingProp = commandProps.FirstOrDefault(p => p.Name == prop.Name);
            if (matchingProp != null && matchingProp.CanWrite)
            {
                matchingProp.SetValue(command, prop.GetValue(requestDTO));
            }
        }

        return command;
    }
}

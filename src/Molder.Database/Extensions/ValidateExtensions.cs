﻿using FluentAssertions;
using Molder.Database.Models;
using Molder.Infrastructures;
using System.Collections.Concurrent;

namespace Molder.Database.Extensions
{
    public static class ValidateExtensions
    {
        public static void InputValidation(this ConcurrentDictionary<string, (IDbClient connection, TypeOfAccess typeOfAccess, int? timeout)> connections, string connectionName, string query)
        {
            connections.Should().ContainKey(connectionName, $"Connection: \"{connectionName}\" does not exist");
            query.Should().NotBeEmpty("Query cannot be empty.");
        }
    }
}
﻿using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Serilog.Core;
using Serilog.Events;

namespace StarterKit.Enrichers
{
    public class TypeEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var type = string.Empty;

            switch (logEvent.Exception)
            {
                case null:
                    type = "application";
                    break;
                case DbUpdateException a:
                case DbException b:
                case DBConcurrencyException c:
                    type = "privacy";
                    break;
                default:
                    type = "technical";
                    break;
            }

            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                "Type", type));
        }
    }
}
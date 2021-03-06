﻿using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Diagnostics;

namespace CAF.Log
{
    public class CAFLogging : SingletonBase<CAFLogging>
    {
        public CAFLogging() { }

        public void Base(string message, TraceEventType eventType, string title, string[] targets)
        {
            var log = new LogEntry();
            foreach (var target in targets)
            {
                log.Categories.Add(target);
            }
            log.Title = title;
            log.Message = message;
            log.Severity = eventType;
            log.TimeStamp = DateTime.UtcNow;
            Logger.Writer.Write(log, "General");
        }

        public void Warm(string message, string title, string[] categories)
        {
            Base(message, TraceEventType.Warning, title, categories);
        }

        public void Error(string message, string title, string[] categories)
        {
            Base(message, TraceEventType.Error, title, categories);
        }

        public void Info(string message, string title, string[] categories)
        {
            Base(message, TraceEventType.Information, title, categories);
        }
    }
}
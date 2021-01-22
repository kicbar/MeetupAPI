﻿
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MeetupAPI.Filters
{
    public class TimeTrackFilter : Attribute, IActionFilter
    {
        private Stopwatch _stopwatch;
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _stopwatch.Stop();
            var miliseconds = _stopwatch.ElapsedMilliseconds;
            var action = context.ActionDescriptor.DisplayName;

            Debug.WriteLine($"Action [{action}], executed in {miliseconds} miliseconds.");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }
    }
}

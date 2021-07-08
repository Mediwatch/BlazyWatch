using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Components;
using BlazingCalendar;
using BlazingCalendar.Data;

namespace BlazingCalendar.Services
{
    public interface ICalendarInteropService
    {
        Task CalendarInit(string elementName, CalendarSettings settings, DotNetObjectReference<ComponentBase> dotNetRef = null);
        Task CalendarChangeDuration(string unit, int amount);
        Task CalendarSetOption(string option, dynamic value);
        Task CalendarChangeResourceFeed(CalendarResourceFeed calendarResourceFeed);
        Task CalendarRefetchEvents();
        Task SetDotNetReference(DotNetObjectReference<FullCalendar> reference);
    }

    public class CalendarInteropService : ICalendarInteropService
    {
        private readonly IJSRuntime jsRuntime;

        public CalendarInteropService(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }


        public Task CalendarInit(string elementName, CalendarSettings settings, DotNetObjectReference<ComponentBase> dotNetRef = null)
        {
            try
            {
                jsRuntime.InvokeAsync<ElementReference>(
                    "BlazingCalendar.interop.calendarInit",
                    elementName,
                    settings.ToJson(),
                    dotNetRef
                    );
                return Task.CompletedTask;
            }
            catch
            {
                return Task.CompletedTask;
            }
        }

        public Task CalendarChangeDuration(string unit, int amount)
        {
            try
            {
                jsRuntime.InvokeAsync<string>(
                    "BlazingCalendar.interop.calendarChangeDuration",
                    unit,
                    amount
                    );
                return Task.CompletedTask;
            }
            catch
            {
                return Task.CompletedTask;
            }
        }

        public Task CalendarSetOption(string option, dynamic value)
        {
            //string json = JsonConvert.SerializeObject(value);
            string json = "";
            try
            {
                jsRuntime.InvokeAsync<string>(
                    "BlazingCalendar.interop.calendarSetOption",
                    option,
                    json
                    );
                return Task.CompletedTask;
            }
            catch
            {
                return Task.CompletedTask;
            }
        }

        public Task CalendarChangeResourceFeed(CalendarResourceFeed calendarResourceFeed)
        {
            try
            {
                jsRuntime.InvokeAsync<string>(
                    "BlazingCalendar.interop.calendarRefetchResources",
                    calendarResourceFeed
                    );
                return Task.CompletedTask;
            }
            catch
            {
                return Task.CompletedTask;
            }
        }

        public Task CalendarRefetchEvents()
        {
            try
            {
                jsRuntime.InvokeAsync<string>(
                    "BlazingCalendar.interop.calendarRefetchEvents"
                    );
                return Task.CompletedTask;
            }
            catch
            {
                return Task.CompletedTask;
            }
        }

        public Task SetDotNetReference(DotNetObjectReference<FullCalendar> reference)
        {
            try
            {
                jsRuntime.InvokeVoidAsync(
                    "BlazingCalendar.interop.SetDotNetReference",
                    reference);
                return Task.CompletedTask;
            }
            catch
            {
                return Task.CompletedTask;
            }
        }
    }
}

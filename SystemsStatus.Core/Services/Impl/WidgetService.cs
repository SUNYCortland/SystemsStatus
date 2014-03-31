using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities;
using SystemsStatus.Core.Data.Repositories;
using SystemsStatus.Core.Data;
using System.Web;
using System.Web.Mvc;

namespace SystemsStatus.Core.Services.Impl
{
    public class WidgetService : IWidgetService
    {
        private readonly IWidgetRepository _widgetRepository;

        public WidgetService(IWidgetRepository widgetRepository)
        {
            _widgetRepository = widgetRepository;
        }

        /// <summary>
        /// Returns all widgets
        /// </summary>
        /// <returns>IQueryable of all widgets</returns>
        public IQueryable<Widget> GetAllWidgets()
        {
            return _widgetRepository.All();
        }

        /// <summary>
        /// Returns all widgets using a service
        /// </summary>
        /// <param name="service">Service that widget is using</param>
        /// <returns>IQueryable of all widgets using a service</returns>
        public IQueryable<Widget> GetAllWidgetsByService(Service service)
        {
            return _widgetRepository.FilterBy(x => x.Services.Where(y => y.Id == service.Id).Count() > 0);
        }

        /// <summary>
        /// Returns all widgets by owner
        /// </summary>
        /// <param name="owner">User who owns widgets</param>
        /// <returns>IQueryable of all widgets who are owned by owner</returns>
        public IQueryable<Widget> GetAllWidgetsByOwner(User owner)
        {
            return _widgetRepository.FilterBy(x => x.Owner.Id == owner.Id);
        }

        /// <summary>
        /// Returns a widget
        /// </summary>
        /// <param name="id">Id of widget to return</param>
        /// <returns>Widget</returns>
        public Widget GetWidgetById(int id)
        {
            return _widgetRepository.FindBy(id);
        }

        /// <summary>
        /// Returns a widget
        /// </summary>
        /// <param name="guid">Guid of widget to return</param>
        /// <returns>Widget</returns>
        public Widget GetWidgetByGuid(Guid guid)
        {
            return _widgetRepository.FindBy(x => x.Guid == guid);
        }

        /// <summary>
        /// Inserts a widget
        /// </summary>
        /// <param name="widget">Widget to insert</param>
        /// <returns>Widget</returns>
        public Widget InsertWidget(Widget widget)
        {
            return _widgetRepository.Insert(widget);
        }

        /// <summary>
        /// Deletes a widget
        /// </summary>
        /// <param name="widget">Widget to delete</param>
        public void DeleteWidget(Widget widget)
        {
            _widgetRepository.Delete(widget);
        }

        /// <summary>
        /// Deletes a collection of widgets
        /// </summary>
        /// <param name="widgets">Collection of widgets to delete</param>
        [UnitOfWork]
        public void DeleteWidget(IEnumerable<Widget> widgets)
        {
            _widgetRepository.Delete(widgets);
        }

        /// <summary>
        /// Saves a widget
        /// </summary>
        /// <param name="widget">Widget to save</param>
        /// <returns>Saved Widget</returns>
        public Widget SaveWidget(Widget widget)
        {
            return _widgetRepository.SaveOrUpdate(widget);
        }

        /// <summary>
        /// Merges a widget with its entity
        /// </summary>
        /// <param name="widget">Widget to merge</param>
        /// <returns>Widget Department</returns>
        public Widget MergeWidget(Widget widget)
        {
            return _widgetRepository.Merge(widget);
        }

        /// <summary>
        /// Returns javascript code to use this widget
        /// </summary>
        /// <param name="widget">Widget to get javascript code for</param>
        /// <returns>Javascript Code</returns>
        public string GenerateCode(Widget widget)
        {
            if (widget.Guid == null)
                throw new Exception("Widget Guid is required to generate JavaScript widget code.");

            var helper = new UrlHelper(HttpContext.Current.Request.RequestContext);

            return "<iframe src=\"" + helper.Action("Index", "Widgets", new { area = "", guid = widget.Guid.ToString("D") }, HttpContext.Current.Request.Url.Scheme) + "\" width=\"400\" style=\"min-width: 180px; max-width: 100%; height: " + (widget.Height + 100) + "px; border: none;\"></iframe>";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities;

namespace SystemsStatus.Core.Services
{
    public interface IWidgetService
    {
        /// <summary>
        /// Returns all widgets
        /// </summary>
        /// <returns>IQueryable of all widgets</returns>
        IQueryable<Widget> GetAllWidgets();

        /// <summary>
        /// Returns all widgets using a service
        /// </summary>
        /// <param name="service">Service that widget is using</param>
        /// <returns>IQueryable of all widgets using a service</returns>
        IQueryable<Widget> GetAllWidgetsByService(Service service);

        /// <summary>
        /// Returns all widgets by owner
        /// </summary>
        /// <param name="owner">User who owns widgets</param>
        /// <returns>IQueryable of all widgets who are owned by owner</returns>
        IQueryable<Widget> GetAllWidgetsByOwner(User owner);

        /// <summary>
        /// Returns a widget
        /// </summary>
        /// <param name="id">Id of widget to return</param>
        /// <returns>Widget</returns>
        Widget GetWidgetById(int id);

        /// <summary>
        /// Returns a widget
        /// </summary>
        /// <param name="guid">Guid of widget to return</param>
        /// <returns>Widget</returns>
        Widget GetWidgetByGuid(Guid guid);

        /// <summary>
        /// Inserts a widget
        /// </summary>
        /// <param name="widget">Widget to insert</param>
        /// <returns>Widget</returns>
        Widget InsertWidget(Widget widget);

        /// <summary>
        /// Deletes a widget
        /// </summary>
        /// <param name="widget">Widget to delete</param>
        void DeleteWidget(Widget widget);

        /// <summary>
        /// Deletes a collection of widgets
        /// </summary>
        /// <param name="widgets">Collection of widgets to delete</param>
        void DeleteWidget(IEnumerable<Widget> widgets);

        /// <summary>
        /// Saves a widget
        /// </summary>
        /// <param name="widget">Widget to save</param>
        /// <returns>Saved Widget</returns>
        Widget SaveWidget(Widget widget);

        /// <summary>
        /// Merges a widget with its entity
        /// </summary>
        /// <param name="widget">Widget to merge</param>
        /// <returns>Widget Department</returns>
        Widget MergeWidget(Widget widget);

        /// <summary>
        /// Returns javascript code to use this widget
        /// </summary>
        /// <param name="widget">Widget to get javascript code for</param>
        /// <returns>Javascript Code</returns>
        string GenerateCode(Widget widget);
    }
}

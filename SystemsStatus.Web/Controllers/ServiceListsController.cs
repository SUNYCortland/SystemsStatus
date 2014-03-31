using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemsStatus.Core.Services;
using SystemsStatus.Core.Data.Entities.Statuses;
using SystemsStatus.Common.Pagination;

namespace SystemsStatus.Web.Controllers
{
    public class ServiceListsController : Controller
    {
        private readonly IServiceService _serviceService;

        private const int DEFAULT_PAGE_SIZE = 25;

        public ServiceListsController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }
        //
        // GET: /Services/AZList

        public ActionResult AZList(int? page, string display, string query)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            var services = _serviceService.GetAllServices();

            if (!String.IsNullOrEmpty(display))
            {
                switch (display.ToLower())
                {
                    case "online":
                        services = _serviceService.GetAllServicesWithCurrentStatusType(typeof(OnlineServiceStatus));
                        break;

                    case "maintenance":
                        services = _serviceService.GetAllServicesWithCurrentStatusType(typeof(OfflineMaintenanceServiceStatus));
                        break;

                    case "degraded":
                        services = _serviceService.GetAllServicesWithCurrentStatusType(typeof(OnlineDegradedServiceStatus));
                        break;

                    case "down":
                        services = _serviceService.GetAllServicesWithCurrentStatusType(typeof(OfflineUnplannedServiceStatus));
                        break;
                }
            }

            if (!String.IsNullOrEmpty(query))
            {
                services = services.Where(x => (x.Name.ToLower().Contains(query.ToLower())) 
                    || (x.Description.ToLower().Contains(query.ToLower()))
                    || (x.Department != null && x.Department.Name.ToLower().Contains(query.ToLower()))
                    || (x.Tags.Any(y => y.ToUpper().Contains(query.ToUpper()))));
            }

            services = services.OrderBy(x => x.Name.ToUpper());

            var servicesPaged = services.ToPagedList(currentPageIndex, DEFAULT_PAGE_SIZE);

            // The specified page is too large
            if (currentPageIndex >= servicesPaged.PageCount)
                return View(services.ToPagedList(0, DEFAULT_PAGE_SIZE));

            return View(servicesPaged);
        }

    }
}

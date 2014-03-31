using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemsStatus.Core.Services;
using SystemsStatus.Web.ViewModels;
using SystemsStatus.Common.Pagination;

namespace SystemsStatus.Web.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IServiceService _serviceService;
        private readonly IServiceStatusService _serviceStatusService;

        private const int DEFAULT_PAGE_SIZE = 10;

        public ServicesController(IServiceService serviceService, IServiceStatusService serviceStatusService)
        {
            _serviceService = serviceService;
            _serviceStatusService = serviceStatusService;
        }

        //
        // GET: /Services/{serviceUrl}

        public ActionResult Index(string serviceUrl)
        {
            if (String.IsNullOrWhiteSpace(serviceUrl))
                return RedirectToAction("AZList", new { controller = "ServiceLists" });

            var vm = new ServicesIndexViewModel();
            vm.Service = _serviceService.GetServiceByUrl(serviceUrl);

            if (vm.Service == null)
                throw new HttpException(404, "Not Found");

            vm.Statuses = _serviceStatusService.GetAllServiceStatuses(vm.Service.Id.Value)
                .OrderByDescending(x => x.CreatedDate)
                .Take(5);

            return View(vm);
        }

        //
        // GET: /Services/{serviceUrl}/StatusHistory

        public ActionResult StatusHistory(string serviceUrl, int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            if (String.IsNullOrWhiteSpace(serviceUrl))
                throw new HttpException(404, "Not Found");

            var service = _serviceService.GetServiceByUrl(serviceUrl);

            if (service == null)
                throw new HttpException(404, "Not Found");

            var statuses = service.Statuses.OrderByDescending(x => x.CreatedDate);

            var statusesPaged = statuses.ToPagedList(currentPageIndex, DEFAULT_PAGE_SIZE);

            var vm = new ServicesStatusHistoryViewModel();
            vm.Service = service;

            // The specified page is too large
            if (currentPageIndex >= statusesPaged.PageCount)
                vm.Statuses = statuses.ToPagedList(0, DEFAULT_PAGE_SIZE);
            else
                vm.Statuses = statusesPaged;

            return View(vm);
        }
    }
}

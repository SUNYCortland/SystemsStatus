using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemsStatus.Core.Services;
using SystemsStatus.Common.Pagination;
using SystemsStatus.Web.Areas.Admin.Attributes;
using SystemsStatus.Core.Data.Entities;
using SystemsStatus.Web.Areas.Admin.ViewModels;
using SystemsStatus.Core.Parsers;
using System.Text;
using Castle.Core.Logging;

namespace SystemsStatus.Web.Areas.Admin.Controllers
{
    [UserAuthorize(Roles = "Administrator")]
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly IXmlParser<Department> _departmentXmlParser;

        private const int DEFAULT_PAGE_SIZE = 10;

        public ILogger Logger { get; set; }

        public DepartmentsController(IDepartmentService departmentService, IXmlParser<Department> departmentXmlParser)
        {
            _departmentService = departmentService;
            _departmentXmlParser = departmentXmlParser;
        }

        //
        // GET: /Admin/Departments/

        public ActionResult Index(int? page, string query)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            var departments = _departmentService.GetAllDepartments();

            if (!String.IsNullOrEmpty(query))
            {
                departments = departments.Where(x => x.Name.ToLower().Contains(query.ToLower())
                    || x.Code.ToLower().Contains(query.ToLower()));
            }

            departments = departments.OrderBy(x => x.Name.ToUpper());

            var departmentsPaged = departments.ToPagedList(currentPageIndex, DEFAULT_PAGE_SIZE);

            // The specified page is too large
            if (currentPageIndex >= departmentsPaged.PageCount)
                return View(departments.ToPagedList(0, DEFAULT_PAGE_SIZE));

            return View(departmentsPaged);
        }

        //
        // GET: /Admin/Departments/Create

        public ActionResult Create()
        {
            var vm = new DepartmentsCreateViewModel();

            return View(vm);
        }

        //
        // POST: /Admin/Departments/Create

        [HttpPost]
        public ActionResult Create(DepartmentsCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                vm.Department.Code = vm.Department.Code.ToUpper();

                _departmentService.InsertDepartment(vm.Department);

                return RedirectToAction("Index");
            }

            return View(vm);
        }

        //
        // GET: /Admin/Departments/Edit/{id}

        public ActionResult Edit(int id)
        {
            var department = _departmentService.GetDepartmentById(id);

            if (department == null)
                throw new HttpException(404, String.Format("No department found with id = '{0}'", id));

            return View(department);
        }

        //
        // POST: /Admin/Department/Edit/{id}

        [HttpPost]
        public ActionResult Edit(int id, Department department)
        {
            var originalDepartment = _departmentService.GetDepartmentById(id);

            if (originalDepartment == null)
                throw new HttpException(404, String.Format("No department found with id = '{0}'", id));

            if (ModelState.IsValid)
            {
                // Check for uniqueness of Name
                if (_departmentService.GetAllDepartments().Where(x => x.Name.ToLower() == department.Name.ToLower() && x.Id != id).Count() > 0)
                {
                    ModelState.AddModelError("Name", "This name is already being used by another department. Please choose another.");
                }

                // Check for uniqueness of Code
                if (_departmentService.GetAllDepartments().Where(x => x.Code.ToLower() == department.Code.ToLower() && x.Id != id).Count() > 0)
                {
                    ModelState.AddModelError("Code", "This department code is already being used by another department. Please choose another.");
                }

                // If any errors found return
                if (ModelState.Values.Any(x => x.Errors.Count >= 1))
                    return View(department);

                // Add the id
                department.Id = id;
                department.Services = originalDepartment.Services;
                department.Code = department.Code.ToUpper();

                // Merge and save
                var departmentEntity = _departmentService.MergeDepartment(department);
                department = _departmentService.SaveDepartment(department);

                ViewBag.Saved = true;
            }

            return View(department);
        }

        //
        // GET: /Admin/Departments/Delete/{id}

        public ActionResult Delete(int id, int? page)
        {
            var department = _departmentService.GetDepartmentById(id);

            if (department == null)
                throw new HttpException(404, String.Format("No department found with id = '{0}'", id));

            _departmentService.DeleteDepartment(department);

            return RedirectToAction("Index", new { page = page });
        }

        //
        // GET: /Admin/Departments/Import

        public ActionResult Import()
        {
            return View(new DepartmentsImportViewModel());
        }

        //
        // POST: /Admin/Departments/Import

        [HttpPost]
        public ActionResult Import(DepartmentsImportViewModel vm)
        {
            if (ModelState.IsValid)
            {
                string xmlString = String.Empty;

                // Import by upload
                if (vm.DepartmentsFile != null && vm.DepartmentsFile.ContentLength > 0)
                {
                    // Convert XML file to a string
                    byte[] bytes = new byte[vm.DepartmentsFile.InputStream.Length];
                    vm.DepartmentsFile.InputStream.Read(bytes, 0, Convert.ToInt32(vm.DepartmentsFile.InputStream.Length));
                    xmlString = Encoding.ASCII.GetString(bytes);
                }
                else // Import by text
                {
                    xmlString = vm.DepartmentsXml;
                }

                if (!_departmentXmlParser.Validate(xmlString))
                {
                    return View("ImportError", _departmentXmlParser.GetErrors());
                }

                var departments = _departmentService.GetAllDepartments();
                var importedDepartments = _departmentXmlParser.Parse(xmlString).OrderBy(x => x.Name.ToUpper());

                // Check whether or not to update or insert
                foreach (var department in importedDepartments)
                {
                    // If this department already exists then update it
                    if (departments.Any(x => x.Code.ToUpper() == department.Code.ToUpper()))
                    {
                        var originalDepartment = departments.Where(x => x.Code.ToUpper() == department.Code.ToUpper()).Single();
                        var departmentEntity = department;

                        // Merge department with original
                        departmentEntity.Id = originalDepartment.Id;
                        departmentEntity.Services = originalDepartment.Services;
                        departmentEntity = _departmentService.MergeDepartment(departmentEntity);
                        departmentEntity = _departmentService.SaveDepartment(departmentEntity);
                    }
                    else // We need to insert this department
                    {
                        _departmentService.InsertDepartment(department);
                    }
                }

                return View("ImportSuccess", importedDepartments);
            }

            return View(new DepartmentsImportViewModel());
        }
    }
}

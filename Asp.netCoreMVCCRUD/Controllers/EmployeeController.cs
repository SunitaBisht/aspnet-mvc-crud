using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asp.netCoreMVCCRUD.DAL;
using Asp.netCoreMVCCRUD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Asp.netCoreMVCCRUD.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeDataService _dataService;

        public EmployeeController()
        {
            _dataService = new EmployeeDataService();
        }

        //GET-Display
        [HttpGet]
        public async Task<IActionResult> Display()
        {
            IEnumerable<Employee> result = await _dataService.Display();

            //IEnumerable<EmployeeViewModel> xEmployeeViewModel = result.Select(Emp => new EmployeeViewModel
            //{
            //    EmpId = Emp.EmpId,
            //    FirstName = Emp.FirstName,
            //    LastName = Emp.LastName,
            //    Gender = Emp.Gender,
            //    Email = Emp.Email,
            //    Phone = Emp.Phone,
            //}).ToList();
            List<EmployeeViewModel> employeeViewModelList = new List<EmployeeViewModel>();
            foreach (var employee in result)
            {
                EmployeeViewModel xEmployeeViewModel = new EmployeeViewModel()
                {
                    EmpId = employee.EmpId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Gender = employee.Gender,
                    Email = employee.Email,
                    Phone = employee.Phone,
                };
                employeeViewModelList.Add(xEmployeeViewModel);
            }
            return View(employeeViewModelList);
        }

        //GET-Register
        public IActionResult Register()
        {
            return View();
        }

        //POST-Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(EmployeeViewModel employee)
        {
            if (ModelState.IsValid)
            {
                Employee xEmployee = new Employee()
                {
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Password = employee.Password,
                    ConfirmPassword = employee.ConfirmPassword,
                    Gender = employee.Gender,
                    Email = employee.Email,
                    Phone = employee.Phone,
                    SecurityQuestion = employee.SecurityQuestion,
                    Answer = employee.Answer,
                };
                int response = await _dataService.Save(xEmployee);

                return RedirectToAction("Display");
            }
            return View(employee);
        }

        //GET-FOR EDIT 
        [HttpGet]
        public async Task<IActionResult> Edit(int? EmpId)
        {
            if (EmpId == null)
            {
                throw new ArgumentNullException($"{nameof(EmpId)} Can't be null.");
            }
            else
            {
                Employee employee = await _dataService.DisplayById(EmpId);
                EmployeeViewModel xEmployeeViewModel = new EmployeeViewModel
                {
                    EmpId = employee.EmpId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Gender = employee.Gender,
                    Email = employee.Email,
                    Phone = employee.Phone,
                };
                return View(xEmployeeViewModel);
            }
        }

        //Post FOR Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int EmpId, EmployeeViewModel employee)
        {
            if (ModelState.IsValid)
            {
                Employee xEmployee = new Employee()
                {
                    EmpId = employee.EmpId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Gender = employee.Gender,
                    Email = employee.Email,
                    Phone = employee.Phone,
                };
                int IsUpdated = await _dataService.Update(xEmployee);

                return RedirectToAction("Display");
            }
            return View(employee);
        }


        //For Get Details
        [HttpGet]
        public async Task<IActionResult> Details(int? EmpId)
        {
            if (EmpId == null)
            {
                throw new ArgumentNullException($"{nameof(EmpId)} Can't be null.");
            }
            else
            {
                Employee employee = await _dataService.DisplayById(EmpId);
                EmployeeViewModel xEmployeeViewModel = new EmployeeViewModel
                {
                    EmpId = employee.EmpId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Gender = employee.Gender,
                    Email = employee.Email,
                    Phone = employee.Phone,
                };

                return View(xEmployeeViewModel);
            }
        }


        // Get for Delete
        public async Task<IActionResult> Delete(int? EmpId)
        {
            if (EmpId == null)
            {
                throw new ArgumentNullException($"{nameof(EmpId)} Can't be null.");
            }
            else
            {
                Employee employee = await _dataService.DisplayById(EmpId);
                EmployeeViewModel xEmployeeViewModel = new EmployeeViewModel
                {
                    EmpId = employee.EmpId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Gender = employee.Gender,
                    Email = employee.Email,
                    Phone = employee.Phone,
                };

                return View(xEmployeeViewModel);
            }
        }

        //POST  for delete

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? EmpId)
        {
            if (EmpId == null)
            {
                throw new ArgumentNullException($"{nameof(EmpId)} Can't be null.");
            }
            int IsDeleted = await _dataService.Remove(EmpId);
           
            return RedirectToAction("Display");
        }
    }
}







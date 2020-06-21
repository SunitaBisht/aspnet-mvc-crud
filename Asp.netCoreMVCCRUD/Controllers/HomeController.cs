using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Asp.netCoreMVCCRUD.Models;
using Microsoft.EntityFrameworkCore;

namespace Asp.netCoreMVCCRUD.Controllers
{
    public class HomeController : Controller
    {
        private readonly EmployeeContext _context;

        public HomeController(EmployeeContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        //POST-Register
        [HttpPost]
        public JsonResult Register([FromBody] EmployeeViewModel employee)
        {
            int res = 0;
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
                _context.Employees.Add(xEmployee);
                res = _context.SaveChanges();
            }
            return Json(res);
        }

        //Get-Display
        [HttpGet]
        public IActionResult Display()
        {
            List<Employee> lstEmployee = _context.Employees.ToList();
            return View(lstEmployee);
        }

        //Get For Edit
        [HttpGet]
        public IActionResult Edit(int? EmpId)
        {
            if (EmpId == null)
            {
                throw new ArgumentNullException($"{nameof(EmpId)} Can't be null.");
            }
            else
            {
                Employee employee = _context.Employees.Find(EmpId);
                if (employee == null)
                {
                    return NotFound();
                }
                return View(employee);
            }
        }

        //Post Edit

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int EmpId, EmployeeViewModel employee)
        {
            int res = 0;
            if (ModelState.IsValid)
            {
                Employee xEmployee = new Employee()
                {
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Gender = employee.Gender,
                    Email = employee.Email,
                    Phone = employee.Phone,
                };
                _context.Employees.Update(xEmployee);
                res = _context.SaveChanges();
                return RedirectToAction("Display");
            }
            return View(employee);
        }



        //For Get Details
        [HttpGet]
        public IActionResult Details(int? EmpId)
        {
            if (EmpId == null)
            {
                throw new ArgumentNullException($"{nameof(EmpId)} Can't be null.");
            }
            Employee employee = _context.Employees.Find(EmpId);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // Get for Delete
        [HttpGet]
        public IActionResult Delete(int? EmpId)
        {
            if (EmpId == null)
            {
                throw new ArgumentNullException($"{nameof(EmpId)} Can't be null.");
            }
            Employee employee = _context.Employees.Find(EmpId);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        //POST  for delete

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? EmpId)
        {
            Employee employee = _context.Employees.Find(EmpId);
            if (employee == null)
            {
                throw new ArgumentNullException($"{nameof(employee)} Can't be null.");
            }
            _context.Employees.Remove(employee);
            _context.SaveChanges();
            return RedirectToAction("Display");
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}

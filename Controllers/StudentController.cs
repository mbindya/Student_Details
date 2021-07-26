using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Department.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Student_Department.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentContext _Db;
        public StudentController(StudentContext Db)
        {
            _Db = Db;

        }
        public IActionResult StudentList()
        {

            try
            {

                var stdList = from a in _Db.tbl_Student
                              join b in _Db.tbl_Departments
                              on a.DeptID equals b.ID
                              into Dep
                              from b in Dep.DefaultIfEmpty()

                              select new Student
                              {
                                  ID = a.ID,
                                  FName = a.FName,
                                  LName = a.LName,
                                  Mobile = a.Mobile,
                                  Email = a.Email,
                                  Description = a.Description,
                                  DeptID = a.DeptID,
                                  Department = b == null ? "" : b.Department

                              };
                return View(stdList);

            }
            catch (Exception)
            {
                return View();
            }

        }
        public IActionResult Create(Student obj)
        {
             LoadDDl();
            return View(obj);
        }
        [HttpPost]
        public async Task<IActionResult> AddStudent(Student obj)
        {
            try
            {
                if (ModelState.IsValid)

                    if (obj.ID == 0)
                    {
                        _Db.tbl_Student.Add(obj);
                        await _Db.SaveChangesAsync();

                    }
                    else
                    {
                        _Db.Entry(obj).State = EntityState.Modified;
                        await _Db.SaveChangesAsync();

                    }
                
                    return RedirectToAction("StudentList");
               
                            
            }
            catch (Exception ex)
            {
                return RedirectToAction("StudentList");
            }
          
        }
        public async Task<IActionResult> Delete(int ID)
        {
            try
            {
                var std = await _Db.tbl_Student.FindAsync(ID);
                if (std!= null)
                {
                    _Db.tbl_Student.Remove(std);
                    await _Db.SaveChangesAsync();
                }
                return RedirectToAction("StudentList");
            }
            catch(Exception ex)
            {
                return RedirectToAction("StudentList");
            }
        }
        private void LoadDDl()
        {
            try
            {
                List<Departments> depList = new List<Departments>();
                depList = _Db.tbl_Departments.ToList();
                depList.Insert(0, new Departments { ID = 0, Department = "Please Select" });
                ViewBag.DepList = depList;
            }
            catch(Exception ex )
            {

            }
        }

    }

}



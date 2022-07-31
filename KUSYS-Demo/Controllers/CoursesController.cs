using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KUSYS_Demo.Models;
using KUSYS_Demo.Persistence;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using KUSYS_Demo.Models.ViewModel;

namespace KUSYS_Demo.Controllers
{
    [Authorize(Roles = "Instructor")]
    public class CoursesController : Controller
    {
        private readonly KUSYSDBContext _context;
        private readonly UserManager<User> _userManager;

        public CoursesController(KUSYSDBContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Shows available Courses with matching students if there is 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var list = await (from student_course in _context.Student_Courses
                              join student in _context.Students on student_course.StudentId equals student.StudentId
                              join course in _context.Courses on student_course.CourseId equals course.Id
                              select new Student_CourseViewModel()
                              {
                                  CourseName = course.CourseName,
                                  StudentName = student.FirstName + " " + student.LastName,
                                  StudentId = student.StudentId,
                                  Id = student_course.Id
                              }).OrderBy(x => x.CourseName).ToListAsync();

            return View(list);
        }

        /// <summary>
        /// This method returns student list by course id to list whether student enroll or not
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetStudentListByCourseId(int courseId)
        {
            if (courseId != -1)
            {
                var studentIds = _context.Student_Courses.Where(x => x.CourseId == courseId).Select(x => x.StudentId).ToList();
                var students = (from student in _context.Students
                                select new
                                {
                                    StudentId = student.StudentId,
                                    FullName = student.FirstName + " " + student.LastName
                                })
                                .Where(x => !studentIds.Contains(x.StudentId)).ToList();

                return Json(students);
            }
            return Json(0);
        }

        /// <summary>
        /// redirect to enroll student to a course page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Create()
        {
            //Send list to select elements
            ViewBag.courses = _context.Courses.ToList();

            // 0 to not get any student at the beginning
            ViewBag.students = _context.Students.Where(x => x.UserId < 0).ToList();
            return View();
        }

        /// <summary>
        /// Allow to register a new student to a course if it is not already registered to the selected course
        /// </summary>
        /// <param name="student_Course"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Create(Student_Course student_Course)
        {
            _context.Student_Courses.Add(student_Course);
            _context.SaveChanges();
            return Json(student_Course);
        }

        /// <summary>
        /// Delete a student from enrolment 
        /// </summary>
        /// <param name="StudentId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult DeleteEnrolment(int StudentId, int Id)
        {
            var result = _context.Student_Courses
                         .Where(x => x.StudentId == StudentId && x.Id == Id)
                         .SingleOrDefault();

            if (result != null)
            {
                _context.Student_Courses.Remove(result);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}

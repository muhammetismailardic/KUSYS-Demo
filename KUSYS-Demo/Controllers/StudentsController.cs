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
    public class StudentsController : Controller
    {
        private readonly KUSYSDBContext _context;
        private readonly UserManager<User> _userManager;

        public StudentsController(KUSYSDBContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var kUSYSDbContext = _context.Students.Include(s => s.User);
            return View(await kUSYSDbContext.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentViewModel studentViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
                studentViewModel.UserId = int.Parse(userId);
                studentViewModel.CreatedAt = DateTime.Now;
                _context.Add(new Student() { BirthDate = studentViewModel.BirthDate, CreatedAt = DateTime.Now, FirstName = studentViewModel.FirstName ,LastName= studentViewModel.LastName,UserId= studentViewModel.UserId, });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(studentViewModel);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName", student.UserId);
            return View(new StudentViewModel() { StudentId = student.StudentId, BirthDate = student.BirthDate, CreatedAt = student.CreatedAt, FirstName = student.FirstName, LastName = student.LastName, UserId = student.UserId });
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentViewModel studentViewModel)
        {
            if (id != studentViewModel.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(new Student() {StudentId=id, BirthDate = studentViewModel.BirthDate, CreatedAt = studentViewModel.CreatedAt, FirstName = studentViewModel.FirstName, LastName = studentViewModel.LastName, UserId = studentViewModel.UserId });
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(studentViewModel.StudentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName", studentViewModel.UserId);
            return View(studentViewModel);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Students == null)
            {
                return Problem("Entity set 'KUSYSDbContext.Student'  is null.");
            }
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return (_context.Students?.Any(e => e.StudentId == id)).GetValueOrDefault();
        }

        [HttpGet]
        public JsonResult GetStudentDetails(int studentId)
        {
            var students = _context.Students.FirstOrDefault(x => x.StudentId == studentId);
            return Json(students);
        }
    }
}

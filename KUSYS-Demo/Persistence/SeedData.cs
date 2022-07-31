using KUSYS_Demo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace KUSYS_Demo.Persistence
{
    public static class SeedData
    {
        // This method provide seeding required default values to database.
        public static void SeedDatabase(IServiceProvider serviceProvider)
        {
            SeedRoles(serviceProvider);
            //SeedUser(serviceProvider);
            SeedCourses(serviceProvider);
        }

        public static void SeedRoles(IServiceProvider serviceProvider)
        {
            string[] roleNames = { "Administrator", "Instructor", "Student" };
            foreach (string roleName in roleNames)
            {
                SeedRole(serviceProvider, roleName);
            }
        }

        /// <summary>
        /// Create a Role if not exists.
        /// </summary>
        /// <param name="serviceProvider">Service Provider</param>
        /// <param name="roleName">SeedRole</param>
        private static void SeedRole(IServiceProvider serviceProvider, string roleName)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

            Task<bool> roleExists = roleManager.RoleExistsAsync(roleName);
            roleExists.Wait();

            if (!roleExists.Result)
            {
                Role role = new Role
                {
                    Name = roleName
                };

                Task<IdentityResult> roleResult = roleManager.CreateAsync(role);
                roleResult.Wait();
            }
        }

        /// <summary>
        /// Create admin user if not exists.
        /// </summary>
        /// <param name="serviceProvider">Service Provider</param>
        /// <param name="roleName">SeedUser</param>
        private static void SeedUser(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var userManager = services.GetRequiredService<UserManager<User>>();

                var user = new User
                {
                    UserName = "admin",
                    Email = "m.ismailardic@gmail.com",
                    FirstName = "Admin",
                    LastName = "User",
                };
                userManager.CreateAsync(user, "Admin?123").Wait();
                userManager.AddToRoleAsync(user, "Administrator").Wait();
            }
        }

        /// <summary>
        /// Create Courses if not exists.
        /// </summary>
        /// <param name="serviceProvider">Service Provider</param>
        /// <param name="roleName">SeedCourses</param>
        private static void SeedCourses(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<KUSYSDBContext>();

                context.Database.Migrate();
                if (!context.Courses.Any())
                {
                    context.Courses.AddRange(
                        new Course
                        {
                            CourseId = "CSI101",
                            CourseName = "Introduction to Computer Science",
                            CreatedAt = DateTime.Now,
                        },
                        new Course
                        {
                            CourseId = "CSI102",
                            CourseName = "Algorithms",
                            CreatedAt = DateTime.Now,
                        },
                        new Course
                        {
                            CourseId = "MAT101",
                            CourseName = "Calculus",
                            CreatedAt = DateTime.Now,
                        },
                        new Course
                        {
                            CourseId = "PHY101",
                            CourseName = "Physics",
                            CreatedAt = DateTime.Now,
                        });
                    context.SaveChanges();
                }
            }
        }
    }
}

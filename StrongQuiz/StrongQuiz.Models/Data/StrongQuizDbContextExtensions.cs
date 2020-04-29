using Microsoft.AspNetCore.Identity;
using StrongQuiz.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StrongQuiz.Models.Data
{
    public class StrongQuizDbContextExtensions
    {
        public async static Task SeedRoles(RoleManager<IdentityRole> RoleMgr)
        {
            IdentityResult roleResult; string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            { //verhinderen dat continue dezelfde rollen worden toegvoegd. 
                var roleExist = await RoleMgr.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleMgr.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        public async static Task SeedUsers(UserManager<ApplicationUser> userMgr)
        {
            //1. Admin aanmaken ---------------------------------------------------
            if (await userMgr.FindByNameAsync("Docent@1") == null)  //controleer de UserName
            {
                var user = new ApplicationUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Johan@1",
                    FirstName = "JohanVanHowest",
                    Email = "Docent@howest.be",
                };

                var userResult = await userMgr.CreateAsync(user, "Docent@1");
                var roleResult = await userMgr.AddToRoleAsync(user, "Admin");
                // var claimResult = await userMgr.AddClaimAsync(user, new Claim("DocentWeb", "True"));

                if (!userResult.Succeeded || !roleResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to build user and roles");
                }
            }
            //2. meerdere users  aanmaken --------------------------------------------
            //2a. persons met rol "Student" aanmaken
            var nmbrStudents = 9;
            for (var i = 1; i <= nmbrStudents; i++)
            {
                if (userMgr.FindByNameAsync("Student@" + i).Result == null)
                {
                    ApplicationUser student = new ApplicationUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = ("Student@" + i).Trim(),
                        FirstName = "nameStudent" + i,
                        Email = "Student" + i + "@howest.be",
                    };

                    var userResult = await userMgr.CreateAsync(student, "Docent1@");
                    var roleResult = await userMgr.AddToRoleAsync(student, "User");
                    if (!userResult.Succeeded || !roleResult.Succeeded)
                    {
                        throw new InvalidOperationException("Failed to build " + student.UserName);
                    }
                }
            }
            

        }
    }
}

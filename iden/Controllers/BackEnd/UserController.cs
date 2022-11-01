using iden.Models;
using iden.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace iden.Controllers.BackEnd
{
    [Authorize(Roles = "dmin")]

    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult >Index()
        {  
            var Users = await _userManager.Users.Select(User=>new UserViewModel {
            
            Id=User.Id,
            FirstName=User.FirstName,
                LastName = User.LastName,
            Email=User.Email,
                UserName = User.UserName,
                Roles= _userManager.GetRolesAsync(User).Result



        }).ToListAsync();
            return View(Users);
        }
                    public async Task<IActionResult> MangeRole (string Id)
        {
            var User = await _userManager.FindByIdAsync(Id);
            if (User==null)
            {
                return NotFound();
            }
            var role = await _roleManager.Roles.ToListAsync();


            var MangUserRole = new UserRolesViewModel
            {
                UserId = User.Id,
                UserName = User.UserName,
                Roles = role.Select(role => new RoleViewModel
                { 
                    RoleId=role.Id,
                    RoleName=role.Name,
                    IsSelected=_userManager.IsInRoleAsync(User ,role.Name).Result
                  


                    }).ToList(

                    
                    
                    ),
               

            };
            return View(MangUserRole);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MangeRole(UserRolesViewModel model)
        {
            var User = await _userManager.FindByIdAsync(model.UserId);
            if (User == null)

                return NotFound();

            var userrole = await _userManager.GetRolesAsync(User);
            foreach (var role in model.Roles)
            {
                if (userrole.Any(r => r == role.RoleName) && !role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(User, role.RoleName);

                if (!userrole.Any(r => r == role.RoleName) && role.IsSelected)
                    await _userManager.AddToRoleAsync(User, role.RoleName);
            }

            return RedirectToAction(nameof(Index));

        }




        [HttpGet]
        public async Task<IActionResult> ManageUserClaims(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            // UserManager service GetClaimsAsync method gets all the current claims of the user
            var existingUserClaims = await _userManager.GetClaimsAsync(user);

            var model = new UserClaimsViewModel
            {
                UserId = userId
            };

            // Loop through each claim we have in our application
            foreach (Claim claim in ClaimsStore.AllClaims)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type
                };

                // If the user has the claim, set IsSelected property to true, so the checkbox
                // next to the claim is checked on the UI
                if (existingUserClaims.Any(c => c.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }

                model.Cliams.Add(userClaim);
            }

            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.UserId} cannot be found";
                return View("NotFound");
            }

            // Get all the user existing claims and delete them
            var claims = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing claims");
                return View(model);
            }

            // Add all the claims that are selected on the UI
            result = await _userManager.AddClaimsAsync(user,
                model.Cliams.Where(c => c.IsSelected).Select(c => new Claim(c.ClaimType, c.ClaimType)));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected claims to user");
                return View(model);
            }

            return RedirectToAction("Index", new { Id = model.UserId });

        }



        public async Task<ActionResult> AllPermission(string UserId)
        {
     
            var user = await _userManager.FindByIdAsync(UserId);
            var claims = await _userManager.GetClaimsAsync(user);
            var uniq = claims.GroupBy(x => x.Value).Select(y => y.FirstOrDefault()).Distinct().ToList();
            return View(uniq);

        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcMusicStore.DataContexts;
using Microsoft.AspNet.Identity.EntityFramework;
using MvcMusicStore.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace MvcMusicStore.Controllers
{
    public class AdminController : Controller
    {
        private IdentityDb identityDb = new IdentityDb();

        //
        // GET: /Admin/
        [Authorize(Roles="AppAdmin")]
        public ActionResult Index()
        {
            return View(identityDb.Roles.OrderBy(r => r.Name));
        }

        // GET: /Admin/UsersInRole?role=
        [Authorize(Roles="AppAdmin")]
        public ActionResult UsersInRole(String role)
        {
            // Store the name of the role in the DoucheBag
            ViewBag.roleName = role;
            // Create a list of users who are members of the selected role
            List<IdentityUser> userMembers = new List<IdentityUser>();
            foreach (var user in identityDb.Roles.Single(r => r.Name == role).Users)
            {
                userMembers.Add(identityDb.Users.Find(user.UserId));
            }
            // Store the list of users in the DoucheBag
            ViewBag.members = userMembers;
            // Return all users in the database
            return View(identityDb.Users.OrderBy(u => u.UserName));
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // VVV This is the simple way, not thread safe VVV
        //
        // POST: Admin/RoleUsers?role=
        //[Authorize(Roles="AppAdmin")]
        //[HttpPost]
        //public ActionResult UsersInRole(String role, FormCollection Form) 
        //{
        //    var store = new UserStore<ApplicationUser>(identityDb);
        //    var manager = new UserManager<ApplicationUser>(store);
        //
        //    string userNameInput = Form["UserNames"].ToString();
        //    string[] userNames = userNameInput.Split(',');
        //    foreach (string userName in userNames)
        //    {
        //        bool isChecked = Convert.ToBoolean(Form[userName].Split(',')[0]);
        //        if (isChecked)
        //        {
        //            if (!manager.IsInRole(manager.Users.FirstOrDefault(u => u.UserName == userName).Id, role))
        //            {
        //                manager.AddToRole(manager.Users.FirstOrDefault(u => u.UserName == userName).Id, role);
        //            }
        //        }
        //        else
        //        {
        //            if (manager.IsInRole(manager.Users.FirstOrDefault(u => u.UserName == userName).Id, role))
        //            {
        //                manager.RemoveFromRole(manager.Users.FirstOrDefault(u => u.UserName == userName).Id, role);
        //            }
        //        }
        //    }
        //    return RedirectToAction("Index");
        //}
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        // VVV This is the thread safe way of making changes, waits for processes to finish before starting a new one
        // (in case multiple admins are working on it at the same time)
        // POST: Admin/RoleUsers?role=
        [Authorize(Roles="AppAdmin")]
        [HttpPost]
        public async Task<ActionResult> UsersInRole(String role, FormCollection Form)
        {
            var store = new UserStore<ApplicationUser>(identityDb);
            var manager = new UserManager<ApplicationUser>(store);

            string userNameInput = Form["UserNames"].ToString();
            string[] userNames = userNameInput.Split(',');
            foreach (string userName in userNames)
            {
                bool isChecked = Convert.ToBoolean(Form[userName].Split(',')[0]);
                if (isChecked)
                {
                    if (!await manager.IsInRoleAsync(manager.Users.FirstOrDefault(u => u.UserName == userName).Id, role))
                    {
                        await manager.AddToRoleAsync(manager.Users.FirstOrDefault(u => u.UserName == userName).Id, role);
                    }
                }
                else
                {
                    if (await manager.IsInRoleAsync(manager.Users.FirstOrDefault(u => u.UserName == userName).Id, role))
                    {
                        await manager.RemoveFromRoleAsync(manager.Users.FirstOrDefault(u => u.UserName == userName).Id, role);
                    }
                }
            }
            return RedirectToAction("Index");
        }
	}
}
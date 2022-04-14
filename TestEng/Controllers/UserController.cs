using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestEng.Data.Models;
using TestEng.AppServices.Services;

namespace TestEng.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var users = _userService.GetUsers();
            return View(users);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Name,BirthDate,IsActive")] UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                await _userService.AddUser(userModel);
                return RedirectToAction(nameof(Index));
            }
            return View(userModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userModel = await _userService.GetUserById(id);
            if (userModel == null)
            {
                return NotFound();
            }
            return View(userModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Name,BirthDate,IsActive")] UserModel userModel)
        {
            if (id != userModel.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _userService.EditUser(userModel);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_userService.UserExists(userModel.UserId).Result)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(userModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userModel = await _userService.GetUserById(id);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userModel = await _userService.GetUserById(id);
            await _userService.DeleteUser(userModel.UserId);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Filter()
        {
            var users = await _userService.GetActiveUsers();
            return View(nameof(Index), users);
        }
    }
}

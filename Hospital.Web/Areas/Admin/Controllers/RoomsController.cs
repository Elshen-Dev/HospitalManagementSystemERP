using Hospital.Services;
using Hospital.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoomsController : Controller
    {
        private IRoomService _room;

        public RoomsController(IRoomService room)
        {
            _room = room;
        }

        #region ContactIndex
        public IActionResult Index(int pageNumber = 1, int pageSize = 5)
        {
            return View(_room.GetAll(pageNumber, pageSize));
        }
        #endregion

        #region ContactUpdate
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var viewModel=_room.GetRoomById(id);
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Edit(RoomViewModel vm)
        {
           _room.UpdateRoom(vm);
            return RedirectToAction("Index");
        }
        #endregion

        #region ContactCreate
        [HttpGet]
        public IActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult Create(RoomViewModel vm)
        {
            _room.InsertRoom(vm);
            return RedirectToAction("Index");
        }
        #endregion

        #region ContactDelete
        public IActionResult Delete(int id)
        {
            _room.DeleteRoom(id);
            return RedirectToAction("Index");
        }
        #endregion

    }
}

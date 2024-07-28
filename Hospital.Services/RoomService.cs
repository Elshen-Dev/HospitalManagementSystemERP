using cloudscribe.Pagination.Models;
using Hospital.Models;
using Hospital.Repositories.Interfaces;
using Hospital.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Services
{
    public class RoomService : IRoomService
    {
        private IUnitOfWork _unitOfWork;

        public RoomService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region DeleteRoom
        public void DeleteRoom(int id)
        {
            var model = _unitOfWork.GenericRepository<Room>().GetById(id);
            _unitOfWork.GenericRepository<Room>().Delete(model);
            _unitOfWork.Save();
        }
        #endregion

        #region GetAllRoom
        public PagedResult<RoomViewModel> GetAll(int pageNumber, int pageSize)
        {
            var va = new RoomViewModel();
            int totaCount;
            List<RoomViewModel> vmList = new List<RoomViewModel>();
            try
            {
                int ExcludeRecords = (pageSize * pageNumber) - pageSize;
                var modelList = _unitOfWork.GenericRepository<Room>().GetAll()
                    .Skip(ExcludeRecords).Take(pageSize).ToList();

                totaCount = _unitOfWork.GenericRepository<Room>().GetAll().Count();

                vmList = ConvertModelToViewModelList(modelList);

            }
            catch (Exception)
            {

                throw;
            }
            var result = new PagedResult<RoomViewModel>()
            {
                Data = vmList,
                TotalItems = vmList.Count,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return result;


        }
        #endregion

        #region GetRoomById
        public RoomViewModel GetRoomById(int RoomId)
        {
            var model = _unitOfWork.GenericRepository<Room>().GetById(RoomId);
            var vm=new RoomViewModel(model);
            return vm;
        }
        #endregion

        #region CreateRoom
        public void InsertRoom(RoomViewModel Room)
        {
            var model=new RoomViewModel().ConvertViewModel(Room);
            _unitOfWork.GenericRepository<Room>().Add(model);
            _unitOfWork.Save();
        }
        #endregion

        #region UpdateRoom
        public void UpdateRoom(RoomViewModel Room)
        {
           var model=new RoomViewModel().ConvertViewModel(Room);
            var ModelById = _unitOfWork.GenericRepository<Room>().GetById(model.Id);
            ModelById.Type=Room.Type;
            ModelById.RoomNumber = Room.RoomNumber;
            ModelById.Status=Room.Status;
            ModelById.HospitalId = Room.HospitalInfoId;

            _unitOfWork.GenericRepository<Room>().Update(ModelById);
            _unitOfWork.Save();
        }
        #endregion

        #region ConvertModelToViewModelList
        private List<RoomViewModel> ConvertModelToViewModelList(List<Room> modelList)
        {
            return modelList.Select(x => new RoomViewModel(x)).ToList();
        }
        #endregion
    }
}

using Hospital.Models;
using Hospital.Repositories.Interfaces;
using Hospital.ViewModels;
using hospitals.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Services
{
    public class ContactService : IContactService
    {
        private IUnitOfWork _unitOfWork;

        public ContactService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region DeleteContact
        public void DeleteContact(int id)
        {
            var model = _unitOfWork.GenericRepository<Contact>().GetById(id);
            _unitOfWork.GenericRepository<Contact>().Delete(model);
            _unitOfWork.Save();
        }
        #endregion

        #region GetAllContact
        public PagedResult<ContactViewModel> GetAll(int pageNumber, int pageSize)
        {
            var va = new ContactViewModel();
            int totaCount;
            List<ContactViewModel> vmList = new List<ContactViewModel>();
            try
            {
                int ExcludeRecords = (pageSize * pageNumber) - pageSize;
                var modelList = _unitOfWork.GenericRepository<Contact>().GetAll()
                    .Skip(ExcludeRecords).Take(pageSize).ToList();

                totaCount = _unitOfWork.GenericRepository<Contact>().GetAll().Count();

                vmList = ConvertModelToViewModelList(modelList);

            }
            catch (Exception)
            {

                throw;
            }
            var result = new PagedResult<ContactViewModel>()
            {
                Data = vmList,
                TotalItems = vmList.Count,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return result;
        }
        #endregion

        #region GetContactById
        public ContactViewModel GetContactById(int ContactId)
        {
            var model = _unitOfWork.GenericRepository<Contact>().GetById(ContactId);
            var vm = new ContactViewModel(model);
            return vm;
        }
        #endregion

        #region CreateContact
        public void InsertContact(ContactViewModel contact)
        {
            var model = new ContactViewModel().ConvertViewModel(contact);
            _unitOfWork.GenericRepository<Contact>().Add(model);
            _unitOfWork.Save();
        }
        #endregion

        #region UpdateContact
        public void UpdateContact(ContactViewModel contact)
        {
            var model=new ContactViewModel().ConvertViewModel(contact);
            var ModelById = _unitOfWork.GenericRepository<Contact>().GetById(model.Id);
            ModelById.Phone = contact.Phone;
            ModelById.Email = contact.Email;
            model.HospitalId = contact.HospitalInfoId;

            _unitOfWork.GenericRepository<Contact>().Update(ModelById);
            _unitOfWork.Save();
        }
        #endregion

        #region ConvertModelToViewModelList
        private List<ContactViewModel> ConvertModelToViewModelList(List<Contact> modelList)
        {
            return modelList.Select(x => new ContactViewModel(x)).ToList();
        }
        #endregion

    }
}

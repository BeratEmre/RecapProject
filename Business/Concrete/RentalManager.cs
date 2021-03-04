using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.CacheAspect;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class RentalManager : IRentalService
    {
        IRentalDal _rentalDal;
        public RentalManager(IRentalDal rentalDal)
        {
            _rentalDal = rentalDal;
        }

        [SecuredOperation("rental.add,admin")]
        [ValidationAspect(typeof(RentalValidator))]
        public IResult Add(Rental rental)
        {
            var result = CheckReturnDate(rental.CarId);
            if (!result.Success)
            {
                return new ErrorResult(result.Message);
            }
            rental.RentDate = DateTime.Now;
            _rentalDal.Add(rental);
            return new SuccessResult(result.Message);
        }

        public IResult CheckReturnDate(int carId)
        {
            var result = _rentalDal.GetRentalDetails();
            if (result.Count > 0 && result.Count(x => x.ReturnDate == null) > 0)
            {
                return new ErrorResult(Messages.RentalAddedError);
            }
            return new SuccessResult(Messages.RentalAdded);
        }

        [CacheAspect]
        [PerformanceAspect(1)]       
        public IDataResult<List<Rental>> GetAll()
        {
            System.Threading.Thread.Sleep(1000);
            var result=_rentalDal.GetAll();
            return new SuccessDataResult<List<Rental>>(result,Messages.RentalGetall);
        }
        [CacheAspect]
        public IDataResult<Rental> GetRentalDetail(int id)
        {
            var result = _rentalDal.Get(r=>r.Id==id);
            return new SuccessDataResult<Rental>(result, Messages.RentalGetall);
        }

        public IDataResult<List<RentalDetailDto>> GetRentalDetailsDto()
        {
            var result=_rentalDal.GetRentalDetails();
            return new SuccessDataResult<List<RentalDetailDto>>(result);
        }

        public IResult UpdateReturnDate(int Id)
        {
            var result = _rentalDal.GetAll(x => x.CarId == Id);
            var updatedRental = result.LastOrDefault();
            if (updatedRental.ReturnDate != null)
            {
                return new ErrorResult();
            }
            updatedRental.ReturnDate = DateTime.Now;
            _rentalDal.Update(updatedRental);
            return new SuccessResult();
        }
    }
}

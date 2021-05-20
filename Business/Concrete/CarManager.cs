using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class CarManager : ICarService
    {
        ICarDal _cardal;
        public CarManager(ICarDal cardal)
        {
            _cardal = cardal;
        }
        [ValidationAspect(typeof(CarValidator))]
        //[SecuredOperation("admin")]
        public IResult Add(Car car)
        {            
                _cardal.Add(car);
                return new SuccessResult(Messages.Added);
        }

        //[SecuredOperation("rental.add,admin")]
        public IResult Delete(Car car)
        {
            _cardal.Delete(car);
            return new SuccessResult(Messages.Deleted);
        }
        [PerformanceAspect(1)]
        public IDataResult<List<Car>> GetAll()
        {
            var result=_cardal.GetAll();
            return new SuccessDataResult<List<Car>>(result,Messages.GetAllCars);
        }

        public IDataResult<Car> GetById(int Id)
        {
            return new SuccessDataResult<Car>(_cardal.Get(c => c.CarId == Id),Messages.CarGet);
        }

        public IDataResult<CarDetailDto> GetCarDetail(int carId)
        {
            var result=_cardal.GetCarDetail(carId);
            return new SuccessDataResult<CarDetailDto>(result,Messages.CarDetail);
        }

        public IDataResult<List<CarDetailDto>> GetCarDetails()
        {
            return new SuccessDataResult<List<CarDetailDto>>(_cardal.GetCarDetails());
        }

        public IDataResult<List<Car>> GetCarsByBrandId(int brandId)
        {
            return new SuccessDataResult<List<Car>>(_cardal.GetAll(p => p.BrandId == brandId));
        }

        public IDataResult<List<Car>> GetCarsByColorId(int colorId)
        {
            return new SuccessDataResult<List<Car>>(_cardal.GetAll(p => p.ColorId == colorId));
        }

        public IResult Update(Car car)
        {
            _cardal.Update(car);
            return new SuccessResult(Messages.Updated);
        }
    }
}

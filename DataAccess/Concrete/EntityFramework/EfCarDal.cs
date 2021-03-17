using Core.DataAccess;
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCarDal : EntityRepositoryBase<Car, CarRentalContexts>, ICarDal
    {

        public CarDetailDto GetCarDetail(int carId)
        {
            using (CarRentalContexts context = new CarRentalContexts())
            {
                var result = from c in context.Cars.Where(c=>c.CarId==carId)
                             join b in context.Brands
                             on c.BrandId equals b.Id
                             join co in context.Colors
                             on c.ColorId equals co.Id
                             select new CarDetailDto
                             {
                                 CarId = c.CarId,
                                 BrandName = b.Name,
                                 ColorName = co.Name,
                                 DailyPrice = c.DailyPrice,
                                 ModelYear = c.ModelYear,
                                 Description = c.Description
                             };
                return result.SingleOrDefault();
            }
        }


        public List<CarDetailDto> GetCarDetails(Expression<Func<Car, bool>> filter = null)
        {
            using (CarRentalContexts context=new CarRentalContexts())
            {
                var result = from c in context.Cars
                             join b in context.Brands
                             on c.BrandId equals b.Id
                             join co in context.Colors
                             on c.ColorId equals co.Id
                             select new CarDetailDto { CarId = c.CarId, BrandName = b.Name,ColorName=co.Name, 
                                 DailyPrice = c.DailyPrice,ModelYear=c.ModelYear,
                                 Description=c.Description 
                             };
                return result.ToList();
            }
        }     
    }
}

using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfRentalDal : EntityRepositoryBase<Rental, CarRentalContexts>, IRentalDal
    {
        public List<RentalDetailDto> GetRentalDetails(Expression<Func<Rental, bool>> filter = null)
        {
            using (CarRentalContexts context=new CarRentalContexts())
            {
                var result = from re in filter is null ? context.Rentals : context.Rentals.Where(filter)
                             join cu in context.Customers
                             on re.CustomerId equals cu.Id
                             join ca in context.Cars
                             on re.CarId equals ca.CarId
                             join u in context.Users
                             on re.Id equals u.Id
                             select new RentalDetailDto
                             {
                                 CarId = ca.CarId,
                                 Id=u.Id,
                                 CustomerId=cu.Id,
                                 CarName=ca.Description,
                                 CustomerName = cu.CompanyName,
                                 RentDate = re.RentDate,
                                 ReturnDate = re.ReturnDate,
                                 UserName = u.FirstName + " " + u.LastName
                             };
                return result.ToList();
            }
        }
    }
}

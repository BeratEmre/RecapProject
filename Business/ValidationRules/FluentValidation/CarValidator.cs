using Business.Constants;
using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation
{
    public class CarValidator : AbstractValidator<Car>
    {
        public CarValidator()
        {
            RuleFor(c => c.Description).NotEmpty();
            RuleFor(c => c.Description.Length).GreaterThan(2);
            RuleFor(c => c.DailyPrice).GreaterThan(100).WithMessage("Günlük araba kirası  100 den büyük olmalı");
        }
    }
}

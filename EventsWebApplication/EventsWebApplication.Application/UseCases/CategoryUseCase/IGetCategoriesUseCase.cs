﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.GeneralUseCases.Interface;

namespace EventsWebApplication.Application.UseCases.CategoryUseCase
{
    public interface IGetCategoriesUseCase : IGetManyUseCase<CategoryDto>
    {
    }
}

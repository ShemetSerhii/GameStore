using GameStore.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace GameStore.BLL.FilterPipeline.Abstract
{
    public interface IFilter<T>
    {
        Expression<Func<Game, bool>> Execute();
    }
}
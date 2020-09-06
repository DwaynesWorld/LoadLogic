﻿using System;
using System.Linq.Expressions;
using LoadLogic.Services.Abstractions;

namespace LoadLogic.Services
{
    public abstract class SpecificationBase<T> : ISpecification<T>
    {
        private Func<T, bool>? _compiledExpression;

        private Func<T, bool> CompiledExpression
        {
            get { return _compiledExpression ??= SpecExpression.Compile(); }
        }

        public abstract Expression<Func<T, bool>> SpecExpression { get; }

        public bool IsSatisfiedBy(T obj)
        {
            return CompiledExpression(obj);
        }
    }
}

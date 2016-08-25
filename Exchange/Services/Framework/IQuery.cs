using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kadevjo.Core.Dependencies
{
    public interface IQuery
    {
        Dictionary<string, object> Parameters { get; }
        Dictionary<string, object> FormattedParameters { get; }

        IQuery Add(IQuery query);
        IQuery Add(string property, object value);
        IQuery Equal(string property, object value);
        IQuery NotEqual(string property, object value);
        IQuery GreaterThan(string property, object value);
        IQuery GreaterThanOrEqual(string property, object value);
        IQuery LowerThan(string property, object value);
        IQuery LowerThanOrEqual(string property, object value);
        IQuery ContainedIn(string property, object value);
        IQuery NotContainedIn(string property, object value);
        IQuery Skip(int value);
        IQuery Limit(int value);
        IQuery OrderBy(string property);
        IQuery OrderByDescending(string property);
    }
}
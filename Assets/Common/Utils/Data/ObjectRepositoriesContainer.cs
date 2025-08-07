using System;
using System.Collections.Generic;
using System.Linq;

namespace CubeTower.Common.Data
{
    [Serializable]
    public class ObjectRepositoriesContainer
    {
        public readonly List<ObjectRepository> ObjectRepositories;

        public ObjectRepositoriesContainer(List<ObjectRepository> objectRepositories)
        {
            ObjectRepositories = objectRepositories;
        }

        public override bool Equals(object obj)
        {
            if (obj is not ObjectRepositoriesContainer other)
            {
                return false;
            }
            
            return ObjectRepositories?.SequenceEqual(other.ObjectRepositories) ?? other.ObjectRepositories == null;
        }

        public override int GetHashCode()
        {
            return ObjectRepositories.Aggregate(17, (current, repo) => current * 31 + (repo?.GetHashCode() ?? 0));
        }
    }
}
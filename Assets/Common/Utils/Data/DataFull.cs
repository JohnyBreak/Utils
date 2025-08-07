using System.Collections.Generic;
using System.Linq;

namespace CubeTower.Common.Data
{
    public class DataFull
    {
        private Dictionary<string, ObjectRepository> _data = new();
        
        
        public void AddData(ObjectRepository data)
        {
            _data[data.ObjectType] = data;
        }
        
        public List<ObjectRepository> GetRepositories()
        {
            return _data.Values.ToList();
        }

        public ObjectRepositoriesContainer GetContainer()
        {
            var container = new ObjectRepositoriesContainer(GetRepositories());
            return container;
        }
    }
}
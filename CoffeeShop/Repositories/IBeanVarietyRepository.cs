using CoffeeShop.Models;

namespace CoffeeShop.Repositories
{
    public interface IBeanVarietyRepository
    {
        public List<BeanVariety> GetAll();
        public BeanVariety Get(int id);
        public void Add(BeanVariety variety);
        public void Update(BeanVariety variety);
        public void Delete(int id);
    }
}

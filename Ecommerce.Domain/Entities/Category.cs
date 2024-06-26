﻿namespace Ecommerce.Domain.Entities
{
    public class Category:BaseEntity<int>
    {
        public Category()
        {
            
        }
        public Category(int id,string name,string discription)
        {
            this.Id = id;
            this.Name = name;
            this.Description = discription;
        }
        public string Name { set; get; }
        public string Description { set; get; }

        public virtual ICollection<Product> Products { get;} = new HashSet<Product>();
    }
}

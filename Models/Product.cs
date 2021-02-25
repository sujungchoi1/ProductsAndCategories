using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsAndCategories.Models
{
    public class Product
    {
        [Key]
        public int ProductId {get;set;}

        [Required]
        public string Name {get;set;}

        [Required]
        public string Description {get;set;}

        [Required]
        [Range(0, 100000.00)]
        [DataType(DataType.Currency)]
        public decimal Price {get;set;}
        
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public List<Association> Categories {get;set;}


    }
}
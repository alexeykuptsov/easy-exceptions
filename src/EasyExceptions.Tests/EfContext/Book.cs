using System;
using System.ComponentModel.DataAnnotations;

namespace EasyExceptions.Tests.EfContext
{
    public class Book
    {
        public Guid Id { get; set; } 
        public string Name { get; set; }
        [Required]
        public Author Author { get; set; }
    }
}
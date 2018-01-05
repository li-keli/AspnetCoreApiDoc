using System;
using Nest;

namespace AspnetCoreDocTest.Logger
{
    public class Product
    {
        public Guid Id { get; set; }

        [Text(Name="name")]
        public string Name { get; set; }

        [Text(Name = "description")]
        public string Description { get; set; }

        [Keyword(Name = "tag")]
        public string[] Tags { get; set; }        
    }
}

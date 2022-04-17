﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog_rev02.Models {

    public class Tag {

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public IList<Post> Posts { get; set; }
    }
}

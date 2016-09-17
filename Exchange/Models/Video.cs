using System;
using Exchange.Interfaces;
using Kadevjo.Core.Models;
using System.Collections.Generic;

namespace Exchange.Models
{
    public class Video : Model, IModel
    {
        public string ObjectId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public User User { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public string Url { get; set; }

        public Dictionary<string, User> Likes { get; set; }
    }
}


using System;
using System.Collections.Generic;
using System.Numerics;

namespace SocialNetwork.Dal.Models
{
    public class Attachment
    {
        public int Id { get; set; }
        public string ContentType { get; set; }
        public string Path { get; set; }
        public string Preview { get; set; }
        public DateTime CreateDateTime { get; set; }
        public Guid Hash { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string DisplayName { get; set; }
    }
}

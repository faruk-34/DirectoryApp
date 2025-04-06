using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryApi.Application.Dtos
{
    public class ContactInfoDto
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public int PersonCount { get; set; }
        public int DirectoryId { get; set; }
    }
}

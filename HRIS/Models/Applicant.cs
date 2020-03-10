using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;



namespace HRIS.Models
{
    public class Applicant
    {
        public Guid Id { get; set; }
        public string NIK { get; set; }
        public string FullName { get; set; }
        public DateTime DOB { get; set; }
        public string Religion { get; set; }
        public DateTime JoinDate { get; set; }
        public string Phone { get; set; }
        [JsonIgnore]
        public string _role { get; set; }
        public string Photo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime EditedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public int DataStatus { get; set; }

        [JsonIgnore]
        public string _address { get; set; }

        [NotMapped]
        public Address Address
        {
            get { return _address == null ? null : JsonConvert.DeserializeObject<Address>(_address); }
            set { _address = JsonConvert.SerializeObject(value); }
        }

        [NotMapped]
        public Role Role
        {
            get { return _role == null ? null : JsonConvert.DeserializeObject<Role>(_role); }
            set { _role = JsonConvert.SerializeObject(value); }
        }
    }
}

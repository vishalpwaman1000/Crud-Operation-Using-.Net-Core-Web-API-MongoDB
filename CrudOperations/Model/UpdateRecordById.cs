using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CrudOperations.Model
{
    public class UpdateSalaryByIdRequest
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public int Salary { get; set; }
    }

    public class UpdateRecordByIdResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}

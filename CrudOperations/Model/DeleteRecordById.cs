using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CrudOperations.Model
{
    public class DeleteRecordByIdRequest
    {
        [Required]
        public string Id { get; set; }
    }

    public class DeleteRecordByIdResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}

using CrudOperations.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudOperations.DataAccessLayer
{
    public interface ICrudOperationDL
    {
        public Task<InsertRecordResponse> InsertRecord(InsertRecordRequest request);

        public Task<GetAllRecordResponse> GetAllRecord();

        public Task<GetRecordByIdResponse> GetRecordById(string ID);

        public Task<GetRecordByNameResponse> GetRecordByName(string Name);

        public Task<UpdateRecordByIdResponse> UpdateRecordById(InsertRecordRequest request);

        public Task<UpdateRecordByIdResponse> UpdateSalaryById(UpdateSalaryByIdRequest request);

        public Task<DeleteRecordByIdResponse> DeleteRecordById(DeleteRecordByIdRequest request);

        public Task<DeleteAllRecordResponse> DeleteAllRecord();
    }
}

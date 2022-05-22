using CrudOperations.Model;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudOperations.DataAccessLayer
{
    public class CrudOperationDL : ICrudOperationDL
    {
        private readonly IConfiguration _configuration;
        private readonly MongoClient _mongoConnection;
        private readonly IMongoCollection<InsertRecordRequest> _booksCollection;
        public CrudOperationDL(IConfiguration configuration)
        {
            _configuration = configuration;
            _mongoConnection = new MongoClient(_configuration["BookStoreDatabase:ConnectionString"]);
            var MongoDataBase = _mongoConnection.GetDatabase(_configuration["BookStoreDatabase:DatabaseName"]);
            _booksCollection = MongoDataBase.GetCollection<InsertRecordRequest>(_configuration["BookStoreDatabase:BooksCollectionName"]);
        }

        public async Task<InsertRecordResponse> InsertRecord(InsertRecordRequest request)
        {
            InsertRecordResponse response = new InsertRecordResponse();
            response.IsSuccess = true;
            response.Message = "Data Successfully Insert";

            try
            {
                request.CreatedDate = DateTime.Now.ToString(); // Insert Current Time
                request.UpdatedDate = string.Empty;
                await _booksCollection.InsertOneAsync(request);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : " + ex.Message;
            }

            return response;
        }

        public async Task<GetAllRecordResponse> GetAllRecord()
        {
            GetAllRecordResponse response = new GetAllRecordResponse();
            response.IsSuccess = true;
            response.Message = "Data Fetch Successfully";

            try
            {
                response.data = new List<InsertRecordRequest>();
                response.data = await _booksCollection.Find(x => true).ToListAsync();
                if (response.data.Count == 0)
                {
                    response.Message = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : " + ex.Message;
            }

            return response;
        }

        public async Task<GetRecordByIdResponse> GetRecordById(string ID)
        {
            GetRecordByIdResponse response = new GetRecordByIdResponse();
            response.IsSuccess = true;
            response.Message = "Fetch Data Successfully by ID";

            try
            {
                response.data = await _booksCollection.Find(x => (x.Id == ID)).FirstOrDefaultAsync();
                if(response.data == null)
                {
                    response.Message = "No Data Found";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : " + ex.Message;
            }

            return response;
        }

        public async Task<GetRecordByNameResponse> GetRecordByName(string Name)
        {
            GetRecordByNameResponse response = new GetRecordByNameResponse();
            response.IsSuccess = true;
            response.Message = "Fetch data Successfully By Name";

            try
            {
                response.data = new List<InsertRecordRequest>();
                response.data = await _booksCollection.Find(x => (x.FirstName == Name) || (x.LastName == Name)).ToListAsync();
                if (response.data.Count==0)
                {
                    response.Message = "No Data Found";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : " + ex.Message;
            }

            return response;
        }

        public async Task<UpdateRecordByIdResponse> UpdateRecordById(InsertRecordRequest request)
        {
            UpdateRecordByIdResponse response = new UpdateRecordByIdResponse();
            response.IsSuccess = true;
            response.Message = "Update Record Successfully By Id";

            try
            {
                GetRecordByIdResponse GetResponse = await GetRecordById(request.Id);
                if (!GetResponse.IsSuccess)
                {
                    response.IsSuccess = false;
                    response.Message = GetResponse.Message;
                    return response;
                }

                request.CreatedDate = GetResponse.data.CreatedDate;
                request.UpdatedDate = DateTime.Now.ToString();

                var Result = await _booksCollection.ReplaceOneAsync(x => x.Id == request.Id, request);
                if (!Result.IsAcknowledged)
                {
                    response.Message = "Input Id Not Found / Updation Not Occurs";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : " + ex.Message;
            }

            return response;
        }

        public async Task<UpdateRecordByIdResponse> UpdateSalaryById(UpdateSalaryByIdRequest request)
        {
            UpdateRecordByIdResponse response = new UpdateRecordByIdResponse();
            response.IsSuccess = true;
            response.Message = "Update Salary Successfully";

            try
            {
               /* var filter = new BsonDocument()
                    .Add("_id", request.Id);*/
                var updateDoc = new BsonDocument("$set",
                    new BsonDocument("Salary", request.Salary));

                var Result = await _booksCollection.UpdateOneAsync(x=>x.Id==request.Id, updateDoc);

            }catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : " + ex.Message;
            }

            return response;
        }

        public async Task<DeleteRecordByIdResponse> DeleteRecordById(DeleteRecordByIdRequest request)
        {
            DeleteRecordByIdResponse response = new DeleteRecordByIdResponse();
            response.IsSuccess = true;
            response.Message = "Delete Record Successfully By Id";

            try
            {

                var result = await _booksCollection.DeleteOneAsync(x => x.Id == request.Id);
                if (!result.IsAcknowledged)
                {
                    response.IsSuccess = true;
                    response.Message = "Record Not Found In Database For Deletion, Please Enter Valid Id";
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : " + ex.Message;
            }

            return response;
        }

        public async Task<DeleteAllRecordResponse> DeleteAllRecord()
        {
            DeleteAllRecordResponse response = new DeleteAllRecordResponse();
            response.IsSuccess = true;
            response.Message = "Clear Database Successfully";

            try
            {
                var Result = await _booksCollection.DeleteManyAsync(x => true);
                if (!Result.IsAcknowledged)
                {
                    response.IsSuccess = false;
                    response.Message = "Something Went Wrong.";
                }
                else
                {
                    response.Message = Result.DeletedCount == 0 ? "Database Already Cleaned." : response.Message;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : " + ex.Message;
            }

            return response;
        }

        
    }
}

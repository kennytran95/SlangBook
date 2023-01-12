using Microsoft.AspNetCore.Mvc;
using System.Data;
using Npgsql;
using SlangBook.Models;

namespace SlangBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordController : Controller
    {
        private readonly IConfiguration _configuration;

        public WordController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT * FROM words";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WordsAppCon");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader(); 
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close(); 
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Word word)
        {
            string query = @"INSERT INTO words(name, slang) 
                            VALUES (@WordName, @WordSlang)
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WordsAppCon");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@WordName", word.Name);
                    myCommand.Parameters.AddWithValue("@WordSlang", word.Slang);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Successfully added to words database!");
        }


        [HttpPut]
        public JsonResult Put(Word word)
        {
            string query = @"UPDATE words 
                             SET name = @WordName 
                             WHERE id = @WordId
                             ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WordsAppCon");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@WordId", word.Id);
                    myCommand.Parameters.AddWithValue("@WordName", word.Name);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Successfully edited word in the database!");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"DELETE FROM words WHERE id = @WordId";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WordsAppCon");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@WordId", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Successfully deleted word in the database!");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Data;
using Npgsql;
using SlangBook.Models;
using Microsoft.Extensions.Logging;

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
            try
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
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        [HttpPost]
        public JsonResult Post(Word word)
        {
            try
            {
                string query = @"INSERT INTO words(name, definition, slang) 
                                VALUES (@WordName, @WordDefinition, @WordSlang)
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
                        myCommand.Parameters.AddWithValue("@WordDefinition", word.Definition);
                        myCommand.Parameters.AddWithValue("@WordSlang", word.Slang);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }
                return new JsonResult("Successfully added to words database!");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }


        [HttpPut]
        public JsonResult Put(Word word)
        {
            try
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
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            try
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
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using CoffeeShop.Models;

namespace CoffeeShop.Repositories
{
    public class CoffeeRepository : ICoffeeRepository
    {
        private readonly string _connectionString;
        public CoffeeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        private SqlConnection Connection
        {
            get { return new SqlConnection(_connectionString); }
        }
        public List<Coffee> GetAll()
        {
            using (var conn = Connection) 
            {
                conn.Open();

                using (var cmd = conn.CreateCommand()) 
                {
                    cmd.CommandText = @"
                        SELECT c.Id, c.Title, c.BeanVarietyId, b.[Name] AS 'Bean_Variety_Name' 
                        FROM Coffee c 
                        JOIN BeanVariety b ON c.BeanVarietyId = b.id";
                    var reader = cmd.ExecuteReader();
                    var coffeeList = new List<Coffee>();
                    while (reader.Read())
                    {
                        var coffee = new Coffee()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            BeanVarietyId = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                            BeanVariety = new BeanVariety()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Bean_Variety_Name")),
                            }
                        };
                        coffeeList.Add(coffee);
                    }              
                    reader.Close();
                    return coffeeList;
                }
            }
        }
    
        public Coffee Get(int id)
        {
            using (var conn = Connection) 
            {
                conn.Open();
                using ( var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT c.Id, c.Title, c.BeanVarietyId, b.[Name] AS 'Bean_Variety_Name'
                        FROM Coffee c
                        JOIN BeanVariety b ON c.BeanVarietyId = b.Id
                        WHERE c.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    var reader = cmd.ExecuteReader();

                    Coffee coffee = null;
                    if (reader.Read())
                    {
                        coffee = new Coffee()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            BeanVarietyId = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                            BeanVariety = new BeanVariety()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Bean_Variety_Name")),
                            }
                        };
                        reader.Close();
                        return coffee;
                    }
                    else
                    {
                        reader.Close() ;
                        return null;
                    }
                }
            }
        }
        public void Add(Coffee coffee)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Coffee (Title, BeanVarietyId)
                        OUTPUT INSERTED.ID
                        VALUES (@tile, @beanVariety)";
                    cmd.Parameters.AddWithValue("@title", coffee.Title);
                    cmd.Parameters.AddWithValue("@beanVariety", coffee.BeanVariety);

                    coffee.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
        public void Update(Coffee coffee) 
        {
            using (var conn = Connection) 
            {
                conn.Open();
                using (var cmd = conn.CreateCommand()) 
                {
                    cmd.CommandText = @"
                        UPDATE Coffee
                        SET Title = @title,
                            BeanVarietyId = @beanVarietyId
                        WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", coffee.Id);
                    cmd.Parameters.AddWithValue("@beanVarietyId", coffee.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Delete(int id) 
        {
            using (var conn = Connection) 
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Coffee Where Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
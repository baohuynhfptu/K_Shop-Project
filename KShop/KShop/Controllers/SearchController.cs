﻿
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections;
using KShop.Models;

namespace KShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        SqlConnection connection = null;
        private readonly IConfiguration configuration;
        public SearchController(IConfiguration config)
        {
            this.configuration = config;
            connection = new SqlConnection(config.GetConnectionString("DefaultConnectionStrings"));
        }

        /// <summary>
        /// Search product by name, priceMin, priceMax, categoryId
        /// </summary>
        /// <param name="search"></param>
        /// ex: ["b","1.2","12.3", ""]
        /// <returns>List<Product></returns>

        [HttpGet]
        public ActionResult<List<Product>> Search(string name, string max, string min, string cate)
        {
            List<Product> listProduct = null;
            string txtConnection = configuration.GetConnectionString("DefaultConnectionStrings");
            SqlConnection connection = new SqlConnection(txtConnection);
            try
            {
                connection.Open();
                #region
                // string sql = "SELECT * FROM Product WHERE ProductName LIKE '%" + search[0] + "%' AND " + search[1] + " <= Price AND Price <= " + search[2] + " AND CategoryId LIKE '%" + search[3] + "%'" ;
                string sql = "SELECT * FROM Product WHERE ProductName LIKE '%" + name + "%' AND " + min + " <= Price AND Price <= " + max + " AND CategoryId LIKE '%" + cate + "%'";
                SqlCommand command = new SqlCommand(sql, connection);

                SqlDataReader dr = command.ExecuteReader();
                #endregion

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        int productId = int.Parse(dr["ProductId"].ToString() + "");
                        int categoryId = int.Parse(dr["CategoryId"].ToString() + "");
                        int quantity = int.Parse(dr["Quantity"].ToString() + "");
                        float price = float.Parse(dr["Price"].ToString() + "");
                        DateTime createdDate = DateTime.Parse(dr["CreatedDate"].ToString() + "");
                        string productName = dr["ProductName"].ToString() + "";
                        string image = dr["Image"].ToString() + "";
                        if (listProduct == null)
                        {
                            listProduct = new List<Product>();
                        }
                        listProduct.Add(new Product(productId, productName, image, quantity, price, categoryId, createdDate, true));
                    }
                }
                connection.Close();
            }
            catch
            {
            }
            return listProduct;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using DamianGonzalezCSharp.Models;
using DamianGonzalezCSharp;
using Microsoft.AspNetCore.Mvc;
using DamianGonzalesCSharp.Models;
using DamianGonzalesCSharp.Handlers;
using System.Net;
using System.Runtime.Intrinsics.Arm;

namespace DamianGonzalezCSharp.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        [HttpGet("GetProducts")]
        public ProductResponse GetProducts(string? descrip, string? order)
        {
            List<Product> products = new List<Product>();
            DataSet dsProducts = new DataSet();
            ProductHandler productHandler = new ProductHandler();
            ProductResponse response = new ProductResponse();

            if (productHandler.HandleGetProducts(dsProducts, 0, descrip, order))
            {
                if (dsProducts.Tables.Contains("DAT") && dsProducts.Tables["DAT"].Rows.Count > 0)
                {
                    DataTable dtDAT = dsProducts.Tables["DAT"];
                    response.Success = true;
                    response.Message = "Successfull";
                    response.StatusCode = HttpStatusCode.OK;

                    foreach (DataRow dr in dtDAT.Rows)
                    {
                        Product product = new Product();

                        product.Id = Convert.ToInt32(dr["id"]);
                        if (dr["descripciones"] != DBNull.Value) product.Description = Convert.ToString(dr["Descripciones"]).Trim();
                        if (dr["costo"] != DBNull.Value) product.Cost = Convert.ToDecimal(dr["costo"]);
                        if (dr["precioventa"] != DBNull.Value) product.SellingPrice = Convert.ToDecimal(dr["precioventa"]);
                        if (dr["stock"] != DBNull.Value) product.Stock = Convert.ToInt32(dr["stock"]);
                        if (dr["idusuario"] != DBNull.Value) product.UserId = Convert.ToInt32(dr["idusuario"]);

                        products.Add(product);

                    }

                    response.Products = products;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Can't obtain requested data";
                    response.StatusCode = HttpStatusCode.Conflict;
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Server not accessible";
                response.StatusCode = HttpStatusCode.InternalServerError;
            }


            return response;
        }

        [HttpGet("GetProduct")]
        public ProductResponse GetProduct(Int32 productId)
        {
            Product product = new Product();
            List < Product > productList = new List<Product>();
            DataSet dsProduct = new DataSet();
            ProductHandler productHandler = new ProductHandler();
            ProductResponse response = new ProductResponse();

            if (productHandler.HandleGetProducts(dsProduct, productId, "", ""))
            {
                if (dsProduct.Tables.Contains("DAT") && dsProduct.Tables["DAT"].Rows.Count > 0)
                {
                    response.Success = true;
                    response.Message = "Successfull";
                    response.StatusCode = HttpStatusCode.OK;
                    DataRow dr = dsProduct.Tables["DAT"].Rows[0];
                    product.Id = Convert.ToInt32(dr["id"]);
                    if (dr["descripciones"] != DBNull.Value) product.Description = Convert.ToString(dr["Descripciones"]).Trim();
                    if (dr["costo"] != DBNull.Value) product.Cost = Convert.ToDecimal(dr["costo"]);
                    if (dr["precioventa"] != DBNull.Value) product.SellingPrice = Convert.ToDecimal(dr["precioventa"]);
                    if (dr["stock"] != DBNull.Value) product.Stock = Convert.ToInt32(dr["stock"]);
                    if (dr["idusuario"] != DBNull.Value) product.UserId = Convert.ToInt32(dr["idusuario"]);
                    productList.Add(product);
                    response.Products = productList;

                } else
                {
                    response.Success = false;
                    response.Message = "Can't obtain requested data";
                    response.StatusCode = HttpStatusCode.Conflict;
                }

            } else
            {
                response.Success = false;
                response.Message = "Server not accessible";
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        [HttpPost("CreateProduct")]
        public ProductResponse CreateProduct(Product data)
        {
            ProductResponse response = new ProductResponse();
            ProductHandler productHandler = new ProductHandler();

            if(productHandler.HandleProductData(data, true))
            {
                response.Success = true;
                response.Message = "Product created successfully";
                response.StatusCode = HttpStatusCode.OK;
            } else
            {
                response.Message = "An error has ocurred when inserting the product";
                response.StatusCode = HttpStatusCode.InternalServerError;
            }
            
            return response;
        }

        [HttpPut("EditProduct")]

        public ProductResponse ModifyProduct(Product data)
        {
            ProductResponse response = new ProductResponse();
            ProductHandler productHandler = new ProductHandler();

            if (data.Id != 0 && productHandler.HandleProductData(data, false))
            {
                response.Success = true;
                response.Message = "Product modified successfully";
                response.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                response.Message = "An error has ocurred when editing the product";
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        [HttpDelete("DeleteProduct")]

        public ProductResponse DeleteProduct(Int32 id)
        {
            ProductResponse response = new ProductResponse();
            ProductHandler productHandler = new ProductHandler();

            if (id != 0 && productHandler.HandleDeleteProduct(id))
            {
                response.Success = true;
                response.Message = "Product deleted successfully";
                response.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                response.Message = "An error has ocurred when deleting the product";
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }
    }
}

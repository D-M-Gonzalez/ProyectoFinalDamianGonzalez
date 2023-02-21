using System.Data;
using DamianGonzalezCSharp.Models;
using DamianGonzalezCSharp.Handlers;
using System.Net;
using DamianGonzalezCSharp.Validations;
using Microsoft.AspNetCore.Mvc;

namespace DamianGonzalezCSharp.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private static readonly ProductValidations validations = new();
        private LoginHandler loginHandler = new LoginHandler();

        [HttpGet("GetProducts")]
        public ProductResponse GetProducts(string? descrip = "", string? order = "")
        {
            List<Product> products = new List<Product>();
            DataSet dsProducts = new DataSet();
            ProductHandler productHandler = new ProductHandler();
            ProductResponse response = new ProductResponse(false, "There was a problem communicating with the server", HttpStatusCode.InternalServerError);

            if (productHandler.HandleGetProducts(dsProducts, descrip, order))
            {
                if (dsProducts.Tables.Contains("DAT") && dsProducts.Tables["DAT"].Rows.Count > 0)
                {
                    DataTable dtDAT = dsProducts.Tables["DAT"];

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

                    response.GenerateResponse(true, "Successfull", HttpStatusCode.OK);
                    response.Products = products;
                }
                else
                {
                    response.GenerateResponse(false, "Can't obtain requested data", HttpStatusCode.Conflict);
                }
            }

            dsProducts.Dispose();
            return response;
        }

        [HttpGet("GetProduct")]
        public ProductResponse GetProduct(Int32 productId)
        {
            Product product = new Product();
            List < Product > productList = new List<Product>();
            DataSet dsProduct = new DataSet();
            ProductHandler productHandler = new ProductHandler();
            ProductResponse response = new ProductResponse(false, "There was a problem communicating with the server", HttpStatusCode.InternalServerError);

            if (productHandler.HandleGetProducts(dsProduct, productId))
            {
                if (dsProduct.Tables.Contains("DAT") && dsProduct.Tables["DAT"].Rows.Count > 0)
                {
                    DataRow dr = dsProduct.Tables["DAT"].Rows[0];
                    product.Id = Convert.ToInt32(dr["id"]);
                    if (dr["descripciones"] != DBNull.Value) product.Description = Convert.ToString(dr["Descripciones"]).Trim();
                    if (dr["costo"] != DBNull.Value) product.Cost = Convert.ToDecimal(dr["costo"]);
                    if (dr["precioventa"] != DBNull.Value) product.SellingPrice = Convert.ToDecimal(dr["precioventa"]);
                    if (dr["stock"] != DBNull.Value) product.Stock = Convert.ToInt32(dr["stock"]);
                    if (dr["idusuario"] != DBNull.Value) product.UserId = Convert.ToInt32(dr["idusuario"]);
                    productList.Add(product);

                    response.GenerateResponse(true, "Successfull", HttpStatusCode.OK);
                    response.Products = productList;

                } else
                {
                    response.GenerateResponse(false, "Product not found", HttpStatusCode.Conflict);
                }

            }

            dsProduct.Dispose();
            return response;
        }

        [HttpPost("CreateProduct")]
        public ProductResponse CreateProduct(Product data)
        {
            ProductResponse response = validations.ValidateCreation(data);
            ProductHandler productHandler = new ProductHandler();

            if (!loginHandler.HandleGetToken(Request.Headers))
            {
                response.GenerateResponse(false, "Unauthorized", HttpStatusCode.Unauthorized);
            }
            if (response.Success == true && productHandler.HandleCreateProduct(data))
            {
                response.GenerateResponse(true, "Successfull", HttpStatusCode.OK);
            }
            else if (response.Success == true)
            {
                response.GenerateResponse(false, "There was a problem communicating with the server", HttpStatusCode.InternalServerError);
            }

            return response;
        }

        [HttpPut("EditProduct")]

        public ProductResponse ModifyProduct(Product data)
        {
            ProductResponse response = validations.ValidateUpdate(data);
            ProductHandler productHandler = new ProductHandler();

            if (!loginHandler.HandleGetToken(Request.Headers))
            {
                response.GenerateResponse(false, "Unauthorized", HttpStatusCode.Unauthorized);
            }

            if (response.Success == true && data.Id != 0 && productHandler.HandleUpdateProduct(data))
            {
                response.GenerateResponse(true, "Successfull", HttpStatusCode.OK);
            } 
            else if (response.Success == true)
            {
                response.GenerateResponse(false, "There was a problem communicating with the server", HttpStatusCode.InternalServerError);
            }

            return response;
        }

        [HttpDelete("DeleteProduct")]

        public ProductResponse DeleteProduct(Int32 id)
        {
            ProductResponse response = validations.ValidateDelete(id);
            ProductHandler productHandler = new ProductHandler();

            if (!loginHandler.HandleGetToken(Request.Headers))
            {
                response.GenerateResponse(false, "Unauthorized", HttpStatusCode.Unauthorized);
            }

            if (response.Success == true && productHandler.HandleDeleteProduct(id))
            {
                response.GenerateResponse(true, "Successfull", HttpStatusCode.OK);
            }
            else if (response.Success == true)
            {
                response.GenerateResponse(false, "There was a problem communicating with the server", HttpStatusCode.InternalServerError);
            }

            return response;
        }
    }
}

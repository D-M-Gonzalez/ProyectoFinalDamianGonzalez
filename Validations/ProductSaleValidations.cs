using DamianGonzalezCSharp.Models;
using System.Net;

namespace DamianGonzalezCSharp.Validations
{
    public class ProductSaleValidations
    {
        private ProductSaleResponse response = new ProductSaleResponse();

        public ProductSaleResponse ValidateDelete(Int32 id)
        {
            response.Success = true;

            if (response.Success == true && id == 0)
            {
                response.GenerateResponse(false, "Product Sale Id required for delete", HttpStatusCode.Conflict);
            }

            return response;
        }
    }
}

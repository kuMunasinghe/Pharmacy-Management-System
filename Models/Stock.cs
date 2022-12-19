
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PMS.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public int MedicineID { get; set; }
        public string MedicineName { get; set; }
        public int Quantity { get; set; }
        public int Capacity { get; set; }
        public string StockStatus
        {
            get
            {
                int frac = Capacity / 4;

                
                if (2*frac >= Quantity)
                {
                    return "CHECK ⚠️";
                   
                }
                else if (frac <Quantity)
                {
                    return "OK ✅";
                }
               
                else
                {
                    return "UNKNOWN !";
                }
            }
        }
    }
}

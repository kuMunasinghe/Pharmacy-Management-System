
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
                int ratio= Capacity/Quantity ;
                if (ratio > 0.5)
                {
                    return "OK ✅";
                } 
                else if (ratio <0.3)
                {
                    return "LOW❗";
                }
                else if(ratio<0.5 && ratio > 0.3)
                {
                    return "CHECK ⚠️";
                }
                else
                {
                    return null;
                }
            }
        }
    }
}

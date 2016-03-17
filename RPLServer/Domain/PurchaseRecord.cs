namespace Domain
{
    public class PurchaseRecord
    {
        public string RESPONSE_CODE { get; set; }
        public PurchaseData INAPP_PURCHASE_DATA { get; set; }
        public string INAPP_DATA_SIGNATURE { get; set; }
    }
}
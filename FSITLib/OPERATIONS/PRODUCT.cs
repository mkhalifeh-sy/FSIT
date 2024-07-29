namespace FSITLib.OPERATION
{
    public class PRODUCT
    {
       
        public long PRODUCT_ID { get; set; }
        public string? PRODUCT_NAME { get; set; }
        public long PRODUCT_ISDELETED { get; set; }

        internal List<object>? GetFieldsDataList(DataListOptions opt)
        {
            List<object>? res = null;
            switch (opt)
            {
                case DataListOptions.All:
                    res = new List<object>() {
                        this.PRODUCT_ID,
                        this.PRODUCT_NAME,
                        this.PRODUCT_ISDELETED,
                    };
                    break;
                case DataListOptions.NoID:
                    res = new List<object>() {
                        this.PRODUCT_NAME,
                        this.PRODUCT_ISDELETED,
                    };
                    break;

            }
            return res;
        }

    }
}

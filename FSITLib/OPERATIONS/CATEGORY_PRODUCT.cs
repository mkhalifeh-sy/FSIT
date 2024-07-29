using FSITLib.AUTH;

namespace FSITLib.OPERATION
{
    public class CATEGORY_PRODUCT
    {
        public long CATEGORY_PRODUCT_ID { get; set; }
        public long CATEGORY_ID { get; set; }
        public long PRODUCT_ID { get; set; }

        public List<object>? GetFieldsDataList(DataListOptions opt)
        {
            List<object>? res = null;
            switch (opt)
            {
                case DataListOptions.All:
                    res = new List<object>() {
                        this.CATEGORY_PRODUCT_ID,
                        this.CATEGORY_ID,
                        this.PRODUCT_ID,
                    };
                    break;
                case DataListOptions.NoID:
                    res = new List<object>() {
                         this.CATEGORY_ID,
                        this.PRODUCT_ID,
                    };
                    break;

            }
            return res;
        }
    }
}

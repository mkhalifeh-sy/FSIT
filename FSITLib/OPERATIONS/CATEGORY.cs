namespace FSITLib.OPERATION
{
    public class CATEGORY
    {
        public long CATEGORY_ID { get; set; }
        public string? CATEGORY_NAME { get; set; }
        public long CATEGORY_ISDELETED { get; set; }

        internal List<object>? GetFieldsDataList(DataListOptions opt)
        {
            List<object>? res = null;
            switch (opt)
            {
                case DataListOptions.All:
                    res = new List<object>() {
                        this.CATEGORY_ID,
                        this.CATEGORY_NAME,
                        this.CATEGORY_ISDELETED,
                    };
                    break;
                case DataListOptions.NoID:
                    res = new List<object>() {
                        this.CATEGORY_NAME,
                        this.CATEGORY_ISDELETED,
                    };
                    break;

            }
            return res;
        }

    }
}

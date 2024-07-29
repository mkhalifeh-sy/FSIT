
namespace FSITLib.AUTH
{
    public class USER
    {
        public long USER_ID { get; set; }
        public string? USER_EMAIL { get; set; }
        public string? USER_PASSSWORD { get; set; }
        public string? USER_FIRST_NAME { get; set; }
        public string? USER_LAST_NAME { get; set; }
        public string? USER_MOBILE { get; set; }

        internal List<object>? GetFieldsDataList(DataListOptions opt)
        {
            List<object>? res = null;
            switch (opt)
            {
                case DataListOptions.All:
                    res = new List<object>() {
                        this.USER_ID,
                        this.USER_EMAIL,
                        this.USER_PASSSWORD,
                        this.USER_FIRST_NAME,
                        this.USER_LAST_NAME,
                        this.USER_MOBILE
                    };
                    break;
                case DataListOptions.NoID:
                    res = new List<object>() {
                        this.USER_EMAIL,
                        this.USER_PASSSWORD,
                        this.USER_FIRST_NAME,
                        this.USER_LAST_NAME,
                        this.USER_MOBILE
                    };
                    break;

            }
            return res;
        }
    }
}

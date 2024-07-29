using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSITLib.AUTH;
using FSITLib;
using FSITLib.OPERATION;
using FSITLib.DB;

namespace FSITLib.API
{
    public class FSITCategoryProductAPI
    {
        #region
        #endregion

        #region Private Vars
        private DBManager _db;
        private List<USER> _lstUsers;
        private List<CATEGORY> _lstCategories;
        private List<PRODUCT> _lstProducts;
        private List<CATEGORY_PRODUCT> _lstCategoriesProduct;

        private List<int> _currentPages;
        #endregion
        #region Constructor - Init
        public FSITCategoryProductAPI()
        {
            InitiateClassVars();
        }
        
        private void InitiateClassVars()
        {
            _db = new();
            _lstUsers = [];
            _lstCategories = [];
            _lstProducts = [];
            _lstCategoriesProduct = [];
            _currentPages = new List<int>()
            {
                0,0,
            };
        }
        #endregion

        #region Public Functions
        public List<PRODUCT> GetProducts (QueryPageSize PageSize = QueryPageSize.All ) {
            var xReturnedData = _db.ExcuteReaderOrderByID("OPERATION.PRODUCT");

            if (xReturnedData != null)
            {
                if (xReturnedData.Count > 0)
                {
                    return xReturnedData.ConvertAll(new Converter<object, PRODUCT>(ConvertToPRODUCT));
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public List<CATEGORY> GetCategories(QueryPageSize PageSize = QueryPageSize.All) {
            var xReturnedData = _db.ExcuteReaderOrderByID("OPERATION.CATEGORY");

            if (xReturnedData != null)
            {
                if (xReturnedData.Count > 0)
                {
                    return xReturnedData.ConvertAll(new Converter<object, CATEGORY>(ConvertToCATEGORY));
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public List<CATEGORY_PRODUCT> GetCategoryProduct(QueryPageSize PageSize = QueryPageSize.All)
        {
            var xReturnedData = _db.ExcuteReaderOrderByID("OPERATION.CATEGORY_PRODUCT");

            if (xReturnedData != null)
            {
                if (xReturnedData.Count > 0)
                {
                    return xReturnedData.ConvertAll(new Converter<object, CATEGORY_PRODUCT>(ConvertToCATEGORY_PRODUCT));
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public List<CATEGORY> GetCategoriesByProductID(long _id)
        {
            var data = GetCategoryProduct();
            var cat_ids =  data.Where(x => x.PRODUCT_ID == _id).ToList();
            var catData = GetCategories();
            List<CATEGORY> returnedData = new();
            foreach (var cat in catData)
            {
                foreach (var selectedcat in cat_ids)
                {
                    if (cat.CATEGORY_ID == selectedcat.CATEGORY_ID)
                    {
                        if (cat.CATEGORY_ISDELETED == 0)
                        {
                            if (!returnedData.Contains(cat))
                            {
                                returnedData.Add(cat);
                            }
                        }
                    }
                }
            }
            return returnedData;
        }

        public PRODUCT AddNewProduct(PRODUCT newProduct)
        {
            var res = _db.InsertNewRecord("OPERATION.PRODUCT", newProduct);
            if (int.Parse(res[1].ToString()) != -1)
            {
                var newRecord = _db.ExcuteReaderWithWhere("OPERATION.PRODUCT", " [PRODUCT_NAME] = '" + newProduct.PRODUCT_NAME + "'");
                return ConvertToPRODUCT(newRecord[0]);
            }
            return null;
        }
        public CATEGORY_PRODUCT AddNewCATEGORY_PRODUCT(CATEGORY_PRODUCT newCategoryProduct)
        {
            var res = _db.InsertNewRecord("OPERATION.CATEGORY_PRODUCT", newCategoryProduct);
            if (int.Parse(res[1].ToString()) != -1)
            {
                var newRecord = _db.ExcuteReaderWithWhere("OPERATION.CATEGORY_PRODUCT", " [CATEGORY_ID] = " + newCategoryProduct.CATEGORY_ID + " AND [PRODUCT_ID] = " + newCategoryProduct.PRODUCT_ID);
                return ConvertToCATEGORY_PRODUCT(newRecord[0]);
            }
            return null;
        }

        public void DeleteCategory_Product(CATEGORY_PRODUCT DeletedCategoryProduct)
        {
            var res = _db.DeleteRecordByID("OPERATION.CATEGORY_PRODUCT", DeletedCategoryProduct);
            if (int.Parse(res[1].ToString()) != -1)
            {
                var newRecord = _db.ExcuteReaderWithWhere("OPERATION.CATEGORY_PRODUCT", " [CATEGORY_ID] = " + newCategoryProduct.CATEGORY_ID + " AND [PRODUCT_ID] = " + newCategoryProduct.PRODUCT_ID);
                //return ConvertToCATEGORY_PRODUCT(newRecord[0]);
            }
            //return null;
        }
        public CATEGORY AddNewCATEGORY(CATEGORY newCategory)
        {
            var res = _db.InsertNewRecord("OPERATION.CATEGORY", newCategory);
            if (int.Parse(res[1].ToString()) != -1)
            {
                var newRecord = _db.ExcuteReaderWithWhere("OPERATION.CATEGORY", " [CATEGORY_NAME] = '" + newCategory.CATEGORY_NAME + "'");
                return ConvertToCATEGORY(newRecord[0]);
            }
            return null;
        }


        public PRODUCT UpdateProduct(PRODUCT UpdateProduct)
        {
            var res = _db.UpdateRecordByID("OPERATION.PRODUCT", UpdateProduct);
            if (int.Parse(res[1].ToString()) != -1)
            {
                var UpdatedRecord = _db.ExcuteReaderWithWhere("OPERATION.PRODUCT", " [PRODUCT_NAME] = '" + newProduct.PRODUCT_NAME + "'");
                return ConvertToPRODUCT(UpdatedRecord[0]);
            }
            return null;
        }
        public USER getUser(string username)
        {
            List<USER> users = new List<USER>();
            var xReturnedData = _db.ExcuteReaderOrderByID("SECURITY.AUTH.USERS");

            if (xReturnedData != null)
            {
                if (xReturnedData.Count > 0)
                {
                    users = xReturnedData.ConvertAll(new Converter<object, USER>(ConvertToUSER));
                }
            };

            return users.First(x => x.USER_EMAIL == username);
        }

        #endregion

        #region Helpers
        private CATEGORY ConvertToCATEGORY(object input)
        {
            return (CATEGORY)input;
        }

        private PRODUCT ConvertToPRODUCT(object input)
        {
            return (PRODUCT)input;
        }
        private CATEGORY_PRODUCT ConvertToCATEGORY_PRODUCT(object input)
        {
            return (CATEGORY_PRODUCT)input;
        }

        private USER ConvertToUSER(object input)
        {
            return (USER)input;
        }
        #endregion
    }
}

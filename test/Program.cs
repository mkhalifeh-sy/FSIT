// See https://aka.ms/new-console-template for more information

using FSITLib.AUTH;
using FSITLib.DB;
using FSITLib.OPERATION;
Console.WriteLine("Testing Insert ...");
DBManager _db = new();
List<object> res = _db.InsertNewRecord("SECURITY.AUTH.USERS", new USER()
{
    USER_EMAIL = "mk@gmail.com",
    USER_PASSSWORD = "123",
    //USER_FIRST_NAME = "M",
    USER_LAST_NAME = "K",
    USER_MOBILE = "123456789"
});
Console.WriteLine((string)res[0]);
Console.WriteLine((int)res[1]);

res = _db.InsertNewRecord("OPERATION.CATEGORY_PRODUCT", new CATEGORY_PRODUCT()
{
    CATEGORY_ID = 11,
    PRODUCT_ID = 22
});

Console.WriteLine((string)res[0]);
Console.WriteLine((int)res[1]);

Console.WriteLine("Testing Update ...");

res = _db.UpdateRecordByID("SECURITY.AUTH.USERS", new USER()
{
    USER_ID = 1,
    USER_EMAIL = "mk@gmail.com",
    USER_PASSSWORD = "123",
    //USER_FIRST_NAME = "M",
    USER_LAST_NAME = "K",
    USER_MOBILE = "123456789"
});
Console.WriteLine((string)res[0]);
Console.WriteLine((int)res[1]);

res = _db.UpdateRecordByID("OPERATION.CATEGORY_PRODUCT", new CATEGORY_PRODUCT()
{
    CATEGORY_PRODUCT_ID = 2,
    CATEGORY_ID = 11,
    PRODUCT_ID = 22
});

Console.WriteLine((string)res[0]);
Console.WriteLine((int)res[1]);

Console.WriteLine("Testing Delete ...");
res = _db.DeleteRecordByID("SECURITY.AUTH.USERS", new USER()
{
    USER_ID = 1,
    USER_EMAIL = "mk@gmail.com",
    USER_PASSSWORD = "123",
    //USER_FIRST_NAME = "M",
    USER_LAST_NAME = "K",
    USER_MOBILE = "123456789"
});
Console.WriteLine((string)res[0]);
Console.WriteLine((int)res[1]);

res = _db.DeleteRecordByID("OPERATION.CATEGORY_PRODUCT", new CATEGORY_PRODUCT()
{
    CATEGORY_PRODUCT_ID = 2,
    CATEGORY_ID = 11,
    PRODUCT_ID = 22
});

Console.WriteLine((string)res[0]);
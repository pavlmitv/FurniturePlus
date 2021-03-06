namespace FurniturePlus.Data.Models
{
    public class Constants
    {
        public class User
        { 
            public const int UsernameMinLength = 6;
            public const int UsernameMaxLength = 30;
            public const int FirstLastNameMinLength = 2;
            public const int FirstLastNameMaxLength = 30;
            public const int PasswordMinLength = 8;
            public const int PasswordMaxLength = 30;
        }
        public class Item
        {
            public const int ItemNameMaxLength = 60;
            public const int ItemDescriptionMaxLength = 250;
        }
        public class Category
        {
            public const int CategoryNameMaxLength = 30;
        }
        public class Vendor
        {
            public const int VendorNameMaxLength = 30;
            public const int VendorAddressMaxLength = 250;
        }

    }
}

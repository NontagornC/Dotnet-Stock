namespace dotnet_stock.Installers
{
    public static class MyStringExtensions
    {
        public static bool IsValidProductName(this string name)
        {
            if (name.Length < 3)
            {
                throw new Exception("Product name must be at least 3 characters long");
            }
            return true;
        }
    }
}
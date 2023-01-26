namespace sale_test.Models.Customers
{
    public class UpdateCustomerRequest
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
    }
}

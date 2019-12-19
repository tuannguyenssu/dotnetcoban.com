namespace GenFuTest
{
    class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public int Age { get; set; }
        public int NumberOfKids { get; set; }
        private string _middleName;

        public void SetMiddleName(string middleName)
        {
            _middleName = middleName;
        }
    }
}
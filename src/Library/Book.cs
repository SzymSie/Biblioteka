namespace Library
{
    internal class Book
    {
        public string title {get; set; }
        public string author {get; set; }
        public int ISBN {get; set; }
        public string lastRentalDate {get; set; }
        public string borrowerName {get; set; }

        public Book()
        {
        }
        public Book(string title, string author, int ISBN, string lastRentalDate, string borrowerName)
        {
            this.title = title;
            this.author = author;
            this.ISBN = ISBN;
            this.lastRentalDate = lastRentalDate;
            this.borrowerName = borrowerName;
        }

        override public string ToString()
        {
            return $"Title: {title}\nAuthor: {author}\nISBN: {ISBN}\nLast rental date: {lastRentalDate}\nBorrower's name: {borrowerName}";
        }
    }
}
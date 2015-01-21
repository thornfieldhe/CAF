using System;

namespace CAF.Model
{

    public class ReadOnlyBookList : ReadOnlyCollectionBase<BookView, ReadOnlyBookList>
    {
        public ReadOnlyBookList() : base("ViewBooks") { }
    }

    public class BookView
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Isbn { get; private set; }

        public int Page { get; private set; }

        public string Author { get; private set; }

        public DateTime Publication { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DateTime ChangedDate { get; private set; }

        public int Status { get; private set; }

        public Guid StoreId { get; private set; }

        public DateTime? TestDate { get; private set; }

        public string StoreName { get; private set; }
    }

}



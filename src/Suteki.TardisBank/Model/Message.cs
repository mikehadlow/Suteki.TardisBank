using System;

namespace Suteki.TardisBank.Model
{
    public class Message
    {
        public Message(int id, DateTime date, string text)
        {
            Id = id;
            Date = date;
            Text = text;
            HasBeenRead = false;
        }

        public void Read()
        {
            HasBeenRead = true;
        }

        public int Id { get; private set; }
        public DateTime Date { get; private set; }
        public string Text { get; private set; }
        public bool HasBeenRead { get; private set; }
    }
}
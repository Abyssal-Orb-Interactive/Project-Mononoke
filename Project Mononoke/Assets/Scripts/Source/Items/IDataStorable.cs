using System;

namespace Source.ItemsModule
{
    public interface IDataStorable
    {
        public int ID { get; }
        public IPickUpableDatabase Database { get; }
    }
}

﻿namespace Core.Entities
{
    public interface IEntity<T>
    {
        T Id { get; set; }
        DateTime DateCreated { get; set; }
    }

    public class BaseEntity<T> : IEntity<T>
    {
        public T Id { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now.ToUniversalTime();
    }
}

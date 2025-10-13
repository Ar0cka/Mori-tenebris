using System;

namespace Systems
{
    public class ReactiveValue<T>
    {
        private T _value;
        public event Action<T> OnChange;

        public ReactiveValue(T value)
        {
            _value = value;
        }

        public T Value
        {
            get => _value;
            set
            {
                if (!Equals(_value, value))
                {
                    _value = value;
                    OnChange?.Invoke(_value);
                }
            }
        }
    }
}
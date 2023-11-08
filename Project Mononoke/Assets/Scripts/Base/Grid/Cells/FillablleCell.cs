using System;
using UnityEngine;

namespace Base.Grid
{
    public class FillableCell<TFillableCellContent> : ICell<TFillableCellContent> where TFillableCellContent : IFillableCellContent
    {
        private const int MAX_WEIGHT_CAPACITY = 100;
        private const int MIN_WEIGHT_CAPACITY = 0;
        
        private TFillableCellContent _content;

        public FillableCell()
        {
            _content = default;
        }

        public FillableCell(TFillableCellContent content)
        {
            _content = content ?? default;
        }
        public float GetValueInPercents()
        {
            return (float) _content.GetWeight() / MAX_WEIGHT_CAPACITY;
        }

        public bool TryAddValue(IFillableCellContent content)
        {
            if (content.GetWeight() <= MIN_WEIGHT_CAPACITY) return false;
            if (content.GetWeight() >= MAX_WEIGHT_CAPACITY) return false;
            return content.GetWeight() + _content.GetWeight() <= MAX_WEIGHT_CAPACITY && _content.TryAdd(content);
        }

        public TFillableCellContent GetValue()
        {
            return _content;
        }
        
        public bool TrySetValue(TFillableCellContent content)
        {
            if (content == null) return false;
            if (content.GetWeight() > MAX_WEIGHT_CAPACITY || content.GetWeight() < MIN_WEIGHT_CAPACITY) return false;

            SetValue(content);
            
            return true;
        }
        
        private void SetValue(TFillableCellContent content)
        {
            _content = content;
        }
    }
    
    public class CellConstructionException : Exception
    {
        public CellConstructionException() { }
        
        public CellConstructionException(string message)
            : base(message) { }
        
        public CellConstructionException(string message, Exception inner)
            : base(message, inner) { }
    }
}
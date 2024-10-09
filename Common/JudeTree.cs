using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class JudeTree<T>
    {
        public List<JudeTreeNode<T>> Trees = [];
    }

    public class JudeTreeNode<T>(T data)
    {
        public T Data { get; private set; } = data;
        public readonly LinkedList<JudeTreeNode<T>> Children = [];
        public JudeTreeNode<T>? Parent { get; set; }

        #region Methods
        public JudeTreeNode<T> AddFirstChild(JudeTreeNode<T> data)
        {
            Children.AddFirst(data);
            return this;
        }
        public JudeTreeNode<T> AddLastChild(JudeTreeNode<T> data)
        {
            Children.AddLast(data);
            return this;
        }
        public void RemoveChild(JudeTreeNode<T> data) => Children.Remove(data);
        public void ClearChild() => Children.Clear();
        public void ResetData(T data) => Data = data;
        #endregion

        public JudeTreeNode<T> this[int index]
        {
            get => Children.ElementAt(index);
            set
            {
                var _chi = Children.ElementAt(index);
                _chi = value;
            }
        }
    }
}

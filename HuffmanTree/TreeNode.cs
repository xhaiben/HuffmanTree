using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanTree
{
    class TreeNode
    {
        public char data;
        public double weight;
        public int id;
        public int x;
        public int y;
        public TreeNode leftChild;
        public TreeNode rightChild;
        public TreeNode()
        {
            this.data = '\0';
            this.weight = 0;
            this.leftChild = null;
            this.rightChild = null;
            this.id = 0;
        }
        public TreeNode(char ch, double weight)
        {
            this.data = ch;
            this.weight = weight;
            this.leftChild = null;
            this.rightChild = null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanTree
{
    class Huffman
    {
        private Dictionary<char, string> resultDic;
        private Dictionary<char, double> tempDic;
        private TreeNode root;
        private List<TreeNode> nodes;
        private string str;

        public Huffman()
        {
            resultDic = new Dictionary<char, string>();
        }
        public Dictionary<char, string> get_HuffmanCode(Dictionary<char, double> dic)
        {
            /*建立huffman二叉树
             *创建节点。选取权值最小的两个作为左右儿子
             *在List中删除左右儿子节点，添加新建节点
             */
            this.tempDic = dic;
            init_Nodes();
            while (nodes.Count > 1)
            {
                TreeNode subNode = new TreeNode();
                subNode.leftChild = nodes.ElementAt(0);
                subNode.rightChild = nodes.ElementAt(1);
                subNode.weight = subNode.leftChild.weight + subNode.rightChild.weight;
                nodes.RemoveAt(1);
                nodes.RemoveAt(0);
                nodes.Add(subNode);
                nodes = nodes.OrderBy(r => r.weight).ToList();
            }
            try
            {
                //获取根节点
                this.root = nodes.ElementAt(0);
            }
            catch
            {

            }
            str = "";
            //遍历二叉树 获得编码字典
            middle_Order(root);
            //按编码长度排序
            resultDic = resultDic.OrderBy(r => r.Value.Length).ToDictionary(r => r.Key, r => r.Value);
            return resultDic;
        }
        private void middle_Order(TreeNode subNode)
        {
            if (subNode != null)
            {
                if (subNode.leftChild == null && subNode.rightChild == null)
                {
                    try
                    {
                        resultDic.Add(subNode.data, str);
                    }
                    catch
                    {
                        
                    }
                }
                //左0右1
                str += "0";
                middle_Order(subNode.leftChild);
                str = str.Substring(0, str.Length - 1);
                str += "1";
                middle_Order(subNode.rightChild);
                str = str.Substring(0, str.Length - 1);
            }
        }
        private void init_Nodes()
        {
            nodes = new List<TreeNode>();
            foreach (char key in tempDic.Keys)
            {
                nodes.Add(new TreeNode(key, tempDic[key]));
            }
        }
        public string translateCode(string code)
        {
            //遍历二叉树获取译文
            string passage = "";
            TreeNode subNode = root;
            try
            {
                foreach (char c in code)
                {

                    if (c == '0')
                    {
                        subNode = subNode.leftChild;

                    }
                    else if (c == '1')
                    {
                        subNode = subNode.rightChild;

                    }
                    if (subNode.leftChild == null && subNode.rightChild == null)
                    {
                        passage += subNode.data;
                        subNode = root;
                    }
                }
            }
            catch
            {

            }
            return passage;
        }
        public void Clear()
        {
            try
            {
                this.tempDic.Clear();
                this.resultDic.Clear();
                nodes.Clear();
            }
            catch
            {

            }
        }
    }
}
